using System.ServiceModel;

namespace WCF_Shared_Library
{
    public interface IChatServiceCallback
    {
        [OperationContract]
        void SendMessageToClient(string msg);

        [OperationContract]
        void NotifyOnServerClosing();
    }
}
