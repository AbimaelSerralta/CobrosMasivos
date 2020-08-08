<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarExcelPadres.aspx.cs" Inherits="PagaLaEscuela.Views.Excel.ExportarExcelPadres" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="gvPadres" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="StrNombre" HeaderText="NOMBRE(S)" />
                <asp:BoundField DataField="StrApePaterno" HeaderText="APEPATERNO" />
                <asp:BoundField DataField="StrApeMaterno" HeaderText="APEMATERNO" />
                <asp:BoundField DataField="StrCorreo" HeaderText="CORREO" />
                <asp:BoundField DataField="StrTelefono" HeaderText="CELULAR" />
                <asp:BoundField DataField="VchMatricula" HeaderText="MATRICULA(S)" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
