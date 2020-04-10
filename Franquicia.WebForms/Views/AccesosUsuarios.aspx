<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="AccesosUsuarios.aspx.cs" Inherits="Franquicia.WebForms.Views.AccesosUsuarios" %>

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
            padding-left: 10px;
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
                                <div class="card-header card-header-tabs card-header-primary" style="padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group">

                                                <asp:Label Text="Listado de Accesos" runat="server" />

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
                                            <asp:GridView ID="gvPerfiles" OnRowCommand="gvPerfiles_RowCommand" OnRowDataBound="gvPerfiles_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidSegPerfil" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay accesos registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField SortExpression="VchRFC" DataField="VchNombre" HeaderText="NOMBRE" />
                                                    <asp:BoundField SortExpression="VchRazonSocial" DataField="VchPerfil" HeaderText="TIPO PERFIL" />
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
        <div class="modal-dialog modal-dialog-scrollable" role="document">
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
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul class="nav nav-tabs" data-tabs="tabs">
                                            <li class="nav-item">
                                                <a class="nav-link active show" href="#general" data-toggle="tab">
                                                    <i class="material-icons">business</i>General<div class="ripple-container"></div>
                                                </a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" href="#accesos" data-toggle="tab">
                                                    <i class="material-icons">directions</i>Accesos<div class="ripple-container"></div>
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

                                    <div class="tab-pane active show" id="general">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row">
                                                    <div class="form-group col-md-6">
                                                        <label for="txtNombre" style="color: black;">Nombre</label>
                                                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="ddlTipoPerfil" style="color: black;">Tipo Perfil</label>
                                                        <asp:DropDownList ID="ddlTipoPerfil" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoPerfil_SelectedIndexChanged" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="Seleccione" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="ddlModuloInicial" style="color: black;">Modulo Inicial</label>
                                                        <asp:DropDownList ID="ddlModuloInicial" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="Seleccione" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="ddlEstatusPerfil" style="color: black;">Estatus</label>
                                                        <asp:DropDownList ID="ddlEstatus" Enabled="false" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="Seleccione" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="tab-pane" id="accesos">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <ul class="nav nav-pills nav-pills-primary" role="tablist">
                                                    <li id="ActivarClientes" runat="server">
                                                        <asp:LinkButton ID="btnVisibilidadPanelClientes" OnClick="VisibilidadPanelClientes" class="nav-link active" runat="server">
                                                <span class="glyphicon glyphicon-list-alt"> Clientes</span>
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li id="ActivarUsuarios" runat="server">
                                                        <asp:LinkButton ID="btnVisibilidadPanelUsuarios" OnClick="VisibilidadPanelUsuarios" class="nav-link" runat="server">
                                            <span class="glyphicon glyphicon-road"> Usuarios</span>
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>

                                                <asp:Panel ID="PanelClientes" runat="server">
                                                    <div class="row">
                                                        <div class="form-group col col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                            <asp:GridView ID="gvModulosClientes" Style="cursor: pointer;" OnSelectedIndexChanged="gvModulosClientes_SelectedIndexChanged" OnRowDataBound="gvModulosClientes_RowDataBound" EnablePersistedSelection="true" AllowSorting="true" AutoGenerateColumns="false" CssClass="table-hover" DataKeyNames="UidSegModulo" GridLines="None" border="0" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <div class="alert alert-info">No hay modulos registrados</div>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                                    <asp:BoundField ControlStyle-ForeColor="Black" DataField="VchNombre" HeaderText="Modulos" />
                                                                </Columns>
                                                                <SelectedRowStyle BackColor="#dff0d8" />
                                                            </asp:GridView>
                                                        </div>
                                                        <div class="form-group col col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                            <table>
                                                                <thead>
                                                                    <tr>
                                                                        <th>Permisos
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBoxList ID="cblPermisosClientes" AutoPostBack="true" OnSelectedIndexChanged="cblPermisosClientes_SelectedIndexChanged" CssClass="table-hover" RepeatDirection="Vertical" runat="server" /></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="PanelUsuarios" Visible="false" runat="server">
                                                    <div class="row">
                                                        <div class="row">
                                                            <div class="form-group col col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                                <asp:GridView ID="gvModulosUsuarios" Style="cursor: pointer;" OnSelectedIndexChanged="gvModulosUsuarios_SelectedIndexChanged" OnRowDataBound="gvModulosUsuarios_RowDataBound" EnablePersistedSelection="true" AllowSorting="true" AutoGenerateColumns="false" CssClass="table-hover" DataKeyNames="UidSegModulo" GridLines="None" border="0" runat="server">
                                                                    <EmptyDataTemplate>
                                                                        <div class="alert alert-info">No hay modulos registrados</div>
                                                                    </EmptyDataTemplate>
                                                                    <Columns>
                                                                        <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                                        <asp:BoundField ControlStyle-ForeColor="Black" DataField="VchNombre" HeaderText="Modulos" />
                                                                    </Columns>
                                                                    <SelectedRowStyle BackColor="#dff0d8" />
                                                                </asp:GridView>
                                                            </div>
                                                            <div class="form-group col col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                                <table>
                                                                    <thead>
                                                                        <tr>
                                                                            <th>Permisos
                                                                            </th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="cblPermisosUsuarios" AutoPostBack="true" OnSelectedIndexChanged="cblPermisosUsuarios_SelectedIndexChanged" CssClass="table-hover" RepeatDirection="Vertical" runat="server" /></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>



                                        <%--<asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <ul class="nav nav-pills nav-pills-primary" role="tablist">
                                                    <li id="liClientes" class="nav-item" runat="server">
                                                        <a class="nav-link" data-toggle="tab" href="#clientes" role="tablist" aria-expanded="false">Clientes
                                                        </a>
                                                    </li>
                                                    <li id="liUsuarios" class="nav-item" runat="server">
                                                        <a class="nav-link" data-toggle="tab" href="#usuarios" role="tablist" aria-expanded="false">Usuarios
                                                        </a>
                                                    </li>
                                                </ul>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>

                                        <%--<div class="tab-content">
                                            <div class="tab-pane" id="clientes" aria-expanded="false">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <div class="tab-pane" id="usuarios" aria-expanded="false">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>--%>
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
