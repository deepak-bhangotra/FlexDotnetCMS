﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsForm.ascx.cs" Inherits="WebApplication.Controls.CommentsForm" %>


<style>
	.comment-form {
		margin: 5px 0;
	}
	
	.comment-form > div{
		margin-bottom: 10px;
	}

	.comment-form label{
		padding: 0;
		margin: 0;
	}

	.comment-form input, .comment-form textarea, .comment-form .ServerMessageWrapper {
		width: 300px;
	}

</style>

<script>
	$(document).ready(function () {
		$(document).on("click", ".PostComment", function () {			
			$(this).parents(".comment-form").find(".ServerMessageWrapper").text("Please wait ...");
		});
	});
</script>

<asp:UpdatePanel runat="server">
	<ContentTemplate>
		<div class="comment-form">
			<div>
				<label for="Name">Username: </label>
				<br />
				<asp:TextBox ID="Name" runat="server"></asp:TextBox>
			</div>
			<div>
				<label for="Email">Email (optional):</label><br />
				<asp:TextBox ID="Email" runat="server"></asp:TextBox>
			</div>
			<div>
				<label for="Message">Comment:</label><br />
				<asp:TextBox ID="Message" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox>
			</div>
			<div id="ServerMessageWrapper" runat="server">
				<div runat="server" ID="ServerMessage" class="ServerMessageWrapper">
				</div>
			</div>
			<div>
				<asp:LinkButton ID="PostComment" Text="Post Comment" runat="server" OnClick="PostComment_OnClick" class="PostComment"/>
			</div>
		</div>
	</ContentTemplate>
</asp:UpdatePanel>