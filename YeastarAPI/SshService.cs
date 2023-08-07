using Renci.SshNet;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingNetAU.YeastarAPI
{
    public class SshService
    {
        SimpleTcpClient client;
        Renci.SshNet.SshClient SshClient;
        public Boolean LoggedIn;
        public ObservableStringArray LastResponse { get; private set; }
        string _USERNAME;
        string _PASSWORD;
        bool _LOGINSEND;
        public bool Connected => client.TcpClient.Connected;
        public SshService(string host, int port)
        {
            LastResponse = new ObservableStringArray(16);
            ConnectionInfo ci = new PasswordConnectionInfo(host, "root", "ys123456");
            SshClient = new Renci.SshNet.SshClient(ci);
            SshClient.Connect();

            // If connected
            if (SshClient.IsConnected)
            {
                //var command = SshClient.CreateCommand("asterisk -r");
                // string commandOutput = command.Execute();
                // Console.WriteLine("Command output: \n" + commandOutput);
                // LogCommandOutput(commandOutput);
            }
        }
        public void SetPhoneDivert(GSMPort port, FwdReason reason, FwdMode mode, FwdType type, string number)
        {
            var command = SshClient.CreateCommand(string.Format("asterisk -rx \"gsm send at {0} AT+CCFC={1},{2},\\\\\\\"{3}\\\\\\\",{4},\"", (int)port, (int)reason, (int)mode, number, (int)type));

            string commandOutput = command.Execute();
            LogCommandOutput(commandOutput);

            //  return commandOutput;

        }
        public void SendTextMessage(GSMPort port, string number, string message)
        {
            var command = SshClient.CreateCommand(string.Format("asterisk -rx \"gsm send sms {0} \\\"{1}\\\" \\\"{2}\\\"\"", (int)port, number, message));
            string str = string.Format("asterisk -rx \"gsm send sms {0} \\\"{1}\\\" \\\"{2}\\\"\"", (int)port, number, message);
            string commandOutput = command.Execute();
            LogCommandOutput(commandOutput);

            //  return commandOutput;

        }
        public void QueryPhoneDivert(GSMPort port)
        {
            var command = SshClient.CreateCommand(string.Format("asterisk -rx \"gsm send at {0} AT+CCFC=0,2,,,3\"", (int)port));
            string commandOutput = command.Execute();
            LogCommandOutput(commandOutput);
        }
        public string GetLastMessages()
        {
            var command = SshClient.CreateCommand(string.Format("cat /var/log/yslog/gateway"));
            string commandOutput = command.Execute();
            LogCommandOutput(commandOutput);
            return commandOutput;
        }
        public List<SmsEntry> GetSmsEntries()
        {
            string allLogs = GetLastMessages();
            //ClearLastMessages();
            string[] logEntries = allLogs.Split(new string[] { "[202" }, StringSplitOptions.None);

            List<SmsEntry> smsEntries = new List<SmsEntry>();

            foreach (string entry in logEntries)
            {
                string logEntry = "[202" + entry;
                // Check if the log entry is not empty or whitespace and contains all required fields

                if (!string.IsNullOrWhiteSpace(logEntry) &&
                    logEntry.Contains("sender=") &&
                    logEntry.Contains("header=") &&
                    logEntry.Contains("pdu=") &&
                    logEntry.Contains("text="))
                {
                    smsEntries.Add(new SmsEntry(logEntry));
                }
            }
            return smsEntries;


        }
        public void ClearLastMessages()
        {
            SshClient.RunCommand("echo \"\" > /var/log/yslog/gateway");
        }

        private string SendATCommand()
        {// gsm send at 3 AT+CCFC=0,2,,,3 
            var command = SshClient.CreateCommand("");
            string commandOutput = command.Execute();
            LogCommandOutput(commandOutput);
            if (commandOutput != "Asterisk -rx ")
            {

            }
            return commandOutput;
        }
        private void LogCommandOutput(string message)
        {
            for (int i = LastResponse.Length - 2; i >= 0; i--)
            {
                LastResponse[i + 1] = LastResponse[i];
            }
            LastResponse[0] = message;
        }
    }
    public enum FwdReason { unconditional, mobilebusy, noreply, notreachable, allcallforward, allconditionalcall }
    public enum FwdMode { disable, enable, query, register, erase }
    public enum FwdType { otherwise = 129, incicc = 145 }
    public enum GSMPort { Port1 = 2, Port2 = 3, Port3 = 4, port4 = 5, Port5 = 6, Port6 = 7, Port7 = 8, port8 = 9 }

    public class ObservableStringArray
    {
        public event Action<int, string> Changed;

        private string[] _data;

        public int Length => _data.Length;

        public ObservableStringArray(int length)
        {
            _data = new string[length];
        }

        public string this[int index]
        {
            get
            {
                return _data[index];
            }
            set
            {
                _data[index] = value;
                Changed?.Invoke(index, value);
            }
        }
    }
}
