<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportarExcelAlumnos.aspx.cs" Inherits="PagaLaEscuela.Views.Excel.ExportarExcelAlumnos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="gvAlumnos" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="VchIdentificador" HeaderText="IDENTIFICADOR" />
                <asp:BoundField DataField="VchMatricula" HeaderText="MATRICULA" />
                <asp:BoundField DataField="VchNombres" HeaderText="NOMBRE(S)" />
                <asp:BoundField DataField="VchApePaterno" HeaderText="APEPATERNO" />
                <asp:BoundField DataField="VchApeMaterno" HeaderText="APEMATERNO" />
                <asp:BoundField DataField="VchCorreo" HeaderText="CORREO" />
                <asp:BoundField DataField="VchTelefono" HeaderText="CELULAR" />
                <asp:BoundField DataField="BitBeca" HeaderText="BECA" />
                <asp:BoundField DataField="VchTipoBeca" HeaderText="TIPO BECA" />
                <asp:BoundField DataField="DcmBeca" HeaderText="CANTIDAD" />
                <asp:BoundField DataField="VchDescripcion" HeaderText="ESTATUS" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
