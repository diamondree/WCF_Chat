using System;

namespace WCF_Shared_Library
{
    public static class RepliesFormatService
    {
        public static string MessageFormat(string sender, string message)
        {
            return $"{DateTime.Now.ToLongTimeString()}, {sender}: {message}";
        }

        public static string UserConnectedReply(string username)
        {
            return $"{DateTime.Now.ToLongTimeString()}, {username} connected";
        }

        public static string UserDisconnectedReply(string username)
        {
            return $"{DateTime.Now.ToLongTimeString()}, {username} disconnected";
        }
    }
}