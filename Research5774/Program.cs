#undef ReferenceFamily
#define OnlyNgrams
//#undef OnlyNgrams
//#define OnlyUniGram
#undef OnlyUniGram
#define A_Way
#undef B_Way


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using ContentFamilies;
using PatternFamilies;
using HtmlLib;


namespace ArticleAnalysis
{
    class ArticleAnalysis
    {
        static P_family[] p_arr;
        static int numberOfRemoved;
        static public List<string> allGramsWords;
        static int startYear = 1998;
        static string dir_of_articles_folders = @"TXT\BOOKS";
        static int INTERVAL_OF_YEAR_A = 3, INTERVAL_OF_YEAR_B = 5;
        static int TotalWords = 0;
        static double THRESHOLD = 0;
        static int TOP_SKIP = 50;
        private static int chooseMethod = 1;
        private static HashSet<SeqString> rare_uni_gram;
        private static HashSet<SeqString> rare_four_gram;
        private static HashSet<SeqString> rare_two_gram;
        private static HashSet<SeqString> rare_three_gram;
        

        static void Main(string[] args)
        {
            Article_Analysis();
            //			OneBookAnalysis();
        }

        private static void OneBookAnalysis()
        {
            long words = 0;
            StreamWriter s;
            string dir_of_articles, output_path;
            Console.WriteLine("Enter DIR:");
            dir_of_articles = Console.ReadLine();
            output_path = dir_of_articles.Substring(dir_of_articles.LastIndexOf("\\") + 1) + ".html";
            try
            {
                s = new StreamWriter(output_path, false);
            }
            catch
            {
                Console.WriteLine("The file cannot be opened !\nPress any key..."); Console.ReadKey(); return;
            }
            //
            s.AutoFlush = true;
            //
            Z_Family arts = new Z_Family(dir_of_articles);
            foreach (var item in arts.PFamily)
            {
                words += item.total_words;
            }
            //
            int counter = 1;
            int ng = 1;
            HTML table_root = HTML.CreateTabel();
            HTML row1 = HTML.GetRowWithNColomons(3);
            row1.Inner[0].SetContent("id");
            row1.Inner[1].SetContent("name");
            row1.Inner[2].SetContent("freq");
            table_root.AddInner(row1);
            foreach (var ngram in arts.words_in_year_detailed_groups)//ngrms
            {
                row1 = HTML.GetRowWithNColomons(3);
                row1.Inner[0].SetContent("---");
                row1.Inner[1].SetContent(ng + "gram");
                row1.Inner[2].SetContent("---");
                table_root.AddInner(row1);
                foreach (var item in ngram)
                {
                    row1 = HTML.GetRowWithNColomons(3);
                    row1.Inner[0].SetContent((counter++).ToString());
                    row1.Inner[1].SetContent(item.word.word);
                    row1.Inner[2].SetContent((item.word.freq / (float)words).ToString());
                    table_root.AddInner(row1);
                }
                ng++;
            }
            //
            s.Write(table_root.ToString());

        }

