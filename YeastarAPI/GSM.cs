using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingNetAU
{
    public class GSM
    {
        public enum FwdReason { unconditional, mobilebusy, noreply, notreachable, allcallforward, allconditionalcall }
        public enum FwdMode { disable, enable, query, register, erase }
        public enum FwdType { otherwise = 129, incicc = 145 }
        public enum GSMPort { Port1 = 2, Port2 = 3, Port3 = 4, port4 = 5, Port5 = 6, Port6 = 7, Port7 = 8, port8 = 9 }

    }
}
