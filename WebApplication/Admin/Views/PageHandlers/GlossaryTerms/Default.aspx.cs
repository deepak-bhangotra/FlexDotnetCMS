﻿using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace WebApplication.Admin.Views.PageHandlers.GlossaryTerms
{
    public partial class Default : AdminBasePage
    {
        private List<GlossaryTerm> Items
        {
            get
            {
                return (List<GlossaryTerm>)ContextHelper.GetFromRequestContext(ItemList.ClientID);
            }
            set
            {
                ContextHelper.SetToRequestContext(ItemList.ClientID, value);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Section.Text = this.Page.Title = "Manage Glossary Terms";

            if (Items == null)
                Items = BaseMapper.GetDataModel().GlossaryTerms.OrderBy(i => i.Term).ToList();

            Bind();
        }

        private void Bind()
        {
            ItemList.DataSource = Items;
            ItemList.DataBind();
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        protected void ItemList_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            var sortDirection = ((e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending) ? "ASC" : "DESC");
            Items = Items.OrderBy(e.SortExpression + " " + sortDirection).ToList();
            Bind();
        }
    }
}