        private static void Article_Analysis()
        {
            Console.WriteLine("The default is using only n-grams do you want to change?\nput 1 to only n-grams\n2to only stylistes\n3 to both");
            var tmp = Console.ReadKey();
            bool keyPressedOk = (tmp.Key == ConsoleKey.D1 || tmp.Key == ConsoleKey.D2
                    || tmp.Key == ConsoleKey.D3 || tmp.Key == ConsoleKey.NumPad1
                    || tmp.Key == ConsoleKey.NumPad2 || tmp.Key == ConsoleKey.NumPad3);
            do
            {
                if (keyPressedOk)
                {
                    switch (tmp.Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            chooseMethod = 1;
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            chooseMethod = 2;
                            break;
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            chooseMethod = 3;
                            break;

                    }

                    break;
                }
                Console.WriteLine("\nYou Have Entered a Wrong char Please Try Again ");
                tmp = Console.ReadKey();
                keyPressedOk = (tmp.Key == ConsoleKey.D1 || tmp.Key == ConsoleKey.D2
                        || tmp.Key == ConsoleKey.D3 || tmp.Key == ConsoleKey.NumPad1
                        || tmp.Key == ConsoleKey.NumPad2 || tmp.Key == ConsoleKey.NumPad3);
            } while (!keyPressedOk);
            Console.Clear();
            //***********************//
            Console.WriteLine("The default DIR is \"TXT\\BOOKS\", and the default StartYear is 2000.");
            Console.WriteLine("For another Conference please Enter \"TXT\\***\"");
            Console.WriteLine("For Example: \"TXT\\ACL\"");
            Console.WriteLine("Do you want to change it? (y/n)");
            do
            {
                tmp = Console.ReadKey();
                if (tmp.Key == ConsoleKey.Y)
                {
                    Console.WriteLine("\nEnter DIR:");
                    dir_of_articles_folders = Console.ReadLine();
                    /* Console.WriteLine("Enter StartYear: (xxxx)");
                     startYear = int.Parse(Console.ReadLine());*/
                    break;
                }
                else if (tmp.Key == ConsoleKey.N)
                {
                    break;
                }else
                    Console.WriteLine("Try agian, Please enter Y/N");
            } while (true);
            Console.Clear();
            //
            /* Console.WriteLine("The default Interval Of Years is 3 and 5.");
             Console.WriteLine("Do you want to change it? (y/n)");
             tmp = Console.ReadKey();
             if (tmp.Key == ConsoleKey.Y)
             {
                 Console.WriteLine("\nEnter first interval:");
                 INTERVAL_OF_YEAR_A = int.Parse(Console.ReadLine());
                 Console.WriteLine("Enter second interval:");
                 INTERVAL_OF_YEAR_B = int.Parse(Console.ReadLine());
             }
             Console.Clear();*/
            //
#if A_Way
            Console.WriteLine("The default of Freq. Threshold: 0 .");
#endif
#if B_Way
			Console.WriteLine("The default of Top Freq.: 50 .");
#endif
            Console.WriteLine("Do you want to change it? (y/n)");
            do
            {
                tmp = Console.ReadKey();
                if (tmp.Key == ConsoleKey.Y)
                {
#if A_Way
                    bool notFirstTime = false;
                    Console.WriteLine("\nEnter Freq. Threshold:");
                    do
                    {

                        if (notFirstTime)
                            Console.WriteLine("Please Try Agian with a real Number");
                        notFirstTime = true;
                    } while (!double.TryParse(Console.ReadLine(), out THRESHOLD));

                    Program.THRESHOLD = THRESHOLD;
                    break;
#endif
#if B_Way
				Console.WriteLine("Enter Top Freq.:");
				TOP_SKIP = int.Parse(Console.ReadLine());
				Program.SKIP_COUNT = TOP_SKIP;
#endif
                }
                else if (tmp.Key == ConsoleKey.N)
                    break;
                else
                    Console.WriteLine("Please try agian with N/Y");
            } while (true);
            Console.Clear();
            //
            Console.WriteLine("Please Select Option For reducing uni-grams with appearance less than 3 For Each Article Or For All Of them");
            Console.WriteLine("Press E for Each Articles / A for All of them / N for None of those options ");
            tmp = Console.ReadKey();
            bool ok;
            do
            {
                switch (tmp.Key)
                {
                    case ConsoleKey.N:
                        Program.ForOneArticle = "N";
                        break;
                    case ConsoleKey.A:
                        Program.ForOneArticle = "A";
                        break;
                    case ConsoleKey.E:
                        Program.ForOneArticle = "E";
                        break;
                }
                ok = (tmp.Key == ConsoleKey.N || tmp.Key == ConsoleKey.A
                    || tmp.Key == ConsoleKey.E);
                if (!ok)
                {
                    Console.WriteLine("Wrong Answer Please Try agian (A\\E\\N)");
                }
            } while (!ok);
            Console.Clear();
            //
            if (!(chooseMethod == 2))
            {
                Console.WriteLine("The default of # of uni-grams: 500 .");
                Console.WriteLine("Do you want to change it? (y/n)");
          do{
                tmp = Console.ReadKey();
                if (tmp.Key == ConsoleKey.Y)
                {
                    bool notFirstTime = false;
                    Console.WriteLine("\nEnter # of uni-grams:");
                    do
                    {

                        if (notFirstTime)
                            Console.WriteLine("Please Try Agian with a Natural Number");
                        notFirstTime = true;
                    } while (!int.TryParse(Console.ReadLine(), out Program.NUM_OF_ONE));
     break;
                    }else if(tmp.Key==ConsoleKey.N)
                        break;
                    else
                        Console.WriteLine("please try agian with N/Y");
	            } while (true);
                Console.Clear();
                //
                //
                Console.WriteLine("The default of # of bi-grams: 100 .");
                Console.WriteLine("Do you want to change it? (y/n)");
                do
	            {
	                tmp = Console.ReadKey();
                    if (tmp.Key == ConsoleKey.Y)
                    {
                        bool notFirstTime = false;
                        Console.WriteLine("\nEnter # of bi-grams:");
                        do
                        {

                            if (notFirstTime)
                                Console.WriteLine("Please Try Agian with a Natural Number");
                            notFirstTime = true;
                        } while (!int.TryParse(Console.ReadLine(), out Program.NUM_OF_TWO));
                        break;
                    }else if(tmp.Key==ConsoleKey.N)
                        break;
                    else
                        Console.WriteLine("please try agian with N/Y");
	            } while (true);
                Console.Clear();
                //
                Console.WriteLine("The default of # of tri-grams: 50 .");
                Console.WriteLine("Do you want to change it? (y/n)");
               do{
                tmp = Console.ReadKey();
                if (tmp.Key == ConsoleKey.Y)
                {
                    bool notFirstTime = false;
                    Console.WriteLine("\nEnter # of tri-grams:");
                    do
                    {

                        if (notFirstTime)
                            Console.WriteLine("Please Try Agian with a Natural Number");
                        notFirstTime = true;
                    } while (!int.TryParse(Console.ReadLine(), out Program.NUM_OF_THREE));
     break;
                
                    }else if(tmp.Key==ConsoleKey.N)
                        break;
                    else
                        Console.WriteLine("please try agian with N/Y");
	            } while (true);
                
                Console.Clear();
                //
                //
                Console.WriteLine("The default of # of quad-grams: 25 .");
                Console.WriteLine("Do you want to change it? (y/n)");
            do{
            tmp = Console.ReadKey();
                if (tmp.Key == ConsoleKey.Y)
                {
                    bool notFirstTime = false;
                    Console.WriteLine("\nEnter # of quad-grams:");
                    do
                    {

                        if (notFirstTime)
                            Console.WriteLine("Please Try Agian with a Natural Number");
                        notFirstTime = true;
                    } while (!int.TryParse(Console.ReadLine(), out Program.NUM_OF_FOUR));
     break;
                    }else if(tmp.Key==ConsoleKey.N)
                        break;
                    else
                        Console.WriteLine("please try agian with N/Y");
	            } while (true);

                Console.Clear();
                //
            }
            else
            {
                Program.NUM_OF_ONE = 0;
                Program.NUM_OF_TWO = 0;
                Program.NUM_OF_THREE = 0;
                Program.NUM_OF_FOUR = 0;
            }
            //
            Console.WriteLine("The default of # of rear uni-grams: 0 .");
            Console.WriteLine("Do you want to change it? (y/n)");
            do{
                tmp = Console.ReadKey();
            if (tmp.Key == ConsoleKey.Y)
            {
                bool notFirstTime = false;
                Console.WriteLine("\nEnter # of rear uni-grams:");
                do
                {

                    if (notFirstTime)
                        Console.WriteLine("Please Try Agian with a Natural Number");
                    notFirstTime = true;
                } while (!int.TryParse(Console.ReadLine(), out Program.RareUniGrams));
     break;
                    }else if(tmp.Key==ConsoleKey.N)
                        break;
                    else
                        Console.WriteLine("please try agian with N/Y");
	            } while (true);
            Console.Clear();
            //
            Console.WriteLine("The default of # of rear bi-grams: 0 .");
            Console.WriteLine("Do you want to change it? (y/n)");
            do{
                tmp = Console.ReadKey();
            if (tmp.Key == ConsoleKey.Y)
            {
                bool notFirstTime = false;
                Console.WriteLine("\nEnter # of rear bi-grams:");
                do
                {

                    if (notFirstTime)
                        Console.WriteLine("Please Try Agian with a Natural Number");
                    notFirstTime = true;
                } while (!int.TryParse(Console.ReadLine(), out Program.RareBiGrams));
     break;
                    }else if(tmp.Key==ConsoleKey.N)
                        break;
                    else
                        Console.WriteLine("please try agian with N/Y");
	            } while (true);
            Console.Clear();
            //
            Console.WriteLine("The default of # of rear tri-grams: 0 .");
            Console.WriteLine("Do you want to change it? (y/n)");
           do{
               tmp = Console.ReadKey();
            if (tmp.Key == ConsoleKey.Y)
            {
                bool notFirstTime = false;
                Console.WriteLine("\nEnter # of rear tri-grams:");
                do
                {

                    if (notFirstTime)
                        Console.WriteLine("Please Try Agian with a Natural Number");
                    notFirstTime = true;
                } while (!int.TryParse(Console.ReadLine(), out Program.RareTriGrams));

                 break;
                    }else if(tmp.Key==ConsoleKey.N)
                        break;
                    else
                        Console.WriteLine("please try agian with N/Y");
	            } while (true);
            Console.Clear();
            //
            Console.WriteLine("The default of # of rear quad-grams: 0 .");
            Console.WriteLine("Do you want to change it? (y/n)");
            do{
                tmp = Console.ReadKey();
            if (tmp.Key == ConsoleKey.Y)
            {
                bool notFirstTime = false;
                Console.WriteLine("\nEnter # of rear quad-grams:");
                do
                {

                    if (notFirstTime)
                        Console.WriteLine("Please Try Agian with a Natural Number");
                    notFirstTime = true;
                } while (!int.TryParse(Console.ReadLine(), out Program.RareQuadGrams));

                break;
                    }else if(tmp.Key==ConsoleKey.N)
                        break;
                    else
                        Console.WriteLine("please try agian with N/Y");
	            } while (true);
            Console.Clear();
            //
            Console.WriteLine("Do you want to take out the Stop Words?");
            Console.WriteLine("press Y to yes and N to no (y/n)");
            tmp = Console.ReadKey();
            bool agian=false;
            do
            {
                switch (tmp.Key)
                {
                    case ConsoleKey.Y:
                        Program.RemoveStopWords = true;
                        break;
                    case ConsoleKey.N:
                        Program.RemoveStopWords = false;
                        break;
                    default:
                        agian = true;
                        break;
                }
                if(agian)
                {
                    Console.WriteLine("wrong key\nplease press Y or N");
                }
            } while (agian);

            Console.Clear();
            
            string output_path = "U-" + Program.NUM_OF_ONE + " B-" + Program.NUM_OF_TWO + " T-" + Program.NUM_OF_THREE + " F-" + Program.NUM_OF_FOUR;
            output_path += (Program.RareUniGrams != 0) ? (" RU-"+Program.RareUniGrams.ToString()) : "";
            output_path += (Program.RareBiGrams != 0) ? (" RB-" + Program.RareBiGrams.ToString()) : "";
            output_path += (Program.RareTriGrams != 0) ? (" RT-" + Program.RareTriGrams.ToString()) : "";
            output_path += (Program.RareQuadGrams != 0) ? (" RQ-" + Program.RareQuadGrams.ToString()) : "";
            output_path += ((chooseMethod == 1) ? "_Only Ngrams" : "_with ortographic family") + ".csv";
            make_csv_file(dir_of_articles_folders, output_path);
        }

