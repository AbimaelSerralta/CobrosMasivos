<%@ Page Title="ExportarAExcelFranquicia" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="ExportarAExcelFranquicia.aspx.cs" Inherits="Franquicia.WebForms.Views.ExportarAExcelFranquicia" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="gvUsuariosSelecciona" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="StrNombre" HeaderText="NOMBRE(S)" />
                <asp:BoundField DataField="StrApePaterno" HeaderText="APEPATERNO" />
                <asp:BoundField DataField="StrApeMaterno" HeaderText="APEMATERNO" />
                <asp:BoundField DataField="StrCorreo" HeaderText="CORREO" />
                <asp:BoundField DataField="StrTelefono" HeaderText="CELULAR" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
