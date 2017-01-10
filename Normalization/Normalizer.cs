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

        static string punctuationString = ".,;()[]{}:-_?!'\\\"/@#$%^&`~";

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
            var normalTweetList = Parser(normalTweet);

            if (flags[NormaliztionMethods.No_Punctuation])
                removePunc(normalTweetList);

            if (flags[NormaliztionMethods.No_HTML_Tags])
                removeHTML(normalTweetList);

            if (flags[NormaliztionMethods.All_Lowercase])
                toLower(normalTweetList);

            if (flags[NormaliztionMethods.All_Capitals])
                toUpper(normalTweetList);

            if (flags[NormaliztionMethods.No_Stop_Words])
                removeStopWord(normalTweetList);

            return listToString(normalTweetList);
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
            if (flags[NormaliztionMethods.No_Stop_Words])
            {
                ch += "_S";
            }

            return ch;
        }

        #region String Functions
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
        #endregion

        #region Array Functions
        /// <summary>
        /// Parsing the string into an array of words and puncuations
        /// </summary>
        /// <param name="tweet"></param>
        /// <returns></returns>
        public static List<PartOfSentenec> Parser(string tweet)
        {
            char[] punctuationString = { ' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@' };
            //string punctuationString = ".,;()[]{}:-_?!'\\\"/@#$%^&`~ <>\n\r";
            List<PartOfSentenec> toReturn = new List<PartOfSentenec>();
            string wordAdder = string.Empty;
            foreach (var letter in tweet)
            {
                //its a punc -> add the word before it , and then add the punc
                if (punctuationString.Contains(letter))
                {
                    //if we already started - > add word first
                    if(wordAdder != string.Empty)
                    {
                        toReturn.Add(new PartOfSentenec(type.word, wordAdder));
                        wordAdder = string.Empty;
                    }
                    //add the punc
                    toReturn.Add(new PartOfSentenec(type.puncuation, letter.ToString()));
                }
                else // its a letter - > continue
                {
                    wordAdder += letter;
                }
            }
            //the last word in the line.
            if(wordAdder != string.Empty)
                toReturn.Add(new PartOfSentenec(type.word, wordAdder));

            return toReturn;
        }
        //public static List<PartOfSentenec> removeHTML(List<PartOfSentenec> tweet)
        //{
        //    int startingPoint=-1, endingPoint=-1;
        //    List<removeObject> toRemove = new List<removeObject>();
        //    foreach (var item in tweet)
        //    {
        //        //if its a start of HTML
        //        if (item.value == "<")
        //            startingPoint = tweet.IndexOf(item);
        //        else if (item.value == ">" && startingPoint != -1)
        //        {
        //            endingPoint = tweet.IndexOf(item)+1;
        //            toRemove.Add(new removeObject() { startingPoint = startingPoint, endingPoint = endingPoint });
        //            startingPoint = -1;
        //        }
        //    }
        //    //removing
        //    toRemove.Reverse();
        //    foreach (var item in toRemove)           
        //        tweet.RemoveRange(item.startingPoint, item.endingPoint - item.startingPoint);
            
        //    return tweet;
        //}
        ///// <summary>
        ///// Removes all punctuations except from " "(space ) > and <
        ///// </summary>
        ///// <param name="tweet"></param>
        ///// <returns></returns>
        //public static List<PartOfSentenec> removePunc(List<PartOfSentenec> tweet)
        //{
        //    List<PartOfSentenec> toRemove = new List<PartOfSentenec>();
        //    foreach (var item in tweet)         
        //        if (item.type == type.puncuation && item.value != " " && item.value != ">" && item.value != "<")
        //            toRemove.Add(item);

        //    toRemove.Reverse();
        //    foreach (var item in toRemove)           
        //        tweet.Remove(item);


        //    return tweet;  
        //}
        //public static List<PartOfSentenec> toLower(List<PartOfSentenec> tweet)
        //{
        //    foreach (var item in tweet)            
        //        if (item.type == type.word)
        //            item.value = item.value.ToLower();
        //    return tweet;           
        //}
        //public static List<PartOfSentenec> toUpper(List<PartOfSentenec> tweet)
        //{
        //    foreach (var item in tweet)
        //        if (item.type == type.word)
        //            item.value = item.value.ToUpper();
        //    return tweet;
        //}
        //public static List<PartOfSentenec> removeStopWord(List<PartOfSentenec> tweet)
        //{
        //    foreach (var item in tweet)
        //    {
        //        if (item.type == type.word)
        //            if (stopwords.Contains(item.value))
        //                item.value = "";
        //    }
        //    return tweet;
        //}
        public static void removeHTML(List<PartOfSentenec> tweet)
        {
            int startingPoint = -1, endingPoint = -1;
            List<removeObject> toRemove = new List<removeObject>();
            foreach (var item in tweet)
            {
                //if its a start of HTML
                if (item.value == "<")
                    startingPoint = tweet.IndexOf(item);
                else if (item.value == ">" && startingPoint != -1)
                {
                    endingPoint = tweet.IndexOf(item) + 1;
                    toRemove.Add(new removeObject() { startingPoint = startingPoint, endingPoint = endingPoint });
                    startingPoint = -1;
                }
            }
            //removing
            toRemove.Reverse();
            foreach (var item in toRemove)
                tweet.RemoveRange(item.startingPoint, item.endingPoint - item.startingPoint);
        }
        /// <summary>
        /// Removes all punctuations except from " "(space ) > and <
        /// </summary>
        /// <param name="tweet"></param>
        /// <returns></returns>
        public static void removePunc(List<PartOfSentenec> tweet)
        {
            List<PartOfSentenec> toRemove = new List<PartOfSentenec>();
            foreach (var item in tweet)
                if (item.type == type.puncuation && item.value != " " && item.value != ">" && item.value != "<")
                    toRemove.Add(item);

            toRemove.Reverse();
            foreach (var item in toRemove)
                tweet.Remove(item);
        }
        /// <summary>
        /// Transforming all words into lowercase
        /// </summary>
        /// <param name="tweet"></param>
        public static void toLower(List<PartOfSentenec> tweet)
        {
            foreach (var item in tweet)
                if (item.type == type.word)
                    item.value = item.value.ToLower();

        }
        /// <summary>
        /// transforming all words into uppercase
        /// </summary>
        /// <param name="tweet"></param>
        public static void toUpper(List<PartOfSentenec> tweet)
        {
            foreach (var item in tweet)
                if (item.type == type.word)
                    item.value = item.value.ToUpper();
        }
        /// <summary>
        /// removing stop words from the list 
        /// stop words are from the 421 chosen stop words
        /// </summary>
        /// <param name="tweet"></param>
        public static void removeStopWord(List<PartOfSentenec> tweet)
        {
            foreach (var item in tweet)
            {
                if (item.type == type.word)
                    if (stopwords.Contains(item.value.ToLower()))
                        item.value = "";
            }
        }
        /// <summary>
        /// returns the list to a normal string.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string listToString(List<PartOfSentenec> list)
        {
            string toReturn = "";
            foreach (var item in list)
            {
                toReturn += item.value;
            }
            return toReturn;
        }
        #endregion

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
    public enum type
    {
        word,
        puncuation
    }
    public class PartOfSentenec
    {
        public type type;
        public string value;
        public PartOfSentenec(type type, string val)
        {
            this.type = type;
            this.value = val;
        }
    }
    public class removeObject
    {
        public int startingPoint = -1;
        public int endingPoint = -1;
    }
}


// for checking

//namespace Normalization
//{
//    class app
//    {
//        public static void Main()
//        {
//            string test = "Hello,the my name is yair. i love to party: espically rock. <LOL> <LOL/>";
//            List<PartOfSentenec> list = normalizer.getList(test);
//            normalizer.removeStopWord(list);
//            normalizer.removePunc(list);
//            normalizer.removeHTML(list);
//            normalizer.toUpper(list);
//            string test2 = normalizer.listToString(list);
//        }
//    }
//}

