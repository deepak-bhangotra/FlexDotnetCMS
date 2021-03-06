﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaFieldsEditor.ascx.cs" Inherits="WebApplication.Admin.Controls.Editors.MediaFieldsEditor" %>

<script type="text/javascript">
    $(document).ready(function () {        

        $(document).on("blur", "#<%= FieldLabel.ClientID %>", function () {            
            $("#<%= FieldCode.ClientID %>").val(getfieldCodeFromLabel());
        });

        function getfieldCodeFromLabel() {
            var fieldLabel = $("#<%= FieldLabel.ClientID %>").val();
            var fieldCode = fieldLabel.replace(/\w+/g, function (w) {
                return w[0].toUpperCase() + w.slice(1).toLowerCase();
            });
            fieldCode = fieldCode.replace(/\s/g, '');

            return fieldCode;
        }

        BindReOrder();

        check($("#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>"));

        $(document).on("change", "#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>", function () {
            check($("#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>"));
        });

        OnUpdatePanelRefreshComplete(function (event) {
            check($("#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>"));

            $("#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>").click(function () {
                check(this);
            });

            BindReOrder();
            BindScrollMagic();
            initAceEditors();
            initAccordians();
            initTinyMCE();

            if (event != undefined && event._postBackSettings != undefined && event._postBackSettings.asyncTarget != undefined)
            {
                if (event._postBackSettings.asyncTarget.indexOf("$Update") != -1)
                {                    
                    ReloadPreviewPanel();

                    if (event._postBackSettings.asyncTarget.indexOf("$Delete") != -1)
                    {
                        if (confirm("Successfully Updated, would you like to reload the page?")) {
                            window.location.reload();
                        }
                    }
                    else if(event._postBackSettings.asyncTarget.indexOf("$Update") != -1)
                    {
                        if($("#<%= Update.ClientID%>").text().indexOf("Add") != -1)
                        {
                            window.location.reload();
                        }
                    }
                }

            }
        });

        function check(elem) {
            if ($(elem).is(":checked")) {
                $("#FrontEndLayoutWrapper").hide();
            }
            else {
                $("#FrontEndLayoutWrapper").show();
            }
        }

        BindScrollMagic();

    });

    $(document).ajaxComplete(function () {
        BindReOrder();
        BindScrollMagic();
        initAceEditors();
        initAccordians();        
    });

    function BindReOrder()
    {        
        BindGridViewSortable("#<%=ItemList.ClientID%>", "/Admin/Views/MasterPages/Webservice.asmx/ReOrderMediaFields", "<%= MediaFieldsUpdatePanel.ClientID%>", function () {
            window.location.href = window.location.href;
        });
    }
</script>

<style type="text/css">
    fieldset {
        position: relative;
    }
</style>

<asp:UpdatePanel runat="server" ID="MediaFieldsUpdatePanel" UpdateMode="Conditional">
    <ContentTemplate>
        <fieldset>
            <legend>Currently Created Fields</legend>

			<div class="buttons">
				<asp:LinkButton runat="server" ID="ExportFields" OnClick="ExportFields_Click">Export / Import Fields</asp:LinkButton>
				<div class="clear"></div>
			</div>
			<asp:Panel runat="server" ID="ExportImportFieldsPanel" Visible="false">			
				<fieldset>
					<legend>Fields JSON</legend>					
					<div>
						<asp:TextBox runat="server" TextMode="MultiLine" ID="ExportImportFieldsJson" class="AceEditor NoFullScreen Wrap" data-acemode="json" Height="200" Wrap="true"></asp:TextBox>
					</div>
					<div class="buttons">						
						<asp:LinkButton runat="server" ID="ImportFields" OnClick="ImportFields_Click">Import Fields</asp:LinkButton>
						<div class="clear"></div>
					</div>	
				</fieldset>					
			</asp:Panel>
			<br />

            <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="false" CssClass="DragDropGrid" OnPageIndexChanging="ItemList_PageIndexChanging" PageSize="10" OnDataBound="ItemList_DataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="OrderIndex" HeaderText="OrderIndex" SortExpression="OrderIndex" />
                    <asp:BoundField DataField="FieldCode" HeaderText="FieldCode" SortExpression="FieldCode" />
                    <asp:BoundField DataField="FieldLabel" HeaderText="FieldLabel" SortExpression="FieldLabel" />
                    <asp:BoundField DataField="GroupName" HeaderText="GroupName" SortExpression="GroupName" />
                    <asp:BoundField DataField="MediaTypeFieldID" HeaderText="MediaTypeFieldID" SortExpression="MediaTypeFieldID" />
