﻿using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class LayoutsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public void UpdateFieldsFromObject()
        {
            MainLayout.Text = selectedItem.MainLayout;
            SummaryLayout.Text = selectedItem.SummaryLayout;
            UseMediaTypeLayouts.Checked = selectedItem.UseMediaTypeLayouts;
        }

        public void UpdateObjectFromFields()
        {
            selectedItem.MainLayout = MainLayout.Text;
            selectedItem.SummaryLayout = SummaryLayout.Text;
            selectedItem.UseMediaTypeLayouts = UseMediaTypeLayouts.Checked;
        }
    }
}