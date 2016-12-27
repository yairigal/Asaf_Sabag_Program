using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_O
{
    /// <summary>
    /// singleton factory for the different IOInterface implementation.
    /// </summary>
    class IOFactory
    {
        private static IOFactory factory;

        private IOFactory() { }
        public IOFactory getFacotry()
        {
            if (factory == null)
                factory = new IOFactory();
            return factory;
        }
    }
}
