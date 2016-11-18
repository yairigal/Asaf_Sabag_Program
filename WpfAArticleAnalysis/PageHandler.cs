using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAArticleAnalysis
{
    public enum Pages_ENUM
    {
        firstPage = 0,
        ngramPage,
        NormaliztionPage
    }

    class PageHandler
    {
        public PageHandler(Frame frame)
        {
            this.frame = frame;
            pages = new Page[Enum.GetValues(typeof(Pages_ENUM)).Length];
        }

        private Page[] pages;
        private Frame frame;

        private static Page FirstPage
        {
            get
            {
                if (firstPage == null)
                    firstPage = new Page();
                return firstPage;
            }

            set
            {
                firstPage = value;
            }
        }

        private static Page NgramPage
        {
            get
            {
                if (ngramPage == null)
                    ngramPage = new Page();
                return ngramPage;
            }

            set
            {
                ngramPage = value;
            }
        }

        private static Page NormalizaionPage
        {
            get
            {
                if (normalizaionPage == null)
                    normalizaionPage = new Page();
                return normalizaionPage;
            }

            set
            {
                normalizaionPage = value;
            }
        }

        //need work
        public void setPage(Pages_ENUM p,string typeName)
        {
            Type typ = Type.GetType(typeName);
            if (pages[(int)p] == null)
                pages[(int)p] = ;
        }


    }
}
