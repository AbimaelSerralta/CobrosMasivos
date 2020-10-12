<%@ Page Title="CrearEventos" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="CrearEventos.aspx.cs" Inherits="Franquicia.WebForms.Views.CrearEventos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
    <style>
        .form-check, label {
            font-size: 14px;
            line-height: 1.42857;
            color: #333333;
            font-weight: 400;
            padding-left: 5px;
            padding-right: 20px;
            /*width: 100%;*/
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
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                                <i class="material-icons">search</i>
                                                </asp:LinkButton>

                                                <asp:Label Text="Listado de Eventos" runat="server" />

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
                                            <asp:GridView ID="gvEventos" OnRowCommand="gvEventos_RowCommand" OnRowDataBound="gvEventos_RowDataBound" OnSorting="gvEventos_Sorting" AllowSorting="true" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvEventos_PageIndexChanging" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidEvento" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay eventos registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:BoundField SortExpression="VchNombreEvento" DataField="VchNombreEvento" HeaderText="EVENTO" />
                                                    <asp:BoundField SortExpression="DtFHInicio" DataField="DtFHInicio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HeaderText="F/H INICIO" />
                                                    <asp:BoundField SortExpression="VchFHFin" DataField="VchFHFin" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HeaderText="F/H FIN" />
                                                    <asp:TemplateField HeaderText="URL">
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtGvCorreo" ReadOnly="true" ToolTip='<%#Eval("VchUrlEvento")%>' Style="width: 100%; text-overflow: ellipsis; margin-top: 8px;" Text='<%#Eval("VchUrlEvento")%>' Enabled="false" BackColor="Transparent" BorderStyle="None" runat="server" /></td>
                                                                    <td>
                                                                        <asp:LinkButton ID="btnCopiar" data-text='<%#Eval("VchUrlEvento")%>' CssClass="copyboard" ToolTip="Copiar" Style="margin-left: 5px;" runat="server">
                                                                            <asp:Label ID="lblCopiar" class="btn btn-sm btn-dark btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">content_copy</i>
                                                                            </asp:Label>
                                                                        </asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="VchTipoEvento" HeaderText="TIPO">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchTipoEvento")%>' runat="server">
                                                                <i class="large material-icons">
                                                                    <%#Eval("VchIconoEvento")%>
                                                                </i>
                                                                </asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                                                                            <asp:LinkButton ID="LinkButn1" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Editar" Style="margin-left: 5px;" runat="server">
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
                            <div class="row">
                                <div class="card card-nav-tabs">
                                    <div class="card-header card-header-primary">
                                        <div class="nav-tabs-navigation">
                                            <div class="nav-tabs-wrapper">
                                                <ul class="nav nav-tabs" data-tabs="tabs">
                                                    <li id="liActivarEvento" class="nav-item" runat="server">
                                                        <asp:LinkButton ID="btnActivarEvento" OnClick="btnActivarEvento_Click" CssClass="nav-link active show" runat="server">
                                                    <i class="material-icons">date_range</i>Evento<div class="ripple-container"></div>
                                                        </asp:LinkButton>
                                                    </li>

                                                    <li id="liActivarUsuarios" class="nav-item" runat="server">
                                                        <asp:LinkButton ID="btnActivarUsuarios" OnClick="btnActivarUsuarios_Click" CssClass="nav-link" runat="server">
                                                    <i class="material-icons">group_add</i>Asignar Usuarios<div class="ripple-container"></div>
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
                                                    <asp:Label CssClass="text-danger" runat="server" ID="lblValidar" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <asp:Panel ID="pnlActivarEvento" Visible="true" runat="server">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div class="row">
                                                                    <div class="form-group col-md-6">
                                                                        <label for="txtNombreEvento" style="color: black;">Nombre del evento *</label>
                                                                        <asp:TextBox ID="txtNombreEvento" CssClass="form-control" Style="margin-top: 17px;" runat="server" />
                                                                    </div>
                                                                    <div class="form-group col-md-6">
                                                                        <label for="txtDescripcion" style="color: black;">Descripción del evento</label>
                                                                        <asp:TextBox ID="txtDescripcion" CssClass="form-control" TextMode="MultiLine" runat="server" />
                                                                    </div>
                                                                    <div class="form-group col-md-6">
                                                                        <label for="txtFHInicio" style="color: black;">F/H de inicio *</label>
                                                                        <asp:TextBox ID="txtFHInicio" ToolTip="F/H de la Ciudad de México, CDMX" TextMode="DateTimeLocal" CssClass="form-control" runat="server" />
                                                                    </div>

                                                                    <div class="form-group col-md-6">
                                                                        <asp:CheckBox ID="cbxActivarFF" Text="F/H Finalización *" AutoPostBack="true" OnCheckedChanged="cbxActivarFF_CheckedChanged" runat="server" />

                                                                        <asp:Panel ID="pnlActivarFHFinal" Enabled="false" Width="100%" runat="server">
                                                                            <asp:TextBox ID="txtFHFinalizacion" ToolTip="F/H de la Ciudad de México, CDMX" TextMode="DateTimeLocal" CssClass="form-control" runat="server" />
                                                                        </asp:Panel>
                                                                    </div>
                                                                    <div class="form-group col-md-6">
                                                                        <label for="txtFechaAlta" style="color: black;">Importe *</label>
                                                                        <div class="row">
                                                                            <div class="form-group col-md-6">
                                                                                <asp:DropDownList ID="ddlTipoImporte" ToolTip="Editable: Lo que la persona considere.&#10;Exacto: Ingresado por el creador del evento." CssClass="form-control" runat="server">
                                                                                    <asp:ListItem Text="EDITABLE" Value="true" />
                                                                                    <asp:ListItem Text="EXACTO" Value="false" />
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="form-group col-md-6">
                                                                                <div class="input-group">
                                                                                    <div class="input-group-prepend">
                                                                                        <span class="input-group-text" style="padding-left: 0px;">
                                                                                            <i class="material-icons">$</i>
                                                                                        </span>
                                                                                    </div>
                                                                                    <asp:TextBox ID="txtImporte" Text="50" CssClass="form-control" Style="margin-top: 4px;" TextMode="Phone" runat="server" />
                                                                                </div>

                                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporte" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group col-md-6">
                                                                        <label for="txtConcepto" style="color: black;">Concepto *</label>
                                                                        <asp:TextBox ID="txtConcepto" CssClass="form-control" Style="margin-top: 11px;" runat="server" />
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="ddlPedirDatos" style="color: black; padding-left: 0px; text-overflow: ellipsis;">¿Pedir datos? *</label>
                                                                        <asp:DropDownList ID="ddlPedirDatos" CssClass="form-control" runat="server">
                                                                            <asp:ListItem Text="SI" Value="true" />
                                                                            <asp:ListItem Text="NO" Value="false" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="ddlTipoEvento" style="color: black; padding-left: 0px; text-overflow: ellipsis;">Tipo evento *</label>
                                                                        <asp:DropDownList ID="ddlTipoEvento" OnSelectedIndexChanged="ddlTipoEvento_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                                                            <asp:ListItem Text="PÚBLICO" Value="publico" />
                                                                            <asp:ListItem Text="PRIVADO" Value="privado" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div id="pnlDatosBeneficiario" class="form-group col-md-4" runat="server">
                                                                        <label for="ddlPedirDatos" style="color: black; padding-left: 0px; text-overflow: ellipsis;">Datos beneficiario *</label>
                                                                        <asp:DropDownList ID="ddlDatosBeneficiario" CssClass="form-control" runat="server">
                                                                            <asp:ListItem Text="SI" Value="true" />
                                                                            <asp:ListItem Text="NO" Value="false" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="ListBoxMultipleMod" style="color: black; padding-left: 0px;">Promoción(es)</label>
                                                                        <br />
                                                                        <asp:ListBox ID="ListBoxMultipleMod" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                                    </div>
                                                                    <div class="form-group col-md-4">
                                                                        <label for="ddlEstatus" style="color: black; padding-left: 0px;">Estatus</label>
                                                                        <asp:DropDownList ID="ddlEstatus" CssClass="form-control" runat="server">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>

                                            <asp:Panel ID="pnlActivarUsuarios" Visible="false" runat="server">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlSeleccionUsuarios" runat="server">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="card" style="margin-top: 0px;">
                                                                        <div class="card-header">
                                                                            <div class="d-flex flex-row-reverse">
                                                                                <div>
                                                                                    <asp:LinkButton ID="btnFiltroBuscar" OnClick="btnFiltroBuscar_Click" BorderWidth="1" CssClass="btn btn-info btn-sm" runat="server">
                                                                                <i class="material-icons">search</i> Buscar
                                                                                    </asp:LinkButton>
                                                                                </div>
                                                                                <div>
                                                                                    <asp:LinkButton ID="btnFiltroLimpiar" OnClick="btnFiltroLimpiar_Click" BorderWidth="1" CssClass="btn btn-warning btn-sm" runat="server">
                                                                                <i class="material-icons">delete</i> Limpiar
                                                                                    </asp:LinkButton>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="card-body" style="padding-top: 0px; padding-bottom: 0px; padding-right: 10px; padding-left: 10px;">
                                                                            <div class="form-group" style="margin-top: 0px;">
                                                                                <div class="row">
                                                                                    <div class="form-group col-sm-6 col-md-6 col-lg-3">
                                                                                        <label for="txtFiltroNombre" style="color: black; padding-left: 0px;">Nombre(s)</label>
                                                                                        <asp:TextBox ID="txtFiltroEveNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                                                    </div>
                                                                                    <div class="form-group col-sm-6 col-md-6 col-lg-3">
                                                                                        <label for="txtFiltroPaterno" style="color: black; padding-left: 0px;">ApePaterno</label>
                                                                                        <asp:TextBox ID="txtFiltroEvePaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                                                    </div>
                                                                                    <div class="form-group col-sm-6 col-md-6 col-lg-3">
                                                                                        <label for="txtFiltroMaterno" style="color: black; padding-left: 0px;">ApeMaterno</label>
                                                                                        <asp:TextBox ID="txtFiltroEveMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                                                    </div>
                                                                                    <div class="form-group col-sm-6 col-md-6 col-lg-3">
                                                                                        <label for="txtFiltroCorreo" style="color: black; padding-left: 0px;">Correo</label>
                                                                                        <asp:TextBox ID="txtFiltroEveCorreo" CssClass="form-control" aria-label="Search" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div>
                                                                    <h5>Seleccionados:
                                                                        <asp:Label ID="lblCantSeleccionado" CssClass="alert alert-warning" Font-Size="Larger" Text="0" Style="padding-top: 5px; padding-bottom: 5px; padding-left: 10px; padding-right: 10px;" runat="server" /></h5>
                                                                </div>

                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="gvUsuarios" OnSorting="gvUsuarios_Sorting" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvUsuarios_PageIndexChanging" runat="server">
                                                                        <EmptyDataTemplate>
                                                                            <div class="alert alert-info">No hay usuarios</div>
                                                                        </EmptyDataTemplate>
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <%--<asp:CheckBox ID="cbTodo" AutoPostBack="true" OnCheckedChanged="cbTodo_CheckedChanged" runat="server" />--%>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <table>
                                                                                        <tbody>
                                                                                            <tr style="background: transparent;">
                                                                                                <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                                                    <asp:CheckBox ID="cbSeleccionado" OnCheckedChanged="cbSeleccionado_CheckedChanged" AutoPostBack="true" Checked='<%#Eval("blSeleccionado")%>' runat="server" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE" />
                                                                            <asp:BoundField SortExpression="StrCorreo" DataField="StrCorreo" HeaderText="CORREO" />
                                                                            <asp:BoundField SortExpression="StrTelefono" DataField="StrTelefono" HeaderText="CELULAR" />
                                                                        </Columns>
                                                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                                    </asp:GridView>
                                                                </div>

                                                            </div>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server" ID="upRegistro">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnCerrar" OnClick="btnCerrar_Click" Visible="false" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnGuardar" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancelar" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnEditar" OnClick="btnEditar_Click" CssClass="btn btn-warning btn-round" runat="server">
                            <i class="material-icons">refresh</i> Editar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCerrar" EventName="Click" />
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
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroNombre" style="color: black;">Nombre evento</label>
                                                                <asp:TextBox ID="txtFiltroNombre" Style="margin-top: 12px;" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="ddlFiltroEstatus" style="color: black;">Estatus</label>
                                                                <asp:DropDownList ID="ddlFiltroEstatus" AppendDataBoundItems="true" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="ddlImporteMayor" style="color: black;">Importe</label>
                                                                <asp:DropDownList ID="ddlImporteMayor" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="(>=) Mayor o igual que" Value=">=" />
                                                                    <asp:ListItem Text="(>) Mayor que" Value=">" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="txtFiltroDcmImporteMayor" style="color: black;"></label>
                                                                <asp:TextBox ID="txtFiltroDcmImporteMayor" CssClass="form-control" placeholder="Mayor" aria-label="Search" Style="margin-top: 11px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtFiltroDcmImporteMayor" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="ddlImporteMenor" style="color: black;"></label>
                                                                <asp:DropDownList ID="ddlImporteMenor" CssClass="form-control" Style="margin-top: 6px;" runat="server">
                                                                    <asp:ListItem Text="(<=) Menor o igual que" Value="<=" />
                                                                    <asp:ListItem Text="(<) Menor que" Value="<" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="txtFiltroDcmImporteMenor" style="color: black;"></label>
                                                                <asp:TextBox ID="txtFiltroDcmImporteMenor" CssClass="form-control" placeholder="Menor" aria-label="Search" Style="margin-top: 11px;" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtFiltroDcmImporteMenor" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroDtInicioDesde" style="color: black;">Fecha Inicio</label>
                                                                <asp:TextBox ID="txtFiltroDtInicioDesde" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroDtInicioHasta" style="color: black;"></label>
                                                                <asp:TextBox ID="txtFiltroDtInicioHasta" Style="margin-top: 5px;" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroDtFinDesde" style="color: black;">Fecha Fin</label>
                                                                <asp:TextBox ID="txtFiltroDtFinDesde" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-6">
                                                                <label for="txtFiltroDtFinHasta" style="color: black;"></label>
                                                                <asp:TextBox ID="txtFiltroDtFinHasta" Style="margin-top: 5px;" CssClass="form-control" TextMode="Date" aria-label="Search" runat="server" />
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
        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
        }

        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }

        function hideAlert() {
            $(".alert").alert('close')
        }
    </script>
    <script>
        function multi() {
            $('[id*=ListBox]').multiselect({
                includeSelectAllOption: true
            });
        }
    </script>
    <script type="text/javascript">
        function CopyGridView(target) {
            var div = document.getElementById(target);
            div.contentEditable = 'true';
            var controlRange;
            if (document.body.createControlRange) {
                controlRange = document.body.createControlRange();
                controlRange.addElement(div);
                controlRange.execCommand('Copy');
            }
            div.contentEditable = 'false';
            return false;
        }
    </script>
    <script>
        function shot() {
            $('.copyboard').on('click', function (e) {
                e.preventDefault();

                var copyText = $(this).attr('data-text');
                var el = $('<input style="position: absolute; bottom: -120%" type="text" value="' + copyText + '" />').appendTo('body');
                el[0].select();
                document.execCommand("copy");
                el.remove();
            })
        }
    </script>
</asp:Content>
