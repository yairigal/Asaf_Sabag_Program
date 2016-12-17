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
    /// Interaction logic for Tagger.xaml
    /// </summary>
    public partial class Tagger : Page
    {
        public Tagger()
        {
            InitializeComponent();
        }

        private static Tagger instance = null;
        public static Tagger getThisPage()
        {
            if (instance == null)
                instance = new Tagger();
            Public_Functions.setPageSize(instance);
            return instance;
        }
    }
}
