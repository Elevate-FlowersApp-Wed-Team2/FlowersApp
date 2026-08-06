using FlowersApp.Auth.Shared.Response;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Extensions;

public static class ResultCodeLocalizationExtensions
{
    // Only add entries here when the resource key must differ
    // from the ResultCode name, or when multiple codes share one message.
    private static readonly Dictionary<ResultCode, string> Overrides = new()
    {
       
    };

    public static string Localize(this ResultCode code, IStringLocalizer<ErrorMessages> localizer)
        => localizer[Overrides.GetValueOrDefault(code, code.ToString())];

    // Convenience overload for parameterized messages, e.g. "{0} is required."
    public static string Localize(this ResultCode code, IStringLocalizer<ErrorMessages> localizer, params object[] args)
        => localizer[Overrides.GetValueOrDefault(code, code.ToString()), args];
}