﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="WebApplication.Controls.Generic.Pager" %>
<div class="pagerControl pagination" id="PagerControl" runat="server">
    <asp:DataPager ID="dataPager" runat="server" PagedControlID="" PageSize="5" Visible="true">
        <%-- <Fields>
        <asp:numericpagerfield ButtonCount="5" NextPageText="..." PreviousPageText="..." />
        <asp:nextpreviouspagerfield FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Previous" />
    </Fields>--%>
        <Fields>
            <asp:NextPreviousPagerField ButtonCssClass="page-link" ButtonType="Link" FirstPageImageUrl=""
                ShowFirstPageButton="true" PreviousPageImageUrl=""
                ShowLastPageButton="false" ShowNextPageButton="false" />
            <asp:NumericPagerField NumericButtonCssClass="page-link" />
            <asp:NextPreviousPagerField ButtonCssClass="page-link" ButtonType="Link" ShowLastPageButton="true" ShowNextPageButton="true"
                ShowPreviousPageButton="false" LastPageImageUrl=""
                NextPageImageUrl="" />
        </Fields>
    </asp:DataPager>
</div>