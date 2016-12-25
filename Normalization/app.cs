using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normalization
{
    class app
    {
        public static void Main()
        {
            normalizer norm = new normalizer("C:\\Users\\user\\Desktop\\test\\alt.atheism", "");
            IDictionary<NormaliztionMethods, bool> flags = new Dictionary<NormaliztionMethods, bool> ();
            flags.Add(NormaliztionMethods.All_Capitals, true);
            flags.Add(NormaliztionMethods.All_Lowercase, false);
            flags.Add(NormaliztionMethods.No_HTML_Tags, false);
            flags.Add(NormaliztionMethods.No_Punctuation, false);

            norm.Normalize(flags);
        }
    }
}
