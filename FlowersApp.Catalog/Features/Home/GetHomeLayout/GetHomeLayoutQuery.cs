using FloweryApp.Api.Common.Contracts;
using MediatR;

namespace FloweryApp.Api.Features.Home.GetHomeLayout;

/// <summary>GET /home/layout — the store-aware, backend-driven Home screen composition (AC1, AC8).</summary>
public sealed record GetHomeLayoutQuery(string AcceptLanguage, int? StoreId)
    : IRequest<OperationResult<IReadOnlyList<HomeLayoutSectionDto>>>;
