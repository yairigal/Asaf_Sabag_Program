﻿using System;
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
using System.Windows.Shapes;

namespace WpfAArticleAnalysis
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public static string text;
        MainWindow mainWindow;
        public LogWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow.LogChanged += addText;
            this.mainWindow = mainWindow;
        }

        public void addText(string str)
        {
            this.Dispatcher.Invoke((Action)(() =>
    {
        LogBlock.Text += ("\n" + str);
    }));

        }

        public void ClearText()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                LogBlock.Text = "";
            }));
        }

        public void SetText(string str)
        {
            this.Dispatcher.Invoke((Action)(() =>
             {
                 LogBlock.Text = str;
             }));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (mainWindow != null)
                mainWindow.killAllRunningThreads();
        }
    }
}
