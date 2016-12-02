using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace I_O
{
    interface I_O_Interface
    {
        FileStream open_file(string path);
    }
}
