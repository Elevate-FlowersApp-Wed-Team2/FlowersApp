using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Features.Login;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Services;
using FlowersApp.Auth.Features.RefreshTokens;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Tests;

public class LoginHandlerTests
{
    private static IConfiguration ConfigurationWithJwtKey()
    {
        // generate a 256-bit key and encode as base64
        var keyBytes = new byte[32];
        Random.Shared.NextBytes(keyBytes);
        var base64 = Convert.ToBase64String(keyBytes);

        var inMemory = new Dictionary<string, string?>
        {
            ["JwtSettings:Key"] = base64,
            ["JwtSettings:Issuer"] = "tests",
            ["JwtSettings:Audience"] = "tests",
            ["JwtSettings:AccessTokenExpirySeconds"] = "600",
        };
        return new ConfigurationBuilder().AddInMemoryCollection(inMemory).Build();
    }

    private static async Task<(AppDbContext db, UserManager<AppUser> userManager, RoleManager<Role> roleManager)> CreateIdentityStoresAsync(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();

        var userStore = new UserStore<AppUser, Role, AppDbContext, Guid>(db);
        var roleStore = new RoleStore<Role, AppDbContext, Guid>(db);

        var identityOptions = Options.Create(new IdentityOptions());
        var passwordHasher = new PasswordHasher<AppUser>();
        var userValidators = new List<IUserValidator<AppUser>> { new UserValidator<AppUser>() };
        var pwdValidators = new List<IPasswordValidator<AppUser>> { new PasswordValidator<AppUser>() };
        var keyNormalizer = new UpperInvariantLookupNormalizer();
        var errors = new IdentityErrorDescriber();

        var userManager = new UserManager<AppUser>(userStore, identityOptions, passwordHasher, userValidators, pwdValidators, keyNormalizer, errors, null, new NullLogger<UserManager<AppUser>>());
        var roleManager = new RoleManager<Role>(roleStore, new List<IRoleValidator<Role>>(), keyNormalizer, errors, new NullLogger<RoleManager<Role>>());

        return (db, userManager, roleManager);
    }

