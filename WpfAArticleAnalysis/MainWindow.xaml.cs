using ContentFamilies;
using PatternFamilies;
using Statistics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAArticleAnalysis
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static P_family[] p_arr;
        static P_family SaveData;
        static int numberOfRemoved;
        static public List<string> allGramsWords;
        static int startYear = 1998;
        static string dir_of_articles_folders = @"TXT\BOOKS";
        static string typeOfTweets = "JSON";
        static int INTERVAL_OF_YEAR_A = 3, INTERVAL_OF_YEAR_B = 5;
        static int TotalWords = 0;
        static double THRESHOLD = 0;
        static int TOP_SKIP = 50;
        private static int chooseMethod = 1;
        private static HashSet<SeqString> rare_uni_gram;
        private static HashSet<SeqString> rare_four_gram;
        private static HashSet<SeqString> rare_two_gram;
        private static HashSet<SeqString> rare_three_gram;
        private static HashSet<SeqString> uni_chars;
        private static HashSet<SeqString> bi_chars;
        private static HashSet<SeqString> tri_chars;
        private static HashSet<SeqString> quad_chars;
        private static HashSet<SeqString> rare_uni_chars;
        private static HashSet<SeqString> rare_bi_chars;
        private static HashSet<SeqString> rare_tri_chars;
        private static HashSet<SeqString> rare_quad_chars;
        private static bool FirstTime = true;
        private static string output_path;
        public delegate void LogDelegate(string str);
        public static event LogDelegate LogChanged;
        private string AritclePath;
        private Thread newLogThread;
        private bool MakingLog = false;
        private static string[] FILES_NAMES;

        public static LogWindow lg = null;
        private Thread newWindowThread = null;
        private Thread myThread = null;
        private bool OrthograficChecked;
        private bool QuantitativeChecked;
        private bool ReachnessLangChecked;
        private bool StemmerChecked;
        private bool TaggerChecked;
        int TrainingSetPres = 0;

        private void NewWindowHandler(object sender, RoutedEventArgs e)
        {
            newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }
        private void ThreadStartingPoint()
        {
            lg = new LogWindow();
            lg.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
        public MainWindow()
        {
            float tr = (float)(1.0 / 888888.0);
            InitializeComponent();

            NewWindowHandler(this, null);


            AnalysisMethod.Items.Add("Only Ngrams");
            AnalysisMethod.Items.Add("Only Stylistis and Tagger");
            AnalysisMethod.Items.Add("Both Ngrams and Other Families");
            AnalysisMethod.SelectedIndex = 0;

            ArticleDir.Text = @"TXT\BOOKS";

            Threshold.Text = "0";


            ReducingUniGrams.Items.Add("None of those");
            ReducingUniGrams.Items.Add("All of them");
            ReducingUniGrams.Items.Add("Each Article");
            ReducingUniGrams.SelectedIndex = 0;


            string[] strs = new string[] { "None", "10", "15", "20", "30", "33", "40", "50" };
            TraningSetNum.ItemsSource = strs;
            TraningSetNum.SelectedIndex = 0;
            TrainingSetPres = 0;

            UniGRams.Text = "500";
            Program.NUM_OF_ONE = 500;

            BiGRams.Text = "500";
            Program.NUM_OF_TWO = 500;

            TriGRams.Text = "500";
            Program.NUM_OF_THREE = 500;

            QuadGrams.Text = "500";
            Program.NUM_OF_FOUR = 500;

            RareUGRAMs.Text = "0";
            Program.RareUniGrams = 0;

            RareBGRAMS.Text = "0";
            Program.RareBiGrams = 0;

            RareTriGrams.Text = "0";
            Program.RareTriGrams = 0;

            RareQuadGrams.Text = "0";
            Program.RareQuadGrams = 0;

            UniChars.Text = "0";
            Program.UniChars = 0;

            BiChars.Text = "0";
            Program.BiChars = 0;

            TriChars.Text = "0";
            Program.TriChars = 0;

            QuadChars.Text = "0";
            Program.QuadChars = 0;

            RareUniChars.Text = "0";
            Program.RareUniChars = 0;

            RareBiChars.Text = "0";
            Program.RareBiChars = 0;

            RareTriChars.Text = "0";
            Program.RareTriChars = 0;

            RareQuadChars.Text = "0";
            Program.RareQuadChars = 0;

            FirstTime = false;

            //added By Yair
            initNormalUI();
        }
        private void make_csv_file(string dir_of_articles, string output_path)
        {

            string[] files = null;
            StreamWriter s = null;
            StreamReader r;
            try
            {
                files = GetFilesInDir(dir_of_articles);
                s = new StreamWriter(output_path, false);
            }
            catch
            {
                MessageBox.Show("dir is not currect ot the csv file is open");
                return;
            }
            //
            s.AutoFlush = true;
            //
            if (chooseMethod == 2 || chooseMethod == 3)
            {
                make_Cons_txt_files(files);
            }
            string[] files_names = files.Where(str => !str.Contains("_tagger_output")).ToArray();
            FILES_NAMES = files_names;
            //
            if (!(chooseMethod == 2))
            {
                MakePFamily();

                /*
                MakePFamily(files_names);
                 * */
                bool log = false;
                this.Dispatcher.Invoke((Action)(() =>
                {
                    log = (bool)MakeLogFiles.IsChecked;
                }));
                if (log)
                    MakeTxtLogNgramsAndNChars(p_arr);

                MakeNGRAMWords();
            }

            #region Headlines
            printHead(s);
            #endregion
            #region print the files in directory
            int run_num = 1;

            foreach (string name_of_file in files_names)
            {
                r = new StreamReader(name_of_file);
                lg.ClearText();
#if OnlyUniGram
				Console.WriteLine("Only Uni-gram");
#endif
#if A_Way
                Console.WriteLine("Way: A");
#endif
#if B_Way
				Console.WriteLine("Way: B");
#endif
                LogChanged("\t---\nThe program calculates :");
                LogChanged("\n" + "no. " + run_num + " of " + files_names.Length + "\t" + ((float)run_num * 100 / files_names.Length).ToString("f2") + "%");
                print(name_of_file, s, r, run_num);
                run_num++;
                r.Close();
            }

            #endregion
            s.Close();
        }
        private void MakeTxtLogNgramsAndNChars(P_family[] arr, bool all = true)
        {
            return;
            newLogThread = new Thread(new ThreadStart(ThreadLog));
            newLogThread.SetApartmentState(ApartmentState.STA);
            newLogThread.IsBackground = true;
            newLogThread.Start();
            DirectoryInfo[] dirs = GetDirsFromPath();
            if (Directory.Exists("Logs\\"))
            {
                var dir = new DirectoryInfo("Logs\\");
                dir.Delete(true);
            }
            Directory.CreateDirectory("Logs\\");

            StreamWriter s = null;


            foreach (DirectoryInfo item in dirs)
            {
                if (File.Exists("LogMakerArgs"))
                {
                    File.Delete("LogMakerArgs");
                }
                s = new StreamWriter("LogMakerArgs");
                s.Write(item.ToString() + "\n");
                s.Write(Program.NUM_OF_ONE + "\n");
                s.Write(Program.RareUniGrams + "\n");
                s.Write(Program.NUM_OF_TWO + "\n");
                s.Write(Program.RareBiGrams + "\n");
                s.Write(Program.NUM_OF_THREE + "\n");
                s.Write(Program.RareTriGrams + "\n");
                s.Write(Program.NUM_OF_FOUR + "\n");
                s.Write(Program.RareQuadGrams + "\n");
                s.Close();

                string strCmdText;
                strCmdText = "/C LogMaker.exe";
                System.Diagnostics.Process process = System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                process.WaitForExit();

            }
            if (File.Exists("LogMakerArgs"))
            {
                File.Delete("LogMakerArgs");
            }
            s = new StreamWriter("LogMakerArgs");
            s.Write("all" + "\n");
            s.Write(Program.NUM_OF_ONE + "\n");
            s.Write(Program.RareUniGrams + "\n");
            s.Write(Program.NUM_OF_TWO + "\n");
            s.Write(Program.RareBiGrams + "\n");
            s.Write(Program.NUM_OF_THREE + "\n");
            s.Write(Program.RareTriGrams + "\n");
            s.Write(Program.NUM_OF_FOUR + "\n");
            s.Write(Program.RareQuadGrams + "\n");
            s.Close();


            System.Diagnostics.Process process2 = System.Diagnostics.Process.Start("CMD.exe", "/C LogMaker.exe");
            process2.WaitForExit();
            newLogThread.Abort();

        }
        private static void DisposeHashSet(HashSet<SeqString> set, HashSet<SeqString> FreqSet, HashSet<SeqString> RareSet)
        {
            if (set != null)
            {
                foreach (var d in set)
                {
                    d.Dispose();
                }
            }
            if (FreqSet != null)
            {
                foreach (var d in FreqSet)
                {
                    d.Dispose();
                }
            }
            if (RareSet != null)
            {
                foreach (var d in RareSet)
                {
                    d.Dispose();
                }
            }
            set = null;
            FreqSet = null;
            RareSet = null;
            GC.Collect();
        }
        private void MakePFamily()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                MakePFamily(FILES_NAMES);
            }));

        }
        private void MakePFamily(string[] files_names)
        {
            int i = 0;
            if ((bool)DomainsCounter.IsChecked)
            {
                DirectoryInfo[] dirs = GetDirsFromPath();
                p_arr = new P_family[dirs.Length];


                foreach (DirectoryInfo item in dirs)
                {
                    lg.ClearText();
                    lg.SetText("Domain: " + item + " Proccesing");
                    p_arr[i] = new P_family(files_names.Where(x => x.Contains(item.ToString())).ToArray());
                    p_arr[i].ResizeTables();
                    TotalWords += p_arr[i].total_words;
                    i++;
                }

                SaveData = P_family.GetTmpCopy(p_arr[0]);
                return;
            }

            p_arr = new P_family[files_names.Length];
            i = 0;
            foreach (string item in files_names)
            {
                lg.ClearText();
                lg.SetText("File: " + item + "\nFile Number: " + (i + 1) + " From "
                    + files_names.Length + "\n" + Math.Round(((i + 1) / (float)files_names.Length) * 100, 2) + "%");
                p_arr[i] = new P_family(item);
                p_arr[i].ResizeTables();
                TotalWords += p_arr[i].total_words;
                i++;
            }

            //SaveData = P_family.GetTmpCopy(p_arr[0]);

            return;
            P_family.Serialize(p_arr.ToList());
        }
        private static string[] GetFilesInDir(string path)
        {

            string[] files = Directory.GetFiles(path);
            string[] dir = Directory.GetDirectories(path);
            if (dir.Length == 0)
                return files;
            List<string> l = files.ToList();
            foreach (var sub_path in dir)
            {
                l.AddRange(GetFilesInDir(sub_path));
            }
            return l.ToArray();
        }
        private void make_Cons_txt_files(string[] names_of_files)
        {
            StreamReader st;
            StreamWriter write;
            string words;
            int count = 1;
            foreach (var file in names_of_files)
            {
                lg.SetText("Making the 500, 1000, 1500 words files.\t" + ((float)count * 100 / names_of_files.Length).ToString("f2") + "%");
                count++;
                if (File.Exists(file.Insert(file.IndexOf('\\'), "of500")) && File.Exists(file.Insert(file.IndexOf('\\'), "of1000")) && File.Exists(file.Insert(file.IndexOf('\\'), "of1500")))
                    continue;
                st = new StreamReader(file);
                words = st.ReadToEnd();
                st.Close();
                int i = words.IndexOf("Abstract", StringComparison.CurrentCultureIgnoreCase);
                if (i < 0)
                {
                    i = words.IndexOf("A str ct", StringComparison.CurrentCultureIgnoreCase);
                    if (i < 0)
                        i = 0;
                }
                words = words.Substring(i);
                {
                    int counter = 0;
                    int ch_counter = 0;
                    string words500;
                    foreach (var ch in words)
                    {
                        ch_counter++;
                        if (ch == ' ')
                            counter++;
                        if (counter == 500)
                            break;
                    }
                    words500 = words.Substring(0, ch_counter);
                    if (!Directory.Exists(file.Substring(0, file.LastIndexOf('\\')).Insert(file.IndexOf('\\'), "of500")))
                        Directory.CreateDirectory(file.Substring(0, file.LastIndexOf('\\')).Insert(file.IndexOf('\\'), "of500"));
                    write = new StreamWriter(file.Insert(file.IndexOf('\\'), "of500"));
                    write.Write(words500);
                    write.Close();
                }
                {
                    int counter = 0;
                    int ch_counter = 0;
                    string words1000;
                    foreach (var ch in words)
                    {
                        ch_counter++;
                        if (ch == ' ')
                            counter++;
                        if (counter == 1000)
                            break;
                    }
                    words1000 = words.Substring(0, ch_counter);
                    if (!Directory.Exists(file.Substring(0, file.LastIndexOf('\\')).Insert(file.IndexOf('\\'), "of1000")))
                        Directory.CreateDirectory(file.Substring(0, file.LastIndexOf('\\')).Insert(file.IndexOf('\\'), "of1000"));
                    write = new StreamWriter(file.Insert(file.IndexOf('\\'), "of1000"));
                    write.Write(words1000);
                    write.Close();
                }
                {
                    int counter = 0;
                    int ch_counter = 0;
                    string words1500;
                    foreach (var ch in words)
                    {
                        ch_counter++;
                        if (ch == ' ')
                            counter++;
                        if (counter == 1500)
                            break;
                    }
                    words1500 = words.Substring(0, ch_counter);
                    if (!Directory.Exists(file.Substring(0, file.LastIndexOf('\\')).Insert(file.IndexOf('\\'), "of1500")))
                        Directory.CreateDirectory(file.Substring(0, file.LastIndexOf('\\')).Insert(file.IndexOf('\\'), "of1500"));
                    write = new StreamWriter(file.Insert(file.IndexOf('\\'), "of1500"));
                    write.Write(words1500);
                    write.Close();
                }
                write.Close();
            }
            lg.ClearText();
        }
        public void ThreadLog()
        {
            StringBuilder str = new StringBuilder("");
            while (true)
            {
                for (int i = 1; i <= 3; i++)
                {
                    str.Clear();
                    str = str.Append('.', i);
                    lg.SetText("Proccessing Logs and Making NGRAMS and NCHARS" + str);
                }

            }
        }
        static Random random = new Random();
        public static List<int> GenerateRandom(int count)
        {
            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();
            while (candidates.Count < count)
            {
                // May strike a duplicate.
                candidates.Add(random.Next(p_arr.Length - 1));
            }

            // load them in to a list.
            List<int> result = new List<int>();
            result.AddRange(candidates);

            // shuffle the results:
            int i = result.Count;
            while (i > 1)
            {
                i--;
                int k = random.Next(i + 1);
                int value = result[k];
                result[k] = result[i];
                result[i] = value;
            }
            return result;
        }
        public HashSet<SeqString> CountFreqs(P_family[] arr, string table)
        {

            HashSet<SeqString> tmp = new HashSet<SeqString>();

            switch (table)
            {
                case "table_of_one":
                    if (Program.NUM_OF_ONE == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting uni gram words");
                    foreach (var item in arr)
                    {
                        foreach (var w in item.table_of_one)
                        {
                            if (tmp.Contains(w))
                            {
                                tmp.LastOrDefault(x => x.Equals(w)).freq += w.freq;
                            }
                            else
                                tmp.Add(new SeqString { freq = w.freq, word = w.word });
                        }
                    }
                    break;
                case "table_of_two":
                    if (Program.NUM_OF_TWO == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting BI gram words");
                    foreach (var item in arr)
                    {
                        foreach (var w in item.table_of_two)
                        {
                            if (tmp.Contains(w))
                            {
                                tmp.LastOrDefault(x => x.Equals(w)).freq += w.freq;
                            }
                            else
                                tmp.Add(new SeqString { freq = w.freq, word = w.word });
                        }
                    }
                    break;
                case "table_of_three":
                    if (Program.NUM_OF_THREE == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting TRI gram words");
                    foreach (var item in arr)
                    {
                        foreach (var w in item.table_of_three)
                        {
                            if (tmp.Contains(w))
                            {
                                tmp.LastOrDefault(x => x.Equals(w)).freq += w.freq;
                            }
                            else
                                tmp.Add(new SeqString { freq = w.freq, word = w.word });
                        }
                    }
                    break;
                case "table_of_four":
                    if (Program.NUM_OF_FOUR == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting QUAD gram words");
                    foreach (var item in arr)
                    {
                        foreach (var w in item.table_of_four)
                        {
                            if (tmp.Contains(w))
                            {
                                tmp.LastOrDefault(x => x.Equals(w)).freq += w.freq;
                            }
                            else
                                tmp.Add(new SeqString { freq = w.freq, word = w.word });
                        }
                    }
                    break;
                case "unichar":
                    if (Program.UniChars == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting UNI gram CHARS");
                    foreach (var item in arr)
                    {
                        foreach (var w in item.unichar)
                        {
                            if (tmp.Contains(w))
                            {
                                tmp.LastOrDefault(x => x.Equals(w)).freq += w.freq;
                            }
                            else
                                tmp.Add(new SeqString { freq = w.freq, word = w.word });
                        }
                    }
                    break;
                case "bichar":
                    if (Program.BiChars == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting BI gram CHARS");
                    foreach (var item in arr)
                    {
                        foreach (var w in item.bichar)
                        {
                            if (tmp.Contains(w))
                            {
                                tmp.LastOrDefault(x => x.Equals(w)).freq += w.freq;
                            }
                            else
                                tmp.Add(new SeqString { freq = w.freq, word = w.word });
                        }
                    }
                    break;
                case "trichar":
                    if (Program.TriChars == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting TRI gram CHARS");
                    foreach (var item in arr)
                    {
                        foreach (var w in item.trichar)
                        {
                            if (tmp.Contains(w))
                            {
                                tmp.LastOrDefault(x => x.Equals(w)).freq += w.freq;
                            }
                            else
                                tmp.Add(new SeqString { freq = w.freq, word = w.word });
                        }
                    }
                    break;
                case "quadchar":
                    if (Program.QuadChars == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting QUAD gram CHARS");
                    foreach (var item in arr)
                    {
                        foreach (var w in item.quadchar)
                        {
                            if (tmp.Contains(w))
                            {
                                tmp.LastOrDefault(x => x.Equals(w)).freq += w.freq;
                            }
                            else
                                tmp.Add(new SeqString { freq = w.freq, word = w.word });
                        }
                    }
                    break;
            }

            return tmp;
        }
        private void MakeNGRAMWords()
        {
            P_family[] arr = null;



            if (TrainingSetPres != 0)
            {
                int amount = (int)Math.Ceiling((p_arr.Length * (((float)TrainingSetPres)) / 100));
                List<int> rnds = GenerateRandom(amount);
                arr = p_arr.OrderBy(r => random.Next()).Take(amount).ToArray();
            }
            else
                arr = p_arr;



            HashSet<SeqString> uni_gram = CountFreqs(arr, "table_of_one");


            if (Program.ForOneArticle.Equals("A"))
            {
                uni_gram.RemoveWhere(x => x.freq < 3);
            }
            rare_uni_gram = null;
            if (Program.RareUniGrams != 0)
            {
                rare_uni_gram = new HashSet<SeqString>(
                    (new HashSet<SeqString>(uni_gram.OrderBy(x => x.freq))
                    .Take(Program.RareUniGrams)));
            }




            uni_gram = new HashSet<SeqString>((new HashSet<SeqString>(uni_gram.OrderByDescending(x => x.freq)).Take(ContentFamilies.Program.NUM_OF_ONE)));

            /*    if (Program.RareUniGrams != 0)
                {
                    uni_gram.UnionWith(rare_uni_gram);
                }*/






#if !OnlyUniGram
            HashSet<SeqString> two_gram = CountFreqs(arr, "table_of_two");

            rare_two_gram = null;
            if (Program.RareBiGrams != 0)
            {
                rare_two_gram = new HashSet<SeqString>(
                    (new HashSet<SeqString>(two_gram.OrderBy(x => x.freq))
                    .Take(Program.RareBiGrams)));
            }


            two_gram = new HashSet<SeqString>((new HashSet<SeqString>(two_gram.OrderByDescending(x => x.freq)).Take(ContentFamilies.Program.NUM_OF_TWO)));

            ///////
            HashSet<SeqString> three_gram = CountFreqs(arr, "table_of_three");
            rare_three_gram = null;
            if (Program.RareTriGrams != 0)
            {
                rare_three_gram = new HashSet<SeqString>(
                    (new HashSet<SeqString>(three_gram.OrderBy(x => x.freq))
                    .Take(Program.RareTriGrams)));
            }


            three_gram = new HashSet<SeqString>((new HashSet<SeqString>(three_gram.OrderByDescending(x => x.freq)).Take(Program.NUM_OF_THREE)));


            ////////
            HashSet<SeqString> four_gram = CountFreqs(arr, "table_of_four");
            rare_four_gram = null;
            if (Program.RareQuadGrams != 0)
            {
                rare_four_gram = new HashSet<SeqString>(
                    (new HashSet<SeqString>(four_gram.OrderBy(x => x.freq))
                    .Take(Program.RareQuadGrams)));
            }

            four_gram = new HashSet<SeqString>((new HashSet<SeqString>(four_gram.OrderByDescending(x => x.freq)).Take(ContentFamilies.Program.NUM_OF_FOUR)));


#endif
            //
#if A_Way
            numberOfRemoved = P_family.FilterByThreshold(ref uni_gram, TotalWords);
#endif
#if B_Way
			P_family.FilterByTopCount(ref uni_gram);
#endif
            //
            allGramsWords = new List<string>();
            allGramsWords.AddRange((from y in uni_gram
                                    select y.word).ToArray());
            if (Program.RareUniGrams != 0)
            {
                allGramsWords = new List<string>(allGramsWords.Union((from y in rare_uni_gram
                                                                      select y.word).ToArray()));
            }
#if !OnlyUniGram
            allGramsWords.AddRange((from y in two_gram
                                    select y.word).ToArray());
            if (Program.RareBiGrams != 0)
            {
                allGramsWords = new List<string>(allGramsWords.Union((from y in rare_two_gram
                                                                      select y.word).ToArray()));
            }
            allGramsWords.AddRange((from y in three_gram
                                    select y.word).ToArray());
            if (Program.RareTriGrams != 0)
            {
                allGramsWords = new List<string>(allGramsWords.Union((from y in rare_three_gram
                                                                      select y.word).ToArray()));
            }
            allGramsWords.AddRange((from y in four_gram
                                    select y.word).ToArray());

#endif



            if (Program.RareQuadGrams != 0)
            {
                allGramsWords = new List<string>(allGramsWords.Union((from y in rare_four_gram
                                                                      select y.word).ToArray()));
            }

            #region N-chars
            uni_chars = CountFreqs(arr, "unichar");

            rare_uni_chars = new HashSet<SeqString>(
                uni_chars.OrderBy(x => x.freq).Take(Program.RareUniChars));

            uni_chars = new HashSet<SeqString>(uni_chars.OrderByDescending(x => x.freq).Take(Program.UniChars));



            bi_chars = CountFreqs(arr, "bichar");

            rare_bi_chars = new HashSet<SeqString>(
                bi_chars.OrderBy(x => x.freq).Take(Program.RareBiChars));

            bi_chars = new HashSet<SeqString>(bi_chars.OrderByDescending(x => x.freq).Take(Program.BiChars));





            tri_chars = CountFreqs(arr, "trichar");

            rare_tri_chars = new HashSet<SeqString>(
                tri_chars.OrderBy(x => x.freq).Take(Program.RareTriChars));

            tri_chars = new HashSet<SeqString>(tri_chars.OrderByDescending(x => x.freq).Take(Program.TriChars));





            quad_chars = CountFreqs(arr, "quadchar");

            rare_quad_chars = new HashSet<SeqString>(
                quad_chars.OrderBy(x => x.freq).Take(Program.RareQuadChars));

            quad_chars = new HashSet<SeqString>(quad_chars.OrderByDescending(x => x.freq).Take(Program.QuadChars));



            allGramsWords = new List<string>(allGramsWords.Union((from x in uni_chars
                                                                  select x.word)));
            allGramsWords = new List<string>(allGramsWords.Union((from x in rare_uni_chars
                                                                  select x.word)));
            allGramsWords = new List<string>(allGramsWords.Union((from x in bi_chars
                                                                  select x.word)));
            allGramsWords = new List<string>(allGramsWords.Union((from x in rare_bi_chars
                                                                  select x.word)));
            allGramsWords = new List<string>(allGramsWords.Union((from x in tri_chars
                                                                  select x.word)));
            allGramsWords = new List<string>(allGramsWords.Union((from x in rare_tri_chars
                                                                  select x.word)));
            allGramsWords = new List<string>(allGramsWords.Union((from x in quad_chars
                                                                  select x.word)));



            allGramsWords = new List<string>(allGramsWords.Union((from x in rare_quad_chars
                                                                  select x.word)));



            #endregion
            //

            allGramsWords = allGramsWords.Distinct().ToList();
            // allGramsWords = new List<string>(allGramsWords.OrderBy(x => x.Count(y => y == ' ')));

            //p_arr[0] = P_family.GetTmpCopy(SaveData);

            return;
            if (MakingLog)
            {
                foreach (var item in arr)
                {
                    item.Dispose();
                }

            }

            arr = null;
            p_arr = null;
            GC.Collect();
            p_arr = P_family.DeSerialize().ToArray();
        }
        private void printHead(StreamWriter s)
        {
            StringBuilder headsave = new StringBuilder();
            if (chooseMethod == 2 || chooseMethod == 3)
            {
                #region OrthographicFamily - 20
                if (OrthograficChecked)
                {
                    for (int i = 0; i < OrthographicFamily.symbols.Length; i++)
                    {
                        switch (OrthographicFamily.symbols[i])
                        {
                            case '\'':
                                headsave.Append("geresh_in_letters_normal,");
                                break;
                            case ',':
                                headsave.Append("comma_in_letters_normal,");
                                break;
                            case '"':
                                headsave.Append("double_geresh_in_letters_normal,");
                                break;
                            case '-':
                                headsave.Append("dash_in_letters_normal,");
                                break;
                            default:
                                headsave.Append(OrthographicFamily.symbols[i] + "_in_letters_normal,");
                                break;
                        }
                    }
                    for (int i = 0; i < OrthographicFamily.symbols.Length; i++)
                    {
                        switch (OrthographicFamily.symbols[i])
                        {
                            case '\'':
                                headsave.Append("geresh_in_words_normal,");
                                break;
                            case ',':
                                headsave.Append("comma_in_words_normal,");
                                break;
                            case '"':
                                headsave.Append("double_geresh_in_words_normal,");
                                break;
                            case '-':
                                headsave.Append("dash_in_words_normal,");
                                break;
                            default:
                                headsave.Append(OrthographicFamily.symbols[i] + "_in_words_normal,");
                                break;
                        }
                    }
                    //
                    for (int i = 0; i < OrthographicFamily.symbols.Length; i++)
                    {
                        switch (OrthographicFamily.symbols[i])
                        {
                            case '\'':
                                headsave.Append("geresh_in_letters_normal_of500,");
                                break;
                            case ',':
                                headsave.Append("comma_in_letters_normal_of500,");
                                break;
                            case '"':
                                headsave.Append("double_geresh_in_letters_normal_of500,");
                                break;
                            case '-':
                                headsave.Append("dash_in_letters_normal_of500,");
                                break;
                            default:
                                headsave.Append(OrthographicFamily.symbols[i] + "_in_letters_normal_of500,");
                                break;
                        }
                    }
                    for (int i = 0; i < OrthographicFamily.symbols.Length; i++)
                    {
                        switch (OrthographicFamily.symbols[i])
                        {
                            case '\'':
                                headsave.Append("geresh_in_words_normal_of500,");
                                break;
                            case ',':
                                headsave.Append("comma_in_words_normal_of500,");
                                break;
                            case '"':
                                headsave.Append("double_geresh_in_words_normal_of500,");
                                break;
                            case '-':
                                headsave.Append("dash_in_words_normal_of500,");
                                break;
                            default:
                                headsave.Append(OrthographicFamily.symbols[i] + "_in_words_normal_of500,");
                                break;
                        }
                    }
                    for (int i = 0; i < OrthographicFamily.symbols.Length; i++)
                    {
                        switch (OrthographicFamily.symbols[i])
                        {
                            case '\'':
                                headsave.Append("geresh_in_letters_normal_of1000,");
                                break;
                            case ',':
                                headsave.Append("comma_in_letters_normal_of1000,");
                                break;
                            case '"':
                                headsave.Append("double_geresh_in_letters_normal_of1000,");
                                break;
                            case '-':
                                headsave.Append("dash_in_letters_normal_of1000,");
                                break;
                            default:
                                headsave.Append(OrthographicFamily.symbols[i] + "_in_letters_normal_of1000,");
                                break;
                        }
                    }
                    for (int i = 0; i < OrthographicFamily.symbols.Length; i++)
                    {
                        switch (OrthographicFamily.symbols[i])
                        {
                            case '\'':
                                headsave.Append("geresh_in_words_normal_of1000,");
                                break;
                            case ',':
                                headsave.Append("comma_in_words_normal_of1000,");
                                break;
                            case '"':
                                headsave.Append("double_geresh_in_words_normal_of1000,");
                                break;
                            case '-':
                                headsave.Append("dash_in_words_normal_of1000,");
                                break;
                            default:
                                headsave.Append(OrthographicFamily.symbols[i] + "_in_words_normal_of1000,");
                                break;
                        }
                    }
                    for (int i = 0; i < OrthographicFamily.symbols.Length; i++)
                    {
                        switch (OrthographicFamily.symbols[i])
                        {
                            case '\'':
                                headsave.Append("geresh_in_letters_normal_of1500,");
                                break;
                            case ',':
                                headsave.Append("comma_in_letters_normal_of1500,");
                                break;
                            case '"':
                                headsave.Append("double_geresh_in_letters_normal_of1500,");
                                break;
                            case '-':
                                headsave.Append("dash_in_letters_normal_of1500,");
                                break;
                            default:
                                headsave.Append(OrthographicFamily.symbols[i] + "_in_letters_normal_of1500,");
                                break;
                        }
                    }
                    for (int i = 0; i < OrthographicFamily.symbols.Length; i++)
                    {
                        switch (OrthographicFamily.symbols[i])
                        {
                            case '\'':
                                headsave.Append("geresh_in_words_normal_of1500,");
                                break;
                            case ',':
                                headsave.Append("comma_in_words_normal_of1500,");
                                break;
                            case '"':
                                headsave.Append("double_geresh_in_words_normal_of1500,");
                                break;
                            case '-':
                                headsave.Append("dash_in_words_normal_of1500,");
                                break;
                            default:
                                headsave.Append(OrthographicFamily.symbols[i] + "_in_words_normal_of1500,");
                                break;
                        }
                    }
                }
                #endregion

                #region quantitativeCharacteristicsFamily - 3
                if (QuantitativeChecked)
                {
                    headsave.Append("averageCharacterPerWords,averageCharacterPerSentences,averageWordsPerSentences,");
                }
                #endregion
                #region ReachnessLang - 6 (*4) - 24
                if (ReachnessLangChecked)
                {
                    headsave.Append("difWordsNormalized,oneWordsNormalized,twoWordsNormalized,threeWordsNormalized,fourWordsNormalized,fiveWordsNormalized,");
                    headsave.Append("difWords_of500Normalized,oneWords_of500Normalized,twoWords_of500Normalized,threeWords_of500Normalized,fourWords_of500Normalized,fiveWords_of500Normalized,");
                    headsave.Append("difWords_of1000Normalized,oneWords_of1000Normalized,twoWords_of1000Normalized,threeWords_of1000Normalized,fourWords_of1000Normalized,fiveWords_of1000Normalized,");
                    headsave.Append("difWords_of1500Normalized,oneWords_of1500Normalized,twoWords_of1500Normalized,threeWords_of1500Normalized,fourWords_of1500Normalized,fiveWords_of1500Normalized,");
                }
                #endregion
                #region Stemmer - 12 (*4) - 48
                if (StemmerChecked)
                {
                    headsave.Append("difStemsNormalized,oneStemsNormalized,twoStemsNormalized,threeStemsNormalized,fourStemsNormalized,fiveStemsNormalized,");
                    headsave.Append("difStemsNormalizedByStem,oneStemsNormalizedByStem,twoStemsNormalizedByStem,threeStemsNormalizedByStem,fourStemsNormalizedByStem,fiveStemsNormalizedByStem,");
                    headsave.Append("difStems_of500Normalized,oneStems_of500Normalized,twoStems_of500Normalized,threeStems_of500Normalized,fourStems_of500Normalized,fiveStems_of500Normalized,");
                    headsave.Append("difStems_of500NormalizedByStem,oneStems_of500NormalizedByStem,twoStems_of500NormalizedByStem,threeStems_of500NormalizedByStem,fourStems_of500NormalizedByStem,fiveStems_of500NormalizedByStem,");
                    headsave.Append("difStems_of1000Normalized,oneStems_of1000Normalized,twoStems_of1000Normalized,threeStems_of1000Normalized,fourStems_of1000Normalized,fiveStems_of1000Normalized,");
                    headsave.Append("difStems_of1000NormalizedByStem,oneStems_of1000NormalizedByStem,twoStems_of1000NormalizedByStem,threeStems_of1000NormalizedByStem,fourStems_of1000NormalizedByStem,fiveStems_of1000NormalizedByStem,");
                    headsave.Append("difStems_of1500Normalized,oneStems_of1500Normalized,twoStems_of1500Normalized,threeStems_of1500Normalized,fourStems_of1500Normalized,fiveStems_of1500Normalized,");
                    headsave.Append("difStems_of1500NormalizedByStem,oneStems_of1500NormalizedByStem,twoStems_of1500NormalizedByStem,threeStems_of1500NormalizedByStem,fourStems_of1500NormalizedByStem,fiveStems_of1500NormalizedByStem,");
                }
                #endregion
                #region Tagger - 72 (*4) - 288
                if (TaggerChecked)
                {
                    string type = "";
                    for (int i = 0; i < Tagger.NUM_OF_TAGS; i++)
                    {
                        type = ((TagTypes)i).ToString().Replace('_', '$');
                        headsave.Append(type + "_normalize_by_sentences_count,");
                    }
                    for (int i = 0; i < Tagger.NUM_OF_TAGS; i++)
                    {
                        type = ((TagTypes)i).ToString().Replace('_', '$');
                        headsave.Append(type + "_normalize_by_words_count,");
                    }
                    for (int i = 0; i < Tagger.NUM_OF_TAGS; i++)
                    {
                        type = ((TagTypes)i).ToString().Replace('_', '$');
                        headsave.Append(type + "_of500_normalize_by_sentences_count,");
                    }
                    for (int i = 0; i < Tagger.NUM_OF_TAGS; i++)
                    {
                        type = ((TagTypes)i).ToString().Replace('_', '$');
                        headsave.Append(type + "_of500_normalize_by_words_count,");
                    }
                    for (int i = 0; i < Tagger.NUM_OF_TAGS; i++)
                    {
                        type = ((TagTypes)i).ToString().Replace('_', '$');
                        headsave.Append(type + "_of1000_normalize_by_sentences_count,");
                    }
                    for (int i = 0; i < Tagger.NUM_OF_TAGS; i++)
                    {
                        type = ((TagTypes)i).ToString().Replace('_', '$');
                        headsave.Append(type + "_of1000_normalize_by_words_count,");
                    }
                    for (int i = 0; i < Tagger.NUM_OF_TAGS; i++)
                    {
                        type = ((TagTypes)i).ToString().Replace('_', '$');
                        headsave.Append(type + "_of1500_normalize_by_sentences_count,");
                    }
                    for (int i = 0; i < Tagger.NUM_OF_TAGS; i++)
                    {
                        type = ((TagTypes)i).ToString().Replace('_', '$');
                        headsave.Append(type + "_of1500_normalize_by_words_count,");
                    }
                }
                #endregion
            }
            #region P Family - 100+50+30+15 - 195
            if (chooseMethod != 2)
            {
                int i = 0;
                foreach (string word in allGramsWords)
                {
                    i++;
                    int numOfWords = word.Count(x => x == ' ') + 1;
                    string ToAdd = word.Replace("'", "_tag_") + "_" + (numOfWords) + "_" + "gram";
                    switch (numOfWords)
                    {
                        case 1:
                            if (Program.RareUniGrams != 0 && ((from x in rare_uni_gram
                                                               select x.word).ToList()).Contains(word))
                                ToAdd = ToAdd.Replace("gram", "rare_gram");
                            break;
                        case 2:
                            if (Program.RareBiGrams != 0 && ((from x in rare_two_gram
                                                              select x.word).ToList()).Contains(word))
                                ToAdd = ToAdd.Replace("gram", "rare_gram");
                            break;
                        case 3:
                            if (Program.RareTriGrams != 0 && ((from x in rare_three_gram
                                                               select x.word).ToList()).Contains(word))
                                ToAdd = ToAdd.Replace("gram", "rare_gram");
                            break;
                        case 4:
                            if (Program.RareQuadGrams != 0 && ((from x in rare_four_gram
                                                                select x.word).ToList()).Contains(word))
                                ToAdd = ToAdd.Replace("gram", "rare_gram");
                            break;
                    }

                    ToAdd = CheckIfChars(ToAdd, word);
                    if (i > 1260)
                    {
                        var sss = ToAdd;
                    }
                    ToAdd = ToAdd.Replace("'", "_tag_");
                    ToAdd = ToAdd.Replace("\"", "_Apostrophes_");
                    ToAdd = ToAdd.Replace("‘", "_Apostrophe_");
                    ToAdd = ToAdd.Replace(",", "_Comma_");
                    headsave.Append(ToAdd + ",");
                }
            }
            #endregion
            //
            #region info - 4
            // headsave.Append("year,");
            if (Program.THRESHOLD != 0) headsave.Append("name,");
            else headsave.Append("name");
            // headsave.Append("interval_of_" + INTERVAL_OF_YEAR_A + "_years,");
            // headsave.Append("interval_of_" + INTERVAL_OF_YEAR_B + "_years,");
            #endregion
#if A_Way
          if(Program.THRESHOLD!=0)  headsave.Append("removed: " + numberOfRemoved);
#endif
            //
            s.WriteLine(headsave);
        }
        private string CheckIfChars(string toAdd, string word)
        {
            if ((from x in uni_chars
                 select x.word).Contains(word))
            {
                return word + "_1_chars";
            }
            if ((from x in bi_chars
                 select x.word).Contains(word))
            {
                return word + "_2_chars";
            }
            if ((from x in tri_chars
                 select x.word).Contains(word))
            {
                return word + "_3_chars";
            }
            if ((from x in quad_chars
                 select x.word).Contains(word))
            {
                return word + "_4_chars";
            }
            if ((from x in rare_uni_chars
                 select x.word).Contains(word))
            {
                return word + "_1_rare_chars";
            }
            if ((from x in rare_bi_chars
                 select x.word).Contains(word))
            {
                return word + "_2_rare_chars";
            }
            if ((from x in rare_tri_chars
                 select x.word).Contains(word))
            {
                return word + "_3_rare_chars";
            }
            if ((from x in rare_quad_chars
                 select x.word).Contains(word))
            {
                return word + "_4_rare_chars";
            }

            return toAdd;
        }
        private void print(string name_of_file, StreamWriter writer, StreamReader reader, int i)
        {
            StringBuilder save = new StringBuilder();
            //
            if (chooseMethod == 2 || chooseMethod == 3)
            {

                #region Orthografic
                if (OrthograficChecked)
                {
                    lg.addText("\n" + "\tOrthographic family...");
                    OrthographicFamily Ortho = new OrthographicFamily(name_of_file);
                    save.Append(Ortho.ToString());
                    OrthographicFamily Ortho500 = new OrthographicFamily(name_of_file.Insert(name_of_file.IndexOf('\\'), "of500"));
                    save.Append(Ortho500.ToString());
                    OrthographicFamily Ortho1000 = new OrthographicFamily(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1000"));
                    save.Append(Ortho1000.ToString());
                    OrthographicFamily Ortho1500 = new OrthographicFamily(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1500"));
                    save.Append(Ortho1500.ToString());
                }
                #endregion
#if ReferenceFamily
                #region References
            Console.WriteLine("\tReferences Family...");
            ReferencesFamily Refe = new ReferencesFamily(name_of_file);
            save.Append(Refe.ToString());
                #endregion
#endif

                #region Quantitative
                if (QuantitativeChecked)
                {
                    lg.addText("\n" + "\tQuantitative Characteristics Family...");
                    quantitativeCharacteristicsFamily quan = new quantitativeCharacteristicsFamily(name_of_file);
                    save.Append(quan.ToString());
                }
                #endregion
                #region Reachness Lang
                if (ReachnessLangChecked)
                {
                    lg.addText("\n" + "\tReachness Lang...");
                    ReachnessLang Reac = new ReachnessLang(name_of_file);
                    save.Append(Reac.ToString());
                    ReachnessLang Reac500 = new ReachnessLang(name_of_file.Insert(name_of_file.IndexOf('\\'), "of500"));
                    save.Append(Reac500.ToString());
                    ReachnessLang Reac1000 = new ReachnessLang(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1000"));
                    save.Append(Reac1000.ToString());
                    ReachnessLang Reac1500 = new ReachnessLang(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1500"));
                    save.Append(Reac1500.ToString());
                }
                #endregion

                #region Stem
                if (StemmerChecked)
                {
                    name_of_file.Replace("articles", "articlesof500");
                    //
                    lg.addText("\n" + "\tStemmer...");

                    porter.Stemmer stem = new porter.Stemmer(name_of_file);
                    save.Append(stem.ToString1());
                    porter.Stemmer stem500 = new porter.Stemmer(name_of_file.Insert(name_of_file.IndexOf('\\'), "of500"));
                    save.Append(stem500.ToString1());
                    porter.Stemmer stem1000 = new porter.Stemmer(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1000"));
                    save.Append(stem1000.ToString1());
                    porter.Stemmer stem1500 = new porter.Stemmer(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1500"));
                    save.Append(stem1500.ToString1());
                }
                #endregion
                #region Tagger
                if (TaggerChecked)
                {
                    lg.addText("\n" + "\tTagger...");
                    Tagger tagg = new Tagger(name_of_file);
                    save.Append(tagg.ToString());
                    Tagger tagg500 = new Tagger(name_of_file.Insert(name_of_file.IndexOf('\\'), "of500"));
                    save.Append(tagg500.ToString());
                    Tagger tagg1000 = new Tagger(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1000"));
                    save.Append(tagg1000.ToString());
                    Tagger tagg1500 = new Tagger(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1500"));
                    save.Append(tagg1500.ToString());
                }
                #endregion
            }
            #region P Family
            if (!(chooseMethod == 2))
            {

                save.Append(p_arr[i - 1].ToString(allGramsWords, p_arr[i - 1].total_words));
            }
            #endregion
            //
            #region info
            string rest;
            #region file name
            int file_pos = name_of_file.LastIndexOf('\\');
            rest = name_of_file.Substring(0, file_pos);
            string file_name = name_of_file.Substring(file_pos + 1, name_of_file.Length - rest.Length - 1);
            #endregion
            #region year
            string year = "-";
            int Iyear = 0;
            if (name_of_file.Contains("\\articles\\"))
            {
                int year_pos = rest.LastIndexOf('\\');
                year = name_of_file.Substring(year_pos + 1, rest.Length - (rest.Substring(0, year_pos + 1)).Length);
                rest = rest.Substring(0, year_pos);
            }
            else if (name_of_file.Contains(dir_of_articles_folders + "\\Books\\"))
            {
                year = "-"; //because the books have NO years...
            }
            if (year != "-")
            {
                Iyear = getYear(year);
                if (Iyear > 0 && Iyear < 100)
                    if (Iyear > 70)
                        Iyear += 1900;
                    else
                        Iyear += 2000;
                year = Iyear.ToString();
            }
            #endregion
            /*
            #region interval of years
            string iofB = "-", iofA = "-";
            if (year != "-")
            {
                iofB = ((char)((Iyear - startYear) / INTERVAL_OF_YEAR_B + (int)'A')).ToString();
                iofA = ((char)((Iyear - startYear) / INTERVAL_OF_YEAR_A + (int)'A')).ToString();
            }
            #endregion
            */

            #region conference/book name
            int conf_pos = rest.LastIndexOf('\\');
            string conf_name = name_of_file.Substring(conf_pos + 1, rest.Length - (rest.Substring(0, conf_pos + 1)).Length);
            rest = rest.Substring(0, conf_pos);
            #endregion
            //
            //save.Append(year + ",");
            if (Program.THRESHOLD == 0) save.Append(conf_name);
            else save.Append(conf_name + ",");

            /* save.Append(iofA + ",");
             save.Append(iofB);*/
            #endregion
            //
            writer.WriteLine(save);
        }
        private static int getYear(string year)
        {
            string n_year = "";
            foreach (char x in year)
            {
                if (x == '0' || x == '1' || x == '2' || x == '3' || x == '4' || x == '5' || x == '6' || x == '7' || x == '8' || x == '9')
                    n_year += x;
            }
            return int.Parse(n_year);
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Normalization.normalizer norm = new Normalization.normalizer(dir_of_articles_folders,typeOfTweets);
            norm.Normalize(getFlags());

            Process.GetCurrentProcess().MaxWorkingSet = new IntPtr(99999999999999999);
            output_path = "U-" + Program.NUM_OF_ONE + " B-" + Program.NUM_OF_TWO + " T-" + Program.NUM_OF_THREE + " F-" + Program.NUM_OF_FOUR;
            output_path += (Program.RareUniGrams != 0) ? (" RU-" + Program.RareUniGrams.ToString()) : "";
            output_path += (Program.RareBiGrams != 0) ? (" RB-" + Program.RareBiGrams.ToString()) : "";
            output_path += (Program.RareTriGrams != 0) ? (" RT-" + Program.RareTriGrams.ToString()) : "";
            output_path += (Program.RareQuadGrams != 0) ? (" RQ-" + Program.RareQuadGrams.ToString()) : "";
            output_path += (Program.UniChars != 0) ? (" UC-" + Program.UniChars.ToString()) : "";
            output_path += (Program.BiChars != 0) ? (" BC-" + Program.BiChars.ToString()) : "";
            output_path += (Program.TriChars != 0) ? (" TC-" + Program.TriChars.ToString()) : "";
            output_path += (Program.QuadChars != 0) ? (" QC-" + Program.QuadChars.ToString()) : "";
            output_path += (Program.RareUniChars != 0) ? (" RUC-" + Program.RareUniChars.ToString()) : "";
            output_path += (Program.RareBiChars != 0) ? (" RBC-" + Program.RareBiChars.ToString()) : "";
            output_path += (Program.RareTriChars != 0) ? (" RTC-" + Program.RareTriChars.ToString()) : "";
            output_path += (Program.RareQuadChars != 0) ? (" RQC-" + Program.RareQuadChars.ToString()) : "";

            output_path += ((chooseMethod == 1) ? "_Only Ngrams" : "_with families_");
            output_path += ((OrthograficChecked) ? "_Orthografic" : "");
            output_path += ((QuantitativeChecked) ? "_Quantitative" : "");
            output_path += ((ReachnessLangChecked) ? "_ReachnessLang" : "");
            output_path += ((StemmerChecked) ? "_Stemmer" : "");
            output_path += ((TaggerChecked) ? "_Tagger" : "");
            output_path += ".csv";


            lg.SetText("The OutPut Path is: \n\n" + output_path);
            Submit.IsEnabled = false;
            TakeOutStopWords.IsEnabled = false;
            AnalysisMethod.IsEnabled = false;
            ArticleDir.IsEnabled = false;
            AritclePath = ArticleDir.Text.ToString();

            myThread = new Thread(new ThreadStart(make_csv_file));
            myThread.Start();

        }
        private DirectoryInfo[] GetDirsFromPath()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                AritclePath = ArticleDir.Text;
            }));

            DirectoryInfo dInfo = new DirectoryInfo(AritclePath);
            DirectoryInfo[] subdirs = dInfo.GetDirectories();
            return subdirs;
        }
        static IEnumerable<string> GetSubdirectoriesContainingOnlyFiles(string path)
        {
            return from subdirectory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                   where Directory.GetDirectories(subdirectory).Length == 0
                   select subdirectory;
        }
        public void make_csv_file()
        {
            make_csv_file(dir_of_articles_folders, output_path);
            this.Dispatcher.Invoke((Action)(() =>
             {
                 Submit.IsEnabled = true;
                 TakeOutStopWords.IsEnabled = true;
                 AnalysisMethod.IsEnabled = true;
                 ArticleDir.IsEnabled = true;
             }));
            // MakeTxtLogNgramsAndNChars(p_arr);

            MessageBox.Show("Program Finished");
        }
        private void AnalysisMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ArticleDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            dir_of_articles_folders = ArticleDir.Text;
        }
        private void Threshold_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            double t;
            if (Double.TryParse(Threshold.Text.ToString(), out t) && t >= 0 && t <= 0.9999)
            {
                FreqWarning.Content = "";
                Program.THRESHOLD = t;
                THRESHOLD = t;
            }
            else if (FreqWarning != null)
                FreqWarning.Content = "Put a number between 0 to 0.9999";
        }
        private void UniGRams_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (UniGRams.Text.ToString().Equals(""))
            {
                Program.NUM_OF_ONE = 0;
                return;
            }
            int n;
            if (int.TryParse(UniGRams.Text.ToString(), out n) && n >= 0)
            {
                Program.NUM_OF_ONE = n;
            }
            else
            {
                MessageBox.Show("Please Enter a natural number");
                UniGRams.Text = "0";
            }
        }
        private void BiGRams_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (BiGRams.Text.ToString().Equals(""))
            {
                Program.NUM_OF_TWO = 0;
                return;
            }
            int n;
            if (int.TryParse(BiGRams.Text.ToString(), out n) && n >= 0)
            {
                Program.NUM_OF_TWO = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void TriGRams_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (TriGRams.Text.ToString().Equals(""))
            {
                Program.NUM_OF_THREE = 0;
                return;
            }
            int n;
            if (int.TryParse(TriGRams.Text.ToString(), out n) && n >= 0)
            {
                Program.NUM_OF_THREE = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void QuadGrams_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (QuadGrams.Text.ToString().Equals(""))
            {
                Program.NUM_OF_FOUR = 0;
                return;
            }
            int n;
            if (int.TryParse(QuadGrams.Text.ToString(), out n) && n >= 0)
            {
                Program.NUM_OF_FOUR = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void RareUGRAMs_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (RareUGRAMs.Text.ToString().Equals(""))
            {
                Program.RareUniGrams = 0;
                return;
            }
            int n;
            if (int.TryParse(RareUGRAMs.Text.ToString(), out n) && n >= 0)
            {
                Program.RareUniGrams = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void RareBGRAMS_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (RareBGRAMS.Text.ToString().Equals(""))
            {
                Program.RareBiGrams = 0;
                return;
            }
            int n;
            if (int.TryParse(RareBGRAMS.Text.ToString(), out n) && n >= 0)
            {
                Program.RareBiGrams = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void RareTriGrams_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (RareTriGrams.Text.ToString().Equals(""))
            {
                Program.RareTriGrams = 0;
                return;
            }
            int n;
            if (int.TryParse(RareTriGrams.Text.ToString(), out n) && n >= 0)
            {
                Program.RareTriGrams = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void RareQuadGrams_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (RareQuadGrams.Text.ToString().Equals(""))
            {
                Program.RareQuadGrams = 0;
                return;
            }
            int n;
            if (int.TryParse(RareQuadGrams.Text.ToString(), out n) && n >= 0)
            {
                Program.RareQuadGrams = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void CreateLog()
        {

            lg = new LogWindow();
            lg.Show();
        }
        private void AnalysisMethod_DropDownClosed(object sender, EventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            switch (AnalysisMethod.Text.ToString())
            {
                case "Only Ngrams":
                    chooseMethod = 1;
                    ResetGramsAndChars();

                    Familes.IsEnabled = false;
                    break;
                case "Only Stylistis and Tagger":
                    DisableNgramsAndChars();
                    chooseMethod = 2;
                    Familes.IsEnabled = true;
                    break;
                case "Both Ngrams and Other Families":
                    ResetGramsAndChars();
                    chooseMethod = 3;
                    Familes.IsEnabled = true;
                    break;
            }
        }
        private void ResetGramsAndChars()
        {
            Threshold.IsEnabled = true;
            ReducingUniGrams.IsEnabled = true;
            UniGRams.IsEnabled = true;
            UniGRams.Text = "500";
            Program.NUM_OF_ONE = 500;
            BiGRams.IsEnabled = true;
            BiGRams.Text = "500";
            Program.NUM_OF_TWO = 500;
            TriGRams.IsEnabled = true;
            TriGRams.Text = "500";
            Program.NUM_OF_THREE = 500;
            QuadGrams.IsEnabled = true;
            QuadGrams.Text = "500";
            Program.NUM_OF_FOUR = 500;
            RareUGRAMs.IsEnabled = true;
            RareUGRAMs.Text = "0";
            Program.RareUniGrams = 0;
            RareBGRAMS.IsEnabled = true;
            RareBGRAMS.Text = "0";
            Program.RareBiGrams = 0;
            RareTriGrams.IsEnabled = true;
            RareTriGrams.Text = "0";
            Program.RareTriGrams = 0;
            RareQuadGrams.IsEnabled = true;
            RareQuadGrams.Text = "0";
            Program.RareQuadGrams = 0;
            UniChars.IsEnabled = true;
            UniChars.Text = "0";
            Program.UniChars = 0;
            BiChars.IsEnabled = true;
            BiChars.Text = "0";
            Program.BiChars = 0;
            TriChars.IsEnabled = true;
            TriChars.Text = "0";
            Program.TriChars = 0;
            QuadChars.IsEnabled = true;
            QuadChars.Text = "0";
            Program.QuadChars = 0;
            RareUniChars.IsEnabled = true;
            RareUniChars.Text = "0";
            Program.RareUniChars = 0;
            RareBiChars.IsEnabled = true;
            RareBiChars.Text = "0";
            Program.RareBiChars = 0;
            RareTriChars.IsEnabled = true;
            RareTriChars.Text = "0";
            Program.RareTriChars = 0;
            RareQuadChars.IsEnabled = true;
            RareQuadChars.Text = "0";
            Program.RareQuadChars = 0;
            TakeOutStopWords.IsEnabled = true;
        }
        private void DisableNgramsAndChars()
        {
            TakeOutStopWords.IsEnabled = false; ;
            Threshold.IsEnabled = false;
            ReducingUniGrams.IsEnabled = false;
            UniGRams.IsEnabled = false;
            UniGRams.Text = "0";
            Program.NUM_OF_ONE = 0;
            BiGRams.IsEnabled = false;
            BiGRams.Text = "0";
            Program.NUM_OF_TWO = 0;
            TriGRams.IsEnabled = false;
            TriGRams.Text = "0";
            Program.NUM_OF_THREE = 0;
            QuadGrams.IsEnabled = false;
            QuadGrams.Text = "0";
            Program.NUM_OF_FOUR = 0;
            RareUGRAMs.IsEnabled = false;
            RareUGRAMs.Text = "0";
            Program.RareUniGrams = 0;
            RareBGRAMS.IsEnabled = false;
            RareBGRAMS.Text = "0";
            Program.RareBiGrams = 0;
            RareTriGrams.IsEnabled = false;
            RareTriGrams.Text = "0";
            Program.RareTriGrams = 0;
            RareQuadGrams.IsEnabled = false;
            RareQuadGrams.Text = "0";
            Program.RareQuadGrams = 0;
            UniChars.IsEnabled = false;
            UniChars.Text = "0";
            Program.UniChars = 0;
            BiChars.IsEnabled = false;
            BiChars.Text = "0";
            Program.BiChars = 0;
            TriChars.IsEnabled = false;
            TriChars.Text = "0";
            Program.TriChars = 0;
            QuadChars.IsEnabled = false;
            QuadChars.Text = "0";
            Program.QuadChars = 0;
            RareUniChars.IsEnabled = false;
            RareUniChars.Text = "0";
            Program.RareUniChars = 0;
            RareBiChars.IsEnabled = false;
            RareBiChars.Text = "0";
            Program.RareBiChars = 0;
            RareTriChars.IsEnabled = false;
            RareTriChars.Text = "0";
            Program.RareTriChars = 0;
            RareQuadChars.IsEnabled = false;
            RareQuadChars.Text = "0";
            Program.RareQuadChars = 0;
        }
        private void UniChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (UniChars.Text.ToString().Equals(""))
            {
                Program.UniChars = 0;
                return;
            }
            int n;
            if (int.TryParse(UniChars.Text.ToString(), out n) && n >= 0)
            {
                Program.UniChars = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void BiChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (BiChars.Text.ToString().Equals(""))
            {
                Program.BiChars = 0;
                return;
            }
            int n;
            if (int.TryParse(BiChars.Text.ToString(), out n) && n >= 0)
            {
                Program.BiChars = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void TriChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (TriChars.Text.ToString().Equals(""))
            {
                Program.TriChars = 0;
                return;
            }
            int n;
            if (int.TryParse(TriChars.Text.ToString(), out n) && n >= 0)
            {
                Program.TriChars = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void QuadChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (QuadChars.Text.ToString().Equals(""))
            {
                Program.QuadChars = 0;
                return;
            }
            int n;
            if (int.TryParse(QuadChars.Text.ToString(), out n) && n >= 0)
            {
                Program.QuadChars = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void RareUniChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (RareUniChars.Text.ToString().Equals(""))
            {
                Program.RareUniChars = 0;
                return;
            }
            int n;
            if (int.TryParse(RareUniChars.Text.ToString(), out n) && n >= 0)
            {
                Program.RareUniChars = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void RareBiChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (RareBiChars.Text.ToString().Equals(""))
            {
                Program.RareBiChars = 0;
                return;
            }
            int n;
            if (int.TryParse(RareBiChars.Text.ToString(), out n) && n >= 0)
            {
                Program.RareBiChars = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void RareTriChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (RareTriChars.Text.ToString().Equals(""))
            {
                Program.RareTriChars = 0;
                return;
            }
            int n;
            if (int.TryParse(RareTriChars.Text.ToString(), out n) && n >= 0)
            {
                Program.RareTriChars = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void RareQuadChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstTime)
            {
                return;

            }
            if (RareQuadChars.Text.ToString().Equals(""))
            {
                Program.RareQuadChars = 0;
                return;
            }
            int n;
            if (int.TryParse(RareQuadChars.Text.ToString(), out n) && n >= 0)
            {
                Program.RareQuadChars = n;
            }
            else
                MessageBox.Show("Please Enter a natural number");
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (myThread != null)
                myThread.Abort();
            if (newWindowThread != null)
            {
                newWindowThread.Abort();
            }
        }
        private void TakeOutStopWords_Checked(object sender, RoutedEventArgs e)
        {
            Program.RemoveStopWords = true;

        }
        private void TakeOutStopWords_Unchecked(object sender, RoutedEventArgs e)
        {
            Program.RemoveStopWords = false;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Familes.IsEnabled = false;

        }
        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)SelectAllCheckBox.IsChecked)
            {
                OrthograficCheckBox.IsChecked = true;
                ReachnessLangCheckBox.IsChecked = true;
                ReachnessLangCheckBox.IsChecked = true;
                StemmerCheckBox.IsChecked = true;
                TaggerCheckBox.IsChecked = true;
                QuantitativeCheckBox.IsChecked = true;

                OrthograficChecked = true;
                QuantitativeChecked = true;
                ReachnessLangChecked = true;
                StemmerChecked = true;
                TaggerChecked = true;
            }
            else
            {
                OrthograficCheckBox.IsChecked = false;
                ReachnessLangCheckBox.IsChecked = false;
                ReachnessLangCheckBox.IsChecked = false;
                StemmerCheckBox.IsChecked = false;
                TaggerCheckBox.IsChecked = false;
                QuantitativeCheckBox.IsChecked = false;

                OrthograficChecked = false;
                QuantitativeChecked = false;
                ReachnessLangChecked = false;
                StemmerChecked = false;
                TaggerChecked = false;
            }
        }
        private void StemmerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)StemmerCheckBox.IsChecked)
                StemmerChecked = true;
            else
                StemmerChecked = false;
        }
        private void TaggerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)TaggerCheckBox.IsChecked)
                TaggerChecked = true;
            else
                TaggerChecked = false;
        }
        private void ReachnessLangCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ReachnessLangCheckBox.IsChecked)
                ReachnessLangChecked = true;
            else
                ReachnessLangChecked = false;
        }
        private void QuantitativeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)QuantitativeCheckBox.IsChecked)
                QuantitativeChecked = true;
            else
                QuantitativeChecked = false;
        }
        private void OrthograficCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)OrthograficCheckBox.IsChecked)
                OrthograficChecked = true;
            else
                OrthograficChecked = false;
        }
        private void TraningSetNum_DropDownClosed(object sender, EventArgs e)
        {
            switch (TraningSetNum.Text)
            {
                case "None":
                    TrainingSetPres = 0;
                    break;
                case "10":
                    TrainingSetPres = 10;
                    break;
                case "15":
                    TrainingSetPres = 15;
                    break;
                case "20":
                    TrainingSetPres = 20;
                    break;
                case "30":
                    TrainingSetPres = 30;
                    break;
                case "33":
                    TrainingSetPres = 33;
                    break;
                case "40":
                    TrainingSetPres = 40;
                    break;
                case "50":
                    TrainingSetPres = 50;
                    break;
                default:
                    TrainingSetPres = 0;
                    break;
            }
        }
        private void ReducingUniGrams_DropDownClosed(object sender, EventArgs e)
        {

            if (FirstTime)
            {
                return;

            }

            switch (ReducingUniGrams.Text.ToString())
            {
                case ("Each Articles"):
                    Program.ForOneArticle = "E";
                    break;
                case ("All of them"):
                    Program.ForOneArticle = "A";
                    break;
                case ("None of those"):
                    Program.ForOneArticle = "N";
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw = new StreamWriter("Tag.bat");

            var files = GetFilesInDir(ArticleDir.Text).Where(x => !x.Contains("_tagger_output")).Where(x => x.Contains(".txt"));

            foreach (var item in files)
            {
                string toWrite = "";
                toWrite += @"java -Xmx3000m -XX:MaxPermSize=4000m -classpath stanford-postagger.jar edu.stanford.nlp.tagger.maxent.MaxentTagger " +
                    "-model models/english-bidirectional-distsim.tagger -sentenceDelimiter newline -textFile ";
                toWrite += item;
                toWrite += " > " + item.Substring(0, item.LastIndexOf('.')) + "_tagger_output.txt";
                sw.WriteLine(toWrite);
            }
            sw.Close();


            var prcs = System.Diagnostics.Process.Start("Tag.bat");
            MessageBox.Show("This going to take a while\n");
            Submit.IsEnabled = false;
            prcs.EnableRaisingEvents = true;
            prcs.Exited += (s, e2) =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Submit.IsEnabled = true;
                }));

            };

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Thread count = new Thread(new ThreadStart(CountDifferentWords));
            count.Start();

        }
        private void CountDifferentWords()
        {

            this.Dispatcher.Invoke((Action)(() =>
            {
                StringBuilder words = new StringBuilder();
                DirectoryInfo[] dirs = GetDirsFromPath();

                string[] files = GetFilesInDir(ArticleDir.Text);
                StreamWriter sw = new StreamWriter("DiffernetWords.txt");
                string[] afterSplit;
                HashSet<SeqString> temp = new HashSet<SeqString>();
                foreach (DirectoryInfo item in dirs)
                {
                    lg.ClearText();
                    lg.SetText("Counting " + item.ToString());

                    words.Clear();
                    temp.Clear();
                    foreach (string file in files.Where(x => x.Contains(item.ToString())).ToArray())
                    {
                        StreamReader sr = new StreamReader(file);
                        words.Append(sr.ReadToEnd());
                        afterSplit = words.ToString().Split(' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@');
                        words.Clear();
                        temp.UnionWith(from word in afterSplit.Cast<string>()
                                       group word by word into g
                                       select new SeqString { word = g.Key, freq = g.Count() });
                    }

                    sw.WriteLine("Differnet Words at Domian " + item.ToString() + ":");
                    sw.WriteLine("\n" + temp.Count);
                }

                words.Clear();
                temp.Clear();
                foreach (string file in files)
                {
                    StreamReader sr = new StreamReader(file);
                    words.Append(sr.ReadToEnd());
                    afterSplit = words.ToString().Split(' ', '.', ',', ':', '\n', '\r', '(', ')', '=', '{', '}', '<', '>', '+', '-', '[', ']', '\t', '\"', '\\', '*', '@');
                    words.Clear();
                    temp.UnionWith(from word in afterSplit.Cast<string>()
                                   group word by word into g
                                   select new SeqString { word = g.Key, freq = g.Count() });
                }


                sw.WriteLine("Differnet Words at All Domians: ");
                sw.WriteLine("\n" + temp.Count);
                sw.Close();
                MessageBox.Show("Finish counting");
            }));


        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Thread StatsThread = new Thread(new ThreadStart(StatsCreator));
            StatsThread.SetApartmentState(ApartmentState.STA);
            StatsThread.IsBackground = true;
            StatsThread.Start();

        }
        private void StatsCreator()
        {

            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    FilesStatistics stats = new FilesStatistics(ArticleDir.Text);
                }));
                MessageBox.Show("finish statistics \nsee Statistics.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /********YAIR**********/
        /// <summary>
        /// Returns a Dictionary with all the flags(NO_HTML,NO_PUNCTUATION,CAPITAL,LOWERCASE) and their values.
        /// </summary>
        /// <returns></returns>
        private IDictionary<Normalization.NormaliztionMethods,bool> getFlags()
        {
            IDictionary<Normalization.NormaliztionMethods, bool> flags =
                new Dictionary<Normalization.NormaliztionMethods, bool>();

            flags.Add(Normalization.NormaliztionMethods.All_Capitals, (bool)LettersCB.IsChecked);
            flags.Add(Normalization.NormaliztionMethods.All_Lowercase, !(bool)LettersCB.IsChecked);
            flags.Add(Normalization.NormaliztionMethods.No_HTML_Tags, (bool)HTMLCB.IsChecked);
            flags.Add(Normalization.NormaliztionMethods.No_Punctuation, (bool)PunCB.IsChecked);
            return flags;
        }
    }
}
