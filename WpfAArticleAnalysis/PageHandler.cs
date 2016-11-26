using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfAArticleAnalysis.Pages;

namespace WpfAArticleAnalysis
{
    public enum Pages_ENUM
    {
        /*
         * when adding a new page here you have to :
         *      add a page here (ENUM)
         *      add a singleton to it in his class
         *      add it to the list on the Constructor (PageArgs)
         */
        firstPage = 0,
        ngramPage,
        NormaliztionPage,
        FeaturesPage
    }

    class PageHandler
    {
        #region Attributes
        private Frame frame;
        private static List<PageArgs> Pages = new List<PageArgs>
        {
            new PageArgs(FeaturesPage.getThisPage(),"featuresPage",Pages_ENUM.FeaturesPage),
            new PageArgs(FirstPage.getThisPage(),"firstPage",Pages_ENUM.firstPage),
            new PageArgs(Ngrampage.getThisPage(),"ngramPage",Pages_ENUM.ngramPage),
            new PageArgs(NormalizationPage.getThisPage(),"normaliztionPage",Pages_ENUM.NormaliztionPage)
        };
        private List<Pages_ENUM> pageOrder;
        private Pages_ENUM first, last;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="frame"></param>
        public PageHandler(Frame frame)
        {
            this.frame = frame;
            pageOrder = Enum.GetValues(typeof(Pages_ENUM)).Cast<Pages_ENUM>().ToList();
            first = pageOrder.First();
            last = pageOrder.Last();
            setPage(first);
        }

        #region Functions
        /// <summary>
        /// sets the page on the frame.
        /// </summary>
        /// <param name="p"></param>
        public void setPage(Pages_ENUM p)
        {
            frame.Navigate(PageConvertor.convertToPage(p));
        }
        /// <summary>
        /// returns the current page on the frame
        /// </summary>
        /// <returns></returns>
        public Page getCurrentPage()
        {
            return frame.Content as Page;
        }
        /// <summary>
        /// Sets the next page on the Frame
        /// </summary>
        /// <returns> returns the next page that has set, - else returns null</returns>
        public Page NextPage()
        {
            //if its the last Page -> finish
            if (getCurrentPage() == PageConvertor.convertToPage(last))
                return null;
            //if its not the last page -> move to the next page
            int currentPageIndex = pageOrder.IndexOf(PageConvertor.convert(getCurrentPage()));
            var nextPage = pageOrder[currentPageIndex + 1];
            setPage(nextPage);
            return PageConvertor.convertToPage(nextPage);
        }
        /// <summary>
        /// sets the pre page on the fram
        /// </summary>
        /// <returns>The prev page , is the first page is current returns null</returns>
        public Page PreviousPage()
        {
            //if its the firstPage -> returns null
            if (getCurrentPage() == PageConvertor.convertToPage(first))
                return null;
            //else set the prev page and return it
            int currentPageIndex = pageOrder.IndexOf(PageConvertor.convert(getCurrentPage()));
            var prevPage = pageOrder[currentPageIndex - 1];
            setPage(prevPage);
            return PageConvertor.convertToPage(prevPage);

        }
        /// <summary>
        /// returns the Pages List.
        /// </summary>
        /// <returns></returns>
        public static List<PageArgs> getList()
        {
            return Pages;
        }
        #endregion


    }
}
