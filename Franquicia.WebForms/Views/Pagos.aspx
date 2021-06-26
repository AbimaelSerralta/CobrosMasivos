<%@ Page Culture="es-MX" Language="C#" AutoEventWireup="true" CodeBehind="Pagos.aspx.cs" Inherits="Franquicia.WebForms.Views.Pagos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SeleccionPago</title>
    <link rel="shortcut icon" href="../Images/logoCobroscontarjetas.png" />
</head>
<body>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="padding: 10px 0 30px 0;">
                <table align="center" border="0" cellpadding="0" cellspacing="0" style="border: 1px solid #cccccc; border-collapse: collapse;">
                    <tr>
                        <td align="center" bgcolor="#00bcd4" style="padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;">
                            <a href="https://www.cobroscontarjeta.com/">
                                <img src="../Images/logo-cobroscontarjetas.png" alt="Cobroscontarjeta" style="display: block;" /></a>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#ffffff" style="padding: 40px 30px 40px 30px;">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="2" style="text-align: center; color: #153643; font-family: Arial, sans-serif; font-size: 24px;">
                                        <b>Hola, 
                                            <asp:Label ID="lblNombreComp" runat="server" /></b>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="2" style="padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: center;">
                                        <h3>
                                            <asp:Label ID="lblNombreComercial" runat="server" />
                                            le ha enviado su liga de pago con los siguientes datos:</h3>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: right;">Concepto:</td>
                                    <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: left;">
                                        <asp:Label ID="lblConcepto" runat="server" /></td>

                                </tr>
                                <tr>
                                    <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: right;">Importe:
                                    </td>

                                    <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: left;">
                                        <asp:Label ID="lblImporte" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: right;">Vencimiento:</td>

                                    <td width="50%" style="color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: left;">
                                        <asp:Label ID="lblVencimiento" runat="server" /></td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <a id="aPagar" style="display: block; color: #fff; font-weight: 400; text-align: center; width: 230px; font-size: 20px; text-decoration: none; background: #28a745; margin: 0 auto; padding: 15px 0" href="#" runat="server"><asp:Label ID="lblPagar" runat="server" /></a>
                            <asp:Panel ID="pnlPromociones" Visible="false" runat="server">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="2" style="padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px; text-align: center;">
                                            <h3>Si lo desea puede pagar con las siguientes promociones:</h3>
                                        </td>
                                    </tr>
                                    <asp:Literal ID="ltlPromociones" runat="server" />
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#df5f16" style="padding: 30px 30px 30px 30px;">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;" width="75%">Cobroscontarjeta &reg; Todos los derechos reservados, 2020<br />
                                    </td>
                                    <td align="right" width="25%"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
