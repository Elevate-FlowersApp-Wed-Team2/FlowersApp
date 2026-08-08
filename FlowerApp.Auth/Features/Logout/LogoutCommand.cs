using FlowerApp.Auth.Common;
using FlowerApp.Auth.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowerApp.Auth.Features.Logout
{
    public record LogoutCommand(Guid UserId) : IRequest<ApiResponse<LogoutResponse>>;
    
    
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<LogoutResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public LogoutCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<LogoutResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var activeTokens = await _dbContext.RefreshTokens.Where(t => t.UserId == request.UserId && !t.RevokedAt.HasValue && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            for (var i = 0; i < activeTokens.Count; i++)
            {
                activeTokens[i].RevokedAt = DateTime.UtcNow;
                _dbContext.RefreshTokens.Update(activeTokens[i]).State = EntityState.Modified;
            }

            if (activeTokens.Any())
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return ApiResponse<LogoutResponse>.Success(new LogoutResponse { LoggedOut = true }, "User logged out successfully.");
        }
    }

}
