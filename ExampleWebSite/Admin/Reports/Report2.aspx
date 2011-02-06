<%@ Page Title="Report 2" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ExampleWebSite.Admin.Reports.Report2" Codebehind="Report2.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="primaryContent" Runat="Server">
	<h2>Report 2</h2>
	<p>Input for, and generation of, 'Report 2' would be here.</p>
	<hr />
	<iframe src='<%= ResolveUrl("~/TextDocument.ashx?textId=Some document identity") %>' style="width: 500px" />
</asp:Content>