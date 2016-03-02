using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class BitwiseOperations
    {
        public static Boolean bitExistsInValues(uint flagBit, uint flagValues)
        {
            if ((flagValues & flagBit) == flagBit)
            {
                return true;
            }
            return false;
        }
    }
}
