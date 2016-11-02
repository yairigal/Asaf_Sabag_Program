using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace porter
{
    public class reachness
    {

        public float difWords;
        public float oneWords;
        public float twoWords;
        public float threeWords;
        public float fourWords;
        public float fiveWords;
        public static string contentFile;
        private string[] content;

        public reachness(string myContent)
        {
            contentFile = myContent;
            content = contentFile.Split(' ');
            init();

        }

        public void init()
        {
            oneWords = 0;
            twoWords = 0;
            threeWords = 0;
            fourWords = 0;
            fiveWords = 0;
            getDifWordsPerAll();
            wordCountInFile();
        }

        public float UniqueWordCountInFile()
        {
            float uniqueWordCount = 0;
            SortedSet<String> sortedSet = new SortedSet<string>();
            var words = content.SelectMany(line => line.Split(new char[] { ' ' , ',', '.'}, StringSplitOptions.RemoveEmptyEntries));
            foreach (string word in words)
            {
                if (!sortedSet.Contains(word.ToLower()))
                {
                    uniqueWordCount++;
                    sortedSet.Add(word.ToLower());
                }
            }
            return uniqueWordCount;
        }

        public void getDifWordsPerAll()
        {
            difWords = UniqueWordCountInFile();
        }

        public void wordCountInFile()
        {
            float count = 0;
            SortedSet<String> sortedSet = new SortedSet<string>();

            var words = content.SelectMany(line => line.Split(new char[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries));

            foreach (string word in words)
            {
                count = 0;
                foreach (string wordTemp in words)
                {
                    if (word.ToLower() == wordTemp.ToLower())
                        count++;
                }
                if (count == 1) oneWords++;
                else if (count == 2) twoWords++;
                else if (count == 3) threeWords++;
                else if (count == 4) fourWords++;
                else if (count == 5) fiveWords++;
            }
            twoWords = twoWords / 2;
            threeWords = threeWords / 3;
            fourWords = fourWords / 4;
            fiveWords= fiveWords / 5;
        }

    }
}
