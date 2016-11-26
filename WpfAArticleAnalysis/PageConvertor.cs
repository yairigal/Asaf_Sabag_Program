using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAArticleAnalysis
{
    /// <summary>
    /// Converts from a Page to the Enum and vice versa
    /// </summary>
    class PageConvertor
    {
        private static List<PageArgs> pageList = PageHandler.getList();
        /// <summary>
        /// Converts from Page to its Enum
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Pages_ENUM convert(Page page)
        {
            foreach (var item in pageList)
            {
                if (item.page == page)
                    return item.Enum;
            }
            throw new Exception("Cannot convert from page to enum");
        }
        /// <summary>
        /// Converts from an Enum to its Page
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        public static Page convertToPage(Pages_ENUM pe)
        {
            foreach (var item in pageList)
            {
                if (item.Enum == pe)
                    return item.page;
            }
            throw new Exception("Cannot convert from enum to page");
        }
    }
}
