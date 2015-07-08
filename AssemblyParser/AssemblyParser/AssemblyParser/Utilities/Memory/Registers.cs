using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyParser.Utilities.Memory
{
    class Registers
    {
        public Dictionary<string, int> Reg;

        public Registers()
        {
            Reg = new Dictionary<string, int>();
            Reg.Add("$zero", 0);
            Reg.Add("$at", 0);
            Reg.Add("$v0", 0);
            Reg.Add("$v1", 0);
            Reg.Add("$a0", 0);
            Reg.Add("$a1", 0);
            Reg.Add("$a2", 0);
            Reg.Add("$a3", 0);
            Reg.Add("$t0", 0);
            Reg.Add("$t1", 0);
            Reg.Add("$t2", 0);
            Reg.Add("$t3", 0);
            Reg.Add("$t4", 0);
            Reg.Add("$t5", 0);
            Reg.Add("$t6", 0);
            Reg.Add("$t7", 0);
            Reg.Add("$s0", 0);
            Reg.Add("$s1", 0);
            Reg.Add("$s3", 0);
            Reg.Add("$s4", 0);
            Reg.Add("$s5", 0);
            Reg.Add("$s6", 0);
            Reg.Add("$s7", 0);
            Reg.Add("$t8", 0);
            Reg.Add("$t9", 0);
            Reg.Add("$k0", 0);
            Reg.Add("$k1", 0);
            Reg.Add("$gp", 0);
            Reg.Add("$sp", 0);
            Reg.Add("$fp", 0);
            Reg.Add("$ra", 0);
        }
    }
}
