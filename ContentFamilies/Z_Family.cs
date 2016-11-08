using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentFamilies
{
    public class Z_Family // Family of N-Grams for YEAR (including its articls).
    {
        //props
        //public IOrderedEnumerable<WordInYear>[] words_detailed_ordered_groups { set; get; }// Like word_groups, but sorted by freq
        public List<WordInYear>[] words_in_year_detailed_groups { set; get; }// For each word, show year, total freq in this year, and in how many articles had appeard
		public List<P_family> PFamily
		{
			get
			{
				return words;
			}
		}

        //Helpers
        List<P_family> words;
		List<IGrouping<string, Word>>[] words_groups_detailed { set; get; }// For each word, show the years belog to her (with the freq). sorted by word
		public readonly string year_path; // Like "TXT/acl/09". When: "TXT/acl/09/aaa.txt"
		public readonly int Year;
		
        //Funcs
        public Z_Family(string path_of_year)
        {
            #region init
            this.Year = YearInStr(path_of_year.Substring(path_of_year.LastIndexOf('\\')));
            this.year_path = path_of_year;
            this.words = new List<P_family>();
            words_in_year_detailed_groups = new List<WordInYear>[4];
            //words_detailed_ordered_groups = new IOrderedEnumerable<WordInYear>[4];
            words_groups_detailed = new List<IGrouping<string, Word>>[4];
            for (int i = 0; i < 4; i++)
            {
                words_in_year_detailed_groups[i] = new List<WordInYear>();
                words_groups_detailed[i] = new List<IGrouping<string, Word>>();
            }
            #endregion
            InitPFamily(path_of_year);
            //var ngrams = Enum.GetValues(typeof(NGrams));
            foreach (var item in (NGrams[])Enum.GetValues(typeof(NGrams)))
            {
                CalculateFreqsInYears(item);
            }
        }

        private void CalculateFreqsInYears(NGrams ngram)
        {
            MakeAllDetails(ngram);
            Organize(ngram);
        }

        private void MakeAllDetails(NGrams ngram)
        {
            switch (ngram)
            {
                case NGrams.unigram:
                    words_groups_detailed[(int)ngram] =
                        (from x in words
                         from a in x.table_of_one
                         from b in x.table_of_one
                         where a.word == b.word
                         group new Word { word = b, year = Year } by a.word into WordsG
                         orderby WordsG.Key
                         select WordsG).ToList();
                    break;
                case NGrams.bigram:
                    words_groups_detailed[(int)ngram] =
                        (from x in words
                         from a in x.table_of_two
                         from b in x.table_of_two
                         where a.word == b.word
                         group new Word { word = b, year = Year } by a.word into WordsG
                         orderby WordsG.Key
                         select WordsG).ToList();
                    break;
                case NGrams.trigram:
                    words_groups_detailed[(int)ngram] =
                        (from x in words
                         from a in x.table_of_three
                         from b in x.table_of_three
                         where a.word == b.word
                         group new Word { word = b, year = Year } by a.word into WordsG
                         orderby WordsG.Key
                         select WordsG).ToList();
                    break;
                case NGrams.fourgram:
                    words_groups_detailed[(int)ngram] =
                        (from x in words
                         from a in x.table_of_four
                         from b in x.table_of_four
                         where a.word == b.word
                         group new Word { word = b, year = Year } by a.word into WordsG
                         orderby WordsG.Key
                         select WordsG).ToList();
                    break;
            }
        }

        private void Organize(NGrams ngram)
        {
            int _freq;
            string _word = null;
            int _i;

            foreach (var group in words_groups_detailed[(int)ngram])
            {
                _freq = 0;
                _word = group.Key;
                _i = 0;
                foreach (var w in group)
                {
                    _i++;
                    _freq += w.word.freq;
                }
                words_in_year_detailed_groups[(int)ngram].Add(new WordInYear
                {
                    word = new SeqString{ word = _word, freq = _freq},
                    year = Year,
                    appearInArticles = _i
                });
            }
            words_in_year_detailed_groups[(int)ngram] = words_in_year_detailed_groups[(int)ngram].OrderByDescendingList(x => x.word.freq);
        }

         
        /// P Family
        /// P Family walks on one article.
        /// It take all the x-grams of this article.
        private void InitPFamily(string path)
        {
            string[] files = Directory.GetFiles(path);
            P_family p;
            foreach (var file in files)
            {
				if (file.Contains("tagger_output"))
					continue;
                p = new P_family(file);
                p.RemoveLessThan(ContentFamilies.Program.NUM_OF_MIN_FREQ);
                words.Add(p);
            }
        }

        public static int YearInStr(string str_with_year)
        {
            char[] digs = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int start_pos = str_with_year.IndexOfAny(digs);
			if (start_pos == -1)
				return -1;
            int len = 2;
            if (str_with_year.Length - start_pos >= 4)
                len = 4;
            string str = str_with_year.Substring(start_pos, len);
            return Convert.ToInt32(str);
        }
    }

}
