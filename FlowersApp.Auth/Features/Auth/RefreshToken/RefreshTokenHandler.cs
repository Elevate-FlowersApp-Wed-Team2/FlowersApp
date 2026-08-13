using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Auth.RefreshToken;

public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ISessionService _sessionService;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(
        UserManager<AppUser> userManager,
        ISessionService sessionService,
        ITokenService tokenService,
        AppDbContext db,
        IHttpContextAccessor httpContextAccessor,
        ILogger<RefreshTokenHandler> logger)
    {
        _userManager = userManager;
        _sessionService = sessionService;
        _tokenService = tokenService;
        _db = db;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<RequestResult<AuthResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _sessionService.FindByRawTokenAsync(request.RefreshToken, cancellationToken);
        if (existing is null)
            return RequestResult<AuthResponse>.Failure(ResultCode.InvalidRefreshToken);

        // Reuse of an already-rotated token => revoke the whole family.
        if (existing.RevokedAt is not null && existing.ReplacedByTokenId is not null)
        {
            _logger.LogWarning(
                "Refresh-token reuse detected for family {FamilyId}, user {UserId}.",
                existing.FamilyId,
                existing.UserId);
            await _sessionService.RevokeTokenFamilyAsync(existing.FamilyId, cancellationToken);
            return RequestResult<AuthResponse>.Failure(ResultCode.RefreshTokenReuseDetected);
        }

        if (!existing.IsActive)
            return RequestResult<AuthResponse>.Failure(ResultCode.InvalidRefreshToken);

        var user = await _userManager.FindByIdAsync(existing.UserId.ToString());
        if (user is null)
            return RequestResult<AuthResponse>.Failure(ResultCode.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? UserRoles.Customer;
        var applicationStatus = await ResolveApplicationStatusAsync(user, role, cancellationToken);

        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var deviceInfo = request.DeviceInfo ?? existing.DeviceInfo;

        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var access = await _tokenService.GenerateAccessTokenAsync(
                user, role, applicationStatus, cancellationToken);

            var refresh = await _tokenService.GenerateRefreshTokenAsync(
                user.Id,
                existing.FamilyId,
                deviceInfo,
                ip,
                cancellationToken);

            await _sessionService.MarkReplacedAsync(existing, refresh.Entity.Id, cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return RequestResult<AuthResponse>.succeeded(
                new AuthResponse(
                    access.AccessToken,
                    refresh.RawToken,
                    access.ExpiresInSeconds,
                    role,
                    applicationStatus),
                ResultCode.TokenRefreshedSuccessfully);
        }
        catch
        {
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<string?> ResolveApplicationStatusAsync(
        AppUser user,
        string role,
        CancellationToken cancellationToken)
    {
        if (role != UserRoles.Driver)
            return null;

        var driverAppId = await _db.Drivers
            .AsNoTracking()
            .Where(d => d.Id == user.Id)
            .Select(d => d.DriverApplicationId)
            .FirstOrDefaultAsync(cancellationToken);

        if (driverAppId is Guid appId)
        {
            var status = await _db.Applications
                .AsNoTracking()
                .Where(a => a.Id == appId)
                .Select(a => (DriverApplicationStatus?)a.Status)
                .FirstOrDefaultAsync(cancellationToken);
            return status?.ToString();
        }

        var byEmail = await _db.Applications
            .AsNoTracking()
            .Where(a => a.Email == user.Email)
            .OrderByDescending(a => a.Id)
            .Select(a => (DriverApplicationStatus?)a.Status)
            .FirstOrDefaultAsync(cancellationToken);

        return byEmail?.ToString();
    }
}
