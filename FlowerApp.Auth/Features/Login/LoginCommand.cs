using FlowerApp.Auth.Common;
using FlowerApp.Auth.Common.Enums;
using FlowerApp.Auth.Domain;
using FlowerApp.Auth.Infrastructure.Auth;
using FlowerApp.Auth.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FlowerApp.Auth.Features.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<ApiResponse<LoginResponse>>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResponse>>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager,IJwtService jwtService,ApplicationDbContext context)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _context = context;
        }

        public async Task<ApiResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return ApiResponse<LoginResponse>.Failure
                            ("Invalid email or password",
                            new List<ErrorCode> { ErrorCode.EmailNotValid }
                            ,StatusCodes.Status401Unauthorized);
            }

            var IsPasswordValid = await _userManager.CheckPasswordAsync(user,request.Password);
            if(IsPasswordValid is false)
            {
                return ApiResponse<LoginResponse>.Failure
                            ("Invalid email or password",
                            new List<ErrorCode> {ErrorCode.PasswordIsWrong}
                            , StatusCodes.Status401Unauthorized);
            }

            var Roles=await _userManager.GetRolesAsync(user);     
            
            if (!Roles.Contains("Customer") && !Roles.Contains("Driver"))
            {
                return ApiResponse<LoginResponse>.Failure
                            ("You are not authorized to access this application.",
                            new List<ErrorCode> { ErrorCode.RoleNotAllowed }
                            , StatusCodes.Status403Forbidden);
            }

            var Role = Roles.FirstOrDefault();

            var Token = _jwtService.GenerateAccessToken(user,Role,user.driverStatus);
            var RefreshToken = _jwtService.GenerateRefreshToken();
            RefreshToken.UserId = user.Id;

            _context.RefreshTokens.Add(RefreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            var Result = new LoginResponse(Token,RefreshToken.Token,600,Role,user.driverStatus.ToString());
           

            return ApiResponse<LoginResponse>.Success(Result);
        }
    }
}