        private static void make_csv_file(string dir_of_articles, string output_path)
        {

            string[] files;
            StreamWriter s;
            StreamReader r;
            try
            {
                files = GetFilesInDir(dir_of_articles);
                s = new StreamWriter(output_path, false);
            }
            catch
            {
                Console.WriteLine("The file cannot be opened !\nPress any key..."); Console.ReadKey(); return;
            }
            //
            s.AutoFlush = true;
            //
            if (chooseMethod == 2 || chooseMethod == 3)
            {
                make_Cons_txt_files(files);
            }
            string[] files_names = files.Where(str => !str.Contains("_tagger_output")).ToArray();
            //

            MakePFamily(files_names);
            MakeNGRAMWords();

            #region Headlines
            printHead(s);
            #endregion
            #region print the files in directory
            int run_num = 1;

            foreach (string name_of_file in files_names)
            {
                r = new StreamReader(name_of_file);
                Console.Clear();
#if OnlyUniGram
				Console.WriteLine("Only Uni-gram");
#endif
#if A_Way
                Console.WriteLine("Way: A");
#endif
#if B_Way
				Console.WriteLine("Way: B");
#endif
                Console.WriteLine("\t---\nThe program calculate the interval of year when the first year is 2000");
                Console.WriteLine("no. " + run_num + " of " + files_names.Length + "\t" + ((float)run_num * 100 / files_names.Length).ToString("f2") + "%");
                print(name_of_file, s, r, run_num);
                run_num++;
                r.Close();
            }

            #endregion
            s.Close();
        }

