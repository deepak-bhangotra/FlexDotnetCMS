﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.MediaTypes.Detail" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Literal ID="SectionTitle" runat="server"></asp:Literal></h1>

    <div id="tabs" class="tabs">
        <ul>
            <li><a href="#Main">Main</a></li>
            <li><a href="#Layouts">Layouts</a></li>
            <li><a href="#Fields">Fields</a></li>
        </ul>
        <div id="Main">
            <fieldset>
                <div>
                    <label for="<%= Name.ClientID %>">
                        Name</label>
                    <asp:TextBox ID="Name" runat="server"></asp:TextBox>
                </div>
                <div>
                    <label for="<%= Label.ClientID %>">
                        Label</label>
                    <asp:TextBox ID="Label" runat="server"></asp:TextBox>
                </div>
                <div>
                    <label for="<%= MediaTypeHandler.ClientID %>">
                        Media Type Handler</label>
                    <Admin:FileSelector ID="MediaTypeHandler" runat="server" DirPath="~/Views/MediaTypeHandlers" />
                </div>
                <div>
                    <label for="<%= MasterPageSelector.ClientID %>">
                        Template</label>
                    <Site:MasterPageSelector ID="MasterPageSelector" runat="server" />
                </div>
                <div>
                    <label for="<%= IsActive.ClientID %>">
                        <asp:CheckBox ID="IsActive" runat="server"></asp:CheckBox>
                        Is Active</label>
                </div>
                <div>
                    <label for="<%= ShowInMenu.ClientID %>">
                        <asp:CheckBox ID="ShowInMenu" runat="server"></asp:CheckBox>
                        Show In Menu</label>
                </div>
                <div>
                    <label for="<%= ShowInSearchResults.ClientID %>">
                        <asp:CheckBox ID="ShowInSearchResults" runat="server"></asp:CheckBox>
                        Show In Search Results</label>
                </div>
                <div>
                    <label for="<%= ShowInSiteTree.ClientID %>">
                        <asp:CheckBox ID="ShowInSiteTree" runat="server"></asp:CheckBox>
                        Show In SiteTree</label>
                </div>
                <div>
                    <label for="<%= EnableCaching.ClientID %>">
                        <asp:CheckBox ID="EnableCaching" runat="server"></asp:CheckBox>
                        Enable Caching</label>
                </div>
                <div>
                    <label for="<%= CommentsAreModerated.ClientID %>">
                        <asp:CheckBox ID="CommentsAreModerated" runat="server"></asp:CheckBox>
                        Comments Are Moderated</label>
                </div>
                <div>
                    <fieldset>
                        <label for="<%= MultiRoleSelector.ClientID %>">
                            Limit roles that can access this item:
                        </label>
                        <Admin:MultiRoleSelector ID="MultiRoleSelector" runat="server" />
                    </fieldset>
                </div>
                <div>
                    <fieldset>
                        <label for="<%= MultiMediaTypeSelector.ClientID %>">
                            Allowed Child Media Types:
                        </label>
                        <Admin:MultiMediaTypeSelector ID="MultiMediaTypeSelector" runat="server" />

                    </fieldset>
                </div>
            </fieldset>
        </div>
        <div id="Layouts">
            <fieldset>
                <div>
                    <label class="exception" for="<%= UseMediaTypeLayouts.ClientID %>">
                        <asp:CheckBox runat="server" ID="UseMediaTypeLayouts" />
                        Use Media Type Layouts
                    </label>
                </div>

                <div class="accordian opened">
                    <h3>Main Layout</h3>
                    <div>
                        <asp:TextBox runat="server" ID="MainLayout" TextMode="MultiLine" Height="500px" CssClass="AceEditor" />
                    </div>
                    <h3>Summary Layout</h3>
                    <div>
                        <asp:TextBox runat="server" ID="SummaryLayout" TextMode="MultiLine" Height="500px" CssClass="AceEditor" />
                    </div>
                    <h3>Featured Layout</h3>
                    <div>
                        <asp:TextBox runat="server" ID="FeaturedLayout" TextMode="MultiLine" Height="500px" CssClass="AceEditor" />
                    </div>
                    <h3>On Publish Execute Code</h3>
                    <div>
                        <asp:TextBox runat="server" ID="OnPublishExecuteCode" TextMode="MultiLine" Height="500px" CssClass="AceEditor" />
                    </div>
                </div>
            </fieldset>
        </div>

        <div id="Fields">
            <fieldset>
                <Admin:MediaTypeFieldsEditor runat="server" id="MediaTypeFieldsEditor" />
            </fieldset>
        </div>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="buttons">
                <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick" CssClass="SavePageButton">Save</asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>