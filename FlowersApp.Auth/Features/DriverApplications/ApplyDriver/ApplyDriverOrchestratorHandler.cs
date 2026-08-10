using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Features.DriverApplications.CheckIsUserApplyBefore;
using FlowersApp.Auth.Features.DriverApplications.CreateApplication;
using FlowersApp.Auth.Features.DriverDocuments.CreateDocument;
using FlowersApp.Auth.Features.Drivers.CheckDriverExistence;
using FlowersApp.Auth.Features.Vehicles.CheckVehicleExistence;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Shared.Services;
using MediatR;

namespace FlowersApp.Auth.Features.DriverApplications.ApplyDriver;

public class ApplyDriverOrchestratorHandler(
    IMediator mediator, DocumentService documentService, UnitOfWork unitOfWork,
    ILogger<ApplyDriverOrchestratorHandler> logger)
    : ICommandHandler<ApplyDriverOrchestrator, ApplyDriverOrchestratorResponse>
{
    private readonly IMediator _mediator = mediator;
    private readonly DocumentService _documentStorageService = documentService;
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ApplyDriverOrchestratorHandler> _logger = logger;

    public async Task<RequestResult<ApplyDriverOrchestratorResponse>> Handle(
        ApplyDriverOrchestrator request,
        CancellationToken cancellationToken)
    {
        var uploadedPaths = new List<string>();
        try
        {
            var isUserApplyBefore = await _mediator.Send(new CheckIsUserApplyBeforeQuery(request.Email, request.Nid), cancellationToken);
            if (isUserApplyBefore.Result)
                return RequestResult<ApplyDriverOrchestratorResponse>.Failure(ResultCode.UserAlreadyApplied);

            // Check if the driver already exists
            var isDriverExist = await _mediator.Send(new CheckDriverExistenceQuery(request.Email, request.Nid), cancellationToken);
            if (isDriverExist.Result)
                return RequestResult<ApplyDriverOrchestratorResponse>.Failure(ResultCode.DriverIsAlreadyExist);

            // Check if the vehicle exists
            var isVehicleExist = await _mediator.Send(new CheckVehicleExistenceQuery(request.VehicleId), cancellationToken);
            if (!isVehicleExist.Result)
                return RequestResult<ApplyDriverOrchestratorResponse>.Failure(ResultCode.VehicleNotFound);

            if (!Enum.TryParse(request.Gender, true, out Gender gender))
                return RequestResult<ApplyDriverOrchestratorResponse>.Failure(ResultCode.InvalidGender);

            // Create the application with pending status
            var createApplicationResult = await _mediator.Send(new CreateApplicationCommand(
                request.Name,
                request.Email,
                request.Password,
                request.Phone,
                gender,
                request.VehicleId,
                request.VehicleNumber,
                request.LicenceNumber,
                request.Nid,
                request.FcmToken
            ), cancellationToken);

            if (!createApplicationResult.Success)
                return RequestResult<ApplyDriverOrchestratorResponse>.Failure(createApplicationResult.Code);

            // Upload the license and NID documents
            var licenceResult = await _mediator.Send(new CreateDocumentCommand(createApplicationResult.Result
                ,DocumentType.License, request.LicenceImage), cancellationToken);

            if (!licenceResult.Success)
                return RequestResult<ApplyDriverOrchestratorResponse>.Failure(licenceResult.Code);

            uploadedPaths.Add(licenceResult.Result.DocumentUrl);


            var nidResult = await _mediator.Send(new CreateDocumentCommand(createApplicationResult.Result,
                DocumentType.Identity, request.NidImage), cancellationToken);

            if (!nidResult.Success)
            {
                await _documentStorageService.DeleteDocumentAsync(licenceResult.Result.DocumentUrl, cancellationToken);
                return RequestResult<ApplyDriverOrchestratorResponse>.Failure(nidResult.Code);
            }

            uploadedPaths.Add(nidResult.Result.DocumentUrl);

            var affectedRows = await _unitOfWork.SaveChangeAsync(cancellationToken);
            if (affectedRows <= 0)
            {
                foreach (var path in uploadedPaths)
                    await _documentStorageService.DeleteDocumentAsync(path, cancellationToken);
                return RequestResult<ApplyDriverOrchestratorResponse>.Failure(ResultCode.FailedToSubmitApplication);
            }
            // Return success response
            return RequestResult<ApplyDriverOrchestratorResponse>.succeeded(
                new ApplyDriverOrchestratorResponse(
                    createApplicationResult.Result.ToString(),
                    DriverApplicationStatus.Pending.ToString()
                ),ResultCode.ApplicationSubmittedSuccessfully
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting driver application");
            foreach (var path in uploadedPaths) 
               await _documentStorageService.DeleteDocumentAsync(path, cancellationToken);
            return RequestResult<ApplyDriverOrchestratorResponse>.Failure(ResultCode.FailedToSubmitApplication);
        }
    }
}
