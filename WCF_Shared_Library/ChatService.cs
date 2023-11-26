using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace WCF_Shared_Library
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService : IChatService
    {
        private KeyValuePair<string, IChatServiceCallback> _user;
        public ObservableCollection<string> messages = new ObservableCollection<string>();
        
        public bool Login(string username)
        {
            if (_user.Key == null)
            {
                IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
                _user = new KeyValuePair<string, IChatServiceCallback>(username, callback);
                return true;
            }
            return false;
        }

        public void Logout(string username)
        {
            if (_user.Key == username)
                _user = new KeyValuePair<string, IChatServiceCallback>();
        }

        public void SendMessageToServer(string msg)
        {
            messages.Add($"Recieved message from {_user.Key}: {msg}");
            Console.WriteLine($"Recieved message from {_user.Key}: {msg}");
        }
    }
}
