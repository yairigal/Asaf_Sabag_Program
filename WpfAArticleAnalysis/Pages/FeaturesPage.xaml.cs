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
    /// Interaction logic for FeaturesPage.xaml
    /// </summary>
    public partial class FeaturesPage : Page
    {
        private FeaturesPage()
        {
            InitializeComponent();
        }

        //singleton
        private static FeaturesPage instance = null;
        public static FeaturesPage getThisPage()
        {
            if (instance == null)
                instance = new FeaturesPage();
            Public_Functions.setPageSize(instance);
            return instance;
        }
        
    }
}
