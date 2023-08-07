using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace ConsTest
{
    internal class Program
    {    
      static   ThingNetAU.YeastarAPI.SMSServiceListener s = new ThingNetAU.YeastarAPI.SMSServiceListener("172.18.2.6", 5038, "sms", "careyssms");
      
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            s.ValidSmsReceived += S_ValidSmsReceived;
            Console.Read();
        }

        private static void S_ValidSmsReceived(ThingNetAU.YeastarAPI.SmsEvent obj)
        {
            if (obj.Content.ToLower().StartsWith("activate"))
            {
                Console.WriteLine("User invoking activation");
                if(obj.Sender == "+61400000000")
                {
                    Console.WriteLine("authorised");
                    try
                    {
                        s.RegisterCallForward(obj.GsmSpan, obj.Sender, ThingNetAU.GSM.FwdReason.unconditional, ThingNetAU.GSM.FwdMode.register, ThingNetAU.GSM.FwdType.incicc);
                        s.SendSMSSpan(obj.GsmSpan, obj.Sender, "Activation Successful. Please reply STOP to deactivate.");

                    }
                    catch (Exception ex)
                    {
                        s.SendSMSSpan(obj.GsmSpan, obj.Sender, "Activation Unsuccessful.");

                    }
                }

            }
            else
            {
                Console.WriteLine("Message received but invalid.");
            }
        }
    }
}