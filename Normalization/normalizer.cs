using System;
using System.Collections.Generic;
using System.Linq;
using I_O;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Normalization
{
    public enum NormaliztionMethods
    {
        No_Punctuation = 0,
        All_Capitals,
        All_Lowercase,
        No_HTML_Tags,
        NONE
    }

    public class normalizer
    {
        //is the type json?
        //it should be the interface, no?
        //like this:
        //I_O_Intefrace jFile;
        IOInterface<T> jFile;

        //directory for the files that are not normalized
        string dirToBeNormal = "";
        //directory for the files that are normalized
        string dirForTheNormal = "";

        string punctuationString = ".,;()[]{}:-_?!'\\\"/@#$%^&`~";

        public normalizer(string dir, string type, string dst = "")
        {
            dirToBeNormal = dir;
            if (dst == "")
                dirForTheNormal = dirToBeNormal + "normalaized";
        }

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
        /// returning the cStr without HTML tags.
        /// </summary>
        /// <param name="cStr"></param>
        /// <returns></returns>
        public string removeHTMLTags(string cStr)
        {
            //splitting all the words.
            var words = cStr.Split(' ');
            string newStr = string.Empty;
            foreach (var item in words)
                if (!(item.StartsWith("<") &&
                     (item.EndsWith(">") || item.EndsWith("/>"))))
                    newStr += item + " ";
            return newStr;
        }

        /// <summary>
        /// this function will normalize the text by the user choice
        /// </summary>
        /// <param name="flags">
        /// The flags dictionary is like that :
        /// KEY:"No_Punctuation" - VALUE: true/flase
        /// KEY:"All_Capitals" - VALUE: true/flase
        /// KEY:"All_Lowercase" - VALUE: true/flase
        /// KEY:"No_HTML_Tags" - VALUE: true/flase
        /// </param>
        public void Normalize(IDictionary<NormaliztionMethods,bool> flags)
        {
            try
            {
                if (flags[NormaliztionMethods.No_Punctuation])
                    removePunctuation();

                if (flags[NormaliztionMethods.No_HTML_Tags])
                    removeHTML();

                if (flags[NormaliztionMethods.All_Lowercase])
                    allToLowercase();

                if (flags[NormaliztionMethods.All_Capitals])
                    allToUppercase();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Transforming all text to uppercase letters
        /// </summary>
        private void allToUppercase()
        {
            List<JObject> tweets = new List<JObject>();

            //i inserted "" and 0 because i saw you dont use those parameters , you can change them if you need.
            foreach (var item in jFile.fileToTweets("",0))          
                tweets.Add(JObject.Parse(item.ToString().ToUpper()));
            

            jFile.tweetToFile(tweets, "UPPER", "", 0);
        }
        /// <summary>
        /// Transforing all text to lowercase letters.
        /// </summary>
        private void allToLowercase()
        {
            List<JObject> tweets = new List<JObject>();

            //i inserted "" and 0 because i saw you dont use those parameters , you can change them if you need.
            foreach (var item in jFile.fileToTweets("", 0))          
                tweets.Add(JObject.Parse(item.ToString().ToLower()));
            

            jFile.tweetToFile(tweets, "LOWER", "", 0);
        }
        /// <summary>
        /// Removing all HTML tags from the text
        /// </summary>
        private void removeHTML()
        {
            List<JObject> tweets = new List<JObject>();

            //i inserted "" and 0 because i saw you dont use those parameters , you can change them if you need.
            foreach (var item in jFile.fileToTweets("", 0))          
                tweets.Add(JObject.Parse(removeHTMLTags(item.ToString())));
            

            jFile.tweetToFile(tweets, "NO_HTML", "", 0);
        }
        /// <summary>
        /// removing punctuations from the text.
        /// </summary>
        private void removePunctuation()
        {
            List<JObject> tweets = new List<JObject>();

            //i inserted "" and 0 because i saw you dont use those parameters , you can change them if you need.
            foreach (var item in jFile.fileToTweets("", 0))
                tweets.Add(JObject.Parse(PunctuationFilter(item.ToString())));

            jFile.tweetToFile(tweets, "NO_PUN", "", 0);
        }
    }
}
