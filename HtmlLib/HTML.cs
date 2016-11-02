using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlLib
{
    public class HTML
    {
		string tag;
		string content;
		List<HTML> inner;

		public string Tag
		{
			get
			{
				return tag;
			}
		}
		public string Content
		{
			get
			{
				return content;
			}
		}
		public List<HTML> Inner
		{
			get
			{
				return inner;
			}
		}

		//	-----	Member Functions	-----	//
		public HTML(string tag, string content)
		{
			this.tag = tag;
			this.content = content;
			this.inner = null;
		}

		public HTML(string tag, HTML inner)
		{
			this.tag = tag;
			this.content = "";
			this.inner = new List<HTML>();
			this.inner.Add(inner);
		}

		public override string ToString()
		{
			if (HasContent(this))
				return "<" + tag + ">" + content + "</" + tag + ">\n";
			string str = "<" + tag + ">\n\t";
			foreach (var item in inner)
			{
				str += item.ToString();
			}
			str += "</" + tag + ">";
			return str;
		}

		public void AddInner(HTML inner_node)
		{
			if (HasContent(this))
				throw new Exception("Cannot set inner, because it has content.");
			if (this.inner == null)
				this.inner = new List<HTML>();
			this.inner.Add(inner_node);
		}

		public void SetContent(string content)
		{
			if(HasInner(this))
				throw new Exception("Cannot set content, because it has inner node.");
			this.content = content;
		}

		//	-----	Static Functions	-----	//

		public static bool HasContent(HTML node)
		{
			return (node.content != "");
		}

		public static bool HasInner(HTML node)
		{
			return (node.inner != null);
		}

		public static HTML CreateTabel()
		{
			return new HTML("table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"height: 281px; margin-left: auto; margin-right: auto; text-align: center; width: 800px;\"", "");
		}

		public static HTML AddRow(HTML table_root)
		{
			HTML node = new HTML("tr", "");
			table_root.AddInner(node);
			return table_root;
		}

		public static HTML AddColomn(HTML row_root)
		{
			HTML node = new HTML("td", "");
			row_root.AddInner(node);
			return row_root;
		}

		public static HTML GetRowWithNColomons(int num_of_colomons)
		{
			HTML row = new HTML("tr", "");
			for (int i = 0; i < num_of_colomons; i++)
			{
				AddColomn(row);
			}
			return row;
		}

		public static HTML AddColomonsWithNRows(HTML base_table)
		{
			int rows = base_table.inner.Count;
			for (int i = 0; i < rows; i++)
			{
				AddColomn(base_table.inner[i]);
			}
			return base_table;
		}
    }
}
