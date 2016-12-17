using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAArticleAnalysis
{
    class Public_Functions
    {
        public static readonly int FrameHeight = 590;
        public static readonly int FrameWidth = 500;

        public static void setPageSize(Page page)
        {
            page.Height = FrameHeight;
            page.Width = FrameWidth;
        }
    }
}
