<%@ Import Namespace="TauberMatching.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TauberMatching.Models.ProjectDetailsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tauber Matching Web Application - Match Students with The Project <%=Model.ProjectName %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptOrCssContent" runat="server">
    <link href='<%=ResolveUrl("~/Content/ProjectDetails.css")%>' rel="stylesheet" type="text/css" />
    <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/ProjectDetails.js")%>'> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Model.ProjectName %></h2>
    <div id="divMessage" <% if(TempData["error"]!=null) {%>class="errorMessage"<%} %>>
        <% =ViewContext.TempData["message"] %>
    </div>
    <% using (Html.BeginForm()) {%>
    <input type="hidden" id="hfProjectId" value="<%=Model.ProjectId %>" />
    <div id="divMatching" class="clearfix">
        <ul id="ulNotInterviewed" class="matching">
            <li>
                <label id="lblNotInterviewed" for="selectNotInterviewed" class="matching">Not Interviewed</label>
            </li>
            <li>
                <select id="selectNotInterviewed" name="selectNotInterviewed" class="notinterviewed" multiple="">
                <%foreach (StudentDto sDto in Model.NotInterviewed)
                  {%>
                  <option value="<%=sDto.Id %>"><%=sDto.FullName %></option>
                <%} %>
                </select>
            </li>
        </ul>   
        <ul id="ulTransferButtons" class="matching">
            <li>
                <input id="btnMoveFromNotInterviewed" type="button" value="--&gt;" title="Move selected student from not interviewed to interviewed."></input>
            </li>
            <li>
                <input id="btnMoveFromInterviewed" type="button" value="&lt;--" title="Move selected student from interviewed to not interviewed."></input>
            </li>
            <li>
                <input id="btnClearInterviewed" type="button" value="&lt;&lt;" title="Move all from interviewed to not interviewed."></input>
            </li>
        </ul>
        <ul id="ulInterviewed" class="matching">
            <li>
                <label id="lblInterviewed" for="selectInterviewed" class="matching">Interviewed</label>
            </li>
            <li>
                <select id="selectInterviewed" name="selectInterviewed" class="interviewed" multiple="">
                <%foreach (StudentDto sDto in Model.Interviewed)
                  {%>
                  <option value="<%=sDto.Id %>"><%=sDto.FullName %></option>
                <%} %>
                </select>
            </li>
        </ul>
    </div>
    <p>
        <input id="btnSubmit" type="submit" value="Save" />
    </p>
    <% } %>
        <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>
