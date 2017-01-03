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
    /// Interaction logic for FirstPage.xaml
    /// </summary>
    public partial class FirstPage : Page
    {
        private FirstPage()
        {
            InitializeComponent();
        }

        #region Properties
        public ComboBox AnalysisMethod_code
        {
            get
            {
                return AnalysisMethod;
            }
        }
        public TextBox ArticleDir_code
        {
            get
            {
                return ArticleDir;
            }

        }
        public TextBox Threshold_code
        {
            get
            {
                return Threshold;
            }

        }
        public ComboBox ReducingUniGrams_code
        {
            get
            {
                return ReducingUniGrams;
            }
        }
        public CheckBox MakeLogFiles_code
        {
            get
            {
                return MakeLogFiles;
            }
        }
        public CheckBox DomainsCounter_code
        {
            get
            {
                return DomainsCounter;
            }

        }
        public Label FreqWarning_code
        {
            get
            {
                return FreqWarning;
            }
        }
        #endregion

        //singleton
        private static FirstPage instance = null;
        public static FirstPage getThisPage()
        {
            if (instance == null)
                instance = new FirstPage();
            Public_Functions.setPageSize(instance);
            return instance;
        }
    }
}
