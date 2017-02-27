using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Normalization
{
    public class removeHandler : removeObject
    {
        public int numberOfChars;
    }
    public class ReplicateHandler
    {
        private static string englishWordsFilePath = @"";
        public static string removeReplicates(string tweet)
        {
            var list = normalizer.Parser(tweet);
            foreach (var segment in list)
            {
                if (segment.type == type.word)
                    removeDuplicatesIfNeeded(segment);
            }
            return normalizer.listToString(list);
        }
        //to work on
        //alg:
        //if its eng word -> do nothing
        //if after 1 reduc its english word -> recuc that 1
        //if after 2 reduc its english word -> reduc 2
        private static void removeDuplicatesIfNeeded(PartOfSentenece segment)
        {
            if (isEnglishWord(segment.value))
                return;
            var list = getDuplicates(segment);
            foreach (var item in list)
            {
                string afterRemove = removeDuplicates(segment, item);
                if (isEnglishWord(afterRemove))
                {
                    segment.value = afterRemove;
                    return;
                }
            }

        }

        private static string removeDuplicates(PartOfSentenece seg,removeHandler removeOp,int keep=1)
        {
            if (removeOp.numberOfChars < keep)
                throw new Exception("keep paramterer error");
            return seg.value.Remove(removeOp.startingPoint, removeOp.numberOfChars-keep);
        }

        private static List<removeHandler> getDuplicates(PartOfSentenece segment)
        {
            int count = 1;
            int startingPoint = 0;
            List<removeHandler> removeList = new List<removeHandler>();
            for (int i = 0; i < segment.value.Length; i++)
            {
                char current = segment.value[i];
                //if its not the last char
                if (segment.value.Last() != current)
                {
                    //same char is also next
                    if (segment.value[i + 1] == current)
                    {
                        count++;
                    }
                    //not the same char next
                    else
                    {
                        if (count >= 2)
                        {
                            //save how much duplicates we have from now.
                            removeHandler toRemove = new removeHandler();
                            toRemove.numberOfChars = count;
                            toRemove.startingPoint = startingPoint;
                            removeList.Add(toRemove);
                        }
                        //resetting the counters.
                        count = 1;
                        startingPoint = i + 1;
                    }
                }
            }
            return removeList;
        }

        private static bool isEnglishWord(string word)
        {
            StreamReader wrt = new StreamReader(englishWordsFilePath);
            while(!wrt.EndOfStream)
            {
                if (word == wrt.ReadLine())
                {
                    wrt.Close();
                    return true;
                }
            }
            wrt.Close();
            return false;
        }
        /// <summary>
        /// reduces the letters to the keep occurences.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="keep"></param>
        /// <returns></returns>
        public static string recudeLetter(string s,int keep)
        {
            try
            {
                string regex = @"(.)\1+";
                Regex r1 = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                var c = r1.Matches(s);
                string x = s;
                string currReg = "";
                foreach (var item in c)
                {
                    string toReplaceWith = item.ToString().Substring(0, keep);
                    currReg = "(" + item.ToString()[0] + ")" + "\\1+";
                    Regex r2 = new Regex(currReg, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    x = r2.Replace(x, toReplaceWith);
                }

                return x;
            }
            catch (Exception)
            {
                return "argument too big";
            }

        }

        public static void main(string s)
        {
            var split = s.Split(' ');
            List<String> sl = new List<string>();
            foreach (var item in split)
            {
                sl.Add(recudeLetter(item, 1));
                
            }
            foreach (var item in sl)
            {
                Console.WriteLine(item);
            }
        }

        public static void Main(string[] args)
        {
            Regex r1 = new Regex(@"(.)\1", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Regex r2 = new Regex(@"(.)\1\1", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Regex r3 = new Regex(@"(.){2,}", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            //string w1 = "Blood";
            ////Console.WriteLine("This Regex :{0}", r1.ToString());
            //var c = r1.Match(w1);
            //var x = r1.Replace(w1, c.ToString()[0].ToString());
            ////Console.WriteLine("Replace  {0} to  {1}",w1,x);
            //var a = r3.Matches(w1);
            //foreach (var item in a)
            //{
            //    Console.WriteLine(item.ToString());
            //}
            //string w2 = "OMGGGG";
            //Console.WriteLine("This Regex :{0}", r2.ToString());
            //var x2 = r2.Replace(w2, String.Empty);
            //Console.WriteLine("Replace  {0} to {1}", w2, x2);
            //string a = recudeLetter("aaabbbccc",2);
            main("Hello MYYY Nammmeeee is     YYYAIIIRRR LOLLLLLLL!!!");

        }
    }
}
