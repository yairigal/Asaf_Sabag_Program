using System;
using System.Collections.Generic;
using System.Linq;


namespace Normalization
{
    public class Normalizer
    {
        //the dictionarys
        //const string missplellingPath = @"../../../misspelling.txt";
        //const string abbrevationsPath = @"../../../abbrevations-lexicon.txt";
        //const string slangPath = @"../../../slang.txt";
        //const string stopWordsPath = @"../../../StopWords.txt";
        //const string onomatopoeia = @"../../../onomatopoeia.txt";

        //new directories

        #region New Directories
        const string missplellingPath = @"misspelling.txt";
        const string abbrevationsPath = @"abbrevations-lexicon.txt";
        const string slangPath = @"slang.txt";
        const string stopWordsPath = @"StopWords.txt";
        const string onomatopoeia = @"onomatopoeia.txt";
        #endregion

        #region Dictionaries
        IDictionary<string, string> slangDictionary = new Dictionary<string, string>();
        IDictionary<string, string> abbrevationDictionary = new Dictionary<string, string>();
        IDictionary<string, string> misspellingDictionary = new Dictionary<string, string>();

        List<string> stopWords = new List<string>();

        string punctuationString = ".,;()[]{}:-_?!'\\\"/@#$%^&`~";
        #endregion

        #region Normalization Functions
        /// <summary>
        /// translate some common misspelled words in a string to their real meaning
        /// </summary>
        /// <param name="str">string to translate</param>
        /// <returns></returns>
        //public string MisspellingNormalization(string str)
        //{
        //    string newStr = string.Empty; // newStr="";
        //    string[] lines = str.Split('\n');
        //    string[] words;
        //    foreach (var line in lines)
        //    {
        //        words = line.Split(' ');
        //        foreach (var item in words)
        //        {
        //            if (misspellingDictionary.Exists((X) => (X.Key == item)))
        //                newStr += (misspellingDictionary.Find((X) => (X.Key
        //                  == item)).Value + " "); // add the vallue that match the key
        //            else
        //                newStr += (item + " ");
        //        }
        //        newStr += "\n";
        //    }
        //    return newStr;
        //}

        /// <summary>
        /// translate any slang in a string to its real meaning
        /// </summary>
        /// <param name="str">string to translate</param>
        /// <returns></returns>
        //public string SlangNormalization(string str)
        //{
        //    string newStr = string.Empty; // newStr="";
        //    string[] lines = str.Split('\n');
        //    string[] words;
        //    foreach (var line in lines)
        //    {
        //        words = line.Split(' ');
        //        foreach (var item in words)
        //        {
        //            if (slangDictionary.Exists((X) => (X.Key == item)))
        //                newStr += (slangDictionary.Find((X) => (X.Key
        //                  == item)).Value + " "); // add the vallue that match the key
        //            else
        //                newStr += (item + " ");
        //        }
        //        newStr += "\n";
        //    }
        //    return newStr;
        //}

        /// <summary>
        /// translate any abbravation in a string to its real meaning
        /// </summary>
        /// <param name="str">string to translate</param>
        /// <returns></returns>
        //public string AbbrevationNormalization(string str)
        //{
        //    string newStr = string.Empty; // newStr="";
        //    string[] lines = str.Split('\n');
        //    string[] words;
        //    foreach (var line in lines)
        //    {
        //        words = line.Split(' ');
        //        foreach (var item in words)
        //        {
        //            if (abbrevationDictionary.Exists((X) => (X.Key == item)))
        //                newStr += (abbrevationDictionary.Find((X) => (X.Key
        //                  == item)).Value + " "); // add the vallue that match the key
        //            else
        //                newStr += (item + " ");
        //        }
        //        newStr += "\n";
        //    }
        //    return newStr;
        //}

        /// <summary>
        /// filter out all the stop words from a givenn string
        /// </summary>
        /// <param name="str">string to filter</param>
        /// <returns></returns>
        //public string StopWordsFilter(string str)
        //{
        //    string newStr = string.Empty;
        //    string[] words = str.Split(' ');
        //    foreach (var item in words)
        //    {
        //        if (!stopWords.Contains(item))
        //            newStr += (item + " ");
        //    }
        //    return newStr;
        //}

        /// <summary>
        /// return string without the punctuation chars.
        /// </summary>
        /// <param name="str">string to filter</param>
        /// <returns></returns>
        public string PunctuationFilter(string str)
        {
            string newString = string.Empty;
            foreach (var item in str)
            {
                if (!punctuationString.Contains(item))
                    newString += item;
            }
            return newString;
        }

        /// <summary>
        /// make regular string to uncapital string
        /// </summary>
        /// <param name="cStr">ref argument to make uncapital</param>
        public void UnCapital(ref string cStr)
        {
            cStr = cStr.ToLower();
        }

        /// <summary>
        /// makes the referenced string in the paramter to upper case.
        /// </summary>
        /// <param name="cStr">this string will be all upper case after the fuction call</param>
        public void AllCapital(ref string cStr)
        {
            cStr = cStr.ToUpper();
        }

        /// <summary>
        /// returns cStr all upper case.
        /// </summary>
        /// <param name="cStr"></param>
        /// <returns>returns the parametered string all capital</returns>
        public string AllCapital(string cStr)
        {
            return cStr.ToUpper();
        }

        /// <summary>
        /// return the  argument as uncapital string from
        /// </summary>
        /// <param name="cStr">string to uncapital</param>
        /// <returns></returns>
        public string UnCapital(string cStr)
        {
            return cStr.ToLower();
        }
        #endregion

        /// <summary>
        /// constructor that build the lists that act like a dictionary
        /// </summary>
        public Normalizer()
        {
            try
            {
                string[] buffer = System.IO.File.ReadAllLines(slangPath);
                string[] splitter;
                foreach (var item in buffer)
                {
                    splitter = item.Split('=');
                    slangDictionary.Add(splitter[0], splitter[1]);
                }
                buffer = System.IO.File.ReadAllLines(missplellingPath);
                foreach (var item in buffer)
                {
                    splitter = item.Split('=');
                    misspellingDictionary.Add(splitter[0], splitter[1]);
                }
                buffer = System.IO.File.ReadAllLines(abbrevationsPath);
                foreach (var item in buffer)
                {
                    splitter = item.Split('=');
                    abbrevationDictionary.Add(splitter[0], splitter[1]);
                }
                buffer = System.IO.File.ReadAllLines(stopWordsPath);
                foreach (var item in buffer)
                {
                    stopWords.Add(item);
                }
            }
            catch
            {
                Console.WriteLine("couldnt open one of the dictionary's file's");
            }
        }

    }
}
