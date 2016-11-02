using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ContentFamilies;


namespace PatternFamilies
{
    public class ReachnessLang
    {
        public float difWords;
        public float oneWords;
        public float twoWords;
        public float threeWords;
        public float fourWords;
        public float fiveWords;

        public float difWordsNormalized;
        public float oneWordsNormalized;
        public float twoWordsNormalized;
        public float threeWordsNormalized;
        public float fourWordsNormalized;
        public float fiveWordsNormalized;

        public long wordsCount;
        public static string address;

        public ReachnessLang(string myAddress, long numOfWord = 0)
        {
            address = myAddress;
            if (numOfWord == 0) getNumOfWords();
            else wordsCount = numOfWord;
            init();

        }

        public override string ToString()
        {
            return (difWordsNormalized + "," + oneWordsNormalized + "," + twoWordsNormalized + "," + threeWordsNormalized + "," + fourWordsNormalized + "," + fiveWordsNormalized + ",");
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
        bool checkIfNumber(string s)
        {
            int y = 0;
            return int.TryParse(s, out y);
        }
        public void getNumOfWords()
        {
            StreamReader tr = File.OpenText(address);
            string file= tr.ReadToEnd();
            string[] words = file.Split(' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@');
            
           
            List<SeqString> difWords = (from word in words.Cast<string>()
                                        group word by word into g
                                        select new SeqString { word = g.Key, freq = g.Count() }).Where(x => !x.Equals("") && !checkIfNumber(x.word)).ToList();
            wordsCount = difWords.Sum(x=>x.freq);

        }

        public float UniqueWordCountInFile()
        {
            float uniqueWordCount = 0;
            SortedSet<String> sortedSet = new SortedSet<string>();

            string[] content = File.ReadAllLines(address);
            var words = content.SelectMany(line => line.Split(new char[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries));

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
            difWordsNormalized = (difWords / wordsCount);
        }

        public void wordCountInFile()
        {
            float count = 0;
            SortedSet<String> sortedSet = new SortedSet<string>();


            string[] words = File.ReadAllText(address).ToLower().Split(' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@');


            List<SeqString> difWords = (from word in words.Cast<string>()
                                        group word by word into g
                                        select new SeqString { word = g.Key, freq = g.Count() }).Where(x => !x.Equals("") && !checkIfNumber(x.word)).ToList();

            oneWords=difWords.Where(x => x.freq == 1).Count();
            twoWords=difWords.Where(x => x.freq == 2).Count();
            threeWords=difWords.Where(x => x.freq == 3).Count();
            fourWords=difWords.Where(x => x.freq == 4).Count();
            fiveWords=difWords.Where(x => x.freq == 5).Count();

      
            

            oneWordsNormalized = oneWords / wordsCount;
            twoWordsNormalized = twoWords / wordsCount;
            threeWordsNormalized = threeWords / wordsCount;
            fourWordsNormalized = fourWords / wordsCount;
            fiveWordsNormalized = fiveWords / wordsCount;
        }

    }
}
