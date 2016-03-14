using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class BitwiseOperations
    {
        public static Boolean bitExistsInValues(int flagBit, int flagValues)
        {
            if ((flagValues & flagBit) == flagBit)
            {
                return true;
            }
            return false;
        }

        public static List<T> parseBitValues<T>(byte flagValues)
        {
            List<T> retVals = new List<T>();
            Type genericType = typeof(T);
            if (genericType.IsEnum)
            {
                // parse bit masks into lists
                IEnumerable<T> values = EnumValues.GetValues<T>();
                foreach (T value in values)
                {
                    if (bitExistsInValues(enumToInt<T>(value), flagValues))
                    {
                        retVals.Add(value);
                    }
                }
            }
            return retVals;
        }

        public static int enumToInt<T>(T value)
        {
            Enum eValue = Enum.Parse(typeof(T), value.ToString()) as Enum;
            return Convert.ToInt32(eValue);
        }
    }
}
