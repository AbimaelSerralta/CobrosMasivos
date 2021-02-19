<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarAExcelReportePagosEscuela.aspx.cs" Inherits="PagaLaEscuela.Views.Excel.ExportarAExcelReportePagosEscuela" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="gvReportePagosEscuela" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="VchIdentificador" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="COLEGIATURA" />
                <asp:BoundField DataField="VchMatricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="MATRICULA" />
                <asp:BoundField DataField="VchAlumno" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="ALUMNO" />
                <asp:BoundField DataField="VchNum" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# DE PAGOS" />
                <asp:BoundField DataField="DcmImporteCole" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE COLEGIATURA" />
                <asp:BoundField DataField="DcmImporteSaldado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE SALDADO" />
                <asp:BoundField DataField="DcmImportePagado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE PAGADO" />
                <asp:BoundField DataField="DcmImporteNuevo" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="RESTA" />
                
                <asp:BoundField DataField="DtFHPago" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="FECHA PAGO" />
                <asp:BoundField DataField="VchFolio" HeaderText="FOLIO" />
                <asp:BoundField DataField="DcmImportePagado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE PAGADO" />
                <asp:BoundField DataField="VchFormaPago" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="FORMA PAGO" />
                <asp:BoundField DataField="VchBanco" HeaderText="BANCO" />
                <asp:BoundField DataField="VchCuenta" HeaderText="CUENTA" />

                <asp:BoundField DataField="VchEstatus" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="ESTATUS" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
