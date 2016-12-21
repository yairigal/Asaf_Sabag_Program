using System.Windows.Controls;

namespace WpfAArticleAnalysis
{
    /// <summary>
    /// A Help class that connects page instance , enum , and attributes.
    /// </summary>
    class PageArgs
    {
        /// <summary>
        /// The page instance
        /// </summary>
        public Page page;
        /// <summary>
        /// The instance name
        /// </summary>
        public string name;
        /// <summary>
        /// The page corrisponds enum
        /// </summary>
        public Pages_ENUM Enum;
        /// <summary>
        /// a bool type if to show the page or not (enabled/disabled)
        /// </summary>
        public bool show;

        public PageArgs(Page p,string name,Pages_ENUM ps,bool show)
        {
            page = p;
            this.name = name;
            Enum = ps;
            this.show = show;
        }
    }
}
