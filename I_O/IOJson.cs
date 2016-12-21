﻿ using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace I_O
{
    public class IOJson : IOAbstract<JObject>
    {
        public IOJson(string filen) :base(filen){}

        public override IEnumerable<JObject> fileToTweets(string delim, int count)
        {
            using (StreamReader reader = File.OpenText(filename + extension))
            {
                while (!reader.EndOfStream)
                {
                    yield return JObject.Parse(reader.ReadLine());
                }

            }
        }

        public override string tweetToFile(IEnumerable<JObject> tweets, string change, string delim, int count)
        {
            JsonSerializer serializer = new JsonSerializer();
            string afterChange = filename + "_" + change + extension;
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