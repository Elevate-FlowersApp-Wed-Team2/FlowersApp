using FlowerApp.Auth.Common.Enums;
using FlowerApp.Auth.Infrastructure.Auth.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FlowerApp.Auth.Infrastructure.Auth.Authorization.Handlers
{
    public class ApprovedDriverHandler
        : AuthorizationHandler<ApprovedDriverRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ApprovedDriverRequirement requirement)
        {
            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
            var applicationDriverStatus = context.User.FindFirst("applicationDriverStatus")?.Value;

            if (role == UserRole.Driver.ToString() &&
                applicationDriverStatus == ApplicationDriverStatus.Approved.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}