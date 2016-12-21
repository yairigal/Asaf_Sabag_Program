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
        protected string filename;
        protected string extension;

        public IOText(string filen)
        {
            if (filen.Length > 0)
                changeFile(filen);
        }

        public void changeFile(string filePath)
        {
            filename = Path.GetFileNameWithoutExtension(filePath);
            extension = Path.GetExtension(filePath);
        }

        public IEnumerable<string> fileToTweets(string delim, int count)
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

        public string tweetToFile(IEnumerable<string> tweets, string path, string delim, int count)
        {
            throw new NotImplementedException();
        }
    }
}
