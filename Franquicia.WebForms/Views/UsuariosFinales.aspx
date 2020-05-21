<%@ Page Title="UsuariosFinales" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="UsuariosFinales.aspx.cs" Inherits="Franquicia.WebForms.Views.Usuarios" %>

<%@ MasterType VirtualPath="~/Views/MasterPage.Master" %>

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
                                <div class="card-header card-header-tabs card-header-primary" style="padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de clientes" runat="server" />

                                                <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" ToolTip="Agregar clientes." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">add</i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvAdministradores" OnSorting="gvAdministradores_Sorting" OnRowCommand="gvAdministradores_RowCommand" OnRowDataBound="gvAdministradores_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvAdministradores_PageIndexChanging" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay usuarios registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE COMPLETO" />
                                                    <asp:BoundField SortExpression="StrCorreo" DataField="StrCorreo" HeaderText="CORREO" />
                                                    <asp:BoundField SortExpression="VchUsuario" DataField="VchUsuario" HeaderText="USUARIO" />
                                                    <asp:BoundField SortExpression="VchNombrePerfil" DataField="VchNombrePerfil" HeaderText="PERFIL" />
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
                                                <PagerStyle CssClass="pagination-ys" />
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
                            <div class="card-header card-header-primary">
                                <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul class="nav nav-tabs" data-tabs="tabs">
                                            <li class="nav-item">
                                                <a class="nav-link active show" href="#general" data-toggle="tab">
                                                    <i class="material-icons">business</i>General<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" href="#direccion" data-toggle="tab">
                                                    <i class="material-icons">directions</i>Dirección (Opcional)<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <%--<li class="nav-item">
                                                <a class="nav-link" href="#telefono" data-toggle="tab">
                                                    <i class="material-icons">phone</i>Teléfono<div class="ripple-container"></div>
                                                </a>
                                            </li>--%>
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

                                    <div class="tab-pane active show" id="general">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlAsosiarUsuario" Visible="false" runat="server">
                                                    <div class="card card-stats bg-light border-info" style="margin-top: 0px; margin-bottom: 0px;" runat="server">
                                                        <label for="cblPromociones" style="color: black;">Buscar usuario existente</label>
                                                        <div class="btn-group">
                                                            <asp:TextBox ID="FiltroCorreoUsuario" CssClass="form-control" placeholder="Ingrese un correo" Style="margin-right: 5px;" runat="server" />
                                                            <asp:LinkButton ID="btnLimpiarUsuario" OnClick="btnLimpiarUsuario_Click" BorderWidth="1" CssClass="btn btn-warning btn-sm" Style="margin-right: 5px;" runat="server">
                                                            <i class="material-icons">delete</i> Limpiar
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnBuscarUsuario" OnClick="btnBuscarUsuario_Click" BorderWidth="1" CssClass="btn btn-info btn-sm" runat="server">
                                                            <i class="material-icons">search</i> Buscar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div class="row">
                                                    <div class="form-group col-md-4">
                                                        <label for="txtNombre" style="color: black;">Nombre *</label>
                                                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtApePaterno" style="color: black;">Apellido Paterno *</label>
                                                        <asp:TextBox ID="txtApePaterno" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtApeMaterno" style="color: black;">Apellido Materno *</label>
                                                        <asp:TextBox ID="txtApeMaterno" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtUsuario" style="color: black;">Usuario</label>
                                                        <asp:TextBox ID="txtUsuario" autocomplete="nope" CssClass="form-control" runat="server" />
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
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCorreo" style="color: black; margin-bottom: 12px;">Correo Eléctronico *</label>
                                                        <asp:TextBox ID="txtCorreo" CssClass="form-control" required="required" runat="server" />
                                                        <asp:Label CssClass="text-danger" runat="server" ID="lblExiste" />
                                                        <asp:Label CssClass="text-success" runat="server" ID="lblNoExiste" />
                                                        <asp:LinkButton ID="btnValidarCorreo" CssClass="pull-right" Text="Validar" OnClick="btnValidarCorreo_Click" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlPrefijo" style="color: black;">Código pais *</label>
                                                        <asp:DropDownList ID="ddlPrefijo" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label for="txtNumero" style="color: black;">Celular *</label>
                                                            <asp:TextBox ID="txtNumero" TextMode="Phone" CssClass="form-control" runat="server" />
                                                            <asp:RegularExpressionValidator ID="REVNumero" runat="server" ControlToValidate="txtNumero" ErrorMessage="* Valores númericos" ForeColor="Red" ValidationExpression="^[0-9]*"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlEstatus" style="color: black;">Estatus *</label>
                                                        <asp:DropDownList ID="ddlEstatus" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
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
                                                <div class="d-flex flex-row-reverse">
                                                    <div class="p-2">

                                                        <label for="ddlIncluirDir" style="color: black;">¿Incluir dirección?</label>
                                                        <asp:DropDownList ID="ddlIncluirDir" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlIncluirDir_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="NO" Value="NO" />
                                                            <asp:ListItem Text="SI" Value="SI" />
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <asp:Panel ID="pnlIncluirDir" Visible="false" runat="server">
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
                                                </asp:Panel>
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

    <div id="ModalBusqueda" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTittleLigas" Text="Filtro de busqueda" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col-12 pt-3">
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upBusqueda">
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
                                <asp:Panel ID="pnlFiltrosBusqueda" runat="server">
                                    <div class="card" style="margin-top: 0px;">
                                        <div class="card-header card-header-tabs card-header" style="padding-top: 0px; padding-bottom: 0px;">
                                            <div class="nav-tabs-navigation">
                                                <div class="nav-tabs-wrapper">
                                                    <div class="form-group">
                                                        <asp:Label Text="Busqueda" runat="server" />
                                                        <div class="row">
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroNombre" style="color: black;">Nombre</label>
                                                                <asp:TextBox ID="FiltroNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroApePaterno" style="color: black;">ApePaterno</label>
                                                                <asp:TextBox ID="FiltroApePaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroApeMaterno" style="color: black;">ApeMaterno</label>
                                                                <asp:TextBox ID="FiltroApeMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-8">
                                                                <label for="FiltroCorreo" style="color: black;">Correo</label>
                                                                <asp:TextBox ID="FiltroCorreo" CssClass="form-control" aria-label="Search" runat="server" />
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
                <asp:UpdatePanel runat="server" ID="upBusqueda">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
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
    <!--END MODAL-->

    <script>
        function showModal() {
            $('#ModalNuevo').modal('show');
        }

        function hideModal() {
            $('#ModalNuevo').modal('hide');
        }
    </script>
    <script>
        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
        }

        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }
    </script>
</asp:Content>
