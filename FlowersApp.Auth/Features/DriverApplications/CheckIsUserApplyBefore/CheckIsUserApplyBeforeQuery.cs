using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.DriverApplications.CheckIsUserApplyBefore;

public record CheckIsUserApplyBeforeQuery
(string Email, string NationalIdNumber) : IQuery<bool>;

public class CheckIsUserApplyBeforeQueryHandler(Repository<DriverApplication> repository) : IQueryHandler<CheckIsUserApplyBeforeQuery, bool>
{
    private readonly Repository<DriverApplication> _repository = repository;
    public async Task<RequestResult<bool>> Handle(CheckIsUserApplyBeforeQuery request, CancellationToken cancellationToken)
    {
        var isExist = await _repository.Get(d => d.Email == request.Email
                                        || d.NationalIDNumber == request.NationalIdNumber)
                                        .AnyAsync(cancellationToken);
        if (isExist)
            return RequestResult<bool>.succeeded(isExist, ResultCode.UserAlreadyApplied);
        return RequestResult<bool>.succeeded(isExist, ResultCode.UserNotApplied);
    }
}
