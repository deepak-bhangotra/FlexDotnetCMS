﻿using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Views.MediaTypeHandlers
{
    public partial class RootPage : FrontEndBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicContent.Controls.Add(this.ParseControl(MediaDetailsMapper.ParseWithTemplate(CurrentMediaDetail)));
        }

        public new FrameworkLibrary.RootPage CurrentMediaDetail
        {
            get { return (FrameworkLibrary.RootPage)base.CurrentMediaDetail; }
        }
    }
}