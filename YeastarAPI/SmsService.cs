using SimpleTCP;
using System;
using System.Net;
using System.Net.Sockets;

namespace ThingNetAU.YeastarAPI
{
    public class SmsService
    {
        SimpleTcpClient client;
        public Boolean LoggedIn;
        string _USERNAME;
        string _PASSWORD;
        bool _LOGINSEND;
        public bool Connected => client.TcpClient.Connected; 
        public SmsService(string host, int port, string username ,string password )
        {
            _USERNAME = username;
            _PASSWORD = password;
            client = new SimpleTcpClient();
            client.Connect(host, port);
            
            client.DataReceived += Client_DataReceived;
            while (!Connected || !LoggedIn)
            {
                System.Threading.Thread.Sleep(1000);
            }

            //client.Connect()
        }
        public void Disconnect()
        {
            client.Disconnect();
        }
        public void ShowSpan()
        {
            client.Write(System.Text.Encoding.UTF8.GetBytes("Action: SMSCommand\r\ncommand: gsm show spans\r\n\r\n"));
            Console.WriteLine("Action: SMSCommand\r\ncommand: gsm show spans\r\n\r\n");
        }

        public void SendSMS(int port, string destination, string message)
        {
            client.Write(System.Text.Encoding.UTF8.GetBytes( string.Format("Action: SMSCommand\r\ncommand: gsm send sms {0} {1} \"{2}\" $id\r\n\r\n", port + 1, destination, message)));
            Console.WriteLine( string.Format("Action: SMSCommand\r\ncommand: gsm send sms {0} {1} \"{2}\" $id\r\n\r\n", port + 1, destination, message));
        }
        private void Client_DataReceived(object sender, Message e)
        {
            Console.WriteLine(e.MessageString);
            //PROTOCOL
            if(e.MessageString.Contains("Asterisk"))
            {
                client.Write(System.Text.Encoding.UTF8.GetBytes(string.Format("Action: login\r\nUsername: {0}\r\nSecret: {1}\r\n\r\n", _USERNAME, _PASSWORD)));
                Console.WriteLine(string.Format("Action: login\r\nUsername: {0}\r\nSecret: {1}\r\n\r\n", _USERNAME, _PASSWORD));
                _LOGINSEND = true;
            }
            if(!LoggedIn && _LOGINSEND)
            {
                if (e.MessageString.Contains("Success"))
                {
                    LoggedIn = true;
                }
                if (e.MessageString.Contains("Error"))
                {
                    LoggedIn = false;
                    _LOGINSEND = false;
                }

            }
            switch (e.MessageString)
            {
              //  case 
            }
            //throw new NotImplementedException();
        }
    }
}
