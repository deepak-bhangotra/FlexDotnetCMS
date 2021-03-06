﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkLibrary;

namespace WebApplication.Admin.Views.PageHandlers.FieldEditor
{
    public partial class Default : AdminBasePage
    {
        public MediaDetailField Field { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            var fieldIdStr = Request["fieldId"];

            if (!string.IsNullOrEmpty(fieldIdStr))
            {
                if(!CurrentUser.HasPermission(PermissionsEnum.AccessAdvanceOptions))
                {
                    LayoutsTab.Visible = false;
                    FrontEndLayout.Visible = false;
                }

                var fieldId = long.Parse(fieldIdStr);
                var field = FieldsMapper.GetByID(fieldId);

                if (field != null && field is MediaDetailField)
                {
                    Field = field as MediaDetailField;
                    LoadField();                    
                }
            }
        }

        public void LoadField()
        {
            if (Field == null)
                return;

            var adminControlCode = ParserHelper.ParseData(Field.AdminControl, new RazorFieldParams { Field = Field, MediaDetail = Field.MediaDetail });
            var dynamicField = this.ParseControl(adminControlCode);
            Control control = null;

            if (dynamicField.Controls.Count != 0)
            {
                control = dynamicField.Controls[0];
            }

            if (control != null)
            {
                DynamicField.Controls.Add(control);
            }

            var fieldValue = Field.FieldValue.Replace("{BaseUrl}", URIHelper.BaseUrl);

            if (control is WebApplication.Admin.Controls.Fields.IFieldControl)
            {
                var ctrl = ((WebApplication.Admin.Controls.Fields.IFieldControl)control);

                ctrl.FieldID = Field.ID;
                ctrl.SetValue(fieldValue);
            }
            else if(control != null)
            {
                if (Field.FieldValue != "{" + Field.SetAdminControlValue + "}")
                {
                    if (Field.SetAdminControlValue.Contains("@"))
                    {
                        try
                        {
                            var returnData = ParserHelper.ParseData(Field.SetAdminControlValue, new { Control = control, Field = Field, NewValue = fieldValue });
                        }
                        catch (Exception ex)
                        {
                            DisplayErrorMessage("Error", ErrorHelper.CreateError(ex));
                        }
                    }
                    else
                    {
                        ParserHelper.SetValue(control, Field.SetAdminControlValue, fieldValue);
                    }
                }
            }

            var frontEndLayout = Field.FrontEndLayout;

            if (Field.MediaTypeField != null && Field.UseMediaTypeFieldFrontEndLayout)
                frontEndLayout = Field.MediaTypeField.FrontEndLayout;

            FrontEndLayout.Text = frontEndLayout;
        }

        public void SaveField()
        {
            if (DynamicField.Controls.Count == 0)
                return;

            var control = DynamicField.Controls[0];
            
            if (control is WebApplication.Admin.Controls.Fields.IFieldControl)
            {
                var ctrl = ((WebApplication.Admin.Controls.Fields.IFieldControl)control);
                var value = ctrl.GetValue();

                if(value is string)
                {
                    var str = value.ToString();
                    str = MediaDetailsMapper.ConvertUrlsToShortCodes(str);
                    Field.FieldValue = str;
                }

                ctrl.SetValue(value);
            }
            else
            {
                try
                {
                    Field.FieldValue = ParserHelper.ParseData("{"+Field.GetAdminControlValue+"}", control);
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage("Error", ErrorHelper.CreateError(ex));
                }
            }

            if (Field.MediaTypeField != null && Field.UseMediaTypeFieldFrontEndLayout)
                Field.MediaTypeField.FrontEndLayout = FrontEndLayout.Text;
            else
                Field.FrontEndLayout = FrontEndLayout.Text;

            var returnObj = FieldsMapper.Update(Field);

            if(returnObj.IsError)
            {
                DisplayErrorMessage("Error", returnObj.Error);
            }
            else
            {
                Field.MediaDetail.RemoveFromCache();
                DisplaySuccessMessage("Successfully saved");
            }

        }

        protected void Submit_Click(object sender, EventArgs e)
        {            
            SaveField();
        }
    }
}