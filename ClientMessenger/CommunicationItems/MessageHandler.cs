using ClientMessenger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClientMessenger.CommunicationItems
{
    public static class MessageHandler
    {
        
        

        public static (string Command,string Sender, string Recipient, string Message) ParseIncomeMessage(string incomeMessage)
        {
            var strings=incomeMessage.Split('|',4);
            (string, string, string,string) parsedString = new();
            parsedString.Item1 = strings[0];
            parsedString.Item2 = strings[1];
            parsedString.Item3 = strings[2];
            parsedString.Item4 = strings[3];
            return parsedString;
        }

        public static string CreateSentMessage(string command,string message) 
        {
            return command + "|" + message;
        }

        public static Message CreateMessageObjectFromString(string message)
        {
            var parsed=ParseIncomeMessage(message);
            Message messageObject = new Message() 
            {  
                Sender= parsed.Sender,
                UserName=parsed.Recipient,
                Color="#252525",
                MessageData=parsed.Message,
                Time=DateTime.Now
            };
            return messageObject;
        }

        public static int GetMessageBytesSize(string message)
        {
            int size=Encoding.UTF8.GetByteCount(message);
            return size;
        }


    }
}
