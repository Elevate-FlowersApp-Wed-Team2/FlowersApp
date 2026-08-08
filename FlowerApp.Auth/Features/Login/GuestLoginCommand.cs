using FlowerApp.Auth.Common;
using FlowerApp.Auth.Domain;
using FlowerApp.Auth.Infrastructure.Auth;
using FlowerApp.Auth.Infrastructure.Persistence;
using MediatR;

namespace FlowerApp.Auth.Features.Login
{
    public record GuestLoginCommand
        : IRequest<ApiResponse<LoginResponse>>;

    public class GuestLoginCommandHandler : IRequestHandler<GuestLoginCommand, ApiResponse<LoginResponse>> 
    { 
        private readonly IJwtService _jwtService; 
        private readonly ApplicationDbContext _context; 
        public GuestLoginCommandHandler(IJwtService jwtService, ApplicationDbContext context) 
        { _jwtService = jwtService; _context = context; } 
        public async Task<ApiResponse<LoginResponse>> Handle(GuestLoginCommand request, CancellationToken cancellationToken) 
        { // 1. Create Guest Session
          var guest = new GuestSession();
            _context.GuestSessions.Add(guest); 
            // 2. Generate Guest Access Token
            var accessToken = _jwtService.GenerateGuestAccessToken(guest.Id); 
            // 3. Generate Refresh Token
            var (rawToken, refreshToken) = _jwtService.GenerateRefreshToken();
            // 4. Associate Refresh Token with Guest
            refreshToken.UserId = null; 
            refreshToken.GuestSessionId = guest.Id; 
            _context.RefreshTokens.Add(refreshToken); 
            // 5. Save Guest + RefreshToken
            await _context.SaveChangesAsync(cancellationToken); 
            // 6. Create Response
            var result = new LoginResponse( 
                accessToken, 
                rawToken, 
                _jwtService.AccessTokenExpirationInSeconds, 
                "Guest", 
                null ); 
            return ApiResponse<LoginResponse>.Success(result); 
        } 
    }
}