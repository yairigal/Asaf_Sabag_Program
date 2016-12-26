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
    /// Interaction logic for NormalizationPage.xaml
    /// </summary>
    public partial class NormalizationPage : Page
    {
        private NormalizationPage()
        {
            InitializeComponent();
        }

        //singleton
        private static NormalizationPage instance = null;
        public static NormalizationPage getThisPage()
        {
            if (instance == null)
                instance = new NormalizationPage();
            Public_Functions.setPageSize(instance);
            return instance;
        }
    }
}
