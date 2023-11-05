using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMessenger.Model
{
    internal class Message
    {
        public bool IsMine { get; set; }
        public string UserName { get; set; }
        public string Color { get; set; }
        public string MessageData { get; set; }
        public DateTime Time { get; set; }

       
    }
}
