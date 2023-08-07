using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingNetAU.YeastarAPI
{
    public class SmsEntry
    {
        public DateTime TimeStamp { get; set; }
        public string LogLevel { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public string Header { get; set; }
        public string Id { get; set; }
        public int CurrentNum { get; set; }
        public int TotalNum { get; set; }
        public string Pdu { get; set; }
        public string Text { get; set; }

        public SmsEntry(string block)
        {
            string[] parts = block.Split('\n');

            // Parse timestamp, log level, source, and message
            var match = Regex.Match(parts[0], @"\[(.*?)\]\s(.*?)\[(.*?)\]\s(.*?)\:");
            if (match.Success)
            {
                TimeStamp = DateTime.Parse(match.Groups[1].Value);
                LogLevel = match.Groups[2].Value;
                Source = match.Groups[3].Value;
                Message = match.Groups[4].Value;
            }

            // Parse sender
            foreach(string part in parts)
            {
                if(string.IsNullOrEmpty(part)) continue;
                match = Regex.Match(part, @"sender=(.*)");
                if (match.Success && (Sender is null))
                {
                    Sender = match.Groups[1].Value;
                    continue;
                }

                // Parse header, id, current number and total number
                match = Regex.Match(part, @"header=(.*),id=(.*),curnum=(.*),totalnum=(.*)");
                if (match.Success && (Header is null))
                {
                    Header = match.Groups[1].Value;
                    Id = match.Groups[2].Value;
                    CurrentNum = int.Parse(match.Groups[3].Value);
                    TotalNum = int.Parse(match.Groups[4].Value);
                    continue;
                }

                // Parse pdu
                match = Regex.Match(part, @"pdu=(.*)");
                if (match.Success && (Pdu is null))
                {
                    Pdu = match.Groups[1].Value;
                    continue;
                }

                // Parse text
                match = Regex.Match(part, @"text=(.*)");
                if (match.Success && (Text is null))
                {
                    Text = match.Groups[1].Value;
                    continue;
                }
            }
  
        }
    }
}

