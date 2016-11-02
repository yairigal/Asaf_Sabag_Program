using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace PatternFamilies
{
    public class OrthographicFamily
    {
        public OrthographicFamily(string Address)
        {
            address = Address;
            calculate();
        }

        public static char[] symbols = { '\'', '"', ':', ';', '.', ',', '!', '?', '-', '/'/*, '\\'*/ };
        private static int num_of_symbols = symbols.Length;
        public string address;
        public long count_of_letters = 0;
        public long count_of_words = 0;

        private float[] counts_in_letters = new float[num_of_symbols];
        private float[] counts_in_words = new float[num_of_symbols];

        public float[] counts = new float[num_of_symbols];
        public float[] counts_in_letters_normal = new float[num_of_symbols];
        public float[] counts_in_words_normal = new float[num_of_symbols];

        public void calculate()
        {
            #region count of letters
            {
                quantitativeCharacteristicsFamily q = new quantitativeCharacteristicsFamily(address);
                count_of_letters = (long)q.charactersCount;
                //StreamReader st = new StreamReader(address);
                //int ch;
                //ch = st.Read();
                //while (ch != -1)
                //{
                //    count_of_letters++;
                //    ch = st.Read();
                //}
            }
            #endregion
            #region count of words
            {
                quantitativeCharacteristicsFamily q = new quantitativeCharacteristicsFamily(address);
                count_of_words = (long)q.wordsCount;
                //StreamReader st = new StreamReader(address);
                //string line = st.ReadLine();

                //while (line != null)
                //{

                //    string[] str = line.Split(' ', '.', ',');

                //    count_of_words += str.Length;
                //    for (int i = 0; i < str.Length; i++)
                //        if (str[i] == "") count_of_words--;
                //    line = st.ReadLine();
                //}
            }
            #endregion
            #region count of chars
            {
                StreamReader st = new StreamReader(address);
                int ch;
                for (int i = 0; i < num_of_symbols; i++)
                {
                    ch = st.Read();
                    while (ch != -1)
                    {
                        if (((char)ch).CompareTo(symbols[i]) == 0)
                            counts[i]++;
                        ch = st.Read();
                    }
                    st.Close();
                    st = new StreamReader(address);
                }
            }
            #endregion
            #region normal
            {
                for (int i = 0; i < num_of_symbols; i++)
                {
                    counts_in_words_normal[i] = counts[i] / count_of_words;
                    counts_in_letters_normal[i] = counts[i] / count_of_letters;
                }
            }
            #endregion
        }
        public override string ToString()
        {
            string save = "";
            foreach (var item in counts_in_letters_normal)
            {
                save += (item + ",");
            }
            foreach (var item in counts_in_words_normal)
            {
                save += (item + ",");
            }
            return save;
        }
    }
}
