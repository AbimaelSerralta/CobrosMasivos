<%@ Page Title="Empresas" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Empresas.aspx.cs" Inherits="PagaLaEscuela.Views.Empresas" %>

<%@ MasterType VirtualPath="~/Views/MasterPage.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
    <style>
        .form-check,
        label {
            font-size: 14px;
            line-height: 1.42857;
            color: #333333;
            font-weight: 400;
            padding-left: 5px;
            padding-right: 20px;
        }
    </style>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlAlert" Visible="false" runat="server">
                <div id="divAlert" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                    <asp:Label ID="lblMensajeAlert" runat="server" />
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="content">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="card">
                                <div class="card-header card-header-tabs card-header-primary" style="background: #b9504c; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de escuelas" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvEmpresas" OnRowCreated="gvEmpresas_RowCreated" OnSorting="gvEmpresas_Sorting" OnPageIndexChanging="gvEmpresas_PageIndexChanging" AllowPaging="true" PageSize="10" OnRowCommand="gvEmpresas_RowCommand" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidCliente" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay clientes registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:TemplateField SortExpression="VchIdCliente" HeaderText="IdEscuela">
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtGvIdCliente" ReadOnly="true" ToolTip='<%#Eval("VchIdCliente")%>' Text='<%#Eval("VchIdCliente")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" /></td>
                                                                    <td>
                                                                        <asp:LinkButton ID="btnCopiar" data-text='<%#Eval("VchIdCliente")%>' CssClass="copyboard" ToolTip="Copiar" Style="margin-left: 5px;" runat="server">
                                                                            <asp:Label ID="lblCopiar" class="btn btn-sm btn-dark btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">content_copy</i>
                                                                            </asp:Label>
                                                                        </asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField SortExpression="VchRFC" DataField="VchRFC" HeaderText="RFC" />
                                                    <asp:BoundField SortExpression="VchRazonSocial" DataField="VchRazonSocial" HeaderText="RAZÓN SOCIAL" />
                                                    <asp:BoundField SortExpression="VchNombreComercial" DataField="VchNombreComercial" HeaderText="NOMBRE COMERCIAL" />
                                                    <asp:TemplateField SortExpression="UidEstatus" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchEstatus")%>' runat="server">
                                                                <i class="large material-icons">
                                                                    <%#Eval("VchIcono")%>
                                                                </i>
                                                                </asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCredencialesLigas" ToolTip="Credenciales para generar ligas" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnCredencialesLigas" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-primary btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">security</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnPromociones" ToolTip="Configurar promociones" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnPromociones" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">settings</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <!--MODAL-->
    <div id="ModalBusqueda" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleLigas" Text="Filtro de busqueda" runat="server" /></h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlFiltrosBusqueda" runat="server">
                                    <div class="card" style="margin-top: 0px;">
                                        <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px; margin-top: 0px;">
                                            <div class="nav-tabs-navigation">
                                                <div class="nav-tabs-wrapper">
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroIdEscuela" style="color: black;">IdEscuela</label>
                                                                <asp:TextBox ID="FiltroIdEscuela" CssClass="form-control" TextMode="Number" aria-label="Search" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars="" TargetControlID="FiltroIdEscuela" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroRfc" style="color: black;">RFC</label>
                                                                <asp:TextBox ID="FiltroRfc" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroRazonSocial" style="color: black;">Razon Social</label>
                                                                <asp:TextBox ID="FiltroRazonSocial" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroNombreComercial" style="color: black;">Nombre Comercial</label>
                                                                <asp:TextBox ID="FiltroNombreComercial" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroEstatus" style="color: black;">Estatus</label>
                                                                <asp:DropDownList ID="FiltroEstatus" AppendDataBoundItems="true" CssClass="form-control" runat="server"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center" style="padding-top: 0px; padding-bottom: 10px;">
                            <asp:LinkButton ID="btnBuscar" OnClick="btnBuscar_Click" CssClass="btn btn-primary btn-round" runat="server">
                            <i class="material-icons">search</i> Buscar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpiar" OnClick="btnLimpiar_Click" CssClass="btn btn-warning btn-round" runat="server">
                            <i class="material-icons">clear_all</i> Limpiar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="ModalCredenciales" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleCredenciales" Text="Parámetros de Entrada" runat="server" />
                            </h5>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="row">
                        <div class="card card-nav-tabs">
                            <div class="card-header card-header-primary" style="background: #b9504c;">
                                <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul id="ulTabPadres" class="nav nav-tabs" data-tabs="tabs">
                                            <li class="nav-item">
                                                <a class="nav-link active show" href="#praga" data-toggle="tab">
                                                    <i class="material-icons">account_tree</i>Praga<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" href="#centrosPago" data-toggle="tab">
                                                    <i class="material-icons">login</i>Centros de pago<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body">
                                <div class="tab-content">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlAlertCredenciales" runat="server">
                                                <div id="divAlertCredenciales" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                    <asp:Label ID="lblMnsjAlertCredenciales" runat="server" />
                                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="tab-pane active show" id="praga">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlCredencialesPraga" runat="server">
                                                    <div class="card" style="margin-top: 0px;">
                                                        <div class="card-header card-header-tabs card-header">
                                                            <div class="nav-tabs-navigation">
                                                                <div class="nav-tabs-wrapper">
                                                                    <div class="form-group">
                                                                        <div class="row">
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtIdBusinesPartner" style="color: black;">ID Business Partner *</label>
                                                                                <asp:TextBox ID="txtIdBusinesPartner" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtClaveUsuarioWSREST" style="color: black;">Clave de Usuario WS REST *</label>
                                                                                <asp:TextBox ID="txtClaveUsuarioWSREST" Text="1614725165055" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtMonedaPraga" style="color: black;">Moneda *</label>
                                                                                <asp:TextBox ID="txtMonedaPraga" Text="MXN" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-6">
                                                                                <label for="txtUrlPraga" style="color: black;">Url *</label>
                                                                                <asp:TextBox ID="txtUrlPraga" Text="https://www.praga.io/praga-ws/url/generateUrlV3" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-6">
                                                                                <label for="txtClaveEncripcionWSREST" style="color: black;">Clave de Encripción WS REST En base 64 *</label>
                                                                                <asp:TextBox ID="txtClaveEncripcionWSREST" Text="EFD881A116694CA63F9D33CD2F5B8FDB" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div visible="false" class="form-group col-md-12" runat="server">
                                                                                <label for="txtAPIKey" style="color: black;">API Key *</label>
                                                                                <asp:TextBox ID="txtAPIKey" Text="NGY0MmE4MzUtZWVjMS00ZTc5LTkwYWYtYWIzYWJhNzEwYjhj" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <div class="modal-footer justify-content-center">
                                                    <asp:LinkButton ID="btnGuardarCredencialesPraga" OnClick="btnGuardarCredencialesPraga_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnCancelarCredencialesPraga" class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>

                                    <div class="tab-pane" id="centrosPago">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlCredenciales" runat="server">
                                                    <div class="card" style="margin-top: 0px;">
                                                        <div class="card-header card-header-tabs card-header">
                                                            <div class="nav-tabs-navigation">
                                                                <div class="nav-tabs-wrapper">
                                                                    <div class="form-group">
                                                                        <div class="row">
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtIdCompany" style="color: black;">Id_company *</label>
                                                                                <asp:TextBox ID="txtIdCompany" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtIdBranch" style="color: black;">Id_branch *</label>
                                                                                <asp:TextBox ID="txtIdBranch" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtMoneda" style="color: black;">Moneda *</label>
                                                                                <asp:TextBox ID="txtMoneda" Text="MXN" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtUsuario" style="color: black;">User *</label>
                                                                                <asp:TextBox ID="txtUsuario" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtPassword" style="color: black;">Password *</label>
                                                                                <asp:TextBox ID="txtPassword" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-4">
                                                                                <label for="txtCanal" style="color: black;">Canal *</label>
                                                                                <asp:TextBox ID="txtCanal" Text="W" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-6">
                                                                                <label for="txtData" style="color: black;">Data0 *</label>
                                                                                <asp:TextBox ID="txtData" Text="9265655113" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-6">
                                                                                <label for="txtUrl" style="color: black;">Url *</label>
                                                                                <asp:TextBox ID="txtUrl" Text="https://bc.mitec.com.mx/p/gen" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                            <div class="form-group col-md-6">
                                                                                <label for="txtSemillaAES" style="color: black;">Semilla AES *</label>
                                                                                <asp:TextBox ID="txtSemillaAES" Text="7AACFE849FABD796F6DCB947FD4D5268" Enabled="false" CssClass="form-control" required="required" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <div class="modal-footer justify-content-center">
                                                    <asp:LinkButton ID="btnGuardarCredenciales" OnClick="btnGuardarCredenciales_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnCancelarCredenciales" class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="ModalPromociones" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label1" Text="Configuración de promociones" runat="server" />
                            </h5>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="row">
                        <div class="card card-nav-tabs">
                            <div class="card-header card-header-primary" style="background: #b9504c;">
                                <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul id="ulTabPromociones" class="nav nav-tabs" data-tabs="tabs">
                                            <li class="nav-item">
                                                <a class="nav-link active show" href="#pragaPromo" data-toggle="tab">
                                                    <i class="material-icons">account_tree</i>Praga<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" href="#centrosPagoPromo" data-toggle="tab">
                                                    <i class="material-icons">login</i>Centros de pago<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body">
                                <div class="tab-content">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlAlertPromociones" runat="server">
                                                <div id="divAlertPromociones" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                    <asp:Label ID="lblMnsjAlertPromociones" runat="server" />
                                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="tab-pane active show" id="pragaPromo">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvTipoTarjetaPraga" OnRowDataBound="gvTipoTarjetaPraga_RowDataBound" Width="100%" ShowHeader="false" GridLines="None" AutoGenerateColumns="false" runat="server">
                                                    <Columns>
                                                        <asp:BoundField DataField="UidTipoTarjeta" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div class="row">
                                                                    <div class="accordionTipoTarjeta" style="margin-top: 15px; margin-bottom: 0px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                                        <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>
                                                                                <div>
                                                                                    <label style="font-size: 1.0625rem; font-weight: bold; color: black;">
                                                                                        <div class="row">
                                                                                            <div class="col-sm-2">
                                                                                                <img src="../Images/TipoTarjeta/<%#Eval("VchImagen")%>" style="background-color: #d8ecfe" height="70" width="70" class="float-left rounded-circle">
                                                                                            </div>
                                                                                            <div class="col-sm-5">
                                                                                                <h5 class="card-title" style="padding-top: 15px;">
                                                                                                    <asp:CheckBox ID="cbComisionPraga" Font-Bold="true" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                                </h5>
                                                                                            </div>
                                                                                            <div class="col-sm-5">
                                                                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtComisionTipoTarjetaPraga" Text="0.00" CssClass="form-control" TextMode="Phone" Font-Size="Large" runat="server" />
                                                                                                        <div class="input-group-prepend">
                                                                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                                                                <i class="material-icons">%</i>
                                                                                                            </span>
                                                                                                        </div>
                                                                                                    </div>

                                                                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionTipoTarjetaPraga" runat="server" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </label>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="cbComisionPraga" EventName="CheckedChanged" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                    <div class="panelTipoTarjeta">
                                                                        <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>
                                                                                <div class="row">
                                                                                    <div class="card" style="margin-top: 0px; margin-bottom: 0px; border-left: 8px solid <%#Eval("VchColor")%>;">
                                                                                        <div class="card-body">
                                                                                            <div class="row">
                                                                                                <div class="table-responsive">
                                                                                                    <asp:GridView ID="gvPromocionesPraga" CssClass="table" GridLines="None" border="0" OnRowDataBound="gvPromocionesPraga_RowDataBound" AutoGenerateColumns="false" DataKeyNames="UidPromocion" runat="server">
                                                                                                        <EmptyDataTemplate>
                                                                                                            <div class="alert alert-info">No hay promociones disponibles</div>
                                                                                                        </EmptyDataTemplate>
                                                                                                        <Columns>
                                                                                                            <asp:BoundField DataField="UidPromocion" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                                                            <asp:TemplateField HeaderStyle-CssClass="d-none">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:UpdatePanel runat="server">
                                                                                                                        <ContentTemplate>
                                                                                                                            <table>
                                                                                                                                <tbody>
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 30%;">
                                                                                                                                            <asp:CheckBox ID="cbPromocionPraga" Checked='<%#Eval("blChecked")%>' OnCheckedChanged="cbPromocionPraga_CheckedChanged" AutoPostBack="true" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                                                                        </td>
                                                                                                                                        <td style="width: 35%;">
                                                                                                                                            <div class="input-group">
                                                                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComisionPraga" runat="server" />
                                                                                                                                                <asp:TextBox ID="txtComisionPraga" Text='<%#Eval("DcmComicion")%>' Font-Size="Large" CssClass="form-control text-right" placeholder="123...100" runat="server" />
                                                                                                                                                <div class="input-group-prepend">
                                                                                                                                                    <span class="input-group-text">
                                                                                                                                                        <i class="material-icons">%</i>
                                                                                                                                                    </span>
                                                                                                                                                </div>
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                        <td style="width: 35%;">
                                                                                                                                            <div class="input-group">
                                                                                                                                                <div class="input-group-prepend">
                                                                                                                                                    <span class="input-group-text">
                                                                                                                                                        <i class="material-icons">$</i>
                                                                                                                                                    </span>
                                                                                                                                                </div>
                                                                                                                                                <asp:TextBox ID="txtApartirDePraga" Text='<%#Eval("DcmApartirDe")%>' ToolTip="A partir del monto ingresado se activará esta promoción." Font-Size="Large" CssClass="form-control text-right" placeholder="A partir de" runat="server" />
                                                                                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtApartirDePraga" runat="server" />
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </tbody>
                                                                                                                            </table>
                                                                                                                        </ContentTemplate>
                                                                                                                        <Triggers>
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="cbPromocionPraga" EventName="CheckedChanged" />
                                                                                                                        </Triggers>
                                                                                                                    </asp:UpdatePanel>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                                <div class="modal-footer justify-content-center">
                                                    <asp:LinkButton ID="btnGuardarPromocionesPraga" OnClick="btnGuardarPromocionesPraga_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>

                                    <div class="tab-pane" id="centrosPagoPromo">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="form-group col-md-12">
                                                    <div class="row">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvPromociones" CssClass="table table-hover" GridLines="None" border="0" OnRowDataBound="gvPromociones_RowDataBound" AutoGenerateColumns="false" DataKeyNames="UidPromocion" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <div class="alert alert-info">No hay promociones disponibles</div>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:BoundField DataField="UidPromocion" ItemStyle-CssClass="d-none" HeaderStyle-CssClass="d-none" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="d-none">
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="vertical-align: bottom;">
                                                                                        <asp:CheckBox ID="cbPromocion" Text='<%#Eval("VchDescripcion")%>' runat="server" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-CssClass="d-none">
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr style="background: transparent;">
                                                                                    <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                        <div class="form-row">
                                                                                            <div class="col-sm-4">
                                                                                                <div class="input-group">
                                                                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtComicion" runat="server" />
                                                                                                    <asp:TextBox ID="txtComicion" Font-Size="Large" class="form-control" placeholder="123...100" required="required" runat="server" />
                                                                                                    <div class="input-group-prepend">
                                                                                                        <span class="input-group-text">
                                                                                                            <i class="material-icons">%</i>
                                                                                                        </span>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col-sm-6">
                                                                                                <div class="input-group">
                                                                                                    <div class="input-group-prepend">
                                                                                                        <span class="input-group-text">
                                                                                                            <i class="material-icons">$</i>
                                                                                                        </span>
                                                                                                    </div>
                                                                                                    <asp:TextBox ID="txtApartirDe" ToolTip="A partir del monto ingresado se activará esta promoción." Font-Size="Large" class="form-control" placeholder="A partir de" required="required" runat="server" />
                                                                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtApartirDe" runat="server" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="modal-footer justify-content-center">
                                                    <asp:LinkButton ID="btnGuardarPromociones" OnClick="btnGuardarPromociones_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                                                    </asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--END MODAL-->

    <script>
        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
        }

        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }
    </script>
    <script>
        function showModalCredenciales() {
            $('#ModalCredenciales').modal('show');
        }

        function hideModalCredenciales() {
            $('#ModalCredenciales').modal('hide');
        }

        function showModalPromociones() {
            $('#ModalPromociones').modal('show');

            var acc = document.getElementsByClassName("accordionTipoTarjeta");
            var i;

            for (i = 0; i < acc.length; i++) {
                acc[i].addEventListener("click", function () {
                    this.classList.toggle("activeAccordionTT");
                    var panel = this.nextElementSibling;
                    if (panel.style.maxHeight) {
                        panel.style.maxHeight = null;
                    } else {
                        panel.style.maxHeight = panel.scrollHeight + "px";
                    }
                });
            }
        }

        function hideModalPromociones() {
            $('#ModalPromociones').modal('hide');
        }
    </script>
</asp:Content>
