using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace PatternFamilies
{
    public enum PublicatinType
    {
        Draft_Papers = 0,
        Technical_Papers,
        Full_Workshop_Papers,
        Short_Workshop_Papers,//
        Symposium_Papers,
        Conference_Posters,
        Short_Conference_Papers,
        Full_Conference_Papers,
        Journal_Papers,
        Masters_Thesis,
        Ph_D_Dissertations,
        Papers_in_a_book,
        Chapters_in_a_book,
        Full_books,
        Self_references
    }

    public class ReferencesFamily
    {
        public ReferencesFamily(string Address)
        {
            address = Address;
            calculate();
        }
        public const int num_of_publications = 15;
        public string address;
        //array of lists that contein all the keywords of the REFENECE
        private static List<string>[] keywords = new List<string>[num_of_publications];
        //
        private List<pairString> conferences_workshops = new List<pairString>();
        private List<pairString> journals = new List<pairString>();
        private List<pairString> conference = new List<pairString>();
        private List<pairString> workshop = new List<pairString>();
        private List<pairString> symposium = new List<pairString>();
        //array of counts of every REFERENCE
        public float[] count_of_by_keywords_normalize = new float[keywords.Length];
        public float[] count_of_from_dblp_normalize = new float[4];
        public long num_of_ref = 0;
        public long num_of_numbered_ref = 0;
        public long num_of_lines = 0;
        public long num_of_dotsEndLines = 0;//doesn't printed
        public void calculate()
        {
            #region init
            count_of_by_keywords_normalize.Initialize();
            count_of_from_dblp_normalize.Initialize();
            makeKeywords();
            makeConferences_Workshops();
            makeJournals();
            #endregion
            StreamReader st = new StreamReader(address);
            //the whole text
            string the_whole_text = st.ReadToEnd();
            // the reference part
            string text;

            #region opening the file
            try
            {
                text = the_whole_text.Substring(the_whole_text.LastIndexOf("References", StringComparison.CurrentCultureIgnoreCase));
            }
            catch
            {
                Console.WriteLine("Didn't found the title reference");
                return;
            }
            #endregion
            #region counting...
            //the words in document
            string[] words = text.Split(' ', '.', '\n', '\r', '\t');
            //make the count of any shortname
            for (int i = 0; i < keywords.Length; i++)
            {
                count_of_by_keywords_normalize[i] = (from x in keywords[i]//need fix to FirstOf \ IndexOf
                                             from y in words
                                             where x.CompareTo(y) == 0
                                             select x).ToArray().Length;
            }
            //how many reference according to sum of the count of any shortname
            foreach (var item in count_of_by_keywords_normalize)
            {
                num_of_ref += (long)item;
            }
            //how many lines at reference
            num_of_lines = (from x in text
                            where (x == '\n')// || x == '\r')
                            select x).ToArray().Length;
            num_of_lines--;//because it is include the line of the subtitle REFERENCES

            //how many dots (.) that end the line in reference - another way to assume the number of references
            try
            {
                string copy = text;
                while (true)
                {
                    copy = copy.Substring(copy.IndexOf('.'));
                    if (copy[1] == '\n' || copy[1] == '\r')
                    {
                        num_of_dotsEndLines++;
                    }
                    copy = copy.Substring(1);
                }
            }
            catch
            { }

            //check for nubered lines, and count them
            if (text.IndexOf("[1]") != -1)
            {
                int loc = 0;
                int i;
                for (i = 1; (loc = text.IndexOf("[" + i + "]", loc)) != -1; i++) ;
                num_of_numbered_ref = i - 1;
            }

            //make count od from dblp
            int ind = 0;
            foreach (pairString item in journals)
            {
                ind = text.IndexOf(" " + item.s2, ind) + 1;
                if (ind == 0)
                    continue;
                count_of_from_dblp_normalize[0]++;
            }
            ind = 0;
            foreach (pairString item in conference)
            {
                ind = text.IndexOf(" " + item.s1, ind) + 1;
                if (ind == 0)
                    continue;
                count_of_from_dblp_normalize[1]++;
            }
            ind = 0;
            foreach (pairString item in symposium)
            {
                ind = text.IndexOf(item.s1, ind) + 1;
                if (ind == 0)
                    break;
                count_of_from_dblp_normalize[2]++;
            }
            ind = 0;
            foreach (pairString item in workshop)
            {
                ind = text.IndexOf(item.s1, ind) + 1;
                if (ind == 0)
                    break;
                count_of_from_dblp_normalize[3]++;
            }
            #endregion
            #region normaling by "num_of_ref"
            if (num_of_ref > 0)
            {
                for (int i = 0; i < count_of_by_keywords_normalize.Length; i++)
                {
                    count_of_by_keywords_normalize[i] /= num_of_ref;
                }
                for (int i = 0; i < count_of_from_dblp_normalize.Length; i++)
                {
                    count_of_from_dblp_normalize[i] /= num_of_ref;
                }
            }
            else
            {
                float m = 0;
                foreach (var item in count_of_from_dblp_normalize)
                {
                    m += item;
                }
                for (int i = 0; i < count_of_from_dblp_normalize.Length; i++)
                {
                    count_of_from_dblp_normalize[i] /= m;
                }
            }
            #endregion
        }

        private void makeKeywords()
        {
            keywords.Initialize();
            for (int i = 0; i < keywords.Length; i++)
            {
                keywords[i] = new List<string>();
            }
            keywords[(int)PublicatinType.Draft_Papers].Add("Paper");

            keywords[(int)PublicatinType.Technical_Papers].Add("Technical note");
            keywords[(int)PublicatinType.Technical_Papers].Add("Technical report");
            keywords[(int)PublicatinType.Technical_Papers].Add("Preliminary Report");
            keywords[(int)PublicatinType.Technical_Papers].Add("Report");
            
            keywords[(int)PublicatinType.Full_Workshop_Papers].Add("workshop");

            keywords[(int)PublicatinType.Short_Workshop_Papers].Add("short workshop");

            keywords[(int)PublicatinType.Symposium_Papers].Add("symposium");
            
            keywords[(int)PublicatinType.Conference_Posters].Add("poster");
            
            keywords[(int)PublicatinType.Short_Conference_Papers].Add("short conference");
            
            keywords[(int)PublicatinType.Full_Conference_Papers].Add("conference");
            
            keywords[(int)PublicatinType.Journal_Papers].Add("Journal");
            
            keywords[(int)PublicatinType.Masters_Thesis].Add("thesis");
            
            keywords[(int)PublicatinType.Ph_D_Dissertations].Add("Ph.D.");
            
            keywords[(int)PublicatinType.Papers_in_a_book].Add("pages");
            
            keywords[(int)PublicatinType.Chapters_in_a_book].Add("chapter");
            
            keywords[(int)PublicatinType.Full_books].Add("book");
            //The self reference
            StreamReader st = new StreamReader(address);//should correct
            st.ReadLine();
            string line = st.ReadLine();
            if (line == "(Extended Abstract)")
                line = st.ReadLine();
            string copy = line.Clone() as string;
            List<string> autors = new List<string>();
            //
            int pos = 0;
            int start = 0;
            while ((pos = copy.IndexOf(", ", start)) != -1)
            {
                autors.Add(copy.Substring(start, pos - start + 1));
                start = pos + 1;
            }
            pos = copy.IndexOf("and ", start);
            if (pos != -1)
            {
                autors.Add(copy.Substring(start, pos - start + 1));
                start = pos + 1;
            }
            autors.Add(copy.Substring(start));
            //
            //try
            //{
            //    int i = 0;
            //    while (true)
            //    {
            //        copy = line.Substring(i);
            //        i = copy.IndexOf("and");
            //        autors.Add(copy.Substring(0, i));
            //        i += 4;
            //    }
            //}
            //catch
            //{
            //    //pick the next 2 words
            //    try
            //    {
            //        string a, b;
            //        a = copy.Substring(0, copy.IndexOf(" "));
            //        line = copy.Substring(copy.IndexOf(" ") + 1);
            //        b = copy.Substring(0, copy.IndexOf(" "));
            //        a = a + " " + b;
            //        autors.Add(a);
            //    }
            //    catch { }
            //}
            keywords[(int)PublicatinType.Self_references].AddRange(autors);
        }
        private void makeConferences_Workshops()
        {
            StreamReader cw = new StreamReader("Conference&Workshops.txt");
            string line = "";
            while (!cw.EndOfStream)
            {
                line = cw.ReadLine();
                if (line == "")
                {
                    continue;
                }
                int f = line.IndexOf(" - ");
                if (f == -1)
                {
                    conferences_workshops.Add(new pairString(line, ""));
                }
                else
                {
                    string t1 = line.Substring(0, f);
                    string t2 = line.Substring(f + 3);
                    conferences_workshops.Add(new pairString(t1, t2));
                }
            }
            //
            workshop = (from x in conferences_workshops
                        where x.s2.Contains("Workshop")
                        select x).ToList();
            symposium = (from x in conferences_workshops
                         where x.s2.Contains("Symposium")
                         select x).ToList();
            conference = (from x in conferences_workshops
                          where (!x.s2.Contains("Symposium") && !x.s2.Contains("Workshop"))
                          select x).ToList();
            var v = (from x in conferences_workshops
                     where (x.s2.Contains("Symposium") && x.s2.Contains("Workshop"))
                     select x).ToList();
        }
        private void makeJournals()
        {
            StreamReader j = new StreamReader("Journals.txt");
            string line = "";
            while (!j.EndOfStream)
            {
                line = j.ReadLine();
                if (line == "")
                {
                    continue;
                }
                int f = line.IndexOf(" (");
                if (f == -1)
                {
                    journals.Add(new pairString("", line));
                }
                else
                {
                    string t1 = line.Substring(0, f);
                    string t2;
                    if (line.Substring(line.Length - 4) == "...)")
                        t2 = line.Substring(f + 2, line.Length - (f + 2) - 4) + t1;//I push the () before t1
                    else
                        t2 = line.Substring(f + 2, line.Length - (f + 2) - 1) + t1;
                    journals.Add(new pairString(t1, t2));
                }
            }

        }

        public override string ToString()
        {
            string save = "";
            foreach (var item in count_of_by_keywords_normalize)
            {
                save += (item + ",");
            }
            foreach (var item in count_of_from_dblp_normalize)
            {
                save += (item + ",");
            }
            return save;
        }
    }
    class pairString
    {
        public string s1;
        public string s2;

        public pairString()
        {
            s1 = null;
            s2 = null;
        }
        public pairString(string t1, string t2)
        {
            s1 = t1;
            s2 = t2;
        }
    }
}