        private static void MakePFamily(string[] files_names)
        {
            p_arr = new P_family[files_names.Length];
            int i = 0;
            foreach (string item in files_names)
            {
                Console.WriteLine(item);
                p_arr[i] = new P_family(item);
                p_arr[i].ResizeTables();
                TotalWords += p_arr[i].total_words;
                i++;
            }
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

        private static void make_Cons_txt_files(string[] names_of_files)
        {
            StreamReader st;
            StreamWriter write;
            string words;
            int count = 1;
            foreach (var file in names_of_files)
            {
                Console.Clear();
                Console.Write("Making the 500, 1000, 1500 words files.\t" + ((float)count * 100 / names_of_files.Length).ToString("f2") + "%");
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
            Console.Clear();
        }
        // Unite all tables
        private static void MakeNGRAMWords()
        {
            HashSet<SeqString> uni_gram = new HashSet<SeqString>(from y in p_arr
                                                                 from x in y.table_of_one
                                                                 select x);
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





            StreamWriter sw = new StreamWriter("5000_words.txt");
            string s = "";
            foreach (SeqString ss in uni_gram)
            {
                s += ss.word + "\t" + ss.freq + "\r\n";
            }
            sw.Write(s);
            sw.Close();
#if !OnlyUniGram
            HashSet<SeqString> two_gram = new HashSet<SeqString>(from y in p_arr
                                                                 from x in y.table_of_two
                                                                 select x);

             rare_two_gram = null;
            if (Program.RareBiGrams != 0)
            {
                rare_two_gram = new HashSet<SeqString>(
                    (new HashSet<SeqString>(two_gram.OrderBy(x => x.freq))
                    .Take(Program.RareBiGrams)));
            }


            two_gram = new HashSet<SeqString>((new HashSet<SeqString>(two_gram.OrderByDescending(x => x.freq)).Take(ContentFamilies.Program.NUM_OF_TWO)));

            ///////
            HashSet<SeqString> three_gram = new HashSet<SeqString>(from y in p_arr
                                                                   from x in y.table_of_three
                                                                   select x);

            rare_three_gram = null;
            if (Program.RareTriGrams != 0)
            {
                rare_three_gram = new HashSet<SeqString>(
                    (new HashSet<SeqString>(three_gram.OrderBy(x => x.freq))
                    .Take(Program.RareTriGrams)));
            }


            three_gram = new HashSet<SeqString>((new HashSet<SeqString>(three_gram.OrderByDescending(x => x.freq)).Take(Program.NUM_OF_THREE)));


            ////////
            HashSet<SeqString> four_gram = new HashSet<SeqString>(from y in p_arr
                                                                  from x in y.table_of_four
                                                                  select x);
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
#if !OnlyUniGram
            allGramsWords.AddRange((from y in two_gram
                                    select y.word).ToArray());
            allGramsWords.AddRange((from y in three_gram
                                    select y.word).ToArray());
            allGramsWords.AddRange((from y in four_gram
                                    select y.word).ToArray());

#endif
            if (Program.RareUniGrams != 0)
            {
                allGramsWords.Union((from y in rare_uni_gram
                                        select y.word).ToArray());
            }
            if (Program.RareBiGrams != 0)
            {
                allGramsWords.Union((from y in rare_two_gram
                                        select y.word).ToArray());
            }
            if (Program.RareTriGrams != 0)
            {
                allGramsWords.Union((from y in rare_three_gram
                                        select y.word).ToArray());
            }
            if (Program.RareQuadGrams != 0)
            {
                allGramsWords.Union((from y in rare_four_gram
                                        select y.word).ToArray());
            }
            //
            allGramsWords = new List<string>(allGramsWords.OrderBy(x => x.Count(y => y == ' ')));
        }

        private static void printHead(StreamWriter s)
        {
            StringBuilder headsave = new StringBuilder();
            if (chooseMethod == 2 || chooseMethod == 3)
            {
                #region OrthographicFamily - 20
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
                #endregion

                #region quantitativeCharacteristicsFamily - 3
                headsave.Append("averageCharacterPerWords,averageCharacterPerSentences,averageWordsPerSentences,");
                #endregion
                #region ReachnessLang - 6 (*4) - 24
                headsave.Append("difWordsNormalized,oneWordsNormalized,twoWordsNormalized,threeWordsNormalized,fourWordsNormalized,fiveWordsNormalized,");
                headsave.Append("difWords_of500Normalized,oneWords_of500Normalized,twoWords_of500Normalized,threeWords_of500Normalized,fourWords_of500Normalized,fiveWords_of500Normalized,");
                headsave.Append("difWords_of1000Normalized,oneWords_of1000Normalized,twoWords_of1000Normalized,threeWords_of1000Normalized,fourWords_of1000Normalized,fiveWords_of1000Normalized,");
                headsave.Append("difWords_of1500Normalized,oneWords_of1500Normalized,twoWords_of1500Normalized,threeWords_of1500Normalized,fourWords_of1500Normalized,fiveWords_of1500Normalized,");
                #endregion
                #region Stemmer - 12 (*4) - 48
                headsave.Append("difStemsNormalized,oneStemsNormalized,twoStemsNormalized,threeStemsNormalized,fourStemsNormalized,fiveStemsNormalized,");
                headsave.Append("difStemsNormalizedByStem,oneStemsNormalizedByStem,twoStemsNormalizedByStem,threeStemsNormalizedByStem,fourStemsNormalizedByStem,fiveStemsNormalizedByStem,");
                headsave.Append("difStems_of500Normalized,oneStems_of500Normalized,twoStems_of500Normalized,threeStems_of500Normalized,fourStems_of500Normalized,fiveStems_of500Normalized,");
                headsave.Append("difStems_of500NormalizedByStem,oneStems_of500NormalizedByStem,twoStems_of500NormalizedByStem,threeStems_of500NormalizedByStem,fourStems_of500NormalizedByStem,fiveStems_of500NormalizedByStem,");
                headsave.Append("difStems_of1000Normalized,oneStems_of1000Normalized,twoStems_of1000Normalized,threeStems_of1000Normalized,fourStems_of1000Normalized,fiveStems_of1000Normalized,");
                headsave.Append("difStems_of1000NormalizedByStem,oneStems_of1000NormalizedByStem,twoStems_of1000NormalizedByStem,threeStems_of1000NormalizedByStem,fourStems_of1000NormalizedByStem,fiveStems_of1000NormalizedByStem,");
                headsave.Append("difStems_of1500Normalized,oneStems_of1500Normalized,twoStems_of1500Normalized,threeStems_of1500Normalized,fourStems_of1500Normalized,fiveStems_of1500Normalized,");
                headsave.Append("difStems_of1500NormalizedByStem,oneStems_of1500NormalizedByStem,twoStems_of1500NormalizedByStem,threeStems_of1500NormalizedByStem,fourStems_of1500NormalizedByStem,fiveStems_of1500NormalizedByStem,");
                #endregion
                #region Tagger - 72 (*4) - 288
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
            foreach (string word in allGramsWords)
            {
                int numOfWords = word.Count(x => x == ' ') + 1;
                string ToAdd = word.Replace("'", "_tag_") + "_" + (numOfWords) + "_" + "gram";
                switch(numOfWords)
                {
                    case 1:
                        if (Program.RareUniGrams != 0 && ((from x in rare_uni_gram
                              select x.word).ToList()).Contains(word))
                            ToAdd = ToAdd.Replace("gram", "rare_gram");
                        break;
                    case 2:
                        if (Program.RareBiGrams!=0 && ((from x in rare_two_gram
                              select x.word).ToList()).Contains(word))
                            ToAdd = ToAdd.Replace("gram", "rare_gram");
                        break;
                    case 3:
                        if (Program.RareTriGrams!=0 && ((from x in rare_three_gram
                              select x.word).ToList()).Contains(word))
                            ToAdd = ToAdd.Replace("gram", "rare_gram");
                        break;
                    case 4:
                        if (Program.RareQuadGrams != 0 && ((from x in rare_four_gram
                              select x.word).ToList()).Contains(word))
                            ToAdd = ToAdd.Replace("gram", "rare_gram");
                        break;
                }
                headsave.Append(ToAdd + ",");
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

        private static void print(string name_of_file, StreamWriter writer, StreamReader reader, int i)
        {
            StringBuilder save = new StringBuilder();
            //
            if (chooseMethod == 2 || chooseMethod == 3)
            {
                #region Orthografic
                Console.WriteLine("\tOrthographic family...");
                OrthographicFamily Ortho = new OrthographicFamily(name_of_file);
                save.Append(Ortho.ToString());
                OrthographicFamily Ortho500 = new OrthographicFamily(name_of_file.Insert(name_of_file.IndexOf('\\'), "of500"));
                save.Append(Ortho500.ToString());
                OrthographicFamily Ortho1000 = new OrthographicFamily(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1000"));
                save.Append(Ortho1000.ToString());
                OrthographicFamily Ortho1500 = new OrthographicFamily(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1500"));
                save.Append(Ortho1500.ToString());
                #endregion
#if ReferenceFamily
                #region References
            Console.WriteLine("\tReferences Family...");
            ReferencesFamily Refe = new ReferencesFamily(name_of_file);
            save.Append(Refe.ToString());
                #endregion
#endif
                #region Quantitative
                Console.WriteLine("\tQuantitative Characteristics Family...");
                quantitativeCharacteristicsFamily quan = new quantitativeCharacteristicsFamily(name_of_file);
                save.Append(quan.ToString());
                #endregion
                #region Reachness Lang
                Console.WriteLine("\tReachness Lang...");
                ReachnessLang Reac = new ReachnessLang(name_of_file);
                save.Append(Reac.ToString());
                ReachnessLang Reac500 = new ReachnessLang(name_of_file.Insert(name_of_file.IndexOf('\\'), "of500"));
                save.Append(Reac500.ToString());
                ReachnessLang Reac1000 = new ReachnessLang(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1000"));
                save.Append(Reac1000.ToString());
                ReachnessLang Reac1500 = new ReachnessLang(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1500"));
                save.Append(Reac1500.ToString());
                #endregion
                #region Stem
                name_of_file.Replace("articles", "articlesof500");
                //
                Console.WriteLine("\tStemmer...");
                porter.Stemmer stem = new porter.Stemmer(name_of_file);
                save.Append(stem.ToString1());
                porter.Stemmer stem500 = new porter.Stemmer(name_of_file.Insert(name_of_file.IndexOf('\\'), "of500"));
                save.Append(stem500.ToString1());
                porter.Stemmer stem1000 = new porter.Stemmer(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1000"));
                save.Append(stem1000.ToString1());
                porter.Stemmer stem1500 = new porter.Stemmer(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1500"));
                save.Append(stem1500.ToString1());
                #endregion
                #region Tagger
                Console.WriteLine("\tTagger...");
                Tagger tagg = new Tagger(name_of_file);
                save.Append(tagg.ToString());
                Tagger tagg500 = new Tagger(name_of_file.Insert(name_of_file.IndexOf('\\'), "of500"));
                save.Append(tagg500.ToString());
                Tagger tagg1000 = new Tagger(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1000"));
                save.Append(tagg1000.ToString());
                Tagger tagg1500 = new Tagger(name_of_file.Insert(name_of_file.IndexOf('\\'), "of1500"));
                save.Append(tagg1500.ToString());
                #endregion
            }
            #region P Family
            save.Append(p_arr[i - 1].ToString(allGramsWords, p_arr[i - 1].total_words));
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

    }
}
