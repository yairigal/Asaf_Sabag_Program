﻿using System;
using System.Collections.Generic;
using System.Linq;
using I_O;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

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
        IOText TextRW;
        IOJson JsonRW;

        //directory for the files that are not normalized
        string dirToBeNormal = "";
        //directory for the files that are normalized
        string dirForTheNormal = "";

        string punctuationString = ".,;()[]{}:-_?!'\\\"/@#$%^&`~";

        public normalizer(string dir, string type, string dst = "")
        {
            dirToBeNormal = dir;
            if (dst == "")
            {
                dirForTheNormal = dirToBeNormal + "_normalaized";
                Directory.CreateDirectory(dirForTheNormal);
            }
            TextRW = new IOText();
        }


        /// <summary>
        /// this function will normalize the text by the user choice
        /// its going to run on all the files in the dir its normalizing
        /// for every tweet in every file, the appropriate normalizing function will be activated and the normalized tweet wiil be saved in the list
        /// the list of normalized tweet will be wrriten into a text file in the directory for the normalized files.
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
            List<string> tweets = new List<string>();
            string normalTweet = "";
            string changes = "";
            try
            {
                foreach (string file in Directory.GetFiles(dirToBeNormal))
                {
                    changes = getNormalizaionsExtansions(flags, changes);

                    foreach (string tweet in TextRW.fileToTweets(file, "", 0))
                    {
                        normalTweet = tweet;
                        normalTweet = NormalizeTweet(flags, normalTweet);
                        tweets.Add(normalTweet);
                    }
                    string filename = Path.GetFileName(file);
                    filename += changes;
                    if (Path.GetExtension(filename) == string.Empty)
                        filename += ".txt";
                    TextRW.tweetToFile(tweets, dirForTheNormal + "\\" + filename, "", 0);
                    tweets.Clear();
                    changes = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Normalizes the current string based on the flags given
        /// </summary>
        /// <param name="flags">The normalization methods used to normalize the string.</param>
        /// <param name="normalTweet">the tweet to be normalized</param>
        /// <returns>The normalized tweet</returns>
        private string NormalizeTweet(IDictionary<NormaliztionMethods, bool> flags, string normalTweet)
        {
            if (flags[NormaliztionMethods.No_Punctuation])
            {
                normalTweet = removePunctuation(normalTweet);
            }

            if (flags[NormaliztionMethods.No_HTML_Tags])
            {
                normalTweet = removeHTML(normalTweet);
            }

            if (flags[NormaliztionMethods.All_Lowercase])
            {
                normalTweet = allToLowercase(normalTweet);
            }

            if (flags[NormaliztionMethods.All_Capitals])
            {
                normalTweet = allToUppercase(normalTweet);
            }

            return normalTweet;
        }
        /// <summary>
        /// Returns the file extansion based on his normalizaion
        /// </summary>
        /// <param name="flags">the normalizaions flags(which normalization methods will be applied)</param>
        /// <param name="changes">the extension appended</param>
        /// <returns>
        /// _RP_ = Remove Punctuation.
        /// _RH_ = Remove Html tags.
        /// _TU_ = To upper.
        /// _TL_ = To lower
        /// </returns>
        private static string getNormalizaionsExtansions(IDictionary<NormaliztionMethods, bool> flags,string changes)
        {
            if (flags[NormaliztionMethods.No_Punctuation])
            {
                changes += "_RP_";
            }
            if (flags[NormaliztionMethods.No_HTML_Tags])
            {
                changes += "_RH_";
            }
            if (flags[NormaliztionMethods.All_Capitals])
            {
                changes += "_TU_";
            }
            if (flags[NormaliztionMethods.All_Lowercase])
            {
                changes += "_TL_";
            }

            return changes;
        }

        /// <summary>
        /// Transforming all text to uppercase letters
        /// </summary>
        private string allToUppercase(string tweet)
        {
            return tweet.ToUpper();
        }
        /// <summary>
        /// Transforing all text to lowercase letters.
        /// </summary>
        private string allToLowercase(string tweet)
        {
            return tweet.ToLower();
        }
        
        /// <summary>
        /// Removing all HTML tags from the text
        /// </summary>
        private string removeHTML(string tweet)
        {
            return removeHTMLTags(tweet);
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
        /// removing punctuations from the text.
        /// </summary>
        private string removePunctuation(string tweet)
        {
            return PunctuationFilter(tweet);
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

    }
}


/* for checking
 * using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normalization
{
    class app
    {
        public static void Main()
        {
            normalizer norm = new normalizer("C:\\Users\\user\\Desktop\\test\\alt.atheism", "");
            IDictionary<NormaliztionMethods, bool> flags = new Dictionary<NormaliztionMethods, bool> ();
            flags.Add(NormaliztionMethods.All_Capitals, true);
            flags.Add(NormaliztionMethods.All_Lowercase, false);
            flags.Add(NormaliztionMethods.No_HTML_Tags, false);
            flags.Add(NormaliztionMethods.No_Punctuation, false);

            norm.Normalize(flags);
        }
    }
}
*/