using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_O
{
    public class I_O_Text : I_O_Interface<string>
    {
        string filename;
        string extension;

        public I_O_Text(string filePath)
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
