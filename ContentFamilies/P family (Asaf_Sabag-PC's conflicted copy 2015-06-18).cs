using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ContentFamilies
{
    public class P_family : FreqWordsForOneArticle, ICloneable, IDisposable  // Family of N-Grams per ARTICLE.
    {

        public P_family(string year_path)
            : base(year_path)
        {

            //This family work on txt files.
        }
        public P_family()
            : base()
        {
            //This family work on txt files.
        }


        public static int FilterByThreshold(ref HashSet<SeqString> table, int total_words)
        {
            if (Program.THRESHOLD == 0)
                return 0;
            int num_of_selected = 0;
            double n;
            HashSet<SeqString> new_table = new HashSet<SeqString>();
            foreach (var word in table)
            {
                if ((word.freq / (float)total_words) < Program.THRESHOLD)
                    new_table.Add(word);
                else
                    num_of_selected++;
            }
            table = new_table;
            return num_of_selected;
        }

        public static void FilterByTopCount(ref HashSet<SeqString> table)
        {
            table = new HashSet<SeqString>(table.Skip(Program.SKIP_COUNT));
        }

        public string ToString(List<string> AllNGRAMS, int total_words)
        {
            StringBuilder temp = new StringBuilder();
            float freq;
            foreach (string word in AllNGRAMS)
            {
                freq = 0;
                try
                {
                    /* freq = (from x in table_of_one
                                                 where x.word.Equals(word)
                                                 select x.freq).FirstOrDefault();*/
                    freq = (float)(table_of_one.FirstOrDefault(x => x.word.Contains(word))).freq;

                    freq = freq / (float)total_words;
                    temp.Append(freq + ",");
                    continue;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    freq = (float)(table_of_two.FirstOrDefault(x => x.word.Contains(word))).freq;

                    freq = freq / (float)total_bigrams;
                    temp.Append(freq + ",");
                    continue;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    freq = (float)(table_of_three.FirstOrDefault(x => x.word.Contains(word))).freq;

                    freq = freq / (float)total_trigrams;
                    temp.Append(freq + ",");
                    continue;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    freq = (float)(table_of_four.FirstOrDefault(x => x.word.Contains(word))).freq;

                    freq = freq / (float)total_qaudgrams;
                    temp.Append(freq + ",");
                    continue;
                }
                catch (Exception ex)
                {


                }

                try
                {
                    freq = (float)(unichar.FirstOrDefault(x => x.word.Contains(word))).freq;

                    freq = freq / (float)total_chars;
                    temp.Append(freq + ",");
                    continue;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    freq = (float)(bichar.FirstOrDefault(x => x.word.Contains(word))).freq;

                    freq = freq / (float)total_bichars;
                    temp.Append(freq + ",");
                    continue;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    freq = (float)(trichar.FirstOrDefault(x => x.word.Contains(word))).freq;
                    freq = freq / (float)total_trichars;
                    temp.Append(freq + ",");
                    continue;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    freq = (float)(quadchar.FirstOrDefault(x => x.word.Contains(word))).freq;
                    freq = freq / (float)total_quadchars;
                    temp.Append(freq + ",");
                    continue;
                }
                catch (Exception ex)
                {

                    temp.Append(freq + ",");
                }



            }
            return temp.ToString();
        }

        public void RemoveLessThan(int min_freq)
        {
            table_of_one = new HashSet<SeqString>(table_of_one.Where(x => x.freq >= min_freq));
            table_of_two = new HashSet<SeqString>(table_of_two.Where(x => x.freq >= min_freq));
            table_of_three = new HashSet<SeqString>(table_of_three.Where(x => x.freq >= min_freq));
            table_of_four = new HashSet<SeqString>(table_of_four.Where(x => x.freq >= min_freq));
        }

        public void ResizeTables()
        {
            table_of_one = new HashSet<SeqString>(table_of_one.Take(Program.RESIZE));
#if !OnlyUniGram
            table_of_two = new HashSet<SeqString>(table_of_two.Take(Program.RESIZE));
            table_of_three = new HashSet<SeqString>(table_of_three.Take(Program.RESIZE));
            table_of_four = new HashSet<SeqString>(table_of_four.Take(Program.RESIZE));
#endif
        }



        object ICloneable.Clone()
        {
            return this.Clone();
        }
        public P_family Clone()
        {
            return (P_family)this.MemberwiseClone();
        }

        static public void Serialize(List<P_family> details)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<P_family>));
            using (TextWriter writer = new StreamWriter(@"seriable.xml"))
            {
                serializer.Serialize(writer, details);
            }
        }
        static public List<P_family> DeSerialize()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<P_family>));
            TextReader reader = new StreamReader(@"seriable.xml");
            object obj = deserializer.Deserialize(reader);
            List<P_family> XmlData = (List<P_family>)obj;
            reader.Close();
            return XmlData;
        }

        static public P_family[] GetCopy(P_family[] arr)
        {
            Serialize(arr.ToList());
            return DeSerialize().ToArray();
        }



        static public void Serialize(P_family details)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(P_family));
            using (TextWriter writer = new StreamWriter(@"seriable.xml"))
            {
                serializer.Serialize(writer, details);
            }
        }
        static public P_family DeSerializeOne()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(P_family));
            TextReader reader = new StreamReader(@"seriable.xml");
            object obj = deserializer.Deserialize(reader);
            P_family XmlData = (P_family)obj;
            reader.Close();
            return XmlData;
        }

        static public P_family GetCopy(P_family p)
        {
            Serialize(p);
            return DeSerializeOne();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GC.Collect();
        }


        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in table_of_one)
                {
                    item.Dispose();
                }
                foreach (var item in table_of_two)
                {
                    item.Dispose();
                }
                foreach (var item in table_of_three)
                {
                    item.Dispose();
                }
                foreach (var item in table_of_four)
                {
                    item.Dispose();
                }
                foreach (var item in unichar)
                {
                    item.Dispose();
                }
                foreach (var item in bichar)
                {
                    item.Dispose();
                }
                foreach (var item in trichar)
                {
                    item.Dispose();
                }
                foreach (var item in quadchar)
                {
                    item.Dispose();
                }


                table_of_one = null;
                table_of_two = null;
                table_of_three = null;
                table_of_four = null;

                unichar = null;
                bichar = null;
                trichar = null;
                quadchar = null;

                year = string.Empty;
                total_words = 0;
                total_bigrams = 0;
                total_trigrams = 0;
                total_qaudgrams = 0;
                total_bichars = 0;
                total_trichars = 0;
                total_quadchars = 0;
                total_chars = 0;
                bookType = string.Empty;
                
                //helpers
                isP = false;
                articles = null;
                pathToFile = "";
                year_dir = string.Empty;
                stopwords = null;
            }
        }
    }
}
