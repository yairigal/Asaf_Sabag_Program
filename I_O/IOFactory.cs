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
                switch (type)
                {
                    case IO_DataType.Text:
                        factory = new IOText();
                        break;
                    case IO_DataType.Json:
                        factory = new IOJson();
                        break;
                    default:
                        factory = new IOText();
                        break;
                }

            return factory;
        }
    }
}
