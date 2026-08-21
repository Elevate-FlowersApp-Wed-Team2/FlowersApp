using FlowersApp.Catalog.Response;
using FlowersApp.Catalog.Resources;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Catalog.Shared.Response
{
    // Resolves a ResultCode to its localized message using the ErrorMessages
   // resx files (ErrorMessages_en.resx / ErrorMessages_ar.resx). Keys match
   // the ResultCode enum member names exactly (e.g. "PasswordMismatch").
public class ErrorMessageService : IErrorMessageService
    {
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public ErrorMessageService(IStringLocalizer<ErrorMessages> localizer)
        {
            _localizer = localizer;
        }

        public string Get(ResultCode code)
        {
            var localized = _localizer[code.ToString()];
            return localized.ResourceNotFound
                ? _localizer["Error_Unexpected"]
                : localized;
        }
    }
}
