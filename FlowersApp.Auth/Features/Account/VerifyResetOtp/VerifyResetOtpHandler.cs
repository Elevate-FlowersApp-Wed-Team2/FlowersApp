using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;

namespace FlowersApp.Auth.Features.Account.VerifyResetOtp
{
    public class VerifyResetOtpHandler : IRequestHandler<VerifyResetOtpCommand, RequestResult<VerifyResetOtpResponse>>
    {
        private const int ResetTokenTtlSeconds = 600;

        private readonly IPasswordResetOtpService _otpService;

        public VerifyResetOtpHandler(IPasswordResetOtpService otpService)
        {
            _otpService = otpService;
        }

        public async Task<RequestResult<VerifyResetOtpResponse>> Handle(
            VerifyResetOtpCommand request, CancellationToken cancellationToken)
        {
            var outcome = await _otpService.ValidateOtpAsync(request.Email, request.Otp);

            switch (outcome.Result)
            {
                case OtpValidation.Valid:
                    var token = await _otpService.IssueResetTokenAsync(request.Email);
                    return RequestResult<VerifyResetOtpResponse>.succeeded(
                        new VerifyResetOtpResponse(token, ResetTokenTtlSeconds, outcome.AttemptsRemaining),
                        ResultCode.OtpSent);

                case OtpValidation.InvalidCode:
                    return RequestResult<VerifyResetOtpResponse>.Failure(
                        new VerifyResetOtpResponse(string.Empty, 0, outcome.AttemptsRemaining),
                        ResultCode.OtpInvalid);

                case OtpValidation.Expired:
                    return RequestResult<VerifyResetOtpResponse>.Failure(ResultCode.OtpExpired);

                case OtpValidation.MaxAttemptsExceeded:
                    return RequestResult<VerifyResetOtpResponse>.Failure(ResultCode.OtpMaxAttemptsExceeded);

                default:
                    return RequestResult<VerifyResetOtpResponse>.Failure(ResultCode.OtpInvalid);
            }
        }
    }
}
