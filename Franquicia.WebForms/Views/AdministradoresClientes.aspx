﻿<%@ Page Title="AdministradoresClientes" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="AdministradoresClientes.aspx.cs" Inherits="Franquicia.WebForms.Views.AdministradoresClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
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
                                <div class="card-header card-header-tabs card-header-primary" style="background: #024693; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group">

                                                <asp:Label Text="Listado de Administradores" runat="server" />

                                                <div class="pull-right">
                                                    <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                            <i class="material-icons">add</i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvAdministradores" OnRowCommand="gvAdministradores_RowCommand" OnRowDataBound="gvAdministradores_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidUsuario" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay administradores registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE COMPLETO" />
                                                    <asp:BoundField SortExpression="VchUsuario" DataField="VchUsuario" HeaderText="USUARIO" />
                                                    <asp:BoundField SortExpression="VchNombreComercial" DataField="VchNombreComercial" HeaderText="COMERCIO" />
                                                    <%--<asp:BoundField SortExpression="VchNombrePerfil" DataField="VchNombrePerfil" HeaderText="PERFIL" />--%>
                                                    <asp:TemplateField SortExpression="UidEstatus" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchDescripcion")%>' runat="server">
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
                                                                    </tr>
                                                                </tbody>
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
    <!--MODAL-->
    <div class="modal fade" id="ModalNuevo" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" runat="server" /></h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
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
                            <div class="card-header card-header-primary" style="background: #024693;">
                                <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul class="nav nav-tabs" data-tabs="tabs">
                                            <li class="nav-item">
                                                <a class="nav-link active show" href="#franquicia" data-toggle="tab">
                                                    <i class="material-icons">business</i>Comercio<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" href="#general" data-toggle="tab">
                                                    <i class="material-icons">business</i>General<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" href="#direccion" data-toggle="tab">
                                                    <i class="material-icons">directions</i>Dirección<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
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
                                                <div class="d-flex flex-row-reverse">
                                                    <div class="p-2">
                                                        <asp:LinkButton ID="btnBuscar" OnClick="btnBuscar_Click" BorderWidth="1" CssClass="btn btn-info btn-sm" runat="server">
                                                            <i class="material-icons">search</i> Buscar
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="p-2">
                                                        <asp:LinkButton ID="btnLimpiar" OnClick="btnLimpiar_Click" BorderWidth="1" CssClass="btn btn-warning btn-sm" runat="server">
                                                            <i class="material-icons">delete</i> Limpiar
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="form-group col-md-4">
                                                        <label for="FiltroRFC" style="color: black;">RFC</label>
                                                        <asp:TextBox ID="FiltroRFC" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="FiltroRazonSocial" style="color: black;">Razón Social</label>
                                                        <asp:TextBox ID="FiltroRazonSocial" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="FiltroNombreComercial" style="color: black;">Nombre Comercial</label>
                                                        <asp:TextBox ID="FiltroNombreComercial" CssClass="form-control" runat="server" />
                                                    </div>
                                                </div>
                                                <asp:Panel ID="panelFranquicias" Visible="false" runat="server">
                                                    <div class="row">
                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvClientes" OnSelectedIndexChanged="gvClientes_SelectedIndexChanged" OnRowDataBound="gvClientes_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidCliente" GridLines="None" border="0" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <div class="alert alert-info">No hay clientes registrados</div>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                                    <asp:BoundField SortExpression="VchRFC" DataField="VchRFC" HeaderText="RFC" />
                                                                    <asp:BoundField SortExpression="VchRazonSocial" DataField="VchRazonSocial" HeaderText="RAZÓN SOCIAL" />
                                                                    <asp:BoundField SortExpression="VchNombreComercial" DataField="VchNombreComercial" HeaderText="NOMBRE COMERCIAL" />
                                                                </Columns>
                                                                <SelectedRowStyle BackColor="#dff0d8" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnLimpiar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </div>

                                    <div class="tab-pane" id="general">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-4">
                                                        <label for="txtNombre" style="color: black;">Nombre</label>
                                                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtApePaterno" style="color: black;">Apellido Paterno</label>
                                                        <asp:TextBox ID="txtApePaterno" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtApeMaterno" style="color: black;">Apellido Materno</label>
                                                        <asp:TextBox ID="txtApeMaterno" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-8">
                                                        <label for="txtCorreo" style="color: black; margin-bottom: 12px;">Correo Eléctronico</label>
                                                        <asp:TextBox ID="txtCorreo" CssClass="form-control" required="required" runat="server" />
                                                        <asp:Label CssClass="text-danger" runat="server" ID="lblExiste" />
                                                        <asp:Label CssClass="text-success" runat="server" ID="lblNoExiste" />
                                                        <asp:LinkButton ID="btnValidarCorreo" CssClass="pull-right" Text="Validar" OnClick="btnValidarCorreo_Click" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlEstatus" style="color: black;">Estatus</label>
                                                        <asp:DropDownList ID="ddlEstatus" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtUsuario" style="color: black;">Usuario</label>
                                                        <asp:TextBox ID="txtUsuario" CssClass="form-control" runat="server" />
                                                        <asp:Label CssClass="text-danger" runat="server" ID="lblExisteUsuario" />
                                                        <asp:Label CssClass="text-success" runat="server" ID="lblNoExisteUsuario" />
                                                        <asp:LinkButton ID="btnValidarUsuario" CssClass="pull-right" Text="Validar" OnClick="btnValidarUsuario_Click" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtPassword" style="color: black;">Contraseña</label>
                                                        <asp:TextBox ID="txtPassword" autocomplete="new-password" TextMode="Password" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtRepetirPassword" style="color: black;">Repetir Contraseña</label>
                                                        <asp:TextBox ID="txtRepetirPassword" TextMode="Password" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4" visible="false" runat="server">
                                                        <label for="ddlPerfil" style="color: black;">Perfil</label>
                                                        <asp:DropDownList ID="ddlPerfil" CssClass="form-control" runat="server">
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
                                                    <div class="form-group col-md-4 d-lg-none">
                                                        <label for="txtIdentificador" style="color: black;">Identificador</label>
                                                        <asp:TextBox ID="txtIdentificador" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlPais" style="color: black;">Pais</label>
                                                        <asp:DropDownList ID="ddlPais" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="Seleccione" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlEstado" style="color: black;">Estado</label>
                                                        <asp:DropDownList ID="ddlEstado" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlMunicipio" style="color: black;">Municipio</label>
                                                        <asp:DropDownList ID="ddlMunicipio" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlCiudad" style="color: black;">Ciudad</label>
                                                        <asp:DropDownList ID="ddlCiudad" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlColonia" style="color: black;">Colonia</label>
                                                        <asp:DropDownList ID="ddlColonia" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlColonia_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCalle" style="color: black;">Calle</label>
                                                        <asp:TextBox ID="txtCalle" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtEntreCalle" style="color: black;">Entre Calle</label>
                                                        <asp:TextBox ID="txtEntreCalle" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtYCalle" style="color: black;">Y Calle</label>
                                                        <asp:TextBox ID="txtYCalle" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtNumeroExterior" style="color: black;">Número Exterior</label>
                                                        <asp:TextBox ID="txtNumeroExterior" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtNumeroInterior" style="color: black;">Número Interior</label>
                                                        <asp:TextBox ID="txtNumeroInterior" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCodigoPostal" style="color: black;">Código Postal</label>
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

                                    <div class="tab-pane" id="telefono">
                                        <div class="container-fluid">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="form-group col-md-6">
                                                            <label for="ddlTipoTelefono" style="color: black;">Tipo Teléfono</label>
                                                            <asp:DropDownList ID="ddlTipoTelefono" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="txtNumero" style="color: black;">Numero</label>
                                                                <asp:TextBox ID="txtNumero" TextMode="Phone" CssClass="form-control" runat="server" />
                                                                <asp:RegularExpressionValidator ID="REVNumero" runat="server" ControlToValidate="txtNumero" ErrorMessage="* Valores númericos" ForeColor="Red" ValidationExpression="^[0-9]*"></asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
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
    <!--END MODAL-->

    <script>
        function showModal() {
            $('#ModalNuevo').modal('show');
        }

        function hideModal() {
            $('#ModalNuevo').modal('hide');
        }
    </script>
</asp:Content>
