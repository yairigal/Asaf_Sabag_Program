using System.Windows.Controls;

namespace WpfAArticleAnalysis
{
    class PageArgs
    {
        public Page page;
        public string name;
        public Pages_ENUM Enum;

        public PageArgs(Page p,string name,Pages_ENUM ps)
        {
            page = p;
            this.name = name;
            Enum = ps;
        }
    }
}
