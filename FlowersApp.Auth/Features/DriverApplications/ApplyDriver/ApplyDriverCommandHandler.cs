using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Features.Drivers.CheckDriverExistence;
using FlowersApp.Auth.Features.Vehicles.CheckVehicleExistence;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.DriverApplications.SubmitApplication;

public class ApplyDriverCommandHandler(UserManager<Driver> userManager, IMediator mediator, DocumentService documentService, Repository<DriverApplication> repository, ILogger<ApplyDriverCommandHandler> logger)
    : ICommandHandler<ApplyDriverCommand, ApplyDriverResponse>
{
    private readonly UserManager<Driver> userManager = userManager;
    private readonly IMediator _mediator = mediator;
    private readonly DocumentService _documentStorageService = documentService;
    private readonly Repository<DriverApplication> _applicationRepository = repository;
    private readonly ILogger<ApplyDriverCommandHandler> _logger = logger;

    public async Task<RequestResult<ApplyDriverResponse>> Handle(
        ApplyDriverCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check for duplicate email
            var isExistingUser = await _mediator.Send(new CheckDriverExistenceQuery(request.Email, request.Phone), cancellationToken);
            if (isExistingUser.Result)
                return RequestResult<ApplyDriverResponse>.Failure(ResultCode.DriverIsAlreadyExist);

            var isVehicleExist = await _mediator.Send(new CheckVehicleExistenceQuery(request.VehicleId), cancellationToken);
            if (!isVehicleExist.Result)
                return RequestResult<ApplyDriverResponse>.Failure(ResultCode.VehicleNotFound);

            var licenceDocument = await UploadDocumentAsync(request.LicenceImage, "licence", cancellationToken);
            var nidDocument = await UploadDocumentAsync(request.NidImage, "nid", cancellationToken);
            // Hash the password
            var passwordHash = userManager.PasswordHasher.HashPassword(null, request.Password);

            // Create the user account with pending status
            var application = new DriverApplication
            {
                Id = Guid.NewGuid(),
                FullName = request.Name,
                Email = request.Email,
                PhoneNumber = request.Phone,
                NationalIDNumber = request.Nid,
                VehicleID = request.VehicleId,
                HashedPassword = passwordHash,
                Status = DriverApplicationStatus.Pending,
                Documents = new List<DriverDocument> { licenceDocument, nidDocument }
            };

            _applicationRepository.Add(application);
            var effectedRows = await _applicationRepository.SaveChangeAsync(cancellationToken);
            if(effectedRows <= 0)
                return RequestResult<ApplyDriverResponse>.Failure(ResultCode.FailedToSubmitApplication);
   
            // Return success response
            return RequestResult<ApplyDriverResponse>.succeeded(
                new ApplyDriverResponse(
                    application.Id.ToString(),
                    application.Status.ToString()
                ),ResultCode.ApplicationSubmittedSuccessfully
            );
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Error submitting driver application");

            return RequestResult<ApplyDriverResponse>.Failure(ResultCode.FailedToSubmitApplication);
        }
    }

    private async Task<DriverDocument> UploadDocumentAsync(IFormFile document, string documentType ,CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(document.FileName).ToLowerInvariant();
        var documentId = Guid.NewGuid().ToString();
        var sanitizedFileName = $"{documentId}{extension}";
        var storagePath = $"driver-applications/{DateTime.UtcNow:yyyy-MM-dd}/{documentType}/{sanitizedFileName}";
        // Upload file stream directly
        using var fileStream = document.OpenReadStream();
        await _documentStorageService.UploadDocumentAsync(
            storagePath,
            fileStream,
            document.ContentType,
            cancellationToken);
        return new DriverDocument
        {
            FileUrl = storagePath,
            Name = sanitizedFileName,
            Id = Guid.NewGuid(),
            Type = documentType switch
            {
                "licence" => DocumentType.License,
                "nid" => DocumentType.Identity,
                _ => DocumentType.Other
            },
        };
    }
}
