using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_O
{
    public class IOText : IOInterface
    {
        public IOText()
        {
        }

        public IEnumerable<object> fileToTweets(string filename, string delim, int count)
        {
            if (count < 1)
                count = -1;
            using (StreamReader reader = File.OpenText(filename))
            {
                while(!reader.EndOfStream)
                {
                    if (count == 0)             //so when there were 'count' tweet returned, the loop would be ended.
                        break;
                    string tweet = "";
                    string line = reader.ReadLine();
                    while(!(line == null || line == delim || (line == "" && (delim == "\n" || delim == "\r"))))
                    {
                        tweet += line;
                        if (delim == "")
                            break;
                        tweet += "\n";
                        line = reader.ReadLine();
                    }
                    count--;
                    yield return tweet;
                }
            }
        }

        public string tweetToFile(IEnumerable<object> tweets, string change, string delim, int count)
        {
            if (count < 1)
                count = -1;
            using (StreamWriter writer = File.CreateText(change))
            {
                foreach (string item in tweets)
                {
                    if (count == 0)
                        break;
                    writer.Write(item);
                    if(delim != "" && delim != string.Empty)
                        writer.Write("\n" + delim);
                    writer.Write("\n");
                    count--;
                }
            }
            return change;
        }
    }
}
