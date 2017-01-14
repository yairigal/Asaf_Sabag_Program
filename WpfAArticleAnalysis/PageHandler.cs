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
         *      add it to the list on the attributes (PageArgs)
         */
        firstPage = 0,
        ngramPage,
        tagger,
        NormaliztionPage,
        FeaturesPage
    }

    /// <summary>
    /// Handler class that handles the pages on the frame 
    /// manages the order , how to show them,and more.
    /// </summary>
    class PageHandler
    {
        #region Attributes
        private Frame frame;
        private static List<PageArgs> Pages = new List<PageArgs>
        {
            new PageArgs(FeaturesPage.getThisPage(),"Features",Pages_ENUM.FeaturesPage,true),
            new PageArgs(FirstPage.getThisPage(),"Initial Preferences",Pages_ENUM.firstPage,true),
            new PageArgs(Ngrampage.getThisPage(),"N-Grams",Pages_ENUM.ngramPage,true),
            new PageArgs(NormalizationPage.getThisPage(),"Normalization",Pages_ENUM.NormaliztionPage,true),
            new PageArgs(Tagger.getThisPage(),"Tagger",Pages_ENUM.tagger,false)
        };
        private List<Pages_ENUM> pageOrder;
        private Pages_ENUM first, last, current;
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
        /// returns the next page in the order
        /// </summary>
        /// <returns>throws exepction if its the last page</returns>
        private Pages_ENUM nextPage()
        {
            //if its the last Page -> finish
            if (current == last)
                throw new Exception("This is the last page");
            //if its not the last page -> move to the next page
            int currentPageIndex = pageOrder.IndexOf(getCurrentPage().Enum);
            var nextPage = pageOrder[currentPageIndex + 1];
            return nextPage;
        }
        /// <summary>
        /// returns the next page to 'nextTo' page in the order
        /// </summary>
        /// <param name="nextTo">returns the next page to this parameter</param>
        /// <returns></returns>
        private Pages_ENUM nextPage(Pages_ENUM nextTo)
        {
            Pages_ENUM current = nextTo;
            //if its the last Page -> finish
            if (current == last)
                throw new Exception("This is the last page");
            //if its not the last page -> move to the next page
            int currentPageIndex = pageOrder.IndexOf(current);
            var nextPage = pageOrder[currentPageIndex + 1];
            return nextPage;
        }
        /// <summary>
        /// returns the previous page in the order
        /// </summary>
        /// <returns>throws exepction if its the first page</returns>
        private Pages_ENUM prevPage()
        {
            //if its the firstPage -> returns null
            if (current == first)
                throw new Exception("This is the first page");
            //else set the prev page and return it
            int currentPageIndex = pageOrder.IndexOf(current);
            var prevPage = pageOrder[currentPageIndex - 1];
            return prevPage;
        }
        /// <summary>
        /// returns the previous page to 'nextTo' page in the order
        /// </summary>
        /// <param name="nextTo">returns the previous page to this parameter</param>
        /// <returns></returns>
        private Pages_ENUM prevPage(Pages_ENUM nextTo)
        {
            Pages_ENUM current = nextTo;
            //if its the firstPage -> returns null
            if (current == first)
                throw new Exception("This is the first page");
            //else set the prev page and return it
            int currentPageIndex = pageOrder.IndexOf(current);
            var prevPage = pageOrder[currentPageIndex - 1];
            return prevPage;
        }
        /// <summary>
        /// sets the page on the frame.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="next">if failed , set to next or prev</param>
        /// <returns>returns the page that is currently in the frame</returns>
        public PageArgs setPage(Pages_ENUM p, bool next = true)
        {
            try
            {
                PageArgs cp = PageConvertor.convertToPage(p);
                if (cp.show)
                {
                    frame.Navigate(cp.page);
                    current = p;
                    return cp;
                }
                //if failed to set the page , setting the next one.
                else if (next)
                {
                    Pages_ENUM nextP = nextPage(p);
                    PageArgs toNav = PageConvertor.convertToPage(nextP);
                    frame.Navigate(toNav.page);
                    current = nextP;
                    return toNav;
                }
                //if failed to set the page , setting to the previous one.
                else
                {
                    Pages_ENUM nextP = prevPage(p);
                    PageArgs toNav = PageConvertor.convertToPage(nextP);
                    frame.Navigate(toNav.page);
                    current = nextP;
                    return toNav;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
        /// <summary>
        /// returns the current page on the frame
        /// </summary>
        /// <returns></returns>
        public PageArgs getCurrentPage()
        {
            return PageConvertor.convertToPage(current);
        }
        /// <summary>
        /// Sets the next page on the Frame
        /// </summary>
        /// <returns> returns the next page that has set, - else returns null</returns>
        public PageArgs NextPage()
        {
            try
            {
                Pages_ENUM aNextPage = nextPage();
                PageArgs currentP = setPage(aNextPage);
                return currentP;
            }
            catch (Exception)
            {
                return PageConvertor.convertToPage(last);
            }
        }
        /// <summary>
        /// sets the pre page on the fram
        /// </summary>
        /// <returns>The prev page , is the first page is current returns null</returns>
        public PageArgs PreviousPage()
        {
            try
            {
                Pages_ENUM aPrevPage = prevPage();
                PageArgs currP = setPage(aPrevPage, false);
                return currP;
            }
            catch (Exception)
            {
                return PageConvertor.convertToPage(first);
            }


        }
        /// <summary>
        /// returns the Pages List.
        /// </summary>
        /// <returns></returns>
        public static List<PageArgs> getList()
        {
            return Pages;
        }
        /// <summary>
        /// if you want to disable and not show a page
        /// </summary>
        /// <param name="toDisable">the page to disable</param>
        /// <returns></returns>
        public PageArgs disablePage(Pages_ENUM toDisable)
        {
            foreach (var item in Pages)
                if (item.Enum == toDisable)
                {
                    item.show = false;
                    return item;
                }
            return null;
        }
        /// <summary>
        /// if you want to enable and show a page
        /// </summary>
        /// <param name="toEnable">the page to enable</param>
        /// <returns></returns>
        public PageArgs enablePage(Pages_ENUM toEnable)
        {
            foreach (var item in Pages)
                if (item.Enum == toEnable)
                {
                    item.show = true;
                    return item;
                }
            return null;
        }
        #endregion


    }
}
