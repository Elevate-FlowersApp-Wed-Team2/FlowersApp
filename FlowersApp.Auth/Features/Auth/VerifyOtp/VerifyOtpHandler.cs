using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.Auth.VerifyOtp;

public class VerifyOtpHandler : ICommandHandler<VerifyOtpCommand, VerifyOtpResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordResetOtpService _otpService;
    private readonly ILogger<VerifyOtpHandler> _logger;

    public VerifyOtpHandler(
        UserManager<AppUser> userManager,
        IPasswordResetOtpService otpService,
        ILogger<VerifyOtpHandler> logger)
    {
        _userManager = userManager;
        _otpService = otpService;
        _logger = logger;
    }

    public async Task<RequestResult<VerifyOtpResponse>> Handle(
        VerifyOtpCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            _logger.LogWarning("OTP verification failed for unknown email.");
            return RequestResult<VerifyOtpResponse>.Failure(ResultCode.InvalidOrExpiredOtp);
        }

        var (status, resetToken) = await _otpService.VerifyOtpAsync(user.Id, request.Otp, cancellationToken);
        return status switch
        {
            OtpVerificationStatus.Success => RequestResult<VerifyOtpResponse>.succeeded(
                new VerifyOtpResponse(resetToken!), ResultCode.OtpVerifiedSuccessfully),

            OtpVerificationStatus.Expired => RequestResult<VerifyOtpResponse>.Failure(ResultCode.OtpExpired),

            OtpVerificationStatus.MaxAttemptsExceeded =>
                RequestResult<VerifyOtpResponse>.Failure(ResultCode.OtpMaxAttemptsExceeded),

            _ => RequestResult<VerifyOtpResponse>.Failure(ResultCode.InvalidOrExpiredOtp)
        };
    }
}
