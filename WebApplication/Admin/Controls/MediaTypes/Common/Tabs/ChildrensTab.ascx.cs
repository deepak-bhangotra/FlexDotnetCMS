﻿using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class ChildrensTab : BaseTab, ITab
    {
        private List<IMediaDetail> listItems { get; set; }

        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
            Bind();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Bind();
        }

        private void Bind(string searchText = "")
        {
            var mediaId = selectedItem.Media.ID;

            if (string.IsNullOrEmpty(searchText))
            {
                listItems = BaseMapper.GetDataModel().MediaDetails.Where(i => i.Media.ParentMediaID == mediaId && i.MediaType.ShowInSiteTree && i.HistoryVersionNumber == 0).OrderByDescending(i => i.DateLastModified).ToList<IMediaDetail>();
            }
            else
            {
                listItems = BaseMapper.GetDataModel().MediaDetails.Where(i => i.Media.ParentMediaID == mediaId && i.MediaType.ShowInSiteTree && i.HistoryVersionNumber == 0 && i.LinkTitle.Contains(searchText)).OrderByDescending(i => i.DateLastModified).ToList<IMediaDetail>();
            }

            /*if(listItems.Count > 0)
            {
                SearchPanel.Visible = true;
            }*/

            ItemList.DataSource = listItems;
            ItemList.DataBind();
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        public void UpdateObjectFromFields()
        {

        }

        public void UpdateFieldsFromObject()
        {

        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (id != "")
            {
                var mediaDetail = MediaDetailsMapper.GetByID(long.Parse(id));

                if (mediaDetail != null)
                {
                    var url = AdminBasePage.GetAdminUrl(mediaDetail.MediaTypeID, mediaDetail.MediaID);

                    Response.Redirect(url);
                }
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (id != "")
            {
                var mediaDetail = MediaDetailsMapper.GetByID(long.Parse(id));

                if (mediaDetail != null)
                {
                    var returnObj = MediaDetailsMapper.DeletePermanently((MediaDetail)mediaDetail);

                    if(returnObj.IsError)
                    {
                        BasePage.DisplayErrorMessage("Error", returnObj.Error);
                    }
                    else
                    {
                        BasePage.DisplaySuccessMessage("Successfully deleted item");
                    }
                }
            }
        }

        protected void ItemList_DataBound(object sender, EventArgs e)
        {
            ItemList.UseAccessibleHeader = true;
            if (ItemList.HeaderRow != null)
            {
                ItemList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void ItemList_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            var sortDirection = ((e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending) ? "ASC" : "DESC");
            listItems = listItems.OrderBy(e.SortExpression + " " + sortDirection).ToList();
            Bind();
        }

        /*protected void SearchItems_Click(object sender, EventArgs e)
        {
            if(selectedItem != null)
            {
                Bind(SearchText.Text.Trim());
            }
        }*/

        protected void ItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            var dataItem = (IMediaDetail)e.Row.DataItem;

            if (dataItem != null)
            {
                var editLink = (HyperLink)e.Row.FindControl("Edit");

                if (editLink != null)
                {
                    editLink.NavigateUrl = AdminBasePage.GetAdminUrl(dataItem.MediaTypeID, dataItem.MediaID);
                }
            }
        }
    }
}