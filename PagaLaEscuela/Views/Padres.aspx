﻿<%@ Page Title="Tutores" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Padres.aspx.cs" Inherits="PagaLaEscuela.Views.Padres" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlAlertImportarError" Visible="false" runat="server">
                <div id="divAlertImportarError" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                    <div class="row">
                        <asp:Label ID="lblMnsjAlertImportarError" Style="margin-top: 5px; margin-left: 15px;" runat="server" />
                        <asp:LinkButton ID="btnDescargarError" Visible="false" OnClick="btnDescargarError_Click" Style="padding-bottom: 5px; padding-top: 5px; padding-right: 5px; padding-left: 5px; margin-top: 0px;" class="btn btn-success" runat="server">Descargar Error</asp:LinkButton>
                        <asp:LinkButton ID="btnMasDetalle" Visible="false" OnClick="btnMasDetalle_Click" Style="padding-bottom: 5px; padding-top: 5px; padding-right: 5px; padding-left: 5px; margin-top: 0px;" class="btn btn-info" runat="server">Más detalle</asp:LinkButton>
                    </div>

                    <asp:LinkButton ID="btnCloseAlertImportarError" OnClick="btnCloseAlertImportarError_Click" class="close" aria-label="Close" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
                </div>
            </asp:Panel>

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
            <asp:FileUpload ID="fuSelecionarExcel" Style="display: none;" runat="server" />

            <script type="text/javascript">
                function UploadFile(fileUpload) {
                    if (fileUpload.value != '') {
                        var divProgress = document.getElementById("divProgress");
                        var lblTittleProgress = document.getElementById("lblTittleProgress");
                        divProgress.style = "block";
                        lblTittleProgress.innerText = "Importando...";

                        document.getElementById("<%=btnImportarExcel.ClientID %>").click();
                    }
                }
            </script>
            <asp:Button ID="btnImportarExcel" OnClick="btnImportarExcel_Click" Style="display: none;" Text="Subir" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImportarExcel" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="content">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="card">
                                <div class="card-header card-header-tabs card-header-primary" style="background: #326497; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group" style="margin-top: 0px; padding-bottom: 0px;">
                                                <asp:LinkButton ID="btnFiltros" OnClick="btnFiltros_Click" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Listado de tutores" runat="server" />

                                                <div class="pull-right">
                                                    <asp:LinkButton ID="btnCargarExcel" ToolTip="Importar padres." class="btn btn-lg btn-ligh btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_upload</i>
                                                    </asp:LinkButton>
                                                    ||
                                                    <asp:LinkButton ID="btnExportarLista" OnClick="btnExportarLista_Click" ToolTip="Exportar padres." class="btn btn-lg btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_download</i>
                                                    </asp:LinkButton>
                                                    ||
                                                    <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" ToolTip="Agregar padres." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
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
                                            <asp:GridView ID="gvPadres" OnRowCreated="gvPadres_RowCreated" OnSorting="gvPadres_Sorting" OnRowCommand="gvPadres_RowCommand" OnRowDataBound="gvPadres_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvPadres_PageIndexChanging" ShowFooter="true" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay tutores registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE COMPLETO" />
                                                    <asp:BoundField SortExpression="StrCorreo" DataField="StrCorreo" HeaderText="CORREO" />
                                                    <asp:BoundField SortExpression="StrTelefono" DataField="StrTelefono" HeaderText="CELULAR" />
                                                    <asp:TemplateField SortExpression="IntCantAlumnos" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# ALUMNO(S)">
                                                        <ItemTemplate>
                                                            <asp:Label CssClass="notification" runat="server">
                                                                <span><i class="material-icons">wc</i></span>
                                                                <span class="badge" style="background-color:<%#Eval("ColorNotification")%>;"><%#Eval("IntCantAlumnos")%></span>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField SortExpression="VchUsuario" DataField="VchUsuario" HeaderText="USUARIO" />--%>
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
                                                    <asp:TemplateField ItemStyle-Width="160">
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
                                                                        <td style="display: none; border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCancelarCambio" ToolTip="Visualizar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Ver" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="lblCancelarCambio" class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">remove_red_eye</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ToolTip="Enviar credenciales" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Enviar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">send</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblPaginado" Font-Bold="true" runat="server" />
                                                        </FooterTemplate>
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
                    <div class="row">
                        <div class="card card-nav-tabs">
                            <div class="card-header card-header-primary" style="background: #326497;">
                                <!-- colors: "header-primary", "header-info", "header-success", "header-warning", "header-danger" -->
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul id="ulTabPadres" class="nav nav-tabs" data-tabs="tabs">
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
                                            <li class="nav-item">
                                                <a class="nav-link" href="#alumnos" data-toggle="tab">
                                                    <i class="material-icons">wc</i>Asignar Alumnos<div class="ripple-container"></div>
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
                                                    <div style="display: none" class="form-group col-md-4">
                                                        <label for="txtUsuario" style="color: black;">Usuario</label>
                                                        <asp:TextBox ID="txtUsuario" autocomplete="nope" CssClass="form-control" runat="server" />
                                                        <asp:Label CssClass="text-danger" runat="server" ID="lblExisteUsuario" />
                                                        <asp:Label CssClass="text-success" runat="server" ID="lblNoExisteUsuario" />
                                                        <asp:LinkButton ID="btnValidarUsuario" CssClass="pull-right" Text="Validar" OnClick="btnValidarUsuario_Click" runat="server" />
                                                    </div>
                                                    <div style="display: none" class="form-group col-md-4">
                                                        <label for="txtPassword" style="color: black;">Contraseña</label>
                                                        <asp:TextBox ID="txtPassword" autocomplete="new-password" TextMode="Password" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div style="display: none" class="form-group col-md-4">
                                                        <label for="txtRepetirPassword" style="color: black;">Repetir Contraseña</label>
                                                        <asp:TextBox ID="txtRepetirPassword" TextMode="Password" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCorreo" style="color: black; margin-bottom: 12px;">Correo Eléctronico *</label>
                                                        <asp:TextBox ID="txtCorreo" CssClass="form-control" runat="server" />
                                                        <asp:Label CssClass="text-danger" runat="server" ID="lblExiste" />
                                                        <asp:Label CssClass="text-success" runat="server" ID="lblNoExiste" />
                                                        <asp:LinkButton ID="btnValidarCorreo" CssClass="pull-right" Text="Validar" OnClick="btnValidarCorreo_Click" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="ddlPrefijo" style="color: black;">Código pais *</label>
                                                        <asp:DropDownList ID="ddlPrefijo" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-2">
                                                        <label for="txtNumero" style="color: black; margin-bottom: 12px;">Celular *</label>
                                                        <asp:TextBox ID="txtNumero" TextMode="Phone" MaxLength="10" CssClass="form-control" runat="server" />
                                                        <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtNumero" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-2">
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

                                    <div class="tab-pane" id="alumnos">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
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
                                                                        <div class="form-group col-sm-6 col-md-6 col-lg-4">
                                                                            <label for="ddlFiltroAsociado" style="color: black; padding-left: 0px;">Alumno Vinculado</label>
                                                                            <asp:DropDownList ID="ddlFiltroAsociado" CssClass="form-control" runat="server">
                                                                                <asp:ListItem Text="NO" Value="0" />
                                                                                <asp:ListItem Text="SI" Value ="1" />
                                                                                <asp:ListItem Text="AMBOS" Value="" />
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="form-group col-sm-6 col-md-6 col-lg-4">
                                                                            <label for="txtFiltroAlumIdentificador" style="color: black; padding-left: 0px;">Identificador</label>
                                                                            <asp:TextBox ID="txtFiltroAlumIdentificador" CssClass="form-control" aria-label="Search" runat="server" />
                                                                        </div>
                                                                        <div class="form-group col-sm-6 col-md-6 col-lg-4">
                                                                            <label for="txtFiltroAlumMatricula" style="color: black; padding-left: 0px;">Matricula</label>
                                                                            <asp:TextBox ID="txtFiltroAlumMatricula" CssClass="form-control" aria-label="Search" runat="server" />
                                                                        </div>
                                                                        <div class="form-group col-sm-6 col-md-6 col-lg-4">
                                                                            <label for="txtFiltroAlumNombre" style="color: black; padding-left: 0px;">Nombre(s)</label>
                                                                            <asp:TextBox ID="txtFiltroAlumNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                                        </div>
                                                                        <div class="form-group col-sm-6 col-md-6 col-lg-4">
                                                                            <label for="txtFiltroAlumPaterno" style="color: black; padding-left: 0px;">ApePaterno</label>
                                                                            <asp:TextBox ID="txtFiltroAlumPaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                                        </div>
                                                                        <div class="form-group col-sm-6 col-md-6 col-lg-4">
                                                                            <label for="txtFiltroAlumMaterno" style="color: black; padding-left: 0px;">ApeMaterno</label>
                                                                            <asp:TextBox ID="txtFiltroAlumMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div>
                                                        <h5>Seleccionados:
                                                            <asp:Label ID="lblCantSeleccionado" CssClass="alert alert-warning" Font-Size="Larger" Text="0" Style="padding-top: 5px; padding-bottom: 5px; padding-left: 10px; padding-right: 10px;" runat="server" />
                                                        </h5>
                                                    </div>

                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvAlumnos" OnRowCreated="gvAlumnos_RowCreated" OnRowDataBound="gvAlumnos_RowDataBound" OnPageIndexChanging="gvAlumnos_PageIndexChanging" OnSorting="gvAlumnos_Sorting" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidAlumno" GridLines="None" border="0" AllowPaging="true" PageSize="5" runat="server">
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info">No hay alumnos asignados</div>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="cbTodo" AutoPostBack="true" OnCheckedChanged="cbTodo_CheckedChanged" runat="server" />
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

                                                                <asp:BoundField SortExpression="VchIdentificador" DataField="VchIdentificador" HeaderText="IDENTIFICADOR" />
                                                                <asp:BoundField SortExpression="VchMatricula" DataField="VchMatricula" HeaderText="MATRICULA" />
                                                                <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE" />
                                                            </Columns>
                                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                        </asp:GridView>
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
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroCorreo" style="color: black;">Correo</label>
                                                                <asp:TextBox ID="FiltroCorreo" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroCorreo" style="color: black;">Celular</label>
                                                                <asp:TextBox ID="FiltroCelular" TextMode="Phone" CssClass="form-control" aria-label="Search" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" TargetControlID="FiltroCelular" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroCorreo" style="color: black;">Cant. de Alumnos</label>
                                                                <asp:TextBox ID="FiltroAlumnos" TextMode="Phone" CssClass="form-control" aria-label="Search" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" TargetControlID="FiltroAlumnos" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroColegiatura" style="color: black;">Colegiatura pendiente</label>
                                                                <asp:DropDownList ID="FiltroColegiatura" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="NO IMPORTA" />
                                                                    <asp:ListItem Text="SI" />
                                                                    <asp:ListItem Text="NO" />
                                                                </asp:DropDownList>
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

    <div id="ModalMasDetalle" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" data-backdrop="static" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label1" Text="Más Detalle" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <div class="tab-content">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="row">
                                        <div class="card">
                                            <img src="../Images/LigaSimple.PNG" class="card-img-top" alt="...">
                                            <div class="card-body">
                                                <h5 class="card-title"><strong>Campos obligatorios *</strong></h5>
                                                <p class="card-text">Nombre(s) *.</p>
                                                <p class="card-text">ApePaterno *.</p>
                                                <p class="card-text">ApeMaterno *.</p>
                                                <p class="card-text">Correo * + Formato correcto (ejemplo@ejemplo.com).</p>
                                                <p class="card-text">Celular *.</p>
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
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Aceptar
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
    </script>
    <script>
        function showModalBusqueda() {
            $('#ModalBusqueda').modal('show');
        }

        function hideModalBusqueda() {
            $('#ModalBusqueda').modal('hide');
        }
    </script>
    <script>
        function showModalMasDetalle() {
            $('#ModalMasDetalle').modal('show');
        }

        function hideModalMasDetalle() {
            $('#ModalMasDetalle').modal('hide');
        }
    </script>
    <script>
        function showTab(tab) {
            $('#ulTabPadres a[href="#general"]').tab('show')
        }
    </script>
</asp:Content>
