<%@ Page Title="Ciclo Escolar" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="CicloEscolar.aspx.cs" Inherits="PagaLaEscuela.Views.CicloEscolar" %>

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
                                <div class="card-header card-header-tabs card-header-primary" style="background:#f33527;padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de cliclos escolares" runat="server" />

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
                                            <asp:GridView ID="gvCicloEscolar" OnSorting="gvCicloEscolar_Sorting" OnRowCommand="gvCicloEscolar_RowCommand" OnRowDataBound="gvCicloEscolar_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvCicloEscolar_PageIndexChanging" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay ciclos escolares registrados</div>
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
    <div class="modal fade" id="ModalNuevo" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
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
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Label CssClass="text-danger" runat="server" ID="lblValidar" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="card card-nav-tabs">
                                    <div class="card-header card-header-primary" style="background:#f33527;">
                                        <div class="nav-tabs-navigation">
                                            <div class="nav-tabs-wrapper">
                                                <ul class="nav nav-tabs" data-tabs="tabs">
                                                    <li id="li1Activar" class="nav-item" runat="server">
                                                        <asp:LinkButton ID="btn1Activar" OnClick="btn1Activar_Click" CssClass="nav-link active show" runat="server">
                                                    <i class="material-icons">date_range</i>General<div class="ripple-container"></div>
                                                        </asp:LinkButton>
                                                    </li>

                                                    <li id="li2Activar" class="nav-item" runat="server">
                                                        <asp:LinkButton ID="btn2Activar" OnClick="btn2Activar_Click" CssClass="nav-link" runat="server">
                                                    <i class="material-icons">group_add</i>Cronograma de pago<div class="ripple-container"></div>
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <div class="tab-content">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:Label CssClass="text-danger" runat="server" ID="Label2" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <asp:Panel ID="pnlActivarGeneral" Visible="true" runat="server">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="form-group col-md-4">
                                                                        <label for="txtNombre" style="color: black;">Descripción *</label>
                                                                        <asp:TextBox ID="txtNombre" CssClass="form-control" required="required" runat="server" />
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="txtFInicio" style="color: black;">Fecha inicio *</label>
                                                                        <asp:TextBox ID="txtFInicio" TextMode="Date" CssClass="form-control" required="required" runat="server" />
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="txtFVencimiento" style="color: black;">Fecha vencimiento *</label>
                                                                        <asp:TextBox ID="txtFVencimiento" CssClass="form-control" required="required" runat="server" />
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="ddlNivel" style="color: black;">Nivel de enseñanza</label>
                                                                        <asp:DropDownList ID="ddlNivel" CssClass="form-control" runat="server">
                                                                            <asp:ListItem Text="PRIMARIA" />
                                                                            <asp:ListItem Text="SECUNDARIA" />
                                                                            <asp:ListItem Text="PREPARATORIA" />
                                                                            <asp:ListItem Text="UNIVERSIDAD" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="ddlAnio" style="color: black;">Año de enseñanza</label>
                                                                        <asp:DropDownList ID="ddlAnio" CssClass="form-control" runat="server">
                                                                            <asp:ListItem Text="1" />
                                                                            <asp:ListItem Text="2" />
                                                                            <asp:ListItem Text="3" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="ddlSalon" style="color: black;">Salón</label>
                                                                        <asp:DropDownList ID="ddlSalon" CssClass="form-control" runat="server">
                                                                            <asp:ListItem Text="A" />
                                                                            <asp:ListItem Text="B" />
                                                                            <asp:ListItem Text="C" />
                                                                            <asp:ListItem Text="201" />
                                                                            <asp:ListItem Text="202" />
                                                                            <asp:ListItem Text="203" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="ddlEstatus" style="color: black;">Estatus *</label>
                                                                        <asp:DropDownList ID="ddlEstatus" CssClass="form-control" runat="server">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>

                                            <asp:Panel ID="pnlActivarCronograma" Visible="false" runat="server">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="form-group col-md-4">
                                                                <label for="txtNombre" style="color: black;">Descripción *</label>
                                                                <asp:TextBox ID="TextBox1" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="txtFPago" style="color: black;">Fecha de pago *</label>
                                                                <asp:TextBox ID="txtFPago" TextMode="Date" CssClass="form-control" required="required" runat="server" />
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel Visible="false" runat="server">
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
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
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
