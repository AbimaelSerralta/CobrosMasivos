<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarAExcelReporteLigas.aspx.cs" Inherits="Franquicia.WebForms.Views.ExportarAExcelReporteLigas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="gvReporteLigas" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="VchIdentificador" HeaderText="IDENTIFICADOR" />
                <asp:BoundField DataField="VchUrl" HeaderText="LIGA" />
                <asp:BoundField DataField="VchAsunto" HeaderText="ASUNTO" />
                <asp:BoundField DataField="VchConcepto" HeaderText="CONCEPTO" />
                <asp:BoundField DataField="DcmImporte" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
                <asp:BoundField DataField="DtVencimiento" DataFormatString="{0:d}" HeaderText="VENCIMIENTO" />
                <asp:BoundField DataField="VchEstatus" HeaderText="ESTATUS" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
