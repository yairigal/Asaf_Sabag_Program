using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ContentFamilies
{
	public enum NGrams
	{
		unigram,
		bigram,
		trigram,
		fourgram
	}

    public class SeqString : IComparable, IEquatable<SeqString>, IEquatable<string>, IDisposable
    {
        public int freq { get; set; }
        public string word { get; set; }
        
        public SeqString()
        {
            freq = 0;
            word = "";
        }

        public bool Equals(string other)
        {
            return other.Equals(this.word);
        }

        public int CompareTo(object obj)
        {
            return freq.CompareTo(((SeqString)obj).freq);
        }

        public override string ToString()
        {
            return word + "," + freq;
        }
        public bool Equals(SeqString other)
        {
            // Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            if (word.Equals(other.word))
            {
               // freq += other.freq;
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {

            // Get the hash code for the Name field if it is not null. 
            int hashProductName = word == null ? 0 : word.GetHashCode();

            // Get the hash code for the Code field. 
            int hashProductCode = word.GetHashCode();

            // Calculate the hash code for the product. 
            return hashProductName;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.freq = 0;
                this.word = "";

            }
        }
    }

    public class Word
    {
        public SeqString word { get; set; }
        public int year { get; set; }
    }

    public class WordInYear : Word
    {
        public int appearInArticles { get; set; }
    }

	public class Appearance
	{
		public int year { get; set; }
		public int freq { get; set; }
		public int appearInArticles { get; set; }
	}

	public class WordsAppearance
	{
		public string word;
		public int tot_freq { get; set; }
		public List<Appearance> appears_in_years { get; set; }

		public WordsAppearance(string word)
		{
			this.word = word;
			appears_in_years = new List<Appearance>();
			tot_freq = 0;
		}
		public WordsAppearance()
		{
			word = "";
			appears_in_years = new List<Appearance>();
			tot_freq = 0;
		}
	}

    public class WordInInterval
    {
        public string word { get; set; }
        public int min_year { get; set; }
        public int max_year { get; set; }
        public int interval { get; set; }
        public int total_freq { get; set; }
        public int appearInArticles { get; set; }
    }

    public class GroupOfWordInInterval
    {
        public GroupOfWordInInterval(string word, string conference)
        {
            this.key = word;
            this.conference = conference;
            total_interval = 0;
            words = new List<WordInInterval>();
        }
        //props
        public List<WordInInterval> words { get; set; }
        public int total_interval { get; set; }
        public readonly string key;
        public readonly string conference;
    }

}
