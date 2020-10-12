<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarAExcelMultiple.aspx.cs" Inherits="Franquicia.WebForms.Views.ExportarAExcelMultiple" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="gvLigasMultiples" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="StrNombre" HeaderText="NOMBRE(S)" />
                <asp:BoundField DataField="StrApePaterno" HeaderText="APEPATERNO" />
                <asp:BoundField DataField="StrApeMaterno" HeaderText="APEMATERNO" />
                <asp:BoundField DataField="StrCorreo" HeaderText="CORREO" />
                <asp:BoundField DataField="StrTelefono" HeaderText="CELULAR" />
                <asp:BoundField DataField="StrAsunto" HeaderText="ASUNTO" />
                <asp:BoundField DataField="StrConcepto" HeaderText="CONCEPTO" />
                <asp:BoundField DataField="DcmImporte" HeaderText="IMPORTE" />
                <asp:BoundField DataField="DtVencimiento" HeaderText="VENCIMIENTO" DataFormatString="{0:d}" />
                <asp:BoundField DataField="CBCorreo" HeaderText="EMAIL" />
                <asp:BoundField DataField="CBWhatsApp" HeaderText="WHATS" />
                <asp:BoundField DataField="CBSms" HeaderText="SMS" />
                <asp:BoundField DataField="StrPromociones" HeaderText="PROMOCION(ES)" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
