using System;
using System.Collections.Generic;
using System.Linq;
using I_O;
using System.IO;
using Enums;

namespace Normalization
{

    public class normalizer
    {
        IOInterface ReadWrt;
        /*IOText TextRW;
        IOJson JsonRW;*/

        //directory for the files that are not normalized
        string dirToBeNormal = Directory.GetCurrentDirectory();
        //directory for the files that are normalized
        string dirForTheNormal = "";
        //string tha describes the type of normalizations that had been made
        string changes = "";

        string punctuationString = ".,;()[]{}:-_?!'\\\"/@#$%^&`~";

        public string AfterNormalDir { get { return dirForTheNormal; } }

        public string BeforeNormalDir { get { return dirToBeNormal; } }

        public string Changes { get { return changes; } }

        public normalizer(string dir, IO_DataType type, string dst = "")
        {
            dirToBeNormal = dir;
            if (dst == "")
            {
                dirForTheNormal = dirToBeNormal;
            }

            ReadWrt = IOFactory.getFacotry(type);
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
        public void Normalize(IDictionary<NormaliztionMethods, bool> flags)
        {
            List<string> tweets = new List<string>();
            string normalTweet = "";

            changes = getNormalizaionsExtansions(flags);
            dirForTheNormal += "_normalaized" + changes;
            Directory.CreateDirectory(dirForTheNormal);

            try
            {
                foreach (string file in Directory.GetFiles(dirToBeNormal))
                {
                    foreach (string tweet in ReadWrt.fileToTweets(file, "", 0))
                    {
                        normalTweet = tweet;
                        normalTweet = NormalizeTweet(flags, normalTweet);
                        tweets.Add(normalTweet);
                    }
                    string filename = Path.GetFileNameWithoutExtension(file) + changes + Path.GetExtension(file);
                    if (Path.GetExtension(filename) == string.Empty)
                        filename += ".txt";
                    ReadWrt.tweetToFile(tweets, dirForTheNormal + "\\" + filename, "", 0);
                    tweets.Clear();
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
                normalTweet = removePunctuation(normalTweet);


            if (flags[NormaliztionMethods.No_HTML_Tags])
                normalTweet = removeHTML(normalTweet);


            if (flags[NormaliztionMethods.All_Lowercase])
                normalTweet = allToLowercase(normalTweet);


            if (flags[NormaliztionMethods.All_Capitals])
                normalTweet = allToUppercase(normalTweet);


            if (flags[NormaliztionMethods.No_Stop_Words])
                normalTweet = removeStopWords(normalTweet);

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
        private static string getNormalizaionsExtansions(IDictionary<NormaliztionMethods, bool> flags)
        {
            string ch = "";
            if (flags[NormaliztionMethods.No_Punctuation])
            {
                ch += "_P";
            }
            if (flags[NormaliztionMethods.No_HTML_Tags])
            {
                ch += "_H";
            }
            if (flags[NormaliztionMethods.All_Capitals])
            {
                ch += "_U";
            }
            if (flags[NormaliztionMethods.All_Lowercase])
            {
                ch += "_L";
            }

            return ch;
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
        /// <summary>
        /// removing the stop words from the current string parameter
        /// </summary>
        /// <param name="toRemoveFrom">removing the stop words from this string</param>
        private string removeStopWords(string toRemoveFrom)
        {
            //hope that works
            //return string.Join(" ",
            //    toRemoveFrom.Split(new[] { ' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@' },
            //    StringSplitOptions.RemoveEmptyEntries).Except(stopwords));
            return "";
        }

        #region stop words
        private static string[] stopwords = {
 "a",
"about",
"above",
"across",
"after",
"again",
"against",
"all",
"almost",
"alone",
"along",
"already",
"also",
"although",
"always",
"among",
"an",
"and",
"another",
"any",
"anybody",
"anyone",
"anything",
"anywhere",
"are",
"area",
"areas",
"around",
"as",
"ask",
"asked",
"asking",
"asks",
"at",
"away",
"b",
"back",
"backed",
"backing",
"backs",
"be",
"because",
"become",
"becomes",
"became",
"been",
"before",
"began",
"behind",
"being",
"beings",
"best",
"better",
"between",
"big",
"both",
"but",
"by",
"c",
"came",
"can",
"cannot",
"case",
"cases",
"certain",
"certainly",
"clear",
"clearly",
"come",
"could",
"d",
"did",
"differ",
"different",
"differently",
"do",
"does",
"done",
"down",
"downed",
"downing",
"downs",
"during",
"e",
"each",
"early",
"either",
"end",
"ended",
"ending",
"ends",
"enough",
"even",
"evenly",
"ever",
"every",
"everybody",
"everyone",
"everything",
"everywhere",
"f",
"face",
"faces",
"fact",
"facts",
"far",
"felt",
"few",
"find",
"finds",
"first",
"for",
"four",
"from",
"full",
"fully",
"further",
"furthered",
"furthering",
"furthers",
"g",
"gave",
"general",
"generally",
"get",
"gets",
"give",
"given",
"gives",
"go",
"going",
"good",
"goods",
"got",
"great",
"greater",
"greatest",
"group",
"grouped",
"grouping",
"groups",
"h",
"had",
"has",
"have",
"having",
"he",
"her",
"herself",
"here",
"high",
"higher",
"highest",
"him",
"himself",
"his",
"how",
"however",
"i",
"if",
"important",
"in",
"interest",
"interested",
"interesting",
"interests",
"into",
"is",
"it",
"its",
"itself",
"j",
"just",
"k",
"keep",
"keeps",
"kind",
"knew",
"know",
"known",
"knows",
"l",
"large",
"largely",
"last",
"later",
"latest",
"least",
"less",
"let",
"lets",
"like",
"likely",
"long",
"longer",
"longest",
"m",
"made",
"make",
"making",
"man",
"many",
"may",
"me",
"member",
"members",
"men",
"might",
"more",
"most",
"mostly",
"mr",
"mrs",
"much",
"31",
"must",
"my",
"myself",
"n",
"necessary",
"need",
"needed",
"needing",
"needs",
"never",
"new",
"newer",
"newest",
"next",
"no",
"non",
"not",
"nobody",
"noone",
"nothing",
"now",
"nowhere",
"number",
"numbers",
"o",
"of",
"off",
"often",
"old",
"older",
"oldest",
"on",
"once",
"one",
"only",
"open",
"opened",
"opening",
"opens",
"or",
"order",
"ordered",
"ordering",
"orders",
"other",
"others",
"our",
"out",
"over",
"p",
"part",
"parted",
"parting",
"parts",
"per",
"perhaps",
"place",
"places",
"point",
"pointed",
"pointing",
"points",
"possible",
"present",
"presented",
"presenting",
"presents",
"problem",
"problems",
"put",
"puts",
"q",
"quite",
"r",
"rather",
"really",
"right",
"room",
"rooms",
"s",
"said",
"same",
"saw",
"say",
"says",
"second",
"seconds",
"see",
"sees",
"seem",
"seemed",
"seeming",
"seems",
"several",
"shall",
"she",
"should",
"show",
"showed",
"showing",
"shows",
"side",
"sides",
"since",
"small",
"smaller",
"smallest",
"so",
"some",
"somebody",
"someone",
"something",
"somewhere",
"state",
"states",
"still",
"such",
"sure",
"t",
"take",
"taken",
"than",
"that",
"the",
"their",
"them",
"then",
"there",
"therefore",
"these",
"they",
"thing",
"things",
"think",
"thinks",
"this",
"those",
"though",
"thought",
"thoughts",
"three",
"through",
"thus",
"to",
"today",
"together",
"too",
"took",
"toward",
"turn",
"turned",
"turning",
"turns",
"two",
"u",
"under",
"until",
"up",
"upon",
"us",
"use",
"uses",
"used",
"v",
"very",
"w",
"want",
"wanted",
"wanting",
"wants",
"was",
"way",
"ways",
"we",
"well",
"wells",
"went",
"were",
"what",
"when",
"where",
"whether",
"which",
"while",
"who",
"whole",
"whose",
"why",
"will",
"with",
"within",
"without",
"work",
"worked",
"working",
"works",
"would",
"x",
"y",
"year",
"years",
"yet",
"you",
"young",
"younger",
"youngest",
"your",
"yours",
"z"
        };
        #endregion

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
