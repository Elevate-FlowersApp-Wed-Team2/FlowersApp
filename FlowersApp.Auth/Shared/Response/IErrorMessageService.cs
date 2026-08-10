namespace FlowersApp.Auth.Shared.Response
{
    public interface IErrorMessageService
    {
        string Get(ResultCode code);
    }
}
