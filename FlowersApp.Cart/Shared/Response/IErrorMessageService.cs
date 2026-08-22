namespace FlowersApp.Cart.Shared.Response
{
    public interface IErrorMessageService
    {
        string Get(ResultCode code);
    }
}
