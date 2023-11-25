using System;

namespace WCF_Shared_Library
{
    public class ChatServiceCallback : IChatServiceCallback
    {
        public void SendMessageToClient(string msg)
        {
            Console.WriteLine($"Recieved message from server: {msg}");
        }
    }
}
