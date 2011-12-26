<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About Us
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About</h2>
    <p>
        Tauber Matching Web Application is built to facilitate the collection of preference data from the students and the projects. 
        Admin user for the application is responsible for uploading the project, student and matching data (Which project interviewed with which of the students) to the application in the form of a single Excel Worksheet.
        Admin user can send unique access url e-mails to both students and the projects for them to access the application and submit their preferences for the projects and students they interviewed. After all preference data is collected from both parties, admin user can download the collected data in separate Excel wroksheets from the reports section.
    </p>
</asp:Content>
