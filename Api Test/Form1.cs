using System.Diagnostics.Eventing.Reader;
using ThingNetAU.YeastarAPI;
namespace Api_Test
{
    public partial class Form1 : Form
    {
        SshService sshService;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++) { listBox1.Items.Add(""); }
            for (int i = 2; i < 10; i++) { comboBox1.Items.Add(((GSMPort)i).ToString()); }
            comboBox1.SelectedIndex = 0;
            sshService = new SshService("172.18.2.6", 22);
            sshService.LastResponse.Changed += LastResponse_Changed;
        }

        private void LoadMessages()
        {
            for (int i = 0; i < sshService.LastResponse.Length; i++)
            {
                listBox1.Items[i] = sshService.LastResponse[i] ?? "";
            }

        }

        private void LastResponse_Changed(int arg1, string arg2)
        {
            listBox1.Items[arg1] = arg2 ?? "";

        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            LoadMessages();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            sshService.QueryPhoneDivert(getPort(comboBox1.Text));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sshService.SetPhoneDivert(getPort(comboBox1.Text), FwdReason.unconditional, FwdMode.register, FwdType.incicc, textBox1.Text);

        }
        public GSMPort getPort(string port)
        {
            for (int i = 2; i < 10; i++)
            {
                if (port == ((GSMPort)i).ToString())
                {
                    return (GSMPort)i;
                }
            }
            return GSMPort.Port1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                sshService.SendTextMessage(getPort(comboBox1.Text), textBox2.Text, string.Format("{0} {1}", i, textBox3.Text));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            string allLogs =  sshService.GetLastMessages();
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

            foreach (var entry in smsEntries)
            {
                Console.WriteLine(entry.Text);
            }


        }
    }
}