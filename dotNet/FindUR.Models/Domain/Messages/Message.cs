using Sabio.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Messages
{
    public class Message
    {
        public Message() { }
        public Message(MessageAddRequest request)
        {
            MessageText = request.Message;
            Subject = request.Subject;
            RecipientId = request.RecipientId;
            SenderId = request.SenderId;
            IsFile = request.IsFile;
            DateSent = request.DateSent;
        }
        public int Id { get; set; }
        public string MessageText { get; set; }
        public string Subject { get; set; }
        public int RecipientId { get; set; }
        public int SenderId { get; set; }        
        public Typer Sender { get; set; }
        public Typer Recipient { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime DateRead { get; set; }
        public bool IsFile { get; set; }
    }
}
