using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AssemblyParser.Utilities.Data;

namespace AssemblyParser.Utilities.Memory
{
    /// <summary>
    /// Mem is the base class for a "block" of memory
    /// </summary>
    public class Mem
    {
        public List<int> memory;

        public Mem()
        {

        }

        public void createMemory(int arrayNumber)
        {
            ReadInData array = new ReadInData(arrayNumber);
            memory = array.getData();
        }

        
    }
}
