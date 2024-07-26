using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.controlUnit.Transmission
{
    public class StartParam
    {
        public decimal ColdTmp { get; set; }
        public decimal HotTmp { get; set; }
        public decimal ColdPress { get; set; }
        public decimal HotPress { get; set; }
        public decimal ColdFlow { get; set; }
        public decimal HotFlow { get; set; }
        public string TestProject { get; set; }
    }
}
