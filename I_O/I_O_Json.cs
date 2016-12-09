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
    class I_O_Json<Jobject> : I_O_Interface<JObject>
    {
        public IEnumerable<JObject> fileToTweets(string filePath)
        {
            using (StreamReader reader = File.OpenText(filePath))
            {
                int i = 1;
                while (!reader.EndOfStream)
                {
                    yield return JObject.Parse(reader.ReadLine());
                    i++;
                }

            }
        }

        public string tweetToFile(IEnumerable<JObject> tweets, string filePath)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter file = File.CreateText(@"5j_71cjJOp_number2.json"))
            {
                foreach (JObject item in tweets)
                {
                    serializer.Serialize(file, item, typeof(JObject));
                    file.Write("\n");
                }
            }
            return filePath;
        }

        //static void Main(string[] args)
        //{
        //    List<JObject> li = new List<JObject>();
        //    foreach (JObject j in file_to_tweets("5j_71cjJOp.json"))
        //    {
        //        li.Add(j);
        //        //Console.WriteLine(j["text"]);
        //    }

        //    tweets_to_file(li);
        //}
    }
}
}
