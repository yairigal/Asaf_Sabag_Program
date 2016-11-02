using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace PatternFamilies
{
    public class quantitativeCharacteristicsFamily
    {
        public float charactersCount;
        public float wordsCount;
        public float sentencesCount;
        public float pagesCount;
        public float averageCharacterPerWords;
        public float averageCharacterPerSentences;
        public float averageWordsPerSentences;
        public static string address;

        public quantitativeCharacteristicsFamily(string myAddress) { address = myAddress; init(); }

        public override string ToString()
        {
            return (averageCharacterPerWords + "," + averageCharacterPerSentences + "," + averageWordsPerSentences + ",");
        }

        public void init()
        {
            getNumOfCharacters();
            getNumOfWords();
            getNumOfSentences();
            getNumOfpages();
            getAverageCharacterPerWords();
            getAverageCharacterPerSentences();
            getAverageWordsPerSentences();
        }

        public void getNumOfCharacters()
        {
            using (var sr = new StreamReader(address))
            {
                int s;
                while (((s = sr.Read()) != -1))
                    if (((char)s != ' ') && ((char)s != '.') && ((char)s != ','))
                        charactersCount++;
            }
        }

        public void getNumOfWords()
        {
            TextReader tr = File.OpenText(address);

            wordsCount = 0;

            string line = tr.ReadLine();
            // 
            while (line != null)
            {

                string[] str = line.Split(' ', '.', ',');

                wordsCount += str.Length;
                // check if there is empty words in the sent'. 
                for (int i = 0; i < str.Length; i++)
                    if (str[i] == "") wordsCount--;
                line = tr.ReadLine();
            }
        }

        public void getNumOfSentences()
        {
            TextReader tr = File.OpenText(address);

            sentencesCount = 0;

            string line = tr.ReadLine();

            while (line != null)
            {

                string[] str = line.Split('.');

                sentencesCount += str.Length;
                for (int i = 0; i < str.Length; i++)
                    if (str[i] == "") sentencesCount--;
                line = tr.ReadLine();
            }
        }

        // count pages in the matching pdf file
        public void getNumOfpages()
        {

            using (StreamReader sr = new StreamReader(File.OpenRead(address)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());
                pagesCount = matches.Count;
            }
        }

        public void getAverageCharacterPerWords()
        {
            averageCharacterPerWords = (charactersCount / wordsCount);
        }

        public void getAverageCharacterPerSentences()
        {
            averageCharacterPerSentences = (charactersCount / sentencesCount);
        }

        public void getAverageWordsPerSentences()
        {
            averageWordsPerSentences = (wordsCount / sentencesCount);
        }
    }
}
