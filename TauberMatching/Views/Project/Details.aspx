<%@ Import Namespace="TauberMatching.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.ProjectDetailsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Match Students with The Project <%=Model.ProjectName %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <link href='<%=ResolveUrl("~/Content/ProjectDetails.css")%>' rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Model.ProjectName %></h2>
    
    <% using (Html.BeginForm()) {%>
    <input type="hidden" id="hfProjectId" value="<%=Model.ProjectId %>" />
    <div id="divMatching" class="clearfix">
        <select id="selectNotInterviewed" class="notinterviewed" multiple="">
        <%foreach (StudentDto sDto in Model.NotInterviewed)
          {%>
          <option value="<%=sDto.Id %>"><%=sDto.FullName %></option>
        <%} %>
        </select>
        <select id="selectInterviewed" class="interviewed" multiple="">
        <%foreach (StudentDto sDto in Model.Interviewed)
          {%>
          <option value="<%=sDto.Id %>"><%=sDto.FullName %></option>
        <%} %>
        </select>
    </div>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
        <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>