<%--                    <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                    <asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />--%>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <div style="width:150px">
                                <asp:LinkButton ID="Edit" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Edit_Click">Edit</asp:LinkButton> |
                                <asp:LinkButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Delete_Click" OnClientClick="return confirm('Are you sure you want to perminently delete this field? you will loose all data that has been assigned to this field.')">Delete</asp:LinkButton> |
                                <asp:LinkButton ID="Select" runat="server" OnClientClick='parent.UpdateVisualEditor(this)' data-fieldCode='<%# Eval("FieldCode") %>'>Select</asp:LinkButton>								
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <asp:Panel runat="server">
            <fieldset>
				<div class="accordian closed">
					<h3>Field JSON</h3>
					<div>
						<div>
							<asp:TextBox runat="server" TextMode="MultiLine" ID="LoadJson" class="AceEditor NoFullScreen Wrap" data-acemode="json" Height="200" Wrap="true"></asp:TextBox>
						</div>
						<div class="buttons">
							<asp:LinkButton runat="server" ID="LoadFromJson" OnClick="LoadFromJson_Click">Load From Json</asp:LinkButton>						
							<div class="clear"></div>
						</div>
					</div>
				</div>
                <div style="margin-bottom:50px;">
                    <div id="SaveFields" class="buttons">
                        <asp:LinkButton Text="Save" runat="server" ID="Update" OnClick="Update_Click" CssClass="SaveFieldButton" />
                        <asp:LinkButton Text="Cancel" runat="server" ID="Cancel" OnClick="Cancel_Click" />
                        <div class="clear"></div>
                    </div>                    
                </div>
                <h2>
                    <asp:Literal ID="FieldDetailsTitle" runat="server" /></h2>
                    <asp:HiddenField ID="FieldID" runat="server" Value="0" />
                <div>
                    <label for="<%# FieldLabel.ClientID %>">Field Label:</label>
                    <asp:TextBox runat="server" ID="FieldLabel" />
                </div>
                <div>
                    <asp:DropDownList runat="server" ID="FieldTypeDropDown" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="FieldTypeDropDown_SelectedIndexChanged">
                        <asp:ListItem Text="--Select A Type--" Value="" />
                    </asp:DropDownList>
                </div>
                <div>                    
                    <hr />
                    <strong><em>Advanced Field Settings:</em></strong>
                </div>
                <div>
                    <asp:CheckBox runat="server" ID="IsGlobalField" /> <label for="<%# IsGlobalField.ClientID %>">Is Global Field</label>
                </div>
                <div>
                    <label for="<%# FieldCode.ClientID %>">Field Code:</label>
                    <asp:TextBox runat="server" ID="FieldCode"/>
                </div>                
                <div>
                    <label for="<%# UsageExample.ClientID %>">Usage Example:</label>
                    <asp:TextBox runat="server" ID="UsageExample"/>
                </div>   
                <div id="AssociateWithMediaTypeFieldWrapper" runat="server" visible="false">
                    <asp:CheckBox runat="server" ID="AssociateWithMediaTypeField" /> <label for="<%# AssociateWithMediaTypeField.ClientID %>">Associate With Media Type Field</label>
                </div>
                <div>
                    <asp:CheckBox runat="server" ID="RenderLabelAfterControl" /> <label for="<%# RenderLabelAfterControl.ClientID %>">Render Label After Control</label>
                </div>
                <div>
                    <asp:CheckBox runat="server" ID="ShowFrontEndFieldEditor" Checked="true"/> <label for="<%# ShowFrontEndFieldEditor.ClientID %>">Show Front End Field Editor</label>
                </div>
                <div id="UseMediaTypeFieldDescriptionWrapper" runat="server">
                    <label>
                        <asp:CheckBox ID="UseMediaTypeFieldDescription" runat="server" />
                        <label for="<%# UseMediaTypeFieldDescription.ClientID %>">Use Media Type Field Description</label>
                    </label>
                </div>
                <div>
                    <label for="<%# GroupName.ClientID %>">Group Name:</label>
                <asp:TextBox runat="server" ID="GroupName" />
                </div>
                <div>
                    <label for="<%# FieldDescription.ClientID %>">Field Description:</label>
                    <Admin:Editor ID="FieldDescription" runat="server" Height="200px" />
                </div>
                <div>
                    <label for="<%# AdminControl.ClientID %>">Admin Control:</label>
                <asp:TextBox runat="server" ID="AdminControl" TextMode="MultiLine" class="AceEditor" Height="300" />
                </div>
                <div id="UseMediaTypeFieldFrontEndLayoutWrapper" runat="server" visible="false">
                    <label>
                        <asp:CheckBox ID="UseMediaTypeFieldFrontEndLayout" runat="server" />
                        <label for="<%# UseMediaTypeFieldFrontEndLayout.ClientID %>">Use Media Type Field FrontEnd Layout</label>
                    </label>
                </div>

                <div class="accordian opened">
                    <h3>Front End Layout</h3>
                    <div>
                        <asp:TextBox runat="server" ID="FrontEndLayout" TextMode="MultiLine" class="AceEditor" Height="400"/>
                    </div>
                    <h3>Get Admin Control Value</h3>
                    <div>
                        <asp:TextBox runat="server" ID="GetAdminControlValue" TextMode="MultiLine" class="AceEditor" Height="200"/>
                    </div>
                    <h3>Set Admin Control Value</h3>
                    <div>
                        <asp:TextBox runat="server" ID="SetAdminControlValue" TextMode="MultiLine" class="AceEditor" Height="200"/>
                    </div>
                    <h3>Field Value</h3>
                    <div>
                        <asp:TextBox runat="server" ID="FieldValue" TextMode="MultiLine" />
                    </div>
                </div>

            </fieldset>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="FieldTypeDropDown" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>