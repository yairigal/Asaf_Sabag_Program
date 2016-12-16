using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_O
{
    public class IOText : IOAbstract<string>
    {
        public IOText(string filePath):base(filePath)
        {}

        public override IEnumerable<string> fileToTweets(string delim, int count)
        {
            using (StreamReader reader = File.OpenText(filename+extension))
            {
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    yield return line;
                }
            }
        }

        public override string tweetToFile(IEnumerable<string> tweets, string path, string delim, int count)
        {
            throw new NotImplementedException();
        }
    }
}
