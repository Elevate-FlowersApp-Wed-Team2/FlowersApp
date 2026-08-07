using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Shared.Services;

namespace FlowersApp.Auth.Features.DriverDocuments.CreateDocument;

public record CreateDocumentCommand
(
    Guid DriverApplicationId,
    DocumentType DocumentType,
    IFormFile DocumentFile
) : ICommand<CreateDocumentResponse>;

public record CreateDocumentResponse(Guid DocumentId, DocumentType DocumentType, string DocumentUrl);

public class CreateDocumentCommandHandler(Repository<DriverDocument> repository,DocumentService documentService) : ICommandHandler<CreateDocumentCommand, CreateDocumentResponse>
{
    private readonly Repository<DriverDocument> _repository = repository;
    private readonly DocumentService _documentService = documentService;

    public async Task<RequestResult<CreateDocumentResponse>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(request.DocumentFile.FileName).ToLowerInvariant();
        var documentId = Guid.NewGuid().ToString();
        var sanitizedFileName = $"{documentId}{request.DocumentType}{extension}";
        var storagePath = $"driver-applications/{DateTime.UtcNow:yyyy-MM-dd}/{request.DriverApplicationId}/{sanitizedFileName}";

        using var fileStream = request.DocumentFile.OpenReadStream();
        var docPath = await _documentService.UploadDocumentAsync(
            storagePath,
            fileStream,
            cancellationToken);
        var document = new DriverDocument
        {
            ApplicationId = request.DriverApplicationId ,
            FileUrl = docPath,
            Name = sanitizedFileName,
            Id = Guid.NewGuid(),
            Type = request.DocumentType 
        };
        _repository.Add(document);
        return RequestResult<CreateDocumentResponse>.succeeded(
            new CreateDocumentResponse(document.Id, document.Type, document.FileUrl),
            ResultCode.DocumentCreated);
    }
 
}

