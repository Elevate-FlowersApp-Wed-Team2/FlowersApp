using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.Driver.SubmitApplication;

public class SubmitApplicationCommandHandler
    : ICommandHandler<SubmitApplicationCommand, SubmitApplicationResponse>
{
    private readonly IPasswordHasher _passwordHasher;

    public SubmitApplicationCommandHandler(
        IDriverApplicationRepository applicationRepository,
        IUserRepository userRepository,
        IDocumentStorageService documentStorageService,
        IPasswordHasher passwordHasher)
    {
        _applicationRepository = applicationRepository;
        _userRepository = userRepository;
        _documentStorageService = documentStorageService;
        _passwordHasher = passwordHasher;
        _validator = new SubmitApplicationCommandValidator();
    }

    public async Task<RequestResult<SubmitApplicationResponse>> Handle(
        SubmitApplicationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate the command
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToList()
                    );
                return RequestResult<SubmitApplicationResponse>.ValidationError(errors);
            }

            // Check for duplicate email, phone, or national ID
            var existingUser = await _userRepository.FindByEmailOrPhoneOrNationalIdAsync(
                request.Email,
                request.PhoneNumber,
                request.NationalIdNumber,
                cancellationToken);

            if (existingUser != null)
            {
                var errors = new Dictionary<string, List<string>>();

                if (existingUser.Email == request.Email)
                    errors.Add("Email", new List<string> { "This email is already registered" });

                if (existingUser.PhoneNumber == request.PhoneNumber)
                    errors.Add("PhoneNumber", new List<string> { "This phone number is already registered" });

                if (existingUser.NationalIdNumber == request.NationalIdNumber)
                    errors.Add("NationalIdNumber", new List<string> { "This National ID is already registered" });

                return RequestResult<SubmitApplicationResponse>.ValidationError(errors);
            }

            // Upload documents to secure storage
            var documentUrls = new List<string>();
            foreach (var document in request.Documents)
            {
                // Validate file size and type again (server-side security)
                if (document.Length > 5 * 1024 * 1024)
                {
                    return RequestResult<SubmitApplicationResponse>.ValidationError(
                        new Dictionary<string, List<string>>
                        {
                            ["Documents"] = new List<string>
                            {
                                $"File '{document.FileName}' exceeds the maximum size of 5MB"
                            }
                        }
                    );
                }

                var extension = Path.GetExtension(document.FileName).ToLowerInvariant();
                if (!new[] { ".jpg", ".jpeg", ".png", ".pdf" }.Contains(extension))
                {
                    return RequestResult<SubmitApplicationResponse>.ValidationError(
                        new Dictionary<string, List<string>>
                        {
                            ["Documents"] = new List<string>
                            {
                                $"File '{document.FileName}' has an invalid format. Allowed formats: JPG, PNG, PDF"
                            }
                        }
                    );
                }

                var documentId = Guid.NewGuid().ToString();
                var sanitizedFileName = $"{documentId}{extension}";
                var storagePath = $"driver-applications/{DateTime.UtcNow:yyyy-MM-dd}/{sanitizedFileName}";

                // Upload file stream directly
                using var fileStream = document.OpenReadStream();
                await _documentStorageService.UploadDocumentAsync(
                    storagePath,
                    fileStream,
                    document.ContentType,
                    cancellationToken);

                documentUrls.Add(storagePath);
            }

            // Hash the password
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // Create the user account with pending status
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                NationalIdNumber = request.NationalIdNumber,
                Address = request.Address,
                PasswordHash = passwordHash,
                Role = "Driver",
                Status = UserStatus.PendingReview,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user, cancellationToken);

            // Create the driver application
            var application = new DriverApplication
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                NationalIdNumber = request.NationalIdNumber,
                LicenseNumber = request.LicenseNumber,
                VehicleType = request.VehicleType,
                VehiclePlateNumber = request.VehiclePlateNumber,
                Status = ApplicationStatus.PendingReview,
                DocumentUrls = documentUrls,
                SubmittedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _applicationRepository.CreateAsync(application, cancellationToken);

            // Return success response
            return RequestResult<SubmitApplicationResponse>.Success(
                new SubmitApplicationResponse(
                    application.Id,
                    ApplicationStatus.PendingReview.ToString()
                )
            );
        }
        catch (Exception ex)
        {
            // Log the exception (implement your logging)
            // _logger.LogError(ex, "Error submitting driver application");

            return RequestResult<SubmitApplicationResponse>.Error(
                "An error occurred while submitting your application. Please try again."
            );
        }
    }
}