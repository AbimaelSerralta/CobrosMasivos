<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarAExcelReporteColegiaturas.aspx.cs" Inherits="PagaLaEscuela.Views.Excel.ExportarAExcelReporteColegiaturas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="gvPagos" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="VchIdentificador" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="COLEGIATURA" />
                <asp:BoundField DataField="VchMatricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="MATRICULA" />
                <asp:BoundField DataField="NombreCompleto" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="ALUMNO" />
                <asp:BoundField DataField="VchNum" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# DE PAGOS" />

                <asp:BoundField DataField="DcmImporte" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="IMPORTE" />
                <asp:BoundField DataField="ImpPagado" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="ABONADO" />
                <asp:BoundField DataField="ImpTotal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" DataFormatString="{0:C}" HeaderText="SALDO" />
                <asp:BoundField DataField="DtFHInicio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="INICIO" />
                <asp:BoundField DataField="DtFHInicio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="LIMITE" />
                <asp:BoundField DataField="DtFHInicio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataFormatString="{0:dd/MM/yyyy}" HeaderText="VENCIMIENTO" />
                
                <asp:BoundField DataField="VchEstatusFechas" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="ESTATUS" />
                <asp:BoundField DataField="EstatusPago" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="PAGO" />

            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
