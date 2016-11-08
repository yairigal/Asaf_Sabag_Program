using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ContentFamilies
{
    public class FreqWordsForOneArticle
    {
        #region init variables
        //props
        public HashSet<SeqString> table_of_one;
        public HashSet<SeqString> table_of_two;
        public HashSet<SeqString> table_of_three;
        public HashSet<SeqString> table_of_four;

        public HashSet<SeqString> unichar;
        public HashSet<SeqString> bichar;
        public HashSet<SeqString> trichar;
        public HashSet<SeqString> quadchar;

        public string year { set; get; }
        public int total_words { set; get; }
        public int total_bigrams { set; get; }
        public int total_trigrams { set; get; }
        public int total_qaudgrams { set; get; }
        public int total_bichars { set; get; }
        public int total_trichars { set; get; }
        public int total_quadchars { set; get; }
        public int total_chars = 0;
        public string bookType { get; set; }
        //consts
        const double PERC_OF_MIN_FREQ = 0.0001;//percent of total words that lower that i dont count
        //helpers
        protected bool isP;
        protected string[] articles;
        public string pathToFile = "";
        protected string year_dir;
        protected List<string> stopwords;

        #endregion


        public FreqWordsForOneArticle()
        {
            #region init tables
            table_of_one = new HashSet<SeqString>();
            table_of_two = new HashSet<SeqString>();
            table_of_three = new HashSet<SeqString>();
            table_of_four = new HashSet<SeqString>();
            #endregion

            #region  init
            total_words = 0;
            StreamReader sw = new StreamReader("StopWords.txt");
            stopwords = sw.ReadToEnd().Split('\r', '\n').ToList();
            sw.Close();
            stopwords.RemoveAll(x => x == "");
            #endregion
        }


        public FreqWordsForOneArticle(string [] pathes)
        {
            if (pathes.Equals(""))//
                return;
            pathToFile = pathes[0];
            year = pathes[0].Remove(pathes[0].LastIndexOf('\\'));
            year = year.Substring(year.LastIndexOf('\\') == -1 ? 0 : year.LastIndexOf('\\'));
            #region init tables
            table_of_one = new HashSet<SeqString>();
            table_of_two = new HashSet<SeqString>();
            table_of_three = new HashSet<SeqString>();
            table_of_four = new HashSet<SeqString>();
            unichar = new HashSet<SeqString>();
            bichar = new HashSet<SeqString>();
            trichar = new HashSet<SeqString>();
            quadchar = new HashSet<SeqString>();
            #endregion

            #region  init
            total_words = 0;
            string YearPath = "";
            StreamReader sw = new StreamReader("StopWords.txt");
            stopwords = sw.ReadToEnd().Split('\r', '\n').ToList();
            sw.Close();
            stopwords.RemoveAll(x => x == "");
            #endregion

            #region go through each article in the giving Year and count words and phrases
            foreach (var path in pathes)
            {
                total_words += GetNumOfWords(path);
                YearPath = path;
                CountWords(path);
                if (Program.ForOneArticle.Equals("E"))
                    table_of_one.RemoveWhere(x => x.freq < 3);
                CountPhrases(path);
                if (Program.UniChars != 0)
                    CountCharacters(path);
                CountNCharacters(path);
            }
           

            

            #region sort all elements from high to low
            table_of_one = new HashSet<SeqString>(table_of_one.OrderByDescending(x => x.freq));
            table_of_two = new HashSet<SeqString>(table_of_two.OrderByDescending(x => x.freq));
            table_of_three = new HashSet<SeqString>(table_of_three.OrderByDescending(x => x.freq));
            table_of_four = new HashSet<SeqString>(table_of_four.OrderByDescending(x => x.freq));
            unichar = new HashSet<SeqString>(unichar.OrderByDescending(x => x.freq));
            bichar = new HashSet<SeqString>(bichar.OrderByDescending(x => x.freq));
            trichar = new HashSet<SeqString>(trichar.OrderByDescending(x => x.freq));
            quadchar = new HashSet<SeqString>(quadchar.OrderByDescending(x => x.freq));
            #endregion


            bichar.RemoveWhere(x => (from y in table_of_one select y.word).Contains(x.word));
            trichar.RemoveWhere(x => (from y in table_of_one select y.word).Contains(x.word));
            quadchar.RemoveWhere(x => (from y in table_of_one select y.word).Contains(x.word));


            bichar.RemoveWhere(x => x.word.ToCharArray().Where(a => !(a > 32 && a < 127)).ToArray().Length > 0);
            trichar.RemoveWhere(x => x.word.ToCharArray().Where(a => !(a > 32 && a < 127)).ToArray().Length > 0);
            quadchar.RemoveWhere(x => x.word.ToCharArray().Where(a => !(a > 32 && a < 127)).ToArray().Length > 0);
            #endregion

            total_words = table_of_one.Sum(x => x.freq);
            total_bigrams = table_of_two.Sum(x => x.freq);
            total_trigrams = table_of_three.Sum(x => x.freq);
            total_qaudgrams = table_of_four.Sum(x => x.freq);

            total_chars = unichar.Sum(x => x.freq);
            total_bichars = bichar.Sum(x => x.freq);
            total_trichars = trichar.Sum(x => x.freq);
            total_quadchars = quadchar.Sum(x => x.freq);





        }


        public FreqWordsForOneArticle(string path)
        {
            if (path.Equals(""))//
                return;
            pathToFile = path;
            year = path.Remove(path.LastIndexOf('\\'));
            year = year.Substring(year.LastIndexOf('\\') == -1 ? 0 : year.LastIndexOf('\\'));
            #region init tables
            table_of_one = new HashSet<SeqString>();
            table_of_two = new HashSet<SeqString>();
            table_of_three = new HashSet<SeqString>();
            table_of_four = new HashSet<SeqString>();
            unichar = new HashSet<SeqString>();
            bichar = new HashSet<SeqString>();
            trichar = new HashSet<SeqString>();
            quadchar = new HashSet<SeqString>();
            #endregion

            #region  init
            total_words = 0;
            string YearPath = "";
            StreamReader sw = new StreamReader("StopWords.txt");
            stopwords = sw.ReadToEnd().Split('\r', '\n').ToList();
            sw.Close();
            stopwords.RemoveAll(x => x == "");
            #endregion

            #region go through each article in the giving Year and count words and phrases

            total_words += GetNumOfWords(path);
            YearPath = path;
            CountWords(path);
            if (Program.ForOneArticle.Equals("E"))
                table_of_one.RemoveWhere(x => x.freq < 3);
            CountPhrases(path);

            if (Program.UniChars != 0)
                CountCharacters(path);
            CountNCharacters(path);

            #region sort all elements from high to low
            table_of_one = new HashSet<SeqString>(table_of_one.OrderByDescending(x => x.freq));
            table_of_two = new HashSet<SeqString>(table_of_two.OrderByDescending(x => x.freq));
            table_of_three = new HashSet<SeqString>(table_of_three.OrderByDescending(x => x.freq));
            table_of_four = new HashSet<SeqString>(table_of_four.OrderByDescending(x => x.freq));
            unichar = new HashSet<SeqString>(unichar.OrderByDescending(x => x.freq));
            bichar = new HashSet<SeqString>(bichar.OrderByDescending(x => x.freq));
            trichar = new HashSet<SeqString>(trichar.OrderByDescending(x => x.freq));
            quadchar = new HashSet<SeqString>(quadchar.OrderByDescending(x => x.freq));
            #endregion

            
            bichar.RemoveWhere(x => (from y in table_of_one select y.word).Contains(x.word));
            trichar.RemoveWhere(x => (from y in table_of_one select y.word).Contains(x.word));
            quadchar.RemoveWhere(x => (from y in table_of_one select y.word).Contains(x.word));
            
            
            bichar.RemoveWhere(x => x.word.ToCharArray().Where(a => !(a > 32 && a < 127)).ToArray().Length > 0);
            trichar.RemoveWhere(x => x.word.ToCharArray().Where(a => !(a > 32 && a < 127)).ToArray().Length > 0);
            quadchar.RemoveWhere(x => x.word.ToCharArray().Where(a => !(a > 32 && a < 127)).ToArray().Length > 0);
            #endregion

            total_words = table_of_one.Sum(x => x.freq);
            total_bigrams = table_of_two.Sum(x => x.freq);
            total_trigrams = table_of_three.Sum(x => x.freq);
            total_qaudgrams = table_of_four.Sum(x => x.freq);

            total_chars = unichar.Sum(x => x.freq);
            total_bichars = bichar.Sum(x => x.freq);
            total_trichars = trichar.Sum(x => x.freq);
            total_quadchars = quadchar.Sum(x => x.freq);


            
            

        }

        public void RemoveUncommonWords(double PREC_OF_MIN_FREQ)
        {
            table_of_one.RemoveWhere(x => x.freq < total_words * PERC_OF_MIN_FREQ);
            table_of_two.RemoveWhere(x => x.freq < total_words * PERC_OF_MIN_FREQ);
            table_of_three.RemoveWhere(x => x.freq < total_words * PERC_OF_MIN_FREQ);
            table_of_four.RemoveWhere(x => x.freq < total_words * PERC_OF_MIN_FREQ);
        }

        protected void WriteTables(string file)
        {
            #region generating string for writing Tables information
            string str = "";
            foreach (SeqString item in table_of_one)
            {
                str += item.word + '\t' + '*' + '\t' + item.freq + "\r\n";
            }
            str += "**********" + "\r\n";
            foreach (SeqString item in table_of_two)
            {
                str += item.word + '\t' + "**" + '\t' + item.freq + "\r\n";
            }
            str += "**********" + "\r\n";
            foreach (SeqString item in table_of_three)
            {
                str += item.word + '\t' + "***" + '\t' + item.freq + "\r\n";
            }
            str += "**********" + "\r\n";
            foreach (SeqString item in table_of_four)
            {
                str += item.word + '\t' + "****" + '\t' + item.freq + "\r\n";
            }
            str += "**********" + "\r\n";
            #endregion

            System.IO.File.WriteAllText(pathToFile, str);
        }

        protected void CountCharacters(string file)
        {
            List<char> chars = PureArticleForCharacters(file);
            List<SeqString> temp = (from c in chars.Cast<char>()
                                    group c by c into g
                                    select new SeqString { word = g.Key.ToString(), freq = g.Count() }).ToList();
            unichar.UnionWith(temp);
            total_chars += chars.Count;
        }

        protected void CountWords(string article)
        {

            List<string> words = PureArticle(article);
            List<SeqString> temp = (from word in words.Cast<string>()
                                    group word by word into g
                                    select new SeqString { word = g.Key, freq = g.Count() }).ToList();
            table_of_one.UnionWith(temp);
        }

        protected void CountPhrases(string article)
        {
            StreamReader stream = new StreamReader(article);
            string file = stream.ReadToEnd();
            file = file.ToLower();

            string[] sentences = Regex.Split(file, @"(?<=[\.!\?])\s+");

            //List<string> temp2 = PureArticle(article);


            for (int j = 0; j < sentences.Length; j++)
            {
                string[] words = PureSentense(sentences[j]);
                // words = sentences[j].Split(' ', '.', ',', ':', '\n', '\r', '(', ')', '=');
                List<SeqString> temp;

                //trim each string and get rid of any empty ones
                words = words.Select(t => t.Trim()).Where(t => t.Trim() != string.Empty).ToArray();

                Dictionary<string, int> Counts = new Dictionary<string, int>();

                #region get pharese of length of 2-4 words

                for (int phraseLen = 4; phraseLen >= 2; phraseLen--)
                {
                    for (int i = 0; i < words.Length - 1; i++)
                    {
                        //get the phrase to match based on phraselen
                        string[] phrase = GetPhrase(words, i, phraseLen);
                        string sphrase = string.Join(" ", phrase);

                        int index = FindPhraseIndex(words, i + phrase.Length, phrase);

                        /* if (index > -1)
                         {*/
                        if (!Counts.ContainsKey(sphrase))
                            Counts.Add(sphrase, 1);
                        else
                            Counts[sphrase]++;
                        /*  }*/
                    }

                    switch (phraseLen)
                    {
                        case 2:
                            temp = (from word in Counts
                                    group word by word into g
                                    where g.Key.Key.Count(x => x == ' ') + 1 == 2
                                    select new SeqString { word = g.Key.Key, freq = g.Key.Value }).ToList();
                            table_of_two.UnionWith(temp);

                            Counts.Clear();
                            break;
                        case 3:
                            temp = (from word in Counts
                                    group word by word into g
                                    where g.Key.Key.Count(x => x == ' ') + 1 == 3
                                    select new SeqString { word = g.Key.Key, freq = g.Key.Value }).ToList();
                            table_of_three.UnionWith(temp);

                            Counts.Clear();
                            break;
                        case 4:
                            temp = (from word in Counts
                                    group word by word into g
                                    where g.Key.Key.Count(x => x == ' ') + 1 == 4
                                    select new SeqString { word = g.Key.Key, freq = g.Key.Value }).ToList();
                            table_of_four.UnionWith(temp);

                            Counts.Clear();
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
        }


        protected void CountNCharacters(string article)
        {
            StreamReader stream = new StreamReader(article);
            string file = stream.ReadToEnd();
            file = file.ToLower();
            file = file.Replace("\n", "");
            file = file.Replace("\r", "");
            file = file.Replace("\t", "");
            file = file.Replace("\\", "");
            string temp = file;
            List<string> all = new List<string>();


            string[] sentences;
            if (Program.BiChars != 0)
            {
                sentences = Split(file, 2).ToArray();


                bichar.UnionWith((from x in sentences
                                  group x by x into g
                                  select new SeqString { word = g.Key, freq = g.Count() }));
            }


            if (Program.TriChars != 0)
            {
                sentences = Split(file, 3).ToArray();

                trichar.UnionWith((from x in sentences
                                   group x by x into g
                                   select new SeqString { word = g.Key, freq = g.Count() }));
            }

            if (Program.QuadChars != 0)
            {

                sentences = Split(file, 4).ToArray();


                quadchar.UnionWith((from x in sentences
                                    group x by x into g
                                    select new SeqString { word = g.Key, freq = g.Count() }));
            }

        }

        static IEnumerable<string> Split(string str, int chunkSize)
        {

            return Enumerable.Range(0, str.Length - (chunkSize - 1))
                .Select(i => str.Substring(i, chunkSize));
        }

        static string[] GetPhrase(string[] words, int startpos, int len)
        {
            return words.Skip(startpos).Take(len).ToArray();
        }

        static int FindPhraseIndex(string[] words, int startIndex, string[] matchWords)
        {
            for (int i = startIndex; i < words.Length; i++)
            {
                int j;

                for (j = 0; j < matchWords.Length && (i + j) < words.Length; j++)
                    if (matchWords[j] != words[i + j])
                        break;

                if (j == matchWords.Length)
                    return startIndex;
            }

            return -1;
        }

        protected List<string> PureArticle(string article)
        {
            StreamReader stream = new StreamReader(article);
            string file = stream.ReadToEnd();
            file = file.ToLower();
            //
            string[] maor = file.Split(' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@');
            List<string> temp = maor.ToList();
            if (Program.RemoveStopWords)
            {
                temp.RemoveAll(x => x.Length == 1 || x == "" || stopwords.Contains(x, StringComparer.OrdinalIgnoreCase));
                temp = temp.Where(w => w.Any(c => !Char.IsDigit(c))).ToList();
            }
            else
            {
                temp.RemoveAll(x => x.Length == 1 || x == "");
                temp = temp.Where(w => w.Any(c => !Char.IsDigit(c))).ToList();
            }
            stream.Close();
            return temp;
        }


        protected List<char> PureArticleForCharacters(string article)
        {
            StreamReader stream = new StreamReader(article);
            string file = stream.ReadToEnd();
            file = file.ToLower();
            //
            List<char> temp = file.ToCharArray().ToList();
            char[] crs = { '\\', '\t', '\n', '\r' };
            temp.RemoveAll(x => crs.Contains(x));
            stream.Close();
            return temp;
        }

        protected string[] PureSentense(string article)
        {
            string[] maor = article.Split(' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@');
            List<string> temp = maor.ToList();
            if (Program.RemoveStopWords)
            {
                temp.RemoveAll(x => x.Length == 1 || x == "" || stopwords.Contains(x, StringComparer.OrdinalIgnoreCase));
                temp = temp.Where(w => w.Any(c => !Char.IsDigit(c))).ToList();
            }
            else
            {
                temp.RemoveAll(x => x.Length == 1 || x == "");
                temp = temp.Where(w => w.Any(c => !Char.IsDigit(c))).ToList();
            }
            return temp.ToArray();
        }



        protected string[] GetFilesInDir(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dir = Directory.GetDirectories(path);
            if (dir.Length == 0)
            {
                return files;
            }
            List<string> l = files.ToList();
            foreach (var sub_path in dir)
            {
                l.AddRange(GetFilesInDir(sub_path));
            }
            return l.ToArray();
        }


        public int GetNumOfWords(string address)
        {
            TextReader tr = File.OpenText(address);

            int wordsCount = 0;

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
            tr.Close();
            return wordsCount;
        }

        public void SaveData(string Path)
        {
            if (Path.Equals(""))
                return;
            #region init
            string PORF = "";
            string temp = Path;
            string confress;
            string ACLYear;
            string OtherYear;
            string path;
            #endregion

            #region get information
            temp = temp.Remove(Path.LastIndexOf("\\"));
            ACLYear = temp.Substring(temp.LastIndexOf("\\") + 1);
            OtherYear = ACLYear.Substring((ACLYear.IndexOf('_') == -1) ? 0 : ACLYear.IndexOf('_') + 1, 4);
            PORF = ACLYear.Remove((ACLYear.IndexOf('_') == -1) ? 0 : ACLYear.IndexOf('_'));
            ACLYear = ACLYear.Substring((ACLYear.IndexOf('(') == -1) ? 0 : ACLYear.IndexOf('(') + 1, 4);
            temp = temp.Remove(temp.LastIndexOf("\\"));
            confress = "ACL";// temp.Substring((temp.LastIndexOf("\\") == -1) ? 0 : temp.LastIndexOf("\\") + 1);
            #endregion

            switch (confress)
            {
                case "ACL":
                case "PACLIC12":
                    path = @"Data Base\" + confress + "\\Tables_" + Path.Substring(temp.LastIndexOf("\\") + 1) + ".xml";
                    pathToFile = path;
                    if (File.Exists(path))
                        return;

                    break;
                default:
                    string FullOrPoster = Path.Remove(Path.LastIndexOf("_"));
                    FullOrPoster = FullOrPoster.Substring(FullOrPoster.LastIndexOf("\\") + 1);
                    path = @"Data Base\" + confress + "\\Tables" + "_" + FullOrPoster + "_" + Path.Substring(temp.LastIndexOf("\\") + 1) + ".xml";
                    pathToFile = path;
                    if (File.Exists(path))
                        return;
                    break;
            }
            XmlSerializer serialzer = new XmlSerializer(typeof(FreqWordsForOneArticle));
            TextWriter writer = new StreamWriter(path);
            serialzer.Serialize(writer, this);
        }

        public void ReadData(string Path)
        {
            if (Path.Equals(""))
                return;
            #region init
            string PORF = "";
            string temp = Path;
            string confress;
            string ACLYear;
            string OtherYear;
            string path;
            #endregion

            #region get information
            temp = temp.Remove(Path.LastIndexOf("\\"));
            ACLYear = temp.Substring(temp.LastIndexOf("\\") + 1);
            OtherYear = ACLYear.Substring((ACLYear.IndexOf('_') == -1) ? 0 : ACLYear.IndexOf('_') + 1, 4);
            PORF = ACLYear.Remove((ACLYear.IndexOf('_') == -1) ? 0 : ACLYear.IndexOf('_'));
            ACLYear = ACLYear.Substring((ACLYear.IndexOf('(') == -1) ? 0 : ACLYear.IndexOf('(') + 1, 4);
            temp = temp.Remove(temp.LastIndexOf("\\"));
            confress = "ACL";// temp.Substring((temp.LastIndexOf("\\") == -1) ? 0 : temp.LastIndexOf("\\") + 1);
            #endregion

            switch (confress)
            {
                case "ACL":
                case "PACLIC12":
                    path = @"Data Base\" + confress + "\\Tables_" + Path.Substring(temp.LastIndexOf("\\") + 1) + ".xml";
                    pathToFile = path;
                    if (File.Exists(path))
                        return;

                    break;
                default:
                    string FullOrPoster = Path.Remove(Path.LastIndexOf("_"));
                    FullOrPoster = FullOrPoster.Substring(FullOrPoster.LastIndexOf("\\") + 1);
                    path = @"Data Base\" + confress + "\\Tables" + "_" + FullOrPoster + "_" + Path.Substring(temp.LastIndexOf("\\") + 1) + ".xml";
                    pathToFile = path;
                    if (File.Exists(path))
                        return;
                    break;
            }

            XmlSerializer serialzer = new XmlSerializer(typeof(FreqWordsForOneArticle));
            TextReader reader = new StreamReader(path);
            FreqWordsForOneArticle t = (FreqWordsForOneArticle)serialzer.Deserialize(reader);
            this.table_of_four = t.table_of_four;
            this.table_of_one = t.table_of_one;
            this.table_of_three = t.table_of_three;
            this.table_of_two = t.table_of_two;
            this.stopwords = t.stopwords;
            this.articles = t.articles;
            this.isP = t.isP;
            this.pathToFile = t.pathToFile;
            this.year_dir = t.year_dir;
        }

    }
}
