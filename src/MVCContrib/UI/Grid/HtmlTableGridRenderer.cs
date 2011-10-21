using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Sorting;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Renders a grid as an HTML table.
	/// </summary>
	public class HtmlTableGridRenderer<T> : GridRenderer<T> where T: class 
	{
		private const string DefaultCssClass = "grid";

		public HtmlTableGridRenderer(ViewEngineCollection engines) : base(engines)
		{
			
		}
		public HtmlTableGridRenderer() {}

		protected override void RenderHeaderCellEnd()
		{
			RenderText("</th>");
		}

		protected virtual void RenderEmptyHeaderCellStart()
		{
			RenderText("<th>");
		}

		protected override void RenderHeaderCellStart(GridColumn<T> column) 
		{
			var attributes = new Dictionary<string, object>(column.HeaderAttributes);

			if(IsSortingEnabled && column.Sortable)
			{
				bool isSortedByThisColumn = (GridModel.SortOptions.Column == GenerateSortColumnName(column));

				if (isSortedByThisColumn) 
				{
					string sortClass = GridModel.SortOptions.Direction == SortDirection.Ascending ? "sort_asc" : "sort_desc";

					if(attributes.ContainsKey("class") && attributes["class"] != null)
					{
						sortClass = string.Join(" ", new[] { attributes["class"].ToString(), sortClass });
					}

					attributes["class"] = sortClass;
				}
			}

			string attrs = BuildHtmlAttributes(attributes);

			if (attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<th{0}>", attrs));
		}


		protected override void RenderHeaderText(GridColumn<T> column) 
		{
			if(IsSortingEnabled && column.Sortable)
			{
				string sortColumnName = GenerateSortColumnName(column);

				bool isSortedByThisColumn = GridModel.SortOptions.Column == sortColumnName;

				var sortOptions = new GridSortOptions 
				{
					Column = sortColumnName
				};

				if(isSortedByThisColumn)
				{
					sortOptions.Direction = (GridModel.SortOptions.Direction == SortDirection.Ascending)
						? SortDirection.Descending 
						: SortDirection.Ascending;
				}
				else //default sort order
				{
					sortOptions.Direction = column.InitialDirection ?? GridModel.SortOptions.Direction;
				}

				var routeValues = CreateRouteValuesForSortOptions(sortOptions, GridModel.SortPrefix);

				//Re-add existing querystring
				foreach(var key in Context.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
				{
					if(! routeValues.ContainsKey(key))
					{
						routeValues[key] = Context.RequestContext.HttpContext.Request.QueryString[key];
					}
				}

				var link = HtmlHelper.GenerateLink(Context.RequestContext, RouteTable.Routes, column.DisplayName, null, null, null, routeValues, null);
				RenderText(link);
			}
			else
			{
				base.RenderHeaderText(column);
			}
		}

		private RouteValueDictionary CreateRouteValuesForSortOptions(GridSortOptions sortOptions, string prefix)
		{
			if(string.IsNullOrEmpty(prefix))
			{
				return new RouteValueDictionary(sortOptions);
			}

			//There must be a nice way to do this...
			return new RouteValueDictionary(new Dictionary<string, object>()
			{
				{ prefix + "." + "Column", sortOptions.Column },
				{ prefix + "." + "Direction", sortOptions.Direction }
			});
		}

		protected virtual string GenerateSortColumnName(GridColumn<T> column)
		{
			//Use the explicit sort column name if specified. If not possible, fall back to the property name.
			//If the property name cannot be inferred (ie the expression is not a MemberExpression) then try the display name instead.
			return column.SortColumnName ?? column.Name ?? column.DisplayName;
		}

		protected override void RenderRowStart(GridRowViewData<T> rowData)
		{
			var attributes = GridModel.Sections.Row.Attributes(rowData);

			if(! attributes.ContainsKey("class"))
			{
				attributes["class"] = rowData.IsAlternate ? "gridrow_alternate" : "gridrow";
			}

			string attributeString = BuildHtmlAttributes(attributes);

			if(attributeString.Length > 0)
			{
				attributeString = " " + attributeString;	
			}

			RenderText(string.Format("<tr{0}>", attributeString));
		}

		protected override void RenderRowEnd()
		{
			RenderText("</tr>");
		}

		protected override void RenderEndCell()
		{
			RenderText("</td>");
		}

		protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
		{
			string attrs = BuildHtmlAttributes(column.Attributes(rowData));
			if (attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<td{0}>", attrs));
		}

		protected override void RenderHeadStart()
		{
			string attributes = BuildHtmlAttributes(GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(null, false)));
			if(attributes.Length > 0)
			{
				attributes = " " + attributes;
			}


			RenderText(string.Format("<thead><tr{0}>", attributes));
		}

		protected override void RenderHeadEnd()
		{
			RenderText("</tr></thead>");
		}

		protected override void RenderGridStart()
		{
			if(! GridModel.Attributes.ContainsKey("class"))
			{
				GridModel.Attributes["class"] = DefaultCssClass;
			}

			string attrs = BuildHtmlAttributes(GridModel.Attributes);

			if(attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<table{0}>", attrs));
		}

		protected override void RenderGridEnd(bool isEmpty)
		{
			RenderText("</table>");
		}

		protected override void RenderEmpty()
		{
		    RenderHeadStart();
		    RenderEmptyHeaderCellStart();
		    RenderHeaderCellEnd();
            RenderHeadEnd();            
		    RenderBodyStart();
			RenderText("<tr><td>" + GridModel.EmptyText + "</td></tr>");
            RenderBodyEnd();
		}

		protected override void RenderBodyStart() 
		{
			RenderText("<tbody>");
		}

		protected override void RenderBodyEnd() 
		{
			RenderText("</tbody>");
		} 

		/// <summary>
		/// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
		/// </summary>
		/// <returns></returns>
		protected string BuildHtmlAttributes(IDictionary<string, object> attributes)
		{
			return DictionaryExtensions.ToHtmlAttributes(attributes);
		}
	}
}