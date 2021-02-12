<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pruebas.aspx.cs" Inherits="PagaLaEscuela.Views.Pruebas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Hello, world!</title>

    <!-- Required meta tags -->
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />

    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <!--     Fonts and icons     -->
    <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" />

    <!-- Material Dashboard CSS -->
    <link rel="stylesheet" href="../assets/css/material-dashboard.css" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />


















        <div id="ModalPagoDetalleClubPago" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label3" Text="Detalle del pago" runat="server" /></h5>
                            <asp:LinkButton ID="LinkButton2" data-dismiss="modal" aria-label="Close" CssClass="close" runat="server">
                            <i class="material-icons">close</i>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px; padding-left: 15px; padding-right: 15px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tbody>
                                            <tr>
                                                <td bgcolor="#b62322" align="center">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td align="center" valign="top" style="padding-top: 20px;">
                                                                    <asp:LinkButton ID="btnImprimir" OnClick="btnImprimir_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                        <i class="material-icons">print</i>Imprimir
                                                                    </asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#b62322" align="center" style="padding: 0px 10px 0px 10px;">
                                                    <table bgcolor="#ffffff" border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" align="left" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;"></td>
                                                                                <td bgcolor="#ffffff" align="right" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <img height="80" width="250" src="https://pagalaescuela.mx/images/logoCompetoPagaLaEscuela.png" alt="PagaLaEscuela" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaAlumnoClubPago" Text="Alumno: " runat="server" />
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaMatriculaClubPago" Text="Matricula: " runat="server" />
                                                                                    <br />
                                                                                    <br />

                                                                                </td>
                                                                                <td bgcolor="#ffffff" valign="top" style="border-radius: 4px 4px 0px 0px; padding-left: 10px; padding-right: 10px;">
                                                                                    <br />
                                                                                    <asp:Label ID="lblDetaFHpagoClubPago" Text="Fecha de pago: " runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#b62322" align="center" style="padding: 0px 10px 0px 10px;">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td bgcolor="#ffffff" align="left" style="padding: 10px 10px 10px 10px;">
                                                                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                                                        <thead>
                                                                            <tr>
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="center">N°</th>
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="left">CONCEPTO</th>
                                                                                <th style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px;" align="right">PRECIO</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:Repeater ID="rpDetalleClubPago" runat="server">
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff" align="center"><%#Eval("IntNum")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd;" bgcolor="#ffffff"><%#Eval("VchDescripcion")%></td>
                                                                                        <td style="border-bottom: 1px solid #ddd; color: <%#Eval("VchColor")%>;" bgcolor="#ffffff" align="right">$<%#Eval("DcmImporte")%> </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>

                                                                            <tr id="trsubtotalClubPago" runat="server">
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">Subtotal:</td>
                                                                                <td style="font-weight: bold; padding-top: 15px; padding-bottom: 0px;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmSubtotalClubPago" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trvalidarimporteClubPago" runat="server">
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Importe por validar:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmValidarImporteClubPago" Style="color: #f55145;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Importe pagado:</td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmTotalClubPago" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trcomicionClubPago" runat="server">
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="VchComicionClubPago" runat="server" />
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpComisionClubPago" Style="color: #f55145;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Abono pago:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpAbonoClubPago" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px;" bgcolor="#ffffff" align="center"></td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">Resta:
                                                                                </td>
                                                                                <td style="border-color: white; padding-top: 0px; padding-bottom: 0px; font-weight: bold;" bgcolor="#ffffff" align="right">
                                                                                    <asp:Label ID="DcmImpRestaClubPago" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trdetalleoperacionClubPago" runat="server">
                                                <%--Detalle de la operacion--%>
                                                <td bgcolor="#b62322" align="center" style="padding: 30px 10px 0px 10px;">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                        <tbody>
                                                            <tr>
                                                                <td bgcolor="#ffffff" align="left" style="padding: 10px 10px 10px 10px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                                                                        <thead>
                                                                            <tr>
                                                                                <th colspan="3" style="border-collapse: collapse; background-color: #00adee; color: white; padding-top: 4px; padding-bottom: 4px; padding-left: 8px;" align="left">DETALLE DE LA OPERACIÓN
                                                                                </th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td bgcolor="#ffffff" align="left" style="padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 25px;">
                                                                                    
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#b62322" align="center" style="padding: 30px 10px 0px 10px;"></td>
                                                <%--#f4f4f4--%>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>























        <form id="form2" runat="server">
        <%--<asp:UpdatePanel runat="server">
            <ContentTemplate>--%>
        <asp:ScriptManager runat="server" />

        <asp:UpdateProgress runat="server">
            <ProgressTemplate>
                <div class="PogressModal">
                    <div>
                        <img height="150" width="150" src="../CSSPropio/loader.gif" alt="imgCobrosMasivos" />
                        <%--<div class="loader"></div>--%>
                        <br />
                        <asp:Label Text="Espere por favor..." Style="color: white;" runat="server" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <div class="wrapper ">
            <div class="sidebar" data-color="purple" data-background-color="white" data-image="../assets/img/sidebar-1.jpg">
                <div class="logo">
                    <a href="#" class="simple-text logo-normal">
                        <asp:Label ID="lblTitleMenu" runat="server" />
                    </a>
                </div>
                <div class="sidebar-wrapper">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <ul class="nav">
                                <asp:DataList ID="dlMenu" OnItemDataBound="dlMenu_ItemDataBound" Style="width: 100%;" RepeatDirection="Vertical" runat="server">
                                    <ItemTemplate>
                                        <li id="liMenuActivar" class="nav-item" visible='<%# Eval("Lectura") %>' runat="server">
                                            <a id="aActive" class="nav-link" href='<%# Eval("VchUrl") %>' runat="server">
                                                <i class="material-icons"><%#Eval("VchIcono")%></i>
                                                <p><%#Eval("VchNombre")%></p>
                                            </a>
                                        </li>
                                        <asp:Label ID="lblUrl" Visible="false" Text='<%# Eval("VchUrl") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:DataList>
                                <li class="nav-item active-pro ">
                                    <a class="nav" style="cursor: text;">
                                        <asp:Label CssClass="navbar-text pull-right" ID="lblFecha" Style="color: black;" runat="server" />

                                        <asp:Label ID="lblversion" Style="color: black;" runat="server" CssClass="navbar-text pull-right"></asp:Label>
                                    </a>

                                </li>
                            </ul>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="main-panel">
                <!-- Navbar -->
                <nav class="navbar navbar-expand-lg navbar-absolute fixed-top">
                    <div class="container-fluid">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblGvSaldo" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- <asp:UpdateProgress runat="server">
                            <ProgressTemplate>
                                <div class="loader"></div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                        <div class="navbar-wrapper">
                            <%--<div class="navbar-minimize">
                                <asp:LinkButton OnClick="Unnamed_Click" Text="text" runat="server" />
                                <button id="minimizeSidebar" onclick="sideAction()" class="btn btn-just-icon btn-white btn-fab btn-round">
                                    <i class="material-icons text_align-center visible-on-sidebar-regular">more_vert</i>
                                    <i class="material-icons design_bullet-list-67 visible-on-sidebar-mini">view_list</i>
                                </button>
                            </div>
                            <a class="navbar-brand" href="#pablo">Menu</a>--%>
                        </div>
                        <button class="navbar-toggler" type="button" data-toggle="collapse" aria-controls="navigation-index" aria-expanded="false" aria-label="Toggle navigation">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="navbar-toggler-icon icon-bar"></span>
                            <span class="navbar-toggler-icon icon-bar"></span>
                            <span class="navbar-toggler-icon icon-bar"></span>
                        </button>
                        <div class="collapse navbar-collapse justify-content-end">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <ul class="navbar-nav">
                                        <li class="dropdown" id="liMenuFranquicia" runat="server">
                                            <a id="aMenuFranquicia" href="#" class="dropdown-toggle font-weight-bold" data-toggle="dropdown" runat="server">FRANQUICIAS</a>
                                            <ul class="dropdown-menu">
                                                <asp:DataList ID="dlSubMenuFranquicia" Style="width: 100%;" RepeatDirection="Vertical" runat="server">
                                                    <ItemTemplate>
                                                        <li visible='<%# Eval("Lectura") %>' runat="server">
                                                            <a class="nav-link" href='<%# Eval("VchUrl") %>'>
                                                                <i class="material-icons"><%#Eval("VchIcono")%></i>
                                                                <p><%#Eval("VchNombre")%></p>
                                                            </a>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </ul>
                                        </li>
                                        <li>
                                            <asp:Label ID="lblNombreComercial" Style="color: black;" CssClass="navbar-text" runat="server"></asp:Label>
                                        </li>
                                        <li class="dropdown" id="liMenuCliente" runat="server">
                                            <a id="aMenuCliente" href="#" class="dropdown-toggle font-weight-bold" data-toggle="dropdown" runat="server">&nbsp; COMERCIOS </a>
                                            <ul class="dropdown-menu">
                                                <asp:DataList ID="dlSubMenuCliente" Style="width: 100%;" RepeatDirection="Vertical" runat="server">
                                                    <ItemTemplate>
                                                        <li visible='<%# Eval("Lectura") %>' runat="server">
                                                            <a class="nav-link" href='<%# Eval("VchUrl") %>'>
                                                                <i class="material-icons"><%#Eval("VchIcono")%></i>
                                                                <p><%#Eval("VchNombre")%></p>
                                                            </a>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </ul>
                                        </li>
                                        <li>
                                            <asp:Label ID="lblDescripcionCliente" Style="color: black;" CssClass="navbar-text" runat="server"></asp:Label>
                                        </li>

                                        <%--<li class="nav-item">
                                    <a class="nav-link" href="#pablo">
                                        <i class="material-icons">dashboard</i>
                                        <p class="d-lg-none d-md-block">
                                            Stats
                                        </p>
                                    </a>
                                </li>--%>
                                        <%--<li class="nav-item dropdown">
                                    <a class="nav-link" href="http://example.com" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <i class="material-icons">notifications</i>
                                        <span class="notification">5</span>
                                        <p class="d-lg-none d-md-block">
                                            Some Actions
                                        </p>
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdownMenuLink">
                                        <a class="dropdown-item" href="#">Mike John responded to your email</a>
                                        <a class="dropdown-item" href="#">You have 5 new tasks</a>
                                        <a class="dropdown-item" href="#">You're now friend with Andrew</a>
                                        <a class="dropdown-item" href="#">Another Notification</a>
                                        <a class="dropdown-item" href="#">Another One</a>
                                    </div>
                                </li>--%>
                                        <li class="nav-item dropdown">
                                            <a class="nav-link" href="#pablo" id="navbarDropdownProfile" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                <asp:Label ID="LblNombreUsuario" runat="server" />
                                                <i class="material-icons">person</i>
                                                <p class="d-lg-none d-md-block">
                                                    Account
                                                </p>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdownProfile">
                                                <a class="dropdown-item" href="#">Profile</a>
                                                <a class="dropdown-item" href="#">Settings</a>
                                                <div class="dropdown-divider"></div>
                                                <asp:LinkButton ID="btnSalir" CssClass="dropdown-item" OnClick="btnSalir_Click" Text="Salir" runat="server" />
                                            </div>
                                        </li>
                                    </ul>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </nav>
                <!-- End Navbar -->
                <div class="content" style="padding-top: 0px; padding-bottom: 0px;">
                    <asp:ContentPlaceHolder ID="CPHCaja" runat="server">
                    </asp:ContentPlaceHolder>
                </div>
                <footer class="footer">
                    <div class="container-fluid">
                        <nav class="float-left">
                            <ul>
                                <li>
                                    <div class="row">
                                        <asp:Label Style="padding-top: 15px;" Text="Una solución de" runat="server" />
                                        <a style="padding-top: 0px;" href="https://www.cobroscontarjeta.com/" target="_blank">
                                            <asp:Image Height="50" Width="200" src="../Images/logo-cobroscontarjetas.png" runat="server" /></a>
                                        <asp:Label Style="padding-top: 15px;" Text="desarrollado por " runat="server" />
                                        <a href="https://compuandsoft.com/" target="_blank">
                                            <asp:Image Height="30" Width="130" src="../Images/logo-compuandsoft.png" runat="server" /></a>
                                        <asp:Label Style="padding-top: 15px;" Text="en colaboracion con" runat="server" />
                                        <a href="https://www.mitec.com.mx/" target="_blank">
                                            <asp:Image Height="25" Width="90" src="../Images/logo-mit.png" runat="server" /></a>
                                    </div>
                                </li>
                            </ul>
                        </nav>
                        <!-- your footer here -->
                    </div>
                </footer>
            </div>
        </div>

        <asp:ContentPlaceHolder ID="cBodyBottom" runat="server">
        </asp:ContentPlaceHolder>
    </form>

    <script>
        function sideAction() {

            let elm = document.getElementById('myDIV');

            if (elm.className === 'sidebar-mini') {
                elm.className = '';
            } else {
                elm.className = 'sidebar-mini';
            }
        }
    </script>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-8">
                        <div class="card">
                            <div class="card-header card-header-primary">
                                <h4 class="card-title">Edit Profile</h4>
                                <p class="card-category">Complete your profile</p>
                            </div>
                            <div class="card-body">
                                <form>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">Company (disabled)</label>
                                                <input type="text" class="form-control" disabled="">
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">Username</label>
                                                <input type="text" class="form-control">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">Email address</label>
                                                <input type="email" class="form-control">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">Fist Name</label>
                                                <input type="text" class="form-control">
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">Last Name</label>
                                                <input type="text" class="form-control">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">Adress</label>
                                                <input type="text" class="form-control">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">City</label>
                                                <input type="text" class="form-control">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">Country</label>
                                                <input type="text" class="form-control">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group bmd-form-group">
                                                <label class="bmd-label-floating">Postal Code</label>
                                                <input type="text" class="form-control">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label>About Me</label>
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Lamborghini Mercy, Your chick she so thirsty, I'm in that two seat Lambo.</label>
                                                    <textarea class="form-control" rows="5"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <button type="submit" class="btn btn-primary pull-right">Update Profile</button>
                                    <div class="clearfix"></div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card card-profile">
                            <div class="card-avatar">
                                <a href="javascript:;">
                                    <img class="img" src="../assets/img/faces/marc.jpg">
                                </a>
                            </div>
                            <div class="card-body">
                                <h6 class="card-category text-gray">CEO / Co-Founder</h6>
                                <h4 class="card-title">Alec Thompson</h4>
                                <p class="card-description">
                                    Don't be scared of the truth because we need to restart the human foundation in truth And I love you like Kanye loves Kanye I love Rick Owens’ bed design but the back is...
                                </p>
                                <a href="javascript:;" class="btn btn-primary btn-round">Follow</a>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

    <!--   Core JS Files   -->
    <script src="../assets/js/core/jquery.min.js" type="text/javascript"></script>
    <script src="../assets/js/core/popper.min.js" type="text/javascript"></script>
    <script src="../assets/js/core/bootstrap-material-design.min.js" type="text/javascript"></script>

    <!-- Plugin for the Perfect Scrollbar -->
    <script src="../assets/js/plugins/perfect-scrollbar.jquery.min.js"></script>

    <!-- Plugin for the momentJs  -->
    <script src="../assets/js/plugins/moment.min.js"></script>

    <!--  Plugin for Sweet Alert -->
    <script src="../assets/js/plugins/sweetalert2.js"></script>

    <!-- Forms Validations Plugin -->
    <script src="../assets/js/plugins/jquery.validate.min.js"></script>

    <!--  Plugin for the Wizard, full documentation here: https://github.com/VinceG/twitter-bootstrap-wizard -->
    <script src="../assets/js/plugins/jquery.bootstrap-wizard.js"></script>

    <!--	Plugin for Select, full documentation here: http://silviomoreto.github.io/bootstrap-select -->
    <script src="../assets/js/plugins/bootstrap-selectpicker.js"></script>

    <!--  Plugin for the DateTimePicker, full documentation here: https://eonasdan.github.io/bootstrap-datetimepicker/ -->
    <script src="../assets/js/plugins/bootstrap-datetimepicker.min.js"></script>

    <!--  DataTables.net Plugin, full documentation here: https://datatables.net/    -->
    <script src="../assets/js/plugins/jquery.datatables.min.js"></script>

    <!--	Plugin for Tags, full documentation here: https://github.com/bootstrap-tagsinput/bootstrap-tagsinputs  -->
    <script src="../assets/js/plugins/bootstrap-tagsinput.js"></script>

    <!-- Plugin for Fileupload, full documentation here: http://www.jasny.net/bootstrap/javascript/#fileinput -->
    <script src="../assets/js/plugins/jasny-bootstrap.min.js"></script>

    <!--  Full Calendar Plugin, full documentation here: https://github.com/fullcalendar/fullcalendar    -->
    <script src="../assets/js/plugins/fullcalendar.min.js"></script>

    <!-- Vector Map plugin, full documentation here: http://jvectormap.com/documentation/ -->
    <script src="../assets/js/plugins/jquery-jvectormap.js"></script>

    <!--  Plugin for the Sliders, full documentation here: http://refreshless.com/nouislider/ -->
    <script src="../assets/js/plugins/nouislider.min.js"></script>

    <!-- Include a polyfill for ES6 Promises (optional) for IE11, UC Browser and Android browser support SweetAlert -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/core-js/2.4.1/core.js"></script>

    <!-- Library for adding dinamically elements -->
    <script src="../assets/js/plugins/arrive.min.js"></script>

    <!--  Google Maps Plugin    -->
    <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_HERE"></script>

    <!-- Chartist JS -->
    <script src="../assets/js/plugins/chartist.min.js"></script>

    <!--  Notifications Plugin    -->
    <script src="../assets/js/plugins/bootstrap-notify.js"></script>

    <!-- Control Center for Material Dashboard: parallax effects, scripts for the example pages etc -->
    <script src="../assets/js/material-dashboard.min.js?v=2.1.2" type="text/javascript"></script>

</body>
</html>
