using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMessenger.Model
{
    public class Message
    {
        public string Sender { get; set; }
        public string UserName { get; set; }
        public string Color { get; set; }
        public string MessageData { get; set; }
        public DateTime Time { get; set; }

       
    }
}
