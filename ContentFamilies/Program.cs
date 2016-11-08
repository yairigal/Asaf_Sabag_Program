using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ContentFamilies
{
    public class Program
    {
		//	--	Declare Constants Here	--

        public static int NUM_OF_ONE = 500;//100 * 9;//number of words of 1-gram that will display in the CSV file
        public static int NUM_OF_TWO = 100;//50 * 9;
        public static int NUM_OF_THREE = 50;//30 * 9;
        public static int NUM_OF_FOUR = 25;//15 * 9;

        public const double PERC_OF_MIN_FREQ = 0.0001;//percent of total words that lower that i dont count
		public const int NUM_OF_MIN_FREQ = 2;

		public const int RESIZE = 5000;
		public static double THRESHOLD = 0;
		public static int SKIP_COUNT = 50;
        public static string ForOneArticle="N";
        public static bool TakeRareNGrams = false;
        public static int RareUniGrams = 0;
        public static int RareBiGrams = 0;
        public static int RareTriGrams = 0;
        public static int RareQuadGrams = 0;
        public static bool RemoveStopWords = true;

        public static int UniChars = 0;
        public static int BiChars = 0;
        public static int TriChars = 0;
        public static int QuadChars = 0;
        public static int RareUniChars = 0;
        public static int RareBiChars = 0;
        public static int RareTriChars = 0;
        public static int RareQuadChars = 0;
    }

}
