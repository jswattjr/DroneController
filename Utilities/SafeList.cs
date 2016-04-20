using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class SafeList<T> : List<T>
    {
        private object myLock = new object();

        public new void Add(T obj)
        {
            lock(myLock)
            {
                base.Add(obj);
            }
        }

        public new bool Remove(T obj)
        {
            lock (myLock)
            {
                return base.Remove(obj);
            }
        }
    }
}
