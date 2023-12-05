using System.Collections.ObjectModel;

namespace WCF_Shared_Library
{
    public class ChatServiceCallback : IChatServiceCallback
    {
        public ObservableCollection<string> ServerMessages = new ObservableCollection<string>();
        public ObservableCollection<string> NotifyMessages = new ObservableCollection<string>();

        public void SendMessageToClient(string msg)
        {
            ServerMessages.Add(RepliesFormatService.MessageFormat("Server", msg));
        }

        public void NotifyOnServerClosing()
        {
            NotifyMessages.Add(RepliesFormatService.MessageFormat("System", "Connection to server has been terminated"));
        }
    }
}
