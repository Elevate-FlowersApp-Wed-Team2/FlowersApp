using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FlowersApp.Auth.Infrastructure.Authorization;

public class ApprovedDriverHandler : AuthorizationHandler<ApprovedDriverRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ApprovedDriverRequirement requirement)
    {
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
        var applicationStatus = context.User.FindFirst(AuthClaimTypes.ApplicationStatus)?.Value;

        if (role == UserRoles.Driver &&
            applicationStatus == DriverApplicationStatus.Approved.ToString())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
