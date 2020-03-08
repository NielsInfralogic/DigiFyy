using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class Message
    {
        public int MessageID { get; set; } = 0;
        public string MessageText { get; set; } = "";
        public string SenderName { get; set; } = "";
        public DateTime MessageTime { get; set; } = DateTime.MinValue;
        public string LinkUrl { get; set; } = "";
        public bool IsRead { get; set; } = false;

    }

    public class MessagesResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public List<Message> Messages { get; set; } = null;
    }

    public class MarkMessageRequest
    {
        public string UID { get; set; } = "";
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";
        public List<int> MessageIDList { get; set; } = new List<int>();
    }

}


