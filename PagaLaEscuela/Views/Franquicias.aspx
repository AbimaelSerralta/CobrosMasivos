<%@ Page Title="Franquicias" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Franquicias.aspx.cs" Inherits="PagaLaEscuela.Views.Franquicias" %>

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
                                                <asp:Label Text="Listado de Franquicias" runat="server" />

                                                <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                            <i class="material-icons">add</i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvFranquiciatarios" OnSorting="gvFranquiciatarios_Sorting" OnSelectedIndexChanged="gvFranquiciatarios_SelectedIndexChanged" OnRowCommand="gvFranquiciatarios_RowCommand" OnRowDataBound="gvFranquiciatarios_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidFranquiciatarios" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvFranquiciatarios_PageIndexChanging" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay franquicias registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
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
                                                                            <asp:LinkButton ID="LinkButton1" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Editar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="Label1" class="btn btn-sm btn-info btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">edit</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>

                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCancelarCambio" ToolTip="Visualizar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Ver" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="lblCancelarCambio" class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">remove_red_eye</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCredencialesLigas" ToolTip="Credenciales para generar ligas" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnCredencialesLigas" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="Label2" class="btn btn-sm btn-primary btn-fab btn-fab-mini btn-round" runat="server">
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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
        </Triggers>
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
    <div id="ModalNuevo" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" runat="server" /></h5>
                            <button type="button" class="close invisible" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upRegistro">
                        <ProgressTemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="row">
                        <div class="card card-nav-tabs">
                            <div class="card-header card-header-primary" style="background: #b9504c;">
                                <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul class="nav nav-tabs" data-tabs="tabs">
                                            <li class="nav-item">
                                                <a class="nav-link active show" href="#franquicia" data-toggle="tab">
                                                    <i class="material-icons">business</i>Franquicia<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" href="#direccion" data-toggle="tab">
                                                    <i class="material-icons">directions</i>Dirección<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item invisible">
                                                <a class="nav-link" href="#telefono" data-toggle="tab">
                                                    <i class="material-icons">phone</i>Teléfono<div class="ripple-container"></div>
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
                                            <asp:Label CssClass="text-danger" runat="server" ID="lblValidar" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="tab-pane active show" id="franquicia">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-6">
                                                        <label for="txtRFC" style="color: black;">RFC *</label>
                                                        <asp:TextBox ID="txtRFC" CssClass="form-control" required="required" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="txtRazonSocial" style="color: black;">Razón Social *</label>
                                                        <asp:TextBox ID="txtRazonSocial" CssClass="form-control" required="required" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="txtNombreComercial" style="color: black;">Nombre Comercial *</label>
                                                        <asp:TextBox ID="txtNombreComercial" CssClass="form-control" required="required" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="txtFechaAlta" style="color: black;">Fecha Alta *</label>
                                                        <asp:TextBox ID="txtFechaAlta" Enabled="false" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="txtCorreo" style="color: black; margin-bottom: 12px;">Correo Eléctronico *</label>
                                                        <asp:TextBox ID="txtCorreo" CssClass="form-control" required="required" runat="server" />
                                                        <%--<asp:Label CssClass="text-danger" runat="server" ID="lblExiste" />
                                                        <asp:Label CssClass="text-success" runat="server" ID="lblNoExiste" />
                                                        <asp:LinkButton ID="btnValidarCorreo" CssClass="pull-right" Text="Validar" OnClick="btnValidarCorreo_Click" runat="server" />--%>
                                                    </div>
                                                    <div class="form-group col-md-6" style="display: none;">
                                                        <label for="ddlTipoTelefono" style="color: black;">Tipo Teléfono</label>
                                                        <asp:DropDownList ID="ddlTipoTelefono" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label for="txtNumero" style="color: black;">Celular *</label>
                                                            <asp:TextBox ID="txtNumero" CssClass="form-control" required="required" runat="server" />
                                                            <%--<asp:RegularExpressionValidator ID="REVNumero" runat="server" ControlToValidate="txtNumero" ErrorMessage="* Valores númericos" ForeColor="Red" ValidationExpression="^[0-9]*"></asp:RegularExpressionValidator>--%>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="ddlEstatus" style="color: black;">Estatus *</label>
                                                        <asp:DropDownList ID="ddlEstatus" Enabled="false" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                    <div class="tab-pane" id="direccion">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlPais" style="color: black;">Pais *</label>
                                                        <asp:DropDownList ID="ddlPais" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="Seleccione" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlEstado" style="color: black;">Estado *</label>
                                                        <asp:DropDownList ID="ddlEstado" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlMunicipio" style="color: black;">Municipio *</label>
                                                        <asp:DropDownList ID="ddlMunicipio" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlCiudad" style="color: black;">Ciudad *</label>
                                                        <asp:DropDownList ID="ddlCiudad" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlColonia" style="color: black;">Colonia *</label>
                                                        <asp:DropDownList ID="ddlColonia" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlColonia_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCalle" style="color: black;">Calle *</label>
                                                        <asp:TextBox ID="txtCalle" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtEntreCalle" style="color: black;">Entre Calle *</label>
                                                        <asp:TextBox ID="txtEntreCalle" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtYCalle" style="color: black;">Y Calle *</label>
                                                        <asp:TextBox ID="txtYCalle" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtNumeroExterior" style="color: black;">Número Exterior *</label>
                                                        <asp:TextBox ID="txtNumeroExterior" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtNumeroInterior" style="color: black;">Número Interior</label>
                                                        <asp:TextBox ID="txtNumeroInterior" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCodigoPostal" style="color: black;">Código Postal *</label>
                                                        <asp:TextBox ID="txtCodigoPostal" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-12">
                                                        <label for="txtReferencia" style="color: black;">Referencia</label>
                                                        <asp:TextBox ID="txtReferencia" CssClass="form-control" runat="server" />
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="tab-pane invisible" id="telefono">
                                        <div class="container-fluid">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
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
                <asp:UpdatePanel runat="server" ID="upRegistro">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnCerrar" Visible="false" OnClick="btnCerrar_Click" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnGuardar" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnEditar" OnClick="btnEditar_Click" CssClass="btn btn-warning btn-round" runat="server">
                            <i class="material-icons">refresh</i> Editar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCerrar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click" />
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
                                <asp:Label ID="lblTittleCredenciales" Text="Parámetros de Entrada" runat="server" /></h5>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upCredenciales">
                        <ProgressTemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlAlertCredenciales" runat="server">
                                    <div id="divAlertCredenciales" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                        <asp:Label ID="lblMnsjAlertCredenciales" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlCredenciales" runat="server">
                                    <div class="card" style="margin-top: 0px;">
                                        <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px;">
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upCredenciales">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnGuardarCredenciales" OnClick="btnGuardarCredenciales_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancelarCredenciales" class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarCredenciales" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
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
                                <asp:Label ID="Label1" Text="Configuración de promociones" runat="server" /></h5>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upPromociones">
                        <ProgressTemplate>
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlAlertPromociones" runat="server">
                                    <div id="divAlertPromociones" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                        <asp:Label ID="lblMnsjAlertPromociones" runat="server" />
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                </asp:Panel>
                                <div class="card" style="margin-top: 0px;">
                                    <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px;">
                                        <div class="nav-tabs-navigation">
                                            <div class="nav-tabs-wrapper">
                                                <div class="form-group">
                                                    <div class="row">
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
                                                                                                    <div class="col-sm-6">
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
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upPromociones">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnGuardarPromociones" OnClick="btnGuardarPromociones_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                            </asp:LinkButton>
                            <asp:LinkButton class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarPromociones" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
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
        function showModal() {
            $('#ModalNuevo').modal('show');
        }

        function hideModal() {
            $('#ModalNuevo').modal('hide');
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
        }

        function hideModalPromociones() {
            $('#ModalPromociones').modal('hide');
        }
    </script>
</asp:Content>
