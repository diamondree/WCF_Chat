using System.ServiceModel;

namespace WCF_Shared_Library
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        [OperationContract]
        bool Login(string username);


        [OperationContract(IsOneWay = true)]
        void SendMessageToServer(string msg);


        [OperationContract(IsOneWay = true)]
        void Logout(string username);
    }
}
