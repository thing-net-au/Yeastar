using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ThingNetAU.YeastarAPI
{
    public class SMSServiceListener : SmsService
    {
        // Define an event to be called when a valid SMS is received
        public event Action<SmsEvent> ValidSmsReceived;

        public SMSServiceListener(string host, int port, string username, string password) : base(host, port, username, password)
        {

        }

        protected override void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            base.Client_DataReceived(sender, e); // Call base implementation if needed

            // parse and validate message
             SmsEvent smsEvent = SmsEvent.FromString(e.MessageString);
            if (smsEvent.IsValidMessage() || smsEvent.IsValidStatus())
            {
                // If valid, invoke the event.
                ValidSmsReceived?.Invoke(smsEvent);
            }
        }

    }
    public class SmsEvent
    {
        public string Event { get; set; }
        public string Privilege { get; set; }
        public string ID { get; set; }
        public int GsmSpan { get; set; }
        public string Sender { get; set; }
        public DateTime Recvtime { get; set; }
        public int Index { get; set; }
        public int Status { get; set; }
        public int Total { get; set; }
        public string Smsc { get; set; }
        public string Content { get; set; }
        public bool IsValidMessage()
        {
            // Check if the sender number is valid.
            if (string.IsNullOrEmpty(this.Sender) || !Regex.IsMatch(this.Sender, @"^\+\d{10,15}$"))
            {
                return false;
            }

            // Check if the SMS content is valid.
            if (string.IsNullOrEmpty(this.Content))
            {
                return false;
            }


            // If all checks passed, the message is valid.
            return true;
        }
        public bool IsValidStatus()
        {
            // Check if the sender number is valid.
            if (string.IsNullOrEmpty(this.ID))
            {
                return false;
            }

            // Check if the SMS content is valid.
            if (string.IsNullOrEmpty(this.Status.ToString()))
            {
                return false;
            }


            // If all checks passed, the message is valid.
            return true;
        }

        public static SmsEvent FromString(string data)
        {
            var smsEvent = new SmsEvent();
            var lines = data.Split('\n');

            foreach (string line in lines)
            {
                string[] parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
                if (parts.Length >= 2) 
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    switch (key)
                    {
                        case "Event":
                            smsEvent.Event = value;
                            break;
                        case "Privilege":
                            smsEvent.Privilege = value;
                            break;
                        case "ID":
                            smsEvent.ID = value;
                            break;
                        case "GsmSpan":
                            smsEvent.GsmSpan = int.Parse(value);
                            break;
                        case "Sender":
                            smsEvent.Sender = value;
                            break;
                        case "Recvtime":
                            smsEvent.Recvtime = DateTime.Parse(value);
                            break;
                        case "Index":
                            smsEvent.Index = int.Parse(value);
                            break;
                        case "Total":
                            smsEvent.Total = int.Parse(value);
                            break;
                        case "Smsc":
                            smsEvent.Smsc = value;
                            break;
                        case "Content":
                            smsEvent.Content = value;
                            break;
                        case "Status":
                            smsEvent.Status = int.Parse(value);
                            break;
                    }
                }
            }

            return smsEvent;
        }
    }

    // Usage:
 

}
