﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.MasterPages.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Literal ID="SectionTitle" runat="server"></asp:Literal></h1>
    <div>
        <label for="<%= Name.ClientID %>">Name</label>
        <asp:TextBox ID="Name" runat="server"></asp:TextBox>
    </div>
    <!--<div>
        <label for="<%= PathToFile.ClientID %>">Path To Template</label>
        <Admin:FileSelector ID="PathToFile" runat="server" DirPath="~/Views/MasterPages/" />
    </div>-->
    <div>
        <asp:CheckBox ID="UseLayout" runat="server" />
        <label for="<%= UseLayout.ClientID %>">Use Layout</label>
    </div>
    <div>
        <label for="<%= Layout.ClientID %>">Layout</label>
        <asp:TextBox runat="server" ID="Layout" CssClass="AceEditor" TextMode="MultiLine" Height="500px"></asp:TextBox>
    </div>
    <!--<div>
        <label for="<%= MobileTemplate.ClientID %>">Path To Mobile Template</label>
        <Admin:FileSelector ID="MobileTemplate" runat="server" DirPath="~/Views/MasterPages/" />
    </div>-->
    <div class="buttons">
        <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick"  CssClass="SavePageButton">Save</asp:LinkButton>
    </div>
</asp:Content>