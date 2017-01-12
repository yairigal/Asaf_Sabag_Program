using ContentFamilies;
using PatternFamilies;
using Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Normalization;
using Enums;
using I_O;
using System.Windows.Threading;

namespace WpfAArticleAnalysis
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Static Declerations
        static P_family[] p_arr;
        static P_family SaveData;
        static int numberOfRemoved;
        static public List<string> allGramsWords;
        static int startYear = 1998;
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
        public static event LogDelegate LogChanged;
        public static LogPage lg = null;
        private static string[] FILES_NAMES;
        #endregion

        #region Declerations
        private string AritclePath;
        public delegate void LogDelegate(string str);
        private Thread newLogThread;
        private Thread PercentUpdateThread;
        private Thread PercentNormalThread;
        private Thread newWindowThread = null;
        private Thread myThread = null;
        private bool MakingLog = false;
        private bool OrthograficChecked;
        private bool QuantitativeChecked;
        private bool ReachnessLangChecked;
        private bool StemmerChecked;
        private bool TaggerChecked;
        private bool saveNormalDir = true;
        private bool PercentUpdateThreadFlag = false;
        private bool PercentNormalThreadFlag = false;
        private bool takeOutStopWords = false;
        int TrainingSetPres = 0;


        //added by Yair
        IO_DataType TextType = IO_DataType.Text;
        PageHandler PHandler;
        normalizer Normalizer;

        #region NgramPage
        TextBox UniGRams;
        TextBox BiGRams;
        TextBox TriGRams;
        TextBox QuadGrams;
        TextBox RareUGRAMs;
        TextBox RareBGRAMS;
        TextBox RareTriGrams;
        TextBox RareQuadGrams;

        TextBox UniChars;
        TextBox BiChars;
        TextBox TriChars;
        TextBox QuadChars;
        TextBox RareUniChars;
        TextBox RareBiChars;
        TextBox RareTriChars;
        TextBox RareQuadChars;
        #endregion

        #region FirstPage
        ComboBox AnalysisMethod;
        TextBox ArticleDir;
        TextBox Threshold;
        ComboBox ReducingUniGrams;
        CheckBox TakeOutStopWords;
        CheckBox MakeLogFiles;
        CheckBox DomainsCounter;
        Label FreqWarning;
        Button openDir;
        #endregion

        #region NormalizationPage
        CheckBox PunRB;
        CheckBox HTMLRB;
        ComboBox LettersCB;
        CheckBox saveNormaledFiles;
        #endregion

        #region FeaturesPage
        GroupBox Familes;
        CheckBox OrthograficCheckBox;
        CheckBox QuantitativeCheckBox;
        CheckBox ReachnessLangCheckBox;
        CheckBox StemmerCheckBox;
        CheckBox TaggerCheckBox;
        CheckBox SelectAllCheckBox;
        ComboBox TraningSetNum;
        Button Tag_Articles; // Tag Articles
        Button Count; // Count
        Button Statistics; // Statistics
        #endregion

        #endregion

        #region Fucntions
        public MainWindow()
        {
            float tr = (float)(1.0 / 888888.0);

            InitializeComponent();
            //added by Yair
            setWindowSize();
            pageFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            initPages();
            changeSUBMITbuttonMode(false);
            //added by Yair
            NewWindowHandler(this, null);

            setUpAnalysisCombobox();

            ArticleDir.Text = Directory.GetCurrentDirectory();

            Threshold.Text = "0";

            setUpReducingUnigramsCombobox();

            setUpTrainingSets();

            setUpNGrams();

            FirstTime = false;

            //added By Yair
            initNormalUI();

            lg = new LogPage(this);
            logFrame.Navigate(lg);
        }
        private void DeleteNormalizationDirectoryIfNeeded()
        {
            if (!saveNormalDir)
            {
                foreach (var file in Directory.GetFiles(Normalizer.AfterNormalDir))             
                    File.Delete(file);
                Directory.Delete(Normalizer.AfterNormalDir);
            }
        }
        private void setUpReducingUnigramsCombobox()
        {
            ReducingUniGrams.Items.Add("None of those");
            ReducingUniGrams.Items.Add("All of them");
            ReducingUniGrams.Items.Add("Each Article");
            ReducingUniGrams.SelectedIndex = 0;
        }
        private void setUpTrainingSets()
        {
            string[] strs = new string[] { "None", "10", "15", "20", "30", "33", "40", "50" };
            TraningSetNum.ItemsSource = strs;
            TraningSetNum.SelectedIndex = 0;
            TrainingSetPres = 0;
        }
        private void setUpNGrams()
        {
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
        }
        private void setUpAnalysisCombobox()
        {
            AnalysisMethod.Items.Add("Only Ngrams");
            AnalysisMethod.Items.Add("Only Stylistis and Tagger");
            AnalysisMethod.Items.Add("Both Ngrams and Other Families");
            AnalysisMethod.SelectedIndex = 0;
        }
        private void NewWindowHandler(object sender, RoutedEventArgs e)
        {
            newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }
        private void ThreadStartingPoint()
        {
            //lg = new LogPage(this);

            //System.Windows.Threading.Dispatcher.Run();
        }
        private void make_csv_file(string dir_of_articles, string output_path)
        {

            string[] files = null;
            StreamWriter s = null;
            StreamReader r;
            try
            {
                //files = GetFilesInDir(dir_of_articles);
                files = normalizer.getFiles(dir_of_articles).ToArray();
                s = new StreamWriter(Information.output_path, false);
            }
            catch
            {
                MessageBox.Show("dir is not currect or the csv file is open");
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
            return; // what does that soppose to mean?
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

            return; // what does that soppose to mean?
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
            int overallSize = 0;
            lg.SetText("Counting files...");
            Thread counting =  new Thread(new ThreadStart(() => { getOverallSize(arr, table,ref overallSize); }));
            counting.Start();

            while (true)
            {
                if (counting.ThreadState == System.Threading.ThreadState.Stopped)
                    break;
                double percentage = Math.Round((float)overallSize*100 / arr.Length,2);
                updateBar.Value = percentage;
                Refresh(updateBar);
                Thread.Sleep(1000);
            }

            string title = "";
            int count = 0;
            int currentRun = 0;

            #region SettingUpTheThread
            //percentage for the log window.
            if (PercentUpdateThread != null)
                if(PercentUpdateThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    PercentUpdateThreadFlag = false;
                    PercentUpdateThread.Join();
                }

            PercentUpdateThreadFlag = true;

            if (PercentUpdateThread == null || !(PercentUpdateThread.ThreadState == System.Threading.ThreadState.Running))
                PercentUpdateThread = new Thread(new ThreadStart(() => setLogPercentage(ref currentRun, ref title, overallSize, ref count)));
            #endregion

            switch (table)
            {
                case "table_of_one":
                    currentRun = 0;
                    count = 0;
                    if (Program.NUM_OF_ONE == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting uni gram words");
                    title = "counting uni gram words";
                    PercentUpdateThread.Start();
                    foreach (var item in arr)
                    {
                        foreach (var w in item.table_of_one)
                        {
                            count++;
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
                    currentRun = 1;
                    if (Program.NUM_OF_TWO == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting BI gram words");
                    title = "counting BI gram words";
                    PercentUpdateThread.Start();
                    foreach (var item in arr)
                    {
                        foreach (var w in item.table_of_two)
                        {
                            count++;
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
                    currentRun = 2;
                    if (Program.NUM_OF_THREE == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting TRI gram words");
                    title = "counting TRI gram words";
                    PercentUpdateThread.Start();
                    foreach (var item in arr)
                    {
                        foreach (var w in item.table_of_three)
                        {
                            count++;
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
                    currentRun = 3;
                    if (Program.NUM_OF_FOUR == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting QUAD gram words");
                    title = "counting QUAD gram words";
                    PercentUpdateThread.Start();
                    foreach (var item in arr)
                    {
                        foreach (var w in item.table_of_four)
                        {
                            count++;
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
                    currentRun = 4;
                    if (Program.UniChars == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting UNI gram CHARS");
                    title = "counting UNI gram CHARS";
                    PercentUpdateThread.Start();
                    foreach (var item in arr)
                    {
                        foreach (var w in item.unichar)
                        {
                            count++;
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
                    currentRun = 5;
                    if (Program.BiChars == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting BI gram CHARS");
                    title = "counting BI gram CHARS";
                    PercentUpdateThread.Start();
                    foreach (var item in arr)
                    {
                        foreach (var w in item.bichar)
                        {
                            count++;
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
                    currentRun = 6;
                    if (Program.TriChars == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting TRI gram CHARS");
                    title = "counting TRI gram CHARS";
                    PercentUpdateThread.Start();
                    foreach (var item in arr)
                    {
                        foreach (var w in item.trichar)
                        {
                            count++;
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
                    currentRun = 7;
                    if (Program.QuadChars == 0)
                        return tmp;
                    lg.ClearText();
                    lg.SetText("counting QUAD gram CHARS");
                    title = "counting QUAD gram CHARS";
                    PercentUpdateThread.Start();
                    foreach (var item in arr)
                    {
                        foreach (var w in item.quadchar)
                        {
                            count++;
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

            PercentUpdateThreadFlag = false;
            PercentUpdateThread.Join();
            return tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentRun"></param>
        /// <param name="v"></param>
        /// <param name="overallSize"></param>
        /// <param name="count"></param>
        private void setLogPercentage(ref int currentRun, ref string v, int overallSize, ref int count)
        {
            while (PercentUpdateThreadFlag)
            {
                Thread.Sleep(1000);
                string a = v;
                double percent = 0;
                if (overallSize != 0)
                    percent = Math.Round(((float)count / overallSize) * 100, 2);

                //Dispatcher.Invoke(() => lg.SetText(a + "\n" + percent + " %"));
                Dispatcher.Invoke(()=>updateBar.Value = percent);
                Refresh(updateBar);
            }
        }
        /// <summary>
        /// returns the overall size of the running loop above
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="i">
        ///  table one
        ///  table two
        ///  table three
        ///  table four
        /// unichar
        ///  bichar
        ///  trichar
        ///  quadchar
        /// </param>
        /// <returns></returns>
        private void getOverallSize(P_family[] arr, string i,ref int count)
        {
            switch (i)
            {
                case "table_of_one":
                    foreach (var item in arr) { count += item.table_of_one.Count; }
                    break;
                case "table_of_two":
                    foreach (var item in arr) { count += item.table_of_two.Count; }
                    break;
                case "table_of_three":
                    foreach (var item in arr) { count += item.table_of_three.Count; }
                    break;
                case "table_of_four":
                    foreach (var item in arr) { count += item.table_of_four.Count; }
                    break;
                case "unichar":
                    foreach (var item in arr) { count += item.unichar.Count; }
                    break;
                case "bichar":
                    foreach (var item in arr) { count += item.bichar.Count; }
                    break;
                case "trichar":
                    foreach (var item in arr) { count += item.trichar.Count; }
                    break;
                case "quadchar":
                    foreach (var item in arr) { count += item.quadchar.Count; }
                    break;
                default:
                    foreach (var item in arr) { count += item.table_of_one.Count; }
                    break;
            }
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
            else if (name_of_file.Contains(Information.dir_of_articles_folders + "\\Books\\"))
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
            Process.GetCurrentProcess().MaxWorkingSet = new IntPtr(99999999999999999);
            Information.output_path = "U-" + Program.NUM_OF_ONE + " B-" + Program.NUM_OF_TWO + " T-" + Program.NUM_OF_THREE + " F-" + Program.NUM_OF_FOUR;
            Information.output_path += (Program.RareUniGrams != 0) ? (" RU-" + Program.RareUniGrams.ToString()) : "";
            Information.output_path += (Program.RareBiGrams != 0) ? (" RB-" + Program.RareBiGrams.ToString()) : "";
            Information.output_path += (Program.RareTriGrams != 0) ? (" RT-" + Program.RareTriGrams.ToString()) : "";
            Information.output_path += (Program.RareQuadGrams != 0) ? (" RQ-" + Program.RareQuadGrams.ToString()) : "";
            Information.output_path += (Program.UniChars != 0) ? (" UC-" + Program.UniChars.ToString()) : "";
            Information.output_path += (Program.BiChars != 0) ? (" BC-" + Program.BiChars.ToString()) : "";
            Information.output_path += (Program.TriChars != 0) ? (" TC-" + Program.TriChars.ToString()) : "";
            Information.output_path += (Program.QuadChars != 0) ? (" QC-" + Program.QuadChars.ToString()) : "";
            Information.output_path += (Program.RareUniChars != 0) ? (" RUC-" + Program.RareUniChars.ToString()) : "";
            Information.output_path += (Program.RareBiChars != 0) ? (" RBC-" + Program.RareBiChars.ToString()) : "";
            Information.output_path += (Program.RareTriChars != 0) ? (" RTC-" + Program.RareTriChars.ToString()) : "";
            Information.output_path += (Program.RareQuadChars != 0) ? (" RQC-" + Program.RareQuadChars.ToString()) : "";

            Information.output_path += ((chooseMethod == 1) ? "_Only Ngrams" : "_with families_");
            Information.output_path += ((OrthograficChecked) ? "_Orthografic" : "");
            Information.output_path += ((QuantitativeChecked) ? "_Quantitative" : "");
            Information.output_path += ((ReachnessLangChecked) ? "_ReachnessLang" : "");
            Information.output_path += ((StemmerChecked) ? "_Stemmer" : "");
            Information.output_path += ((TaggerChecked) ? "_Tagger" : "");


            //lg.SetText("The OutPut Path is: \n\n" + Information.output_path);
            Submit.IsEnabled = false;
            TakeOutStopWords.IsEnabled = false;
            AnalysisMethod.IsEnabled = false;
            ArticleDir.IsEnabled = false;
            AritclePath = ArticleDir.Text.ToString();

            //this.Visibility = Visibility.Hidden;

            try
            {
                //setOutputDirectories(); moved to NormalizeText();
                NormalizeText();
                MessageBox.Show("The normalizer has finished his work \nmoving to features extraction",
                    "normalizer finished", MessageBoxButton.OK, MessageBoxImage.Information);
                //lg.SetText("=========\nThe normalizer has finished his work \nmoving to features extraction\n=========");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            myThread = new Thread(new ThreadStart(make_csv_file));
            myThread.Start();

        }
        private void setOutputDirectories()
        {
            Information.dir_of_articles_folders = Normalizer.AfterNormalDir;
            string stop = "";
            if (takeOutStopWords)
                stop = "_S";
            if (!Directory.Exists(Normalizer.BeforeNormalDir + "\\excels"))
                Directory.CreateDirectory(Normalizer.BeforeNormalDir + "\\excels");
            Information.output_path = Normalizer.BeforeNormalDir + "\\excels\\" + Information.output_path + Normalizer.Changes + stop + "__0";
            if (File.Exists(Information.output_path + ".csv"))
            {
                Information.output_path = fixEnding(Information.output_path);
            }
            Information.output_path += ".csv";
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
            make_csv_file(Information.dir_of_articles_folders, Information.output_path);
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
        #region control_events&operations
        private void AnalysisMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ArticleDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            Information.dir_of_articles_folders = ArticleDir.Text;
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
        private void SaveNormaledFiles_Unchecked(object sender, RoutedEventArgs e)
        {
            saveNormalDir = false;
        }
        private void SaveNormaledFiles_Checked(object sender, RoutedEventArgs e)
        {
            saveNormalDir = true;
        }
        private void ArticleDir_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }
        #region Pages Events
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
                    PHandler.disablePage(Pages_ENUM.tagger);
                    PHandler.enablePage(Pages_ENUM.ngramPage);
                    break;
                case "Only Stylistis and Tagger":
                    DisableNgramsAndChars();
                    chooseMethod = 2;
                    Familes.IsEnabled = true;
                    PHandler.disablePage(Pages_ENUM.ngramPage);
                    PHandler.enablePage(Pages_ENUM.tagger);
                    break;
                case "Both Ngrams and Other Families":
                    ResetGramsAndChars();
                    chooseMethod = 3;
                    Familes.IsEnabled = true;
                    PHandler.enablePage(Pages_ENUM.ngramPage);
                    PHandler.enablePage(Pages_ENUM.tagger);
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
        private void TakeOutStopWords_Checked(object sender, RoutedEventArgs e)
        {
            //Program.RemoveStopWords = true;
            takeOutStopWords = true;
        }
        private void TakeOutStopWords_Unchecked(object sender, RoutedEventArgs e)
        {
            //Program.RemoveStopWords = false;
            takeOutStopWords = false;
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
            changeSUBMITbuttonMode(true);
        }
        private void Tag_Articles_Click(object sender, RoutedEventArgs e)
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
            changeSUBMITbuttonMode(false);
            prcs.EnableRaisingEvents = true;
            prcs.Exited += (s, e2) =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Submit.IsEnabled = true;
                }));

            };

        }
        private void Count_Click(object sender, RoutedEventArgs e)
        {
            Thread count = new Thread(new ThreadStart(CountDifferentWords));
            count.Start();

        }
        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            Thread StatsThread = new Thread(new ThreadStart(StatsCreator));
            StatsThread.SetApartmentState(ApartmentState.STA);
            StatsThread.IsBackground = true;
            StatsThread.Start();

        }
        private void OpenDir_Click(object sender, RoutedEventArgs e)
        {
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ArticleDir.Text = folderDialog.SelectedPath;
                    Information.dir_of_articles_folders = folderDialog.SelectedPath;
                }
            }
        }
        #endregion
        #endregion
        private void CreateLog()
        {
            lg = new LogPage(this);
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
        private void Window_Closed(object sender, EventArgs e)
        {
            DeleteNormalizationDirectoryIfNeeded();
            killAllRunningThreads();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Familes.IsEnabled = false;

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
        private void image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            nextPage();
        }
        private void image_Copy_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            prevPage();
        }

        /******MadeByYAIR******/
        #region Coded By Yair Yigal
        /// <summary>
        /// initialzies the Normalizaion UI components (checkboxes)
        /// </summary>
        private void initNormalUI()
        {
            HTMLRB.Content = "No Html Tags";
            PunRB.Content = "No Punctuation";
            LettersCB.Items.Add(NormaliztionMethods.All_Lowercase);
            LettersCB.Items.Add(NormaliztionMethods.All_Capitals);
            LettersCB.Items.Add(NormaliztionMethods.NONE);
        }
        /// <summary>
        ///  returns a code on which normalization mode we should do now.
        /// </summary>
        /// <returns>
        /// codes:
        /// code 1 - with puncuataion and html lower case.
        /// code 2 - with puncuataion and html upper case.
        /// code 3 - with puncuataion and html with no change to letters.
        /// code 4 - with puncutation no html lower case.
        /// code 5 - with puncutation no html upper case.
        /// code 6 - with puncutation no html with no change to letters.
        /// code 7 - no puncutation with html and lower case.
        /// code 8 - no puncutation with html and upper case
        /// code 9 - no puncutation with html and no change to letters.
        /// code 10 - no puncuation no html lower case.
        /// code 11 - no puncuation no html upper case.
        /// code 12 - no puncuation no html no change to letters.
        /// </returns>
        private int getNormalizationMode()
        {
            bool punc = PunRB.IsEnabled;
            bool html = HTMLRB.IsEnabled;
            bool lower = false, upper = false;
            switch (LettersCB.SelectedIndex)
            {
                case 0://lower
                    lower = true;
                    break;
                case 1://upper
                    upper = true;
                    break;
                default:
                    lower = upper = false;
                    break;
            }
            if (punc)
            {
                if (html)
                {
                    if (lower)
                    {
                        return 1;
                    }
                    else if (upper)
                    {
                        return 2;
                    }
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    if (lower)
                    {
                        return 4;
                    }
                    else if (upper)
                    {
                        return 5;
                    }
                    else
                    {
                        return 6;
                    }
                }
            }
            else
            {
                if (html)
                {
                    if (lower)
                    {
                        return 7;
                    }
                    else if (upper)
                    {
                        return 8;
                    }
                    else
                    {
                        return 9;
                    }
                }
                else
                {
                    if (lower)
                    {
                        return 10;
                    }
                    else if (upper)
                    {
                        return 11;
                    }
                    else
                    {
                        return 12;
                    }
                }
            }


        }
        //old normalize function
        //private void normalize(Normalizer nrmlz, params string[] dirToNormal)
        //{
        //    StreamReader read;
        //    StreamWriter wrt;
        //    try
        //    {
        //        foreach (var file in dirToNormal)
        //        {
        //            read = new StreamReader(file);
        //            while (!read.EndOfStream)
        //            {
        //                read.ReadLine();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Cannot retrieve data to normalize");
        //        return;
        //    }


        //}
        /// <summary>
        /// linking the events from here to the contols on the pages.
        /// </summary>
        private void addEventsToControls()
        {
            #region ngrampage
            UniGRams.TextChanged += UniGRams_TextChanged;
            BiGRams.TextChanged += BiGRams_TextChanged;
            TriGRams.TextChanged += TriGRams_TextChanged;
            QuadGrams.TextChanged += QuadGrams_TextChanged;
            RareUGRAMs.TextChanged += RareUGRAMs_TextChanged;
            RareBGRAMS.TextChanged += RareBGRAMS_TextChanged;
            RareTriGrams.TextChanged += RareTriGrams_TextChanged;
            RareQuadGrams.TextChanged += RareQuadGrams_TextChanged;

            UniChars.TextChanged += UniChars_TextChanged;
            BiChars.TextChanged += BiChars_TextChanged;
            TriChars.TextChanged += TriChars_TextChanged;
            QuadChars.TextChanged += QuadChars_TextChanged;
            RareUniChars.TextChanged += RareUniChars_TextChanged;
            RareBiChars.TextChanged += RareBiChars_TextChanged;
            RareTriChars.TextChanged += RareTriChars_TextChanged;
            RareQuadChars.TextChanged += RareQuadChars_TextChanged;

            Threshold.TextChanged += Threshold_TextChanged;
            ReducingUniGrams.DropDownClosed += ReducingUniGrams_DropDownClosed;
            #endregion

            #region firstpage
            AnalysisMethod.DropDownClosed += AnalysisMethod_DropDownClosed;
            AnalysisMethod.SelectionChanged += AnalysisMethod_SelectionChanged;
            ArticleDir.TextChanged += ArticleDir_TextChanged;
            ArticleDir.MouseEnter += ArticleDir_MouseEnter;
            openDir.Click += OpenDir_Click;
            #endregion

            #region FeaturesPage
            OrthograficCheckBox.Click += OrthograficCheckBox_Click;
            QuantitativeCheckBox.Click += QuantitativeCheckBox_Click;
            ReachnessLangCheckBox.Click += ReachnessLangCheckBox_Click;
            StemmerCheckBox.Click += StemmerCheckBox_Click;
            TaggerCheckBox.Click += TaggerCheckBox_Click;
            SelectAllCheckBox.Click += SelectAllCheckBox_Click;
            TraningSetNum.DropDownClosed += TraningSetNum_DropDownClosed;
            Tag_Articles.Click += Tag_Articles_Click;
            Count.Click += Count_Click;
            Statistics.Click += Statistics_Click;
            #endregion

            #region NormalizationPage
            TakeOutStopWords.Checked += TakeOutStopWords_Checked;
            TakeOutStopWords.Unchecked += TakeOutStopWords_Unchecked;
            saveNormaledFiles.Checked += SaveNormaledFiles_Checked;
            saveNormaledFiles.Unchecked += SaveNormaledFiles_Unchecked;
            #endregion

        }
        /// <summary>
        /// initialized the controls from the ngrampage to those variable here.
        /// </summary>
        private void initngramPageControls()
        {
            Pages.Ngrampage currPage = Pages.Ngrampage.getThisPage();
            UniGRams = currPage.getUniGrams;
            BiGRams = currPage.getBiGrams;
            TriGRams = currPage.getTriGrams;
            QuadGrams = currPage.getQuadGrams;

            RareUGRAMs = currPage.getRareUniGrams;
            RareTriGrams = currPage.getRareTriGrams;
            RareBGRAMS = currPage.getRareBiGrams;
            RareQuadGrams = currPage.getRareQuadGrams;

            UniChars = currPage.getUniChars;
            BiChars = currPage.getBiChars;
            TriChars = currPage.getTriChars;
            QuadChars = currPage.getQuadChars;

            RareUniChars = currPage.getRareUniChars;
            RareTriChars = currPage.getRareTriChars;
            RareBiChars = currPage.getRareBiChars;
            RareQuadChars = currPage.getRareQuadChars;

            Threshold = currPage.Threshold;
            ReducingUniGrams = currPage.ReducingUniGrams;
            FreqWarning = currPage.FreqWarning;
        }
        /// <summary>
        /// initializes the variables here to the controls from FirstPage.
        /// </summary>
        private void initFirstPageControls()
        {
            var currPage = Pages.FirstPage.getThisPage();
            AnalysisMethod = currPage.AnalysisMethod;
            ArticleDir = currPage.ArticleDir;
            MakeLogFiles = currPage.MakeLogFiles;
            DomainsCounter = currPage.DomainsCounter;
            openDir = currPage.openDir;

        }
        /// <summary>
        /// initialized the variable here to the conrols from NormalizationPage
        /// </summary>
        private void initNormaliztionPageControls()
        {
            var currPage = Pages.NormalizationPage.getThisPage();
            PunRB = currPage.PunRB;
            HTMLRB = currPage.HTMLRB;
            LettersCB = currPage.LettersCB;
            TakeOutStopWords = currPage.TakeOutStopWords;
            saveNormaledFiles = currPage.SaveNormalizations;

        }
        /// <summary>
        /// initialized the variable here to the conrols from FeatursPage.
        /// </summary>
        private void initFeaturesPageConrols()
        {
            var currPage = Pages.FeaturesPage.getThisPage();
            TraningSetNum = currPage.TraningSetNum;
            Tag_Articles = currPage.Tag_Articles;
            Count = currPage.Count;
            Statistics = currPage.Statistics;
        }
        /// <summary>
        /// initialized the variable here to the conrols from Tagger.
        /// </summary>
        private void initTaggerPageControls()
        {
            var currPage = Pages.Tagger.getThisPage();

            Familes = currPage.Familes;
            OrthograficCheckBox = currPage.OrthograficCheckBox;
            QuantitativeCheckBox = currPage.QuantitativeCheckBox;
            ReachnessLangCheckBox = currPage.ReachnessLangCheckBox;
            StemmerCheckBox = currPage.StemmerCheckBox;
            TaggerCheckBox = currPage.TaggerCheckBox;
            SelectAllCheckBox = currPage.SelectAllCheckBox;
        }
        ///// <summary>
        ///// enables or disables specific pages from showing.
        ///// </summary>
        //private void setPageOrder()
        //{
        //    //not tagger
        //    if (chooseMethod == 1)
        //    {
        //        //PHandler.disablePage(Pages_ENUM.tagger);
        //        PHandler.enablePage(Pages_ENUM.ngramPage);
        //    }
        //    //not ngram
        //    if (chooseMethod == 2)
        //        PHandler.disablePage(Pages_ENUM.ngramPage);
        //    //PHandler.enablePage(Pages.ENUM.tagger)
        //    //show both
        //    if (chooseMethod == 3)
        //    {
        //        PHandler.enablePage(Pages_ENUM.ngramPage);
        //        //PHandler.enablePage(Pages.ENUM.tagger)
        //    }
        //}
        /// <summary>
        /// initializes all the things that are linked to Pages,
        /// Variables , new instance etc...
        /// </summary>
        private void initPages()
        {
            PHandler = new PageHandler(pageFrame);
            initngramPageControls();
            initFirstPageControls();
            initNormaliztionPageControls();
            initFeaturesPageConrols();
            initTaggerPageControls();
            addEventsToControls();

            //setting the Window title
            frameTitle.Content = PHandler.getCurrentPage().name;
        }
        /// <summary>
        /// sets the windwos height and width
        /// </summary>
        private void setWindowSize()
        {
            //Height = Public_Functions.WindowHeight;
            //Width = Public_Functions.WindowWidth;
        }
        /// <summary>
        /// sets the next page on the frame
        /// </summary>
        private void nextPage()
        {
            frameTitle.Content = PHandler.NextPage().name;
        }
        /// <summary>
        /// sets the previous page on the frame.
        /// </summary>
        private void prevPage()
        {
            frameTitle.Content = PHandler.PreviousPage().name;
        }
        /// <summary>
        /// enables or disbales the SUBMIT button.
        /// </summary>
        /// <param name="mode">the mode to change the submit button to</param>
        private void changeSUBMITbuttonMode(bool mode)
        {
            Submit.IsEnabled = mode;
        }
        /// <summary>
        /// Refreshing the UI thread.
        /// </summary>
        /// <param name="ele"></param>
        public static void Refresh(UIElement ele)
        {
            Action a = new Action(() => { });
            ele.Dispatcher.Invoke(a, DispatcherPriority.Render);
        }
        /// <summary>
        /// This functions Normalizes the Text
        /// </summary>
        private void NormalizeText()
        {
            var flags = getNormalizationFlags();
            //new Thread(new ThreadStart(() => NormalThreadfunction(flags))).Start();

            //added here the function of the thread , since we are not using thread

            Normalizer = new normalizer(Information.dir_of_articles_folders, TextType, "");
            //Dispatcher.Invoke(() => MessageBox.Show("Normalizaion Started"));
            MessageBox.Show("The Normalizer has started his work", "Normaliztion Started", MessageBoxButton.OK, MessageBoxImage.Information);
            //lg.SetText("=========/nThe Normalizer has started his work/n=========");
            setOutputDirectories();
            //start NormalPercentThread
            //startNormalPercentThread();
            lg.SetText("Normalizing");
            Thread normalizing = new Thread(new ThreadStart(() => Normalizer.Normalize(flags)));
            normalizing.Start();

            //updaing percaente on UI
            while (true)
            {
                if (normalizing.ThreadState == System.Threading.ThreadState.Stopped)
                    break;
                double percentage = Normalizer.getPercentage();
                updateBar.Value = percentage;
                Action a = new Action(() => { });
                Refresh(updateBar);
                Thread.Sleep(1000);
            }
            //end NormalPercentThread;
            //stopNormalPercentThread();
        }
        /// <summary>
        /// starting the Normalzition percentage Thread
        /// </summary>
        private void startNormalPercentThread()
        {
            PercentNormalThreadFlag = true;
            PercentNormalThread = new Thread(new ThreadStart(()=>
                {
                    while(PercentNormalThreadFlag)
                    {
                        double percentage = Normalizer.getPercentage();
                        Dispatcher.Invoke(() => updateBar.Value = percentage);
                        Thread.Sleep(1000);
                    }
            }));
            PercentNormalThread.IsBackground = true;
            PercentNormalThread.Start();
        }
        /// <summary>
        /// starting the Normalzition percentage Thread
        /// </summary>
        private void stopNormalPercentThread()
        {
            PercentNormalThreadFlag = false;
            PercentNormalThread.Join();
        }
        /// <summary>
        /// Returns the flags for the normalizaion proccess
        /// </summary>
        /// <returns></returns>
        private IDictionary<NormaliztionMethods, bool> getNormalizationFlags()
        {
            bool cap = false, low = false, html = false, punc = false;

            if (LettersCB.SelectedIndex == 0)
                low = true;
            else if (LettersCB.SelectedIndex == 1)
                cap = true;


            html = (bool)HTMLRB.IsChecked;
            punc = (bool)PunRB.IsChecked;

            IDictionary<NormaliztionMethods, bool> flags = new Dictionary<NormaliztionMethods, bool>();
            flags.Add(NormaliztionMethods.All_Capitals, cap);
            flags.Add(NormaliztionMethods.All_Lowercase, low);
            flags.Add(NormaliztionMethods.No_HTML_Tags, html);
            flags.Add(NormaliztionMethods.No_Punctuation, punc);
            flags.Add(NormaliztionMethods.No_Stop_Words, takeOutStopWords);

            return flags;
        }
        /// <summary>
        /// Function that will be executed inside a thread
        /// </summary>
        /// <param name="flags"></param>
        private void NormalThreadfunction(IDictionary<NormaliztionMethods, bool> flags)
        {

        }
        /// <summary>
        /// killing all thread that are left.
        /// </summary>
        public void killAllRunningThreads()
        {
            if (PercentUpdateThread != null)
                if (!(PercentUpdateThread.ThreadState == System.Threading.ThreadState.Stopped))
                    PercentUpdateThreadFlag = false;

            if (myThread != null)
                myThread.Abort();

            if (newWindowThread != null)
                newWindowThread.Abort();
        }
        #endregion
        /******MadeByYAIR******/

        /*****MadeByElroi******/
        #region Coded By Elroi Netzer
        private string fixEnding(string path)
        {
            int i = 1;
            do
            {
                do
                {
                    path = path.Substring(0, path.Length - 1);
                } while (!path.EndsWith("_"));

                path += i.ToString();

            } while (File.Exists(path));
            return path;
        }
        #endregion
        /*****MadeByElroi******/
        #endregion
    }
}
