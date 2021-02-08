<%@ Page Language="C#" Culture="es-MX" AutoEventWireup="true" CodeBehind="FormatoClubPago.aspx.cs" Inherits="PagaLaEscuela.Views.Reports.FormatoClubPago" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="icon" href="../../Images/logoPagaLaEscuela.png" />
    <title>FormatoClubPago</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <rsweb:ReportViewer ID="rvFormatoClubPago" ProcessingMode="Local" AsyncRendering="true" runat="server"></rsweb:ReportViewer>
    </form>
</body>
</html>
