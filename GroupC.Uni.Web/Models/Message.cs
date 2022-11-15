using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Web.Models
{
    public class Message
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public static Message AddSuccessMessage(string text)
        {
           
            return (new Message()
            {
                Type = MessageType.Success,
                Text = text
            });
        }

        public static Message AddFailedMessage(string text)
        {
            return (new Message()
            {
                Type = MessageType.Error,
                Text = text
            });
        }
    }
}
