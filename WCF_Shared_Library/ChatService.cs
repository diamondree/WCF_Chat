using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace WCF_Shared_Library
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService : IChatService
    {
        public ObservableCollection<string> messages = new ObservableCollection<string>();
        public ObservableCollection<KeyValuePair<string, IChatServiceCallback>> users = new ObservableCollection<KeyValuePair<string, IChatServiceCallback>>();

        public bool Login(string username)
        {
            if (!users.Contains(users.FirstOrDefault(x=>x.Key == username)))
            {
                IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
                users.Add(new KeyValuePair<string, IChatServiceCallback>(username, callback));
                messages.Add(RepliesFormatService.UserConnectedReply(username));
                return true;
            }
            return false;
        }

        public void Logout(string username)
        {
            users.Remove(users.FirstOrDefault(x => x.Key == username));
            messages.Add(RepliesFormatService.UserDisconnectedReply(username));
        }

        public void SendMessageToServer(string msg)
        {
            messages.Add(RepliesFormatService.MessageFormat(users.FirstOrDefault().Key, msg));
        }
    }
}
