﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RenderMedia.ascx.cs" Inherits="WebApplication.Controls.RenderMedia.RenderMedia" %>


<asp:PlaceHolder runat="server" ID="DynamicContent" />

<%--<asp:ListView runat="server" ID="ContentBuckets" OnItemDataBound="ContentBuckets_ItemDataBound">
    <LayoutTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
    </LayoutTemplate>
    <ItemTemplate>
        <asp:Literal ID="ParsedCustomCode" runat="server" />
    </ItemTemplate>
</asp:ListView>--%>