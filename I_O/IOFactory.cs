using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;

namespace I_O
{
    /// <summary>
    /// singleton factory for the different IOInterface implementation.
    /// </summary>
    public class IOFactory
    {
        private static IOInterface factory;

        private IOFactory() { }
        public static IOInterface getFacotry(IO_DataType type)
        {
            if (factory == null)
                if (type == IO_DataType.Json)
                    factory = new IOJson();
                else
                    factory = new IOText();

            return factory;
        }
    }
}
