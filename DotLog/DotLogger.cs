using System;
using System.Collections.Generic;

namespace DotLog
{
    public abstract class DotLogger
    {
        protected readonly Object threadLock = new object();
        public virtual void Log(string message) { }
        public virtual void Log(Dictionary<string, string> propertyDictionary) { }
    }
}
