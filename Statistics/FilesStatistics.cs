using ContentFamilies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Statistics
{
    public class FilesStatistics
    {
        public FilesStatistics(String Path)
        {

            string path = Directory.GetCurrentDirectory();
            StreamWriter writer = new StreamWriter("Statistics.txt");

            GetStatisticsForSubDir(path, writer);
            GetStatisticsForDir(path, writer);
        }

        public void GetStatisticsForSubDir(string path, StreamWriter writer)
        {
            int WordsSum = 0;
            int CharactersSum = 0;
            int NumOfFiles = 0;
            List<double> AllWordsMedian = new List<double>();
            List<double> AllCharsMedian = new List<double>();
            List<double> WordsMedianPerFile = new List<double>();
            List<double> CharsMedianPerFile = new List<double>();
            List<double> AvrgWordsMedianPer = new List<double>();
            List<double> AvrgCharsMedianPer = new List<double>();
            List<double> WordsMedian = new List<double>();
            List<double> CharsMedian = new List<double>();

            bool subDir = false;
            foreach (string dir in Directory.GetDirectories(path))
            {
                subDir = true;
                string[] files = Directory.GetFiles(dir);
                WordsMedian.Clear();
                CharsMedian.Clear();
                int words = 0;
                int chars = 0;
                foreach (var file in files)
                {
                    int wordCount = CountWords(file);
                    words += wordCount;
                    WordsMedian.Add(wordCount);
                    int charCount = CountCharacter(file);
                    chars += charCount;
                    CharsMedian.Add(charCount);
                    WordsMedianPerFile.Add(wordCount);
                    CharsMedianPerFile.Add(charCount);
                }
                NumOfFiles += files.Length;
                WordsSum += words;
                CharactersSum += chars;
                AllWordsMedian.Add(words);
                AllCharsMedian.Add(chars);
                AvrgWordsMedianPer.Add(WordsMedian.Average());
                AvrgCharsMedianPer.Add(CharsMedian.Average());
                writer.WriteLine("Statistics for: " + dir.Substring(dir.LastIndexOf("\\")+1));
                writer.WriteLine("Number of Files: " + files.Length);
                writer.WriteLine("Number of Words: " + words);
                writer.WriteLine("Number of Characters: " + chars);
                writer.WriteLine("Avarage number of Words: " + WordsMedian.Average());
                writer.WriteLine("Avarage number of Chars: " + CharsMedian.Average());
                writer.WriteLine("Median For Number of Words: " + GetMedian(WordsMedian.ToArray()));
                writer.WriteLine("Median For Number of Chars: " + GetMedian(CharsMedian.ToArray()));
                writer.WriteLine("Standard Deviation For Number of Words: " + GetStandartDeviation(WordsMedian.ToArray()));
                writer.WriteLine("Standard Deviation For Number of Chars: " + GetStandartDeviation(CharsMedian.ToArray()));
                writer.WriteLine();
            }
            if (!subDir)
                writer.WriteLine("There were no sub-directories");
            else
            {
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("Statistics for All domains: ");
                writer.WriteLine("Number of Words Of all Domains: " + WordsSum);
                writer.WriteLine("Number of Chars Of all Domains: " + CharactersSum);
                writer.WriteLine("Avarage Words Per Document: " + WordsMedianPerFile.Average());
                writer.WriteLine("Avarage Chars Per Document:  " + CharsMedianPerFile.Average());
                writer.WriteLine("Median For Number of Words: " + GetMedian(WordsMedianPerFile.ToArray()));
                writer.WriteLine("Median For Number of Chars: " + GetMedian(CharsMedianPerFile.ToArray()));
                writer.WriteLine("Median For Number of Avarage Words: " + GetMedian(AvrgWordsMedianPer.ToArray()));
                writer.WriteLine("Median For Number of Avarage Chars: " + GetMedian(AvrgCharsMedianPer.ToArray()));
                writer.WriteLine("Standard Deviation For Number of Words:  " + GetStandartDeviation(WordsMedianPerFile.ToArray()));
                writer.WriteLine("Standard Deviation For Number of Chars:  " + GetStandartDeviation(CharsMedianPerFile.ToArray()));
                writer.WriteLine("Standard Deviation For Number of Avarage Words:  " + GetStandartDeviation(AvrgWordsMedianPer.ToArray()));
                writer.WriteLine("Standard Deviation For Number of Avarage Chars:  " + GetStandartDeviation(AvrgCharsMedianPer.ToArray()));
            }
        }
        public void GetStatisticsForDir(string path, StreamWriter writer)
        {
            //int WordsSum = 0;
            //int CharactersSum = 0;
            int NumOfFiles = 0;
            List<double> AllWordsMedian = new List<double>();
            List<double> AllCharsMedian = new List<double>();
            //List<double> WordsMedianPerFile = new List<double>();
            //List<double> CharsMedianPerFile = new List<double>();
            //List<double> AvrgWordsMedianPer = new List<double>();
            //List<double> AvrgCharsMedianPer = new List<double>();
            List<double> WordsMedian = new List<double>();
            List<double> CharsMedian = new List<double>();

            string[] files = Directory.GetFiles(path);
            WordsMedian.Clear();
            CharsMedian.Clear();
            int words = 0;
            int chars = 0;

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    int wordCount = CountWords(file);
                    words += wordCount;
                    WordsMedian.Add(wordCount);
                    int charCount = CountCharacter(file);
                    chars += charCount;
                    CharsMedian.Add(charCount);

                    //WordsMedianPerFile.Add(wordCount);
                    //CharsMedianPerFile.Add(charCount);
                }

                NumOfFiles += files.Length;
                //WordsSum += words;
                //CharactersSum += chars;
                //AllWordsMedian.Add(words);
                //AllCharsMedian.Add(chars);
                //AvrgWordsMedianPer.Add(WordsMedian.Average());
                //AvrgCharsMedianPer.Add(CharsMedian.Average());

                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("Statistics for files in current Directory: " + path);
                writer.WriteLine("Number of Words Of all files: " + words);
                writer.WriteLine("Number of Chars Of all files: " + chars);
                writer.WriteLine("Avarage Words Per file: " + WordsMedian.Average());
                writer.WriteLine("Avarage Chars Per file:  " + CharsMedian.Average());
                writer.WriteLine("Median For Number of Words: " + GetMedian(WordsMedian.ToArray()));
                writer.WriteLine("Median For Number of Chars: " + GetMedian(CharsMedian.ToArray()));
                //writer.WriteLine("Median For Number of Avarage Words: " + GetMedian(AvrgWordsMedianPer.ToArray()));
                //writer.WriteLine("Median For Number of Avarage Chars: " + GetMedian(AvrgCharsMedianPer.ToArray()));
                writer.WriteLine("Standard Deviation For Number of Words:  " + GetStandartDeviation(WordsMedian.ToArray()));
                writer.WriteLine("Standard Deviation For Number of Chars:  " + GetStandartDeviation(CharsMedian.ToArray()));
                //writer.WriteLine("Standard Deviation For Number of Avarage Words:  " + GetStandartDeviation(AvrgWordsMedianPer.ToArray()));
                //writer.WriteLine("Standard Deviation For Number of Avarage Chars:  " + GetStandartDeviation(AvrgCharsMedianPer.ToArray()));

            }
        }

        public int CountCharacter(string file)
        {
            StreamReader ReadFile = new StreamReader(file);
            string text = ReadFile.ReadToEnd();
            ReadFile.Close();
            return text.ToCharArray().Length;
        }
        public int CountWords(string file)
        {
            StreamReader ReadFile = new StreamReader(file);
            string text = ReadFile.ReadToEnd();
            ReadFile.Close();
            /*
            List<string> words=ReadFile.
            List<SeqString> temp = (from word in words.Cast<string>()
                                    group word by word into g
                                    select new SeqString { word = g.Key, freq = g.Count() }).ToList();*/
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }
        public static double GetMedian(double[] sourceNumbers)
        {
            //Framework 2.0 version of this method. there is an easier way in F4        
            if (sourceNumbers == null || sourceNumbers.Length == 0)
                throw new System.Exception("Median of empty array not defined.");

            //make sure the list is sorted, but use a new array
            double[] sortedPNumbers = (double[])sourceNumbers.Clone();
            Array.Sort(sortedPNumbers);

            //get the median
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? (double)sortedPNumbers[mid] : ((double)sortedPNumbers[mid] + (double)sortedPNumbers[mid - 1]) / 2;
            return median;
        }
        public static double GetStandartDeviation(double[] someDoubles)
        {
            double average = someDoubles.Average();
            double sumOfSquaresOfDifferences = someDoubles.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / someDoubles.Length);
            return sd;
        }
    }
}
