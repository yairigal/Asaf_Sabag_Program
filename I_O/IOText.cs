using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_O
{
    public class IOText : IOInterface<string>
    {
        public IOText()
        {
        }

        public IEnumerable<string> fileToTweets(string filename, string delim, int count)
        {
            using (StreamReader reader = File.OpenText(filename))
            {
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    yield return line;
                }
            }
        }

        public string tweetToFile(IEnumerable<string> tweets, string change, string delim, int count)
        {
            using (StreamWriter writer = File.CreateText(change))
            {
                foreach (string item in tweets)
                {
                    writer.Write(item);
                    writer.Write("\n");
                }
            }
            return change;
        }
    }
}
