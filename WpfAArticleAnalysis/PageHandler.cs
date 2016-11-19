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
        /*
         * when adding a new page here you have to :
         *      add a page here (ENUM)
         *      add a page to a private attribute (in the class)
         *      add a property to it (singelton get)
         *      add in to the setPage switch
         */
        firstPage = 0,
        ngramPage,
        NormaliztionPage
    }

    class PageHandler
    {
        //Constructor
        public PageHandler(Frame frame)
        {
            this.frame = frame;
        }

        #region Attributes
        private Frame frame;
        private Page firstPage = null;
        private Page ngramPage = null;
        private Page normalizationPage = null;
        #endregion

        #region Properties
        public Page FirstPage
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

        public Page NgramPage
        {
            get
            {
                if (ngramPage == null)
                    ngramPage = new Pages.Ngrampage();
                return ngramPage;
            }

            set
            {
                ngramPage = value;
            }
        }

        public Page NormalizaionPage
        {
            get
            {
                if (normalizationPage == null)
                    normalizationPage = new Page();
                return normalizationPage;
            }

            set
            {
                normalizationPage = value;
            }
        }
        #endregion

        #region Functions
        //need work
        public void setPage(Pages_ENUM p)
        {
            switch (p)
            {
                case Pages_ENUM.firstPage:
                    frame.Navigate(FirstPage);
                    break;
                case Pages_ENUM.ngramPage:
                    frame.Navigate(NgramPage);
                    break;
                case Pages_ENUM.NormaliztionPage:
                    frame.Navigate(NormalizaionPage);
                    break;
                default:
                    break;
            }
        }

        public Page getCurrentPage()
        {
            return frame.Content as Page;
        }
        #endregion


    }
}
