using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace I_O
{
    /// <summary>
    /// Interface for reading and writing from/to the different tweets files.
    /// </summary>
    public abstract class IOAbstract<T>
    {
        protected string filename;
        protected string extension;

        public IOAbstract(string filen)
        {
            if (filen.Length > 0)
                changeFile(filen);
        }
        public void changeFile(string filePath)
        {
            filename = Path.GetFileNameWithoutExtension(filePath);
            extension = Path.GetExtension(filePath);
        }

        public abstract IEnumerable<T> fileToTweets(string delim, int count);
        public abstract string tweetToFile(IEnumerable<T> tweets, string change, string delim, int count);

    }
}
