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
    public interface IOInterface<T>
    {
        void changeFile(string filePath);

        IEnumerable<T> fileToTweets(string delim, int count);
        string tweetToFile(IEnumerable<T> tweets, string change, string delim, int count);

    }
}
