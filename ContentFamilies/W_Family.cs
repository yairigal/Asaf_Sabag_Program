using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ContentFamilies
{

	public class W_Family // Family of N-Grams per CONFERENCE (including its years)
	{
		//Consts
		private Func<GroupOfWordInInterval, int> OrderOfWords_v1 = (x => x.total_interval * (1 / x.words.Count));
		private Func<WordsAppearance, int> OrderOfWords_v2 = (x => x.tot_freq * x.appears_in_years.Count);

		//Props
		//[XmlIgnore]
		public List<GroupOfWordInInterval>[] groups_of_words { set; get; }
		public List<WordsAppearance>[] appears_of_words { set; get; }


		//Helpers
		private readonly string conf_path; // Like "TXT/acl/". When: "TXT/acl/09/aaa.txt"
		private readonly string conference;
		List<Z_Family> years;
		List<IGrouping<string, WordInYear>>[] words_grouped { get; set; }
		
		//Func
		public W_Family(string path)
		{
			#region init
			this.conf_path = path;
			conference = path.Substring(path.LastIndexOf("\\") + 1);
			years = new List<Z_Family>();
			groups_of_words = new List<GroupOfWordInInterval>[4];
			appears_of_words = new List<WordsAppearance>[4];

			words_grouped = new List<IGrouping<string, WordInYear>>[4];
			for (int i = 0; i < 4; i++)
			{
				words_grouped[i] = new List<IGrouping<string, WordInYear>>();

				groups_of_words[i] = new List<GroupOfWordInInterval>();
				appears_of_words[i] = new List<WordsAppearance>();
			}
			#endregion

			Init_Z(path);
			
			foreach (var item in (NGrams[])Enum.GetValues(typeof(NGrams)))
			{
				CalculateFreqsInConference(item);
			}
			Organize_v2();
			//Organize_v1();
		}

		public W_Family()
		{
			#region init
			this.conf_path = "";
			conference = "";
			appears_of_words = new List<WordsAppearance>[4];
			groups_of_words = new List<GroupOfWordInInterval>[4];
			#endregion
		}

		private void Organize_v2()
		{
			WordsAppearance w_app;

			for (int ngram = 0; ngram < 4; ngram++)
			{
				foreach (var word_key in words_grouped[ngram])
				{
					w_app = new WordsAppearance(word_key.Key);

					foreach (var word in word_key)
					{
						w_app.appears_in_years.Add(new Appearance 
						{ 
							freq = word.word.freq, year = word.year, appearInArticles = word.appearInArticles
						});
						w_app.tot_freq += word.word.freq;
					}

					appears_of_words[ngram].Add(w_app);
				}

				appears_of_words[ngram] = appears_of_words[ngram].OrderByDescendingList(OrderOfWords_v2);
			}
		}

		private void Organize_v1()
		{
			WordInInterval _word;
			GroupOfWordInInterval _single_group;
			int gram = 0;

			foreach (var word_group in words_grouped)
			{
				foreach (var word_key in word_group)
				{
					_single_group = new GroupOfWordInInterval(word_key.Key, conference);
					_word = new WordInInterval();

					_word.word = word_key.Key;
					_word.interval = 0;
					_word.total_freq = 0;
					_word.min_year = word_key.First().year;
					_word.max_year = _word.min_year;
					_word.appearInArticles = 0;

					foreach (var word in word_key)
					{
						if (word.year - _word.max_year <= 1)// The continuous year...
						{
							_word.total_freq += word.word.freq;
							_word.max_year = word.year;
							_word.interval++;
							_word.appearInArticles += word.appearInArticles;
							continue;
						}
						else// Another 'package'...
						{
							_single_group.words.Add(_word);
							_single_group.total_interval += _word.interval;

							_word = new WordInInterval();
							_word.interval = 1;
							_word.total_freq = word.word.freq;
							_word.min_year = word.year;
							_word.max_year = word.year;
							_word.word = word_key.Key;
							_word.appearInArticles = word.appearInArticles;
						}
					}
					_single_group.words.Add(_word);
					_single_group.total_interval += _word.interval;

					groups_of_words[gram].Add(_single_group);
				}
				groups_of_words[gram] = groups_of_words[gram].OrderByDescendingList(OrderOfWords_v1);
				gram++;
			}
		}

		private void Init_Z(string path_of_conf)
		{
			string[] years_dirs = Directory.GetDirectories(path_of_conf);
			Z_Family z;
			foreach (var year in years_dirs)
			{
				z = new Z_Family(year);
				years.Add(z);
			}
		}

		private void CalculateFreqsInConference(NGrams ngram)
		{
			words_grouped[(int)ngram] = (from x in years
										 from a in x.words_in_year_detailed_groups[(int)ngram]
										 from b in x.words_in_year_detailed_groups[(int)ngram]
										 where a.word.word == b.word.word
										 group new WordInYear { word = b.word, year = b.year, appearInArticles = b.appearInArticles } by a.word.word into WordG
										 orderby WordG.Key
										 select WordG).ToList();
			}

	}

	public static class ExtendList
	{
		public static List<T> OrderByDescendingList<T>(this List<T> l, Func<T,int> func)
		{
			return new List<T>(l.OrderByDescending(func));
		}
	}
}