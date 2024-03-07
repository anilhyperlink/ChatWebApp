using System;

namespace ChatWebApp.Models
{
    public class MessageModel
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string MessageText { get; set; }
        public string GroupName { get; set; }
    }
}