    [Fact]
    public async Task Customer_Login_Succeeds_ReturnsTokensAndRole()
    {
        var (db, userManager, roleManager) = await CreateIdentityStoresAsync(nameof(Customer_Login_Succeeds_ReturnsTokensAndRole));
        var config = ConfigurationWithJwtKey();

        // create role
        await roleManager.CreateAsync(new Role { Name = "Customer", NormalizedName = "CUSTOMER" });

        var user = new AppUser { UserName = "cust1", Email = "cust1@example.com", EmailConfirmed = true, FullName = "Customer One" };
        var create = await userManager.CreateAsync(user, "Password123!");
        create.Succeeded.Should().BeTrue();

        var addRole = await userManager.AddToRoleAsync(user, "Customer");
        addRole.Succeeded.Should().BeTrue();

        // setup SignInManager mock to validate password via userManager
        var httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        var httpAccessor = new HttpContextAccessor { HttpContext = httpContext };

        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        userPrincipalFactoryMock.Setup(f => f.CreateAsync(It.IsAny<AppUser>())).ReturnsAsync(new ClaimsPrincipal());

        var signInManagerMock = new Mock<SignInManager<AppUser>>(userManager, httpAccessor, userPrincipalFactoryMock.Object, Options.Create(new IdentityOptions()), new NullLogger<SignInManager<AppUser>>(), Mock.Of<IAuthenticationSchemeProvider>(), Mock.Of<IUserConfirmation<AppUser>>());
        signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns((AppUser u, string pwd, bool lf) => userManager.CheckPasswordAsync(u, pwd).ContinueWith(t => t.Result ? SignInResult.Success : SignInResult.Failed));

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(t => t.GenerateTokens(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string?>()))
            .Returns((AppUser u, IEnumerable<string> roles, string? ds) => new TokenResult("access", "refresh", 600));

        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<SaveRefreshTokenCommand>(), It.IsAny<CancellationToken>()))
            .Returns(async (SaveRefreshTokenCommand cmd, CancellationToken ct) =>
            {
                var entity = new RefreshToken { Id = Guid.NewGuid(), Token = cmd.Token, ExpiresAt = cmd.ExpiresAt, UserId = cmd.UserId };
                db.RefreshTokens.Add(entity);
                await db.SaveChangesAsync(ct);
                return RequestResult<Guid>.succeeded(entity.Id, ResultCode.RegistrationSuccessful);
            });

        var handler = new LoginHandler(userManager, db, signInManagerMock.Object, tokenServiceMock.Object, mediatorMock.Object, config, new NullLogger<LoginHandler>(), httpAccessor);

        var command = new LoginCommand { Email = user.Email, Password = "Password123!" };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Code.Should().Be(FlowersApp.Auth.Shared.Response.ResultCode.LoginSuccessful);
        result.Success.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result!.AccessToken.Should().NotBeNullOrEmpty();
        result.Result.RefreshToken.Should().NotBeNullOrEmpty();
        result.Result.Role.Should().Be("Customer");
        result.Result.DriverStatus.Should().BeNull();

        // refresh token persisted
        var rt = await db.RefreshTokens.FirstOrDefaultAsync(r => r.UserId == user.Id);
        rt.Should().NotBeNull();
    }

    [Fact]
    public async Task Driver_Login_ReturnsDriverStatus()
    {
        var (db, userManager, roleManager) = await CreateIdentityStoresAsync(nameof(Driver_Login_ReturnsDriverStatus));
        var config = ConfigurationWithJwtKey();

        await roleManager.CreateAsync(new Role { Name = "Driver", NormalizedName = "DRIVER" });

        var driver = new Driver { UserName = "drv1", Email = "drv1@example.com", EmailConfirmed = true, DriverStatus = DriverStatus.Pending, NationalIDNumber = "NID123", FullName = "Driver One" };
        var create = await userManager.CreateAsync(driver, "Password123!");
        create.Succeeded.Should().BeTrue();

        var addRole = await userManager.AddToRoleAsync(driver, "Driver");
        addRole.Succeeded.Should().BeTrue();

        var httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        var httpAccessor = new HttpContextAccessor { HttpContext = httpContext };

        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        userPrincipalFactoryMock.Setup(f => f.CreateAsync(It.IsAny<AppUser>())).ReturnsAsync(new ClaimsPrincipal());

        var signInManagerMock = new Mock<SignInManager<AppUser>>(userManager, httpAccessor, userPrincipalFactoryMock.Object, Options.Create(new IdentityOptions()), new NullLogger<SignInManager<AppUser>>(), Mock.Of<IAuthenticationSchemeProvider>(), Mock.Of<IUserConfirmation<AppUser>>());
        signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns((AppUser u, string pwd, bool lf) => userManager.CheckPasswordAsync(u, pwd).ContinueWith(t => t.Result ? SignInResult.Success : SignInResult.Failed));

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(t => t.GenerateTokens(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string?>()))
            .Returns((AppUser u, IEnumerable<string> roles, string? ds) => new TokenResult("access", "refresh", 600));

        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<SaveRefreshTokenCommand>(), It.IsAny<CancellationToken>()))
            .Returns(async (SaveRefreshTokenCommand cmd, CancellationToken ct) =>
            {
                var entity = new RefreshToken { Id = Guid.NewGuid(), Token = cmd.Token, ExpiresAt = cmd.ExpiresAt, UserId = cmd.UserId };
                db.RefreshTokens.Add(entity);
                await db.SaveChangesAsync(ct);
                return RequestResult<Guid>.succeeded(entity.Id, ResultCode.RegistrationSuccessful);
            });

        var handler = new LoginHandler(userManager, db, signInManagerMock.Object, tokenServiceMock.Object, mediatorMock.Object, config, new NullLogger<LoginHandler>(), httpAccessor);

        var command = new LoginCommand { Email = driver.Email, Password = "Password123!" };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Code.Should().Be(FlowersApp.Auth.Shared.Response.ResultCode.LoginSuccessful);
        result.Result.Should().NotBeNull();
        result.Result!.DriverStatus.Should().Be(driver.DriverStatus.ToString());
        result.Result.Role.Should().Be("Driver");
    }

    [Fact]
    public async Task Invalid_Credentials_DoNotRevealAccountExistence()
    {
        var (db, userManager, roleManager) = await CreateIdentityStoresAsync(nameof(Invalid_Credentials_DoNotRevealAccountExistence));
        var config = ConfigurationWithJwtKey();

        var httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        var httpAccessor = new HttpContextAccessor { HttpContext = httpContext };

        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        userPrincipalFactoryMock.Setup(f => f.CreateAsync(It.IsAny<AppUser>())).ReturnsAsync(new ClaimsPrincipal());

        var signInManagerMock = new Mock<SignInManager<AppUser>>(userManager, httpAccessor, userPrincipalFactoryMock.Object, Options.Create(new IdentityOptions()), new NullLogger<SignInManager<AppUser>>(), Mock.Of<IAuthenticationSchemeProvider>(), Mock.Of<IUserConfirmation<AppUser>>());
        signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns((AppUser u, string pwd, bool lf) => userManager.CheckPasswordAsync(u, pwd).ContinueWith(t => t.Result ? SignInResult.Success : SignInResult.Failed));

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(t => t.GenerateTokens(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string?>()))
            .Returns((AppUser u, IEnumerable<string> roles, string? ds) => new TokenResult("access", "refresh", 600));

        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<SaveRefreshTokenCommand>(), It.IsAny<CancellationToken>()))
            .Returns(async (SaveRefreshTokenCommand cmd, CancellationToken ct) =>
            {
                var entity = new RefreshToken { Id = Guid.NewGuid(), Token = cmd.Token, ExpiresAt = cmd.ExpiresAt, UserId = cmd.UserId };
                db.RefreshTokens.Add(entity);
                await db.SaveChangesAsync(ct);
                return RequestResult<Guid>.succeeded(entity.Id, ResultCode.RegistrationSuccessful);
            });

        var handler = new LoginHandler(userManager, db, signInManagerMock.Object, tokenServiceMock.Object, mediatorMock.Object, config, new NullLogger<LoginHandler>(), httpAccessor);

        // unknown email
        var unknown = await handler.Handle(new LoginCommand { Email = "noone@example.com", Password = "whatever" }, CancellationToken.None);
        unknown.Code.Should().Be(FlowersApp.Auth.Shared.Response.ResultCode.InvalidEmailOrPassword);

        // create user and attempt wrong password
        var user = new AppUser { UserName = "u2", Email = "u2@example.com", EmailConfirmed = true, FullName = "User Two" };
        var create = await userManager.CreateAsync(user, "Password123!");
        create.Succeeded.Should().BeTrue();

        var wrong = await handler.Handle(new LoginCommand { Email = user.Email, Password = "WrongPass!" }, CancellationToken.None);
        wrong.Code.Should().Be(FlowersApp.Auth.Shared.Response.ResultCode.InvalidEmailOrPassword);

        // both should be same code
        unknown.Code.Should().Be(wrong.Code);
    }

    [Fact]
    public async Task RateLimiting_Returns_TooManyRequests()
    {
        var (db, userManager, roleManager) = await CreateIdentityStoresAsync(nameof(RateLimiting_Returns_TooManyRequests));
        var config = ConfigurationWithJwtKey();

        var httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        var httpAccessor = new HttpContextAccessor { HttpContext = httpContext };

        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        userPrincipalFactoryMock.Setup(f => f.CreateAsync(It.IsAny<AppUser>())).ReturnsAsync(new ClaimsPrincipal());

        var signInManagerMock = new Mock<SignInManager<AppUser>>(userManager, httpAccessor, userPrincipalFactoryMock.Object, Options.Create(new IdentityOptions()), new NullLogger<SignInManager<AppUser>>(), Mock.Of<IAuthenticationSchemeProvider>(), Mock.Of<IUserConfirmation<AppUser>>());
        signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.LockedOut);

        var tokenServiceMock = new Mock<ITokenService>();
        var mediatorMock = new Mock<IMediator>();

        var handler = new LoginHandler(userManager, db, signInManagerMock.Object, tokenServiceMock.Object, mediatorMock.Object, config, new NullLogger<LoginHandler>(), httpAccessor);

        var result = await handler.Handle(new LoginCommand { Email = "any@example.com", Password = "x" }, CancellationToken.None);

        result.Code.Should().Be(FlowersApp.Auth.Shared.Response.ResultCode.LoginRateLimited);
    }
}
