using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I_O;

namespace Normalization
{
    class normalizer
    {
        I_O_Json jFile;

        string dirToBeNormal = "";
        string dirForTheNormal = "";

        public normalizer(string dir, string type, string dst = "")
        {
            dirToBeNormal = dir;
            if (dst == "")
                dirForTheNormal = dirToBeNormal + "normalaized";
        }
    }
}
