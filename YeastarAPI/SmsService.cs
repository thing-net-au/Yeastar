//using Renci.SshNet.Messages;
using SimpleTCP;
using System;
using System.Net;
using System.Net.Sockets;

namespace ThingNetAU.YeastarAPI
{
    public class SmsService : IDisposable
    {
        protected SimpleTcpClient client;
        bool Disposing = false;
        public Boolean LoggedIn;
        string _USERNAME;
        string _PASSWORD;
        string _HOST;
        int _PORT;
        bool _LOGINSEND;
        public bool Connected => client.TcpClient.Connected;
        DateTime LastSend;
        public SmsService(string host, int port, string username, string password)
        {
            _HOST = host;
            _PORT = port;
            _USERNAME = username;
            _PASSWORD = password;

            Connect();
        }

        public void Connect()
        {
            client = new SimpleTcpClient();
            client.Connect(_HOST.Trim(), _PORT);
            client.DataReceived += Client_DataReceived;
            Console.WriteLine("RECONNECTING");
            while (!Connected || !LoggedIn)
            {
                System.Threading.Thread.Sleep(1000);
            }
            LastSend = DateTime.Now;

        }

        public void Disconnect()
        {
            client.Disconnect();
        }
        public void ShowSpan()
        {
            client.Write(System.Text.Encoding.UTF8.GetBytes("Action: SMSCommand\r\ncommand: gsm show spans\r\n\r\n"));
#if DEBUG
Console.WriteLine("Action: SMSCommand\r\ncommand: gsm show spans\r\n\r\n");
#endif
        }
        public void KeepAlive()
        {
            if (Connected)
            {
                try
                {
                    TimeSpan t = LastSend - DateTime.Now;
                    if (t.TotalMinutes > 5)
                    {
                        Console.WriteLine("NOOP");
                        LastSend = DateTime.Now;
                        client.Write("\r\n");
                    }
                }
                catch
                {
                    Connect();
                }
            }
            else
            {
                Connect();
            }
        }
        public void SendSMSSpan(int span, string destination, string message, string id = "")
        {
            if(!Connected)
                Connect();

            if (Connected)
            {
                LastSend = DateTime.Now;
                if (id == "") id = Guid.NewGuid().ToString();
                client.Write(System.Text.Encoding.UTF8.GetBytes(string.Format("Action: SMSCommand\r\ncommand: gsm send sms {0} {1} \"{2}\" {3}\r\n\r\n", span, destination, message, id)));
#if DEBUG
            Console.WriteLine(string.Format("Action: SMSCommand\r\ncommand: gsm send sms {0} {1} \"{2}\" {3}\r\n\r\n", span, destination, message, id));
#endif
            }
        }
        public void RegisterCallForward(int span, string destination, GSM.FwdReason reason, GSM.FwdMode mode, GSM.FwdType type)
        {// gsm send at 3 AT+CCFC=0,3,\"+614000000000\",145
            if (!Connected)
                Connect();
            if (Connected)
            {
                LastSend = DateTime.Now;
                client.Write(System.Text.Encoding.UTF8.GetBytes(string.Format("Action: SMSCommand\r\ncommand: gsm send at {0} AT+CCFC={1},{2},\\\"{3}\\\",{4}\r\n\r\n", span, (int)reason, (int)mode, destination, (int)type)));

            }
        }
        public void SendSMS(int port, string destination, string message, string id = "")
        {
            if (!Connected)
                Connect();
            if (Connected)
            {
                LastSend = DateTime.Now;
                if (id == "") id = Guid.NewGuid().ToString();
                SendSMSSpan(port + 1, destination, message, id);

            }
        }
        protected virtual void Client_DataReceived(object sender, Message e)
        {
#if DEBUG
            Console.WriteLine(e.MessageString);
#endif            //PROTOCOL
            if (e.MessageString.Contains("Asterisk"))
            {
                client.Write(System.Text.Encoding.UTF8.GetBytes(string.Format("Action: login\r\nUsername: {0}\r\nSecret: {1}\r\n\r\n", _USERNAME, _PASSWORD)));
#if DEBUG
                Console.WriteLine(string.Format("Action: login\r\nUsername: {0}\r\nSecret: {1}\r\n\r\n", _USERNAME, _PASSWORD));
#endif 
                _LOGINSEND = true;
            }
            if (!LoggedIn && _LOGINSEND)
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
