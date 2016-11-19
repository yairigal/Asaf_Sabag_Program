using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAArticleAnalysis.Pages
{
    /// <summary>
    /// Interaction logic for Ngrampage.xaml
    /// </summary>
    public partial class Ngrampage : Page
    {
        public Ngrampage()
        {
            InitializeComponent();
            initTextboxes();
        }

        #region Attributes
        private TextBox uniGrams;
        private TextBox biGrams;
        private TextBox triGrams;
        private TextBox quadGrams;
        private TextBox rareUniGrams;
        private TextBox rareBiGrams;
        private TextBox rareTriGrams;
        private TextBox rareQuadGrams;

        private TextBox uniChars;
        private TextBox biChars;
        private TextBox triChars;
        private TextBox quadChars;
        private TextBox rareUniChars;
        private TextBox rareBiChars;
        private TextBox rareTriChars;
        private TextBox rareQuadChars;
        #endregion

        #region Properties
        public TextBox getUniGrams
        {
            get
            {
                return uniGrams;
            }
        }
        public TextBox getBiGrams
        {
            get
            {
                return biGrams;
            }


        }
        public TextBox getTriGrams
        {
            get
            {
                return triGrams;
            }


        }
        public TextBox getQuadGrams
        {
            get
            {
                return quadGrams;
            }


        }
        public TextBox getRareUniGrams
        {
            get
            {
                return rareUniGrams;
            }


        }
        public TextBox getRareBiGrams
        {
            get
            {
                return rareBiGrams;
            }


        }
        public TextBox getRareTriGrams
        {
            get
            {
                return rareTriGrams;
            }


        }
        public TextBox getRareQuadGrams
        {
            get
            {
                return rareQuadGrams;
            }


        }
        public TextBox getUniChars
        {
            get
            {
                return uniChars;
            }


        }
        public TextBox getBiChars
        {
            get
            {
                return biChars;
            }


        }
        public TextBox getTriChars
        {
            get
            {
                return triChars;
            }


        }
        public TextBox getQuadChars
        {
            get
            {
                return quadChars;
            }


        }
        public TextBox getRareUniChars
        {
            get
            {
                return rareUniChars;
            }


        }
        public TextBox getRareBiChars
        {
            get
            {
                return rareBiChars;
            }


        }
        public TextBox getRareTriChars
        {
            get
            {
                return rareTriChars;
            }


        }
        public TextBox getRareQuadChars
        {
            get
            {
                return rareQuadChars;
            }

        }
        #endregion

        #region Functions

        private void initTextboxes()
        {
            uniGrams = UniGRams;
            biGrams = BiGRams;
            triGrams = TriGRams;
            quadGrams = QuadGrams;

            uniChars = UniChars;
            biChars = BiChars;
            triChars = TriChars;
            quadChars = QuadChars;

            rareUniGrams = RareUGRAMs;
            rareBiGrams = RareBGRAMS;
            rareTriGrams = RareTriGrams;
            rareQuadGrams = RareQuadGrams;
            
            rareUniChars = RareUniChars;
            rareBiChars = RareBiChars;
            rareTriChars = RareTriChars;
            rareQuadChars = RareQuadChars;
        }

        #endregion

    }
}
