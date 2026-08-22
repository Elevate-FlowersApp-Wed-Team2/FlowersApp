namespace FlowersApp.Catalog.Shared.Response
{
    public interface IErrorMessageService
    {
        string Get(ResultCode code);
    }
}
