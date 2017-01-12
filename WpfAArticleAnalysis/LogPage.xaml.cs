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

namespace WpfAArticleAnalysis
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        MainWindow mainWindow;
        public LogPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            MainWindow.LogChanged += addText;
        }

        private string currentTime()
        {
            DateTime cur = DateTime.Now;
            return ("\n"+cur.Hour +":"+ cur.Minute+":"+cur.Second+" - ");
        }

        public void addText(string str)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                logBlock.Text += ("\n" + str);
                textUp();
            }));

        }

        public void ClearText()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                logBlock.Clear();
            }));
        }

        private void textUp()
        {
        }

        public void SetText(string str)
        {
            logBlock.Dispatcher.Invoke((Action)(() =>
            {
                logBlock.Text = str;
            }));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (mainWindow != null)
                mainWindow.killAllRunningThreads();
        }
    }

}
