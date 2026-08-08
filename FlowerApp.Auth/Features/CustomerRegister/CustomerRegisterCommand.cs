using FlowerApp.Auth.Common;
using FlowerApp.Auth.Common.Enums;
using FlowerApp.Auth.Domain;
using FlowerApp.Auth.Features.CustomerRegister.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FlowerApp.Auth.Features.CustomerRegister
{
    public record CustomerRegisterCommand(string Fname, string Lname, string Email, string Phone, string Gender, string Password, string ConfirmPassword)
        :IRequest<ApiResponse<CustomerRegisterResponse>>;

    public class CustomerRegisterCommandHandler:IRequestHandler<CustomerRegisterCommand,ApiResponse<CustomerRegisterResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerRegisterCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<CustomerRegisterResponse>> Handle(CustomerRegisterCommand request,CancellationToken cancellationToken)
        {
            var email = request.Email?.Trim().ToLowerInvariant();
            var phone = request.Phone?.Trim();

            var validationErrors = ValidateRequest(request, email, phone);

            if (validationErrors.Count > 0)
            {
                return ApiResponse<CustomerRegisterResponse>.Failure("Validation failed",
                    validationErrors,
                    StatusCodes.Status400BadRequest);
            }

            var existingUserByEmail =
                await _userManager.FindByEmailAsync(email!);

            if (existingUserByEmail is not null)
            {
                return ApiResponse<CustomerRegisterResponse>.Failure("Email already registered",
                    [ErrorCode.EmailAlreadyRegistered],
                    StatusCodes.Status409Conflict);
            }

            var existingUserByPhone =
                await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phone,cancellationToken);

            if (existingUserByPhone is not null)
            {
                return ApiResponse<CustomerRegisterResponse>.Failure("Phone number already registered",
                    [ErrorCode.PhoneAlreadyRegistered],
                    StatusCodes.Status409Conflict);
            }

            Enum.TryParse<Gender>(request.Gender,true,out var gender);

            var user = new ApplicationUser
            {
                FirstName = request.Fname.Trim(),
                LastName = request.Lname.Trim(),
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Gender = gender,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createResult =await _userManager.CreateAsync(user,request.Password);

            if (!createResult.Succeeded)
            {
                return ApiResponse<CustomerRegisterResponse>.Failure("Registration failed",
                    [ErrorCode.RegistrationFailed],
                    StatusCodes.Status400BadRequest);
            }

            var roleResult =
                await _userManager.AddToRoleAsync(user,"Customer");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return ApiResponse<CustomerRegisterResponse>.Failure("Registration failed",
                    [ErrorCode.RegistrationFailed],
                    StatusCodes.Status400BadRequest);
            }

            return ApiResponse<CustomerRegisterResponse>.Success(new CustomerRegisterResponse(user.Id),
                "Registration successful",
                StatusCodes.Status201Created);
        }

        private static List<ErrorCode> ValidateRequest(CustomerRegisterCommand request,string? email,string? phone)
        {
            var errors = new List<ErrorCode>();

            if (!IsValidFullName(request.Fname, request.Lname))
            {
                errors.Add(ErrorCode.InvalidFullName);
            }

            if (!IsValidEmail(email))
            {
                errors.Add(ErrorCode.InvalidEmail);
            }

            if (!IsValidEgyptianPhone(phone))
            {
                errors.Add(ErrorCode.InvalidPhoneNumber);
            }

            if (!IsValidGender(request.Gender))
            {
                errors.Add(ErrorCode.InvalidGender);
            }

            if (!IsValidPassword(request.Password))
            {
                errors.Add(ErrorCode.InvalidPassword);
            }

            if (!IsPasswordMatching(request.Password,request.ConfirmPassword))
            {
                errors.Add(ErrorCode.PasswordMismatch);
            }

            return errors;
        }

        private static bool IsValidFullName(string? firstName,string? lastName)
        {
            return !string.IsNullOrWhiteSpace(firstName)&& !string.IsNullOrWhiteSpace(lastName);
        }

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(email);

                return mailAddress.Address.Equals(email,StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidEgyptianPhone(string? phone)
        {
            return !string.IsNullOrWhiteSpace(phone)&& Regex.IsMatch(phone,@"^01[0125]\d{8}$");
        }

        private static bool IsValidGender(string? gender)
        {
            return !string.IsNullOrWhiteSpace(gender)&& Enum.TryParse<Gender>(gender,true,out _);
        }

        private static bool IsValidPassword(string? password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            return password.Length >= 6 && password.Any(char.IsUpper)&& password.Any(char.IsDigit);
        }

        private static bool IsPasswordMatching(string? password,string? confirmPassword)
        {
            return !string.IsNullOrEmpty(password)&& string.Equals(password,confirmPassword,StringComparison.Ordinal);
        }
    }
}
