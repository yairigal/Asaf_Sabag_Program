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
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="type"></param>
        /// <param name="dst"></param>
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

            var fileList = getFilesFromDirectory(dirToBeNormal);

            try
            {
                foreach (var file in fileList)
                {
                    foreach (string tweet in ReadWrt.fileToTweets(file.getPathFromRoot(dirToBeNormal), "", 0))
                    {
                        normalTweet = tweet;
                        normalTweet = NormalizeTweet(flags, normalTweet);
                        tweets.Add(normalTweet);
                    }
                    //string filename = file.filename + changes + Path.GetExtension(file.getPathFromRoot(dirToBeNormal));
                    string dir = file.getPathDirectory(dirForTheNormal);

                    if(dir != "")
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                    string ext="";
                    if (Path.GetExtension(file.getPathFromRoot(dirToBeNormal)) == string.Empty)
                        if(ReadWrt is IOText)
                            ext = ".txt";
                        else if(ReadWrt is IOJson)
                            ext = ".json";
                    ReadWrt.tweetToFile(tweets, file.getPathFromRoot(dirForTheNormal)+changes+ext, "", 0);
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
        //yair notes :
        //normalizer needs function get all files to run on -> getAllFiles : array<helperObject> - done
        //the normalizer needs after every normal to save the file in his proper place. -> with the helper object he can do this - done
        //the main window needs a function that will give him the list of files (number 1 ^^)
        /// <summary>
        /// This function returns a list of objects that each of them has a name of a file and and list of the directories he passed from the root.
        /// </summary>
        /// <param name="dir">the root directory</param>
        /// <param name="subdir">for the recursian</param>
        /// <param name="lst">for the recursian</param>
        /// <returns></returns>
        private static List<DirHelper> getFilesFromDirectory(string dir,List<string> subdir = null,List<DirHelper> lst = null)
        {
            List<DirHelper> toReturn;
            List<string> subDir;
            //first time
            if (lst == null)
                toReturn = new List<DirHelper>();
            else
                toReturn = lst;

            subDir = new List<string>();
            if(subdir != null)
                if(subdir.Count != 0)
                    subDir.AddRange(subdir);


            var listOfFiles = Directory.GetFiles(dir);
            var listOfDir = Directory.GetDirectories(dir);

            //add all files
            foreach (var file in listOfFiles)
            {
                var toAdd = new DirHelper(Path.GetFileName(file));
                //sub folder
                if (subDir.Count != 0)
                    toAdd.fromRoot.AddRange(subDir);

                toReturn.Add(toAdd);
            }

            //if there are sub-directories
            if (listOfDir.Length != 0)
                foreach (var directory in listOfDir)
                {
                    string lastFolder = Path.GetFileName(directory);
                    //dont run on output path
                    if (lastFolder != Path.GetFileName(Path.GetDirectoryName(Information.output_path)))
                    {
                        subDir.Add(lastFolder);
                        getFilesFromDirectory(directory, subDir, toReturn);
                        subDir.Remove(subDir.Last());
                    }
                }


            return toReturn;

        }
        /// <summary>
        /// Static function to get a list of the normalized file paths
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static List<string> getFiles(string dir)
        {
            var list = getFilesFromDirectory(dir);
            List<string> toReturn = new List<string>();
            foreach (var file in list)           
                toReturn.Add(file.getPathFromRoot(dir));
            return toReturn;        
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
        public static List<PartOfSentenece> Parser(string tweet)
        {
            char[] punctuationString = { ' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}','!', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@' };
            //string punctuationString = ".,;()[]{}:-_?!'\\\"/@#$%^&`~ <>\n\r";
            List<PartOfSentenece> toReturn = new List<PartOfSentenece>();
            string wordAdder = string.Empty;
            foreach (var letter in tweet)
            {
                //its a punc -> add the word before it , and then add the punc
                if (punctuationString.Contains(letter))
                {
                    //if we already started - > add word first
                    if(wordAdder != string.Empty)
                    {
                        toReturn.Add(new PartOfSentenece(type.word, wordAdder));
                        wordAdder = string.Empty;
                    }
                    //add the punc
                    toReturn.Add(new PartOfSentenece(type.puncuation, letter.ToString()));
                }
                else // its a letter - > continue
                {
                    wordAdder += letter;
                }
            }
            //the last word in the line.
            if(wordAdder != string.Empty)
                toReturn.Add(new PartOfSentenece(type.word, wordAdder));

            return toReturn;
        }
        //public static List<PartOfSentenece> removeHTML(List<PartOfSentenece> tweet)
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
        //public static List<PartOfSentenece> removePunc(List<PartOfSentenece> tweet)
        //{
        //    List<PartOfSentenece> toRemove = new List<PartOfSentenece>();
        //    foreach (var item in tweet)         
        //        if (item.type == type.puncuation && item.value != " " && item.value != ">" && item.value != "<")
        //            toRemove.Add(item);

        //    toRemove.Reverse();
        //    foreach (var item in toRemove)           
        //        tweet.Remove(item);


        //    return tweet;  
        //}
        //public static List<PartOfSentenece> toLower(List<PartOfSentenece> tweet)
        //{
        //    foreach (var item in tweet)            
        //        if (item.type == type.word)
        //            item.value = item.value.ToLower();
        //    return tweet;           
        //}
        //public static List<PartOfSentenece> toUpper(List<PartOfSentenece> tweet)
        //{
        //    foreach (var item in tweet)
        //        if (item.type == type.word)
        //            item.value = item.value.ToUpper();
        //    return tweet;
        //}
        //public static List<PartOfSentenece> removeStopWord(List<PartOfSentenece> tweet)
        //{
        //    foreach (var item in tweet)
        //    {
        //        if (item.type == type.word)
        //            if (stopwords.Contains(item.value))
        //                item.value = "";
        //    }
        //    return tweet;
        //}
        public static void removeHTML(List<PartOfSentenece> tweet)
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
        public static void removePunc(List<PartOfSentenece> tweet)
        {
            List<PartOfSentenece> toRemove = new List<PartOfSentenece>();
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
        public static void toLower(List<PartOfSentenece> tweet)
        {
            foreach (var item in tweet)
                if (item.type == type.word)
                    item.value = item.value.ToLower();

        }
        /// <summary>
        /// transforming all words into uppercase
        /// </summary>
        /// <param name="tweet"></param>
        public static void toUpper(List<PartOfSentenece> tweet)
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
        public static void removeStopWord(List<PartOfSentenece> tweet)
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
        public static string listToString(List<PartOfSentenece> list)
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

    #region Help objects
    public enum type
    {
        word,
        puncuation
    }
    public class PartOfSentenece
    {
        public type type;
        public string value;
        public PartOfSentenece(type type, string val)
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
    public class DirHelper
    {
        public string filename;
        public List<string> fromRoot;
        public DirHelper(string fn)
        {
            filename = fn;
            fromRoot = new List<string>();
        }
        /// <summary>
        /// returns the directory like this :
        /// real file Directory : root->dir\file
        /// function returns root->dir\file
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public string getPathFromRoot(string root)
        {
            string path = "";
            foreach (var item in fromRoot)
            {
                path += "\\";
                path += item;
            }
            return root+path + "\\" + filename;
        }
        /// <summary>
        /// returns the directory like this :
        /// real file Directory : root->dir\file
        /// function returns root->dir
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public string getPathDirectory(string root)
        {
            string path = "";
            foreach (var item in fromRoot)
            {
                path += "\\";
                path += item;
            }
            return root+path;
        }
    }
    #endregion
}


////for checking
//namespace Normalization
//{
//    class app
//    {
//        public static void Main()
//        {
//            var a = normalizer.getFilesFromDirectory(@"C:\Users\Yair\Desktop\data");
//            List<string> ab = new List<string>();
//            foreach (var item in a)
//            {
//                Console.WriteLine(@"C:\Users\Yair\Desktop\data_normal"+item.getPathFromRoot());
//            }
//        }
//    }
//}


