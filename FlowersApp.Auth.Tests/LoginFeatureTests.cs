using CustomerEntity = FlowersApp.Auth.Domain.Entities.Customer;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Features.Auth.Commands.GenerateAuthTokens;
using FlowersApp.Auth.Features.Auth.Commands.RateLimit;
using FlowersApp.Auth.Features.Auth.DriverLogin;
using FlowersApp.Auth.Features.Auth.Queries.GetCustomerByEmail;
using FlowersApp.Auth.Features.Auth.Queries.GetDriverApplicationByEmail;
using FlowersApp.Auth.Features.Auth.Queries.GetDriverUserByEmail;
using FlowersApp.Auth.Features.Auth.Queries.VerifyPassword;
using FlowersApp.Auth.Features.Auth.UserLogin;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Xunit;

namespace FlowersApp.Auth.Tests;

public class LoginFeatureTests
{
    private readonly Mock<IMediator> _mediatorMock;

    public LoginFeatureTests()
    {
        _mediatorMock = new Mock<IMediator>();
    }

    [Fact]
    public async Task UserLoginValidator_Should_Pass_For_Valid_Email_And_Password()
    {
        // Arrange
        var validator = new UserLoginValidator();
        var command = new UserLoginOrchestrator("customer@example.com", "anyPassword123");

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task UserLoginValidator_Should_Fail_For_Malformed_Email()
    {
        // Arrange
        var validator = new UserLoginValidator();
        var command = new UserLoginOrchestrator("not-an-email", "anyPassword123");

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task UserLoginValidator_Should_Not_Fail_Password_For_Registration_Rules()
    {
        // Arrange - Password does not contain uppercase/digit, but is non-empty
        var validator = new UserLoginValidator();
        var command = new UserLoginOrchestrator("customer@example.com", "simple");

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert - Login validator only requires password to be not empty
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task UserLoginOrchestratorHandler_Should_Return_Tokens_On_Valid_Credentials()
    {
        // Arrange
        var command = new UserLoginOrchestrator("customer@example.com", "password");
        var customer = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            Email = "customer@example.com",
            FullName = "Test Customer"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CheckLoginRateLimitQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<bool>.succeeded(false, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCustomerByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<CustomerEntity?>.succeeded(customer, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyPasswordQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<bool>.succeeded(true, ResultCode.LoginSuccessful));

        var expectedAuthResponse = new AuthResponse(
            customer.Id, customer.Email, customer.FullName, "Customer",
            "mock.access.token", "mock.refresh.token", DateTime.UtcNow.AddHours(1)
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GenerateAuthTokensCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<AuthResponse>.succeeded(expectedAuthResponse, ResultCode.LoginSuccessful));

        var handler = new UserLoginOrchestratorHandler(_mediatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Code.Should().Be(ResultCode.LoginSuccessful);
        result.Result.Should().NotBeNull();
        result.Result!.Role.Should().Be("Customer");
        result.Result.AccessToken.Should().Be("mock.access.token");
    }

    [Fact]
    public async Task UserLoginOrchestratorHandler_Should_Return_Generic_InvalidCredentials_On_Wrong_Password()
    {
        // Arrange
        var command = new UserLoginOrchestrator("customer@example.com", "wrongpassword");
        var customer = new CustomerEntity { Id = Guid.NewGuid(), Email = "customer@example.com" };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CheckLoginRateLimitQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<bool>.succeeded(false, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCustomerByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<CustomerEntity?>.succeeded(customer, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyPasswordQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<bool>.succeeded(false, ResultCode.InvalidCredentials));

        var handler = new UserLoginOrchestratorHandler(_mediatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Code.Should().Be(ResultCode.InvalidCredentials);
        _mediatorMock.Verify(m => m.Send(It.IsAny<RecordFailedLoginAttemptCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DriverLoginOrchestratorHandler_Should_Return_403_For_Pending_Driver_Application()
    {
        // Arrange
        var command = new DriverLoginOrchestrator("pendingdriver@example.com", "password");
        var pendingApplication = new DriverApplication
        {
            Id = Guid.NewGuid(),
            Email = "pendingdriver@example.com",
            FullName = "Pending Driver",
            Status = DriverApplicationStatus.Pending,
            NationalIDNumber = "1234567890",
            HashedPassword = "hashedpassword"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CheckLoginRateLimitQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<bool>.succeeded(false, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetDriverUserByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<Driver?>.succeeded(null, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetDriverApplicationByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<DriverApplication?>.succeeded(pendingApplication, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyPasswordQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<bool>.succeeded(true, ResultCode.LoginSuccessful));

        var handler = new DriverLoginOrchestratorHandler(_mediatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Code.Should().Be(ResultCode.DriverAccountNotApproved);
        result.Result.Should().NotBeNull();
        result.Result!.DriverStatus.Should().Be("Pending");
    }

    [Fact]
    public async Task DriverLoginOrchestratorHandler_Should_Return_Tokens_For_Approved_Driver()
    {
        // Arrange
        var command = new DriverLoginOrchestrator("approveddriver@example.com", "password");
        var approvedDriver = new Driver
        {
            Id = Guid.NewGuid(),
            Email = "approveddriver@example.com",
            FullName = "Approved Driver",
            NationalIDNumber = "12345"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CheckLoginRateLimitQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<bool>.succeeded(false, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetDriverUserByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<Driver?>.succeeded(approvedDriver, ResultCode.LoginSuccessful));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyPasswordQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<bool>.succeeded(true, ResultCode.LoginSuccessful));

        var expectedAuthResponse = new AuthResponse(
            approvedDriver.Id, approvedDriver.Email, approvedDriver.FullName, "Driver",
            "driver.access.token", "driver.refresh.token", DateTime.UtcNow.AddHours(1),
            DriverStatus: "Approved"
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GenerateAuthTokensCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RequestResult<AuthResponse>.succeeded(expectedAuthResponse, ResultCode.LoginSuccessful));

        var handler = new DriverLoginOrchestratorHandler(_mediatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Code.Should().Be(ResultCode.LoginSuccessful);
        result.Result!.Role.Should().Be("Driver");
        result.Result.DriverStatus.Should().Be("Approved");
    }
}
