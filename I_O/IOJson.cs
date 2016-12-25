 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace I_O
{
    public class IOJson : IOInterface<JObject>
    {
        public IOJson()
        {
        }

        public IEnumerable<JObject> fileToTweets(string filename, string delim, int count)
        {
            using (StreamReader reader = File.OpenText(filename))
            {
                while (!reader.EndOfStream)
                {
                    yield return JObject.Parse(reader.ReadLine());
                }

            }
        }

        public string tweetToFile(IEnumerable<JObject> tweets, string change, string delim, int count)
        {
            JsonSerializer serializer = new JsonSerializer();
            string afterChange = change;
            using (StreamWriter writer = File.CreateText(afterChange))
            {
                foreach (JObject item in tweets)
                {
                    serializer.Serialize(writer, item, typeof(JObject));
                    writer.Write("\n");
                }
            }
            return afterChange;
        }
    }
}
