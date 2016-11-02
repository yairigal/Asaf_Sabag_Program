using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace PatternFamilies
{
    public enum TagTypes
    {
        CC = 0,
        CD,
        DT,
        EX,
        FW,
        IN,
        JJ,
        JJR,
        JJS,
        LS,
        MD,
        NN,
        NNS,
        NNP,
        NNPS,
        PDT,
        POS,
        PRP,
        PRP_,
        RB,
        RBR,
        RBS,
        RP,
        SYM,
        TO,
        UH,
        VB,
        VBD,
        VBG,
        VBN,
        VBP,
        VBZ,
        WDT,
        WP,
        WP_,
        WRB
    }
    public class Tagger
    {
        string address;
        string old_address;
        public const int NUM_OF_TAGS = 36;
        public float[] count_of_tags = new float[NUM_OF_TAGS];
        public float[] tags_normalize_by_sentences = new float[NUM_OF_TAGS];
        public float[] tags_normalize_by_words = new float[NUM_OF_TAGS];
        void calculate()
        {
            #region init
            count_of_tags.Initialize();
            tags_normalize_by_sentences.Initialize();
            tags_normalize_by_words.Initialize();
            #endregion
            #region openning
            StreamReader st;
            try
            {
                st = new StreamReader(address);
            }
            catch
            {
                Console.WriteLine("There is NO tagger output file !");
                return;
            }
            #endregion
            string text = st.ReadToEnd();
            #region counting...
            for (int i = 0; i < NUM_OF_TAGS; i++)
            {
                string type = ((TagTypes)i).ToString();
                type.Replace('_', '$');
                count_of_tags[i] = Regex.Split(text, "_" + type).Count() - 1;
            }
            //
            count_of_tags[(int)TagTypes.JJ] -= (count_of_tags[(int)TagTypes.JJR] + count_of_tags[(int)TagTypes.JJS]);
            count_of_tags[(int)TagTypes.NN] -= (count_of_tags[(int)TagTypes.NNP] + count_of_tags[(int)TagTypes.NNS]);
            count_of_tags[(int)TagTypes.NNP] -= (count_of_tags[(int)TagTypes.NNPS]);
            count_of_tags[(int)TagTypes.PRP] -= (count_of_tags[(int)TagTypes.PRP_]);
            count_of_tags[(int)TagTypes.RB] -= (count_of_tags[(int)TagTypes.RBR] + count_of_tags[(int)TagTypes.RBS]);
            count_of_tags[(int)TagTypes.VB] -= (count_of_tags[(int)TagTypes.VBD] + count_of_tags[(int)TagTypes.VBG] + count_of_tags[(int)TagTypes.VBN] + count_of_tags[(int)TagTypes.VBP] + count_of_tags[(int)TagTypes.VBZ]);
            count_of_tags[(int)TagTypes.WP] -= (count_of_tags[(int)TagTypes.WP_]);
            #endregion
            #region normaling
            quantitativeCharacteristicsFamily quan = new quantitativeCharacteristicsFamily(old_address);
            float sen_count = quan.sentencesCount;
            float words_count = quan.wordsCount;
            for (int i = 0; i < NUM_OF_TAGS; i++)
            {
                tags_normalize_by_sentences[i] = count_of_tags[i] / sen_count;
                tags_normalize_by_words[i] = count_of_tags[i] / words_count;
            }
            #endregion
        }

        public override string ToString()
        {
            string save = "";
            foreach (var item in tags_normalize_by_sentences)
            {
                save += (item + ",");
            }
            foreach (var item in tags_normalize_by_words)
            {
                save += (item + ",");
            }
            return save;
        }

        public Tagger(string address1)
        {
            old_address = address1;
            address = address1.Insert(old_address.Length-4, "_tagger_output");
            calculate();
        }
    }
}
