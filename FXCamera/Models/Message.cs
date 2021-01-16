using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FXCamera.Models
{
    public class Message
    {
        public string _message { get; set; }
        public string _type { get; set; }
        public Message()
        {

        }
        public Message(string message, string type)
        {
            this._message = message;
            this._type = type;
        }
    }
}