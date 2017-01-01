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
    public interface IOInterface
    {
        /// <summary>
        /// builds a list of tweets (string) from a file.
        /// </summary>
        /// <param name="filename">full path of the file to take the tweet from</param>
        /// <param name="delim">delimeter to devide the tweets by. (if empty, delimitation will be by lines)</param>
        /// <param name="count">how much tweets to take out from the file (from the begining of the file)</param>
        /// <returns>the list of the tweets</returns>
        IEnumerable<object> fileToTweets(string filename, string delim, int count);
        /// <summary>
        /// print the tweet, one by one, into a file.
        /// </summary>
        /// <param name="tweets">list of tweets to be printed</param>
        /// <param name="change">the new file to write the tweets to</param>
        /// <param name="delim">delimeter to put between the tweets</param>
        /// <param name="count">how many tweets from the list to print to the file</param>
        /// <returns>the name of the new file into which the tweets were printed</returns>
        string tweetToFile(IEnumerable<object> tweets, string change, string delim, int count);
    }
}
