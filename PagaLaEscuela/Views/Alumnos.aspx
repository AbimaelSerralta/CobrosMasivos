<%@ Page Title="Alumnos" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Alumnos.aspx.cs" Inherits="PagaLaEscuela.Views.Alumnos" %>

<%@ MasterType VirtualPath="~/Views/MasterPage.Master" %>
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
                                                <asp:Label Text="Listado de alumnos" runat="server" />

                                                <div class="pull-right">
                                                    <asp:LinkButton ID="btnCargarExcel" ToolTip="Importar alumnos." class="btn btn-lg btn-ligh btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_upload</i>
                                                    </asp:LinkButton>
                                                    ||
                                                    <asp:LinkButton ID="btnExportarLista" OnClick="btnExportarLista_Click" ToolTip="Exportar alumnos." class="btn btn-lg btn-warning btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_download</i>
                                                    </asp:LinkButton>
                                                    ||
                                                    <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" ToolTip="Agregar alumnos." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
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
                                            <asp:GridView ID="gvAlumnos" SelectedRowStyle-BackColor="#dff0d8" OnRowCreated="gvAlumnos_RowCreated" OnSorting="gvAlumnos_Sorting" OnRowCommand="gvAlumnos_RowCommand" OnRowDataBound="gvAlumnos_RowDataBound" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidAlumno" GridLines="None" border="0" AllowPaging="true" PageSize="10" EnablePersistedSelection="true" OnPageIndexChanging="gvAlumnos_PageIndexChanging" ShowFooter="true" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay alumnos registrados</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>--%>
                                                    <asp:BoundField SortExpression="VchIdentificador" DataField="VchIdentificador" HeaderText="IDENTIFICADOR" />
                                                    <asp:BoundField SortExpression="VchMatricula" DataField="VchMatricula" HeaderText="MATRICULA" />
                                                    <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE COMPLETO" />
                                                    <asp:BoundField SortExpression="VchBeca" DataField="VchBeca" HeaderText="¿BECA?" />
                                                    <asp:BoundField SortExpression="VchCorreo" DataField="VchCorreo" HeaderText="CORREO" />
                                                    <%--<asp:TemplateField SortExpression="VchDescripcionAsociado" HeaderText="ASOCIADO">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchDescripcionAsociado")%>' runat="server">
                                                                <i class="large material-icons">
                                                                    <%#Eval("VchIconoAsociado")%>
                                                                </i>
                                                                </asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField SortExpression="IntCantPadres" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="# TUTOR(ES)">
                                                        <ItemTemplate>
                                                            <asp:Label CssClass="notification" runat="server">
                                                                <span><i class="material-icons">people</i></span>
                                                                <span class="badge" style="background-color:<%#Eval("ColorNotification")%>;"><%#Eval("IntCantPadres")%></span>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="LinkButton2" Visible='<%#Eval("blVisibleDesasociar")%>' ToolTip="Desvincular Padre" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="Desasociar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="lblDesasociarPadre" class="btn btn-sm btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">person_remove</i>
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
    <div id="ModalNuevo" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
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
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="card card-nav-tabs">
                                    <div class="card-header card-header-primary" style="background: #326497;">
                                        <div class="nav-tabs-navigation">
                                            <div class="nav-tabs-wrapper">
                                                <ul class="nav nav-tabs" data-tabs="tabs">
                                                    <li id="liActivarGeneral" class="nav-item" runat="server">
                                                        <asp:LinkButton ID="btnActivarGeneral" OnClick="btnActivarGeneral_Click" CssClass="nav-link active show" runat="server">
                                                    <i class="material-icons">article</i>General<div class="ripple-container"></div>
                                                        </asp:LinkButton>
                                                    </li>

                                                    <li id="liActivarDatosPadre" class="nav-item" runat="server">
                                                        <asp:LinkButton ID="btnActivarDatosPadre" OnClick="btnActivarDatosPadre_Click" CssClass="nav-link" runat="server">
                                                    <i class="material-icons">description</i>Datos del tutor<div class="ripple-container"></div>
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

                                            <asp:Panel ID="pnlActivarGeneral" Visible="true" runat="server">
                                                <div class="row">
                                                    <div class="form-group col-md-4">
                                                        <label for="txtIdentificador" style="color: black;">Identificador *</label>
                                                        <asp:TextBox ID="txtIdentificador" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtMatricula" style="color: black;">Matrícula *</label>
                                                        <asp:TextBox ID="txtMatricula" CssClass="form-control" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <label for="txtCorreo" style="color: black;">Correo Eléctronico</label>
                                                        <asp:TextBox ID="txtCorreo" CssClass="form-control" runat="server" />
                                                        <asp:Label CssClass="text-danger" runat="server" ID="lblExiste" />
                                                        <asp:Label CssClass="text-success" runat="server" ID="lblNoExiste" />
                                                        <%--<asp:LinkButton ID="btnValidarCorreo" CssClass="pull-right" Text="Validar" OnClick="btnValidarCorreo_Click" runat="server" />--%>
                                                    </div>
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
                                                    <div class="form-group col-md-3">
                                                        <label for="ddlPrefijo" style="color: black;">Código pais</label>
                                                        <asp:DropDownList ID="ddlPrefijo" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <label for="txtNumero" style="color: black; margin-bottom: 13px;">Celular</label>
                                                            <asp:TextBox ID="txtNumero" MaxLength="10" TextMode="Phone" CssClass="form-control" runat="server" />
                                                            <%--<asp:RegularExpressionValidator ID="REVNumero" runat="server" ControlToValidate="txtNumero" ErrorMessage="* Valores númericos" ForeColor="Red" ValidationExpression="^[0-9]*"></asp:RegularExpressionValidator>--%>
                                                            <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtNumero" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label for="ddlBeca" style="color: black;">¿Tiene beca?</label>
                                                        <div class="row">
                                                            <div class="col-md-2">
                                                                <asp:DropDownList ID="ddlBeca" OnSelectedIndexChanged="ddlBeca_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="NO" Value="false" />
                                                                    <asp:ListItem Text="SI" Value="true" />
                                                                </asp:DropDownList>
                                                            </div>

                                                            <asp:Panel ID="pnlBeca" Visible="false" CssClass="row" Style="width: 90%;" runat="server">
                                                                <div class="col-md-4" style="padding-right: 0px;">
                                                                    <asp:DropDownList ID="ddlTipoBeca" CssClass="form-control" runat="server">
                                                                        <asp:ListItem Text="CANTIDAD" Value="CANTIDAD" />
                                                                        <asp:ListItem Text="PORCENTAJE" Value="PORCENTAJE" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-6" style="padding-right: 0px;">
                                                                    <asp:TextBox ID="txtBeca" CssClass="form-control" Style="padding-bottom: 20px; padding-top: 20px;" runat="server" />
                                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtBeca" runat="server" />
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-4" visible="false" runat="server">
                                                        <label for="ddlNivel" style="color: black;">Nivel de enseñanza</label>
                                                        <asp:DropDownList ID="ddlNivel" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="PRIMARIA" />
                                                            <asp:ListItem Text="SECUNDARIA" />
                                                            <asp:ListItem Text="PREPARATORIA" />
                                                            <asp:ListItem Text="UNIVERSIDAD" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4" visible="false" runat="server">
                                                        <label for="ddlAnio" style="color: black;">Año de enseñanza</label>
                                                        <asp:DropDownList ID="ddlAnio" CssClass="form-control" runat="server">
                                                            <asp:ListItem Text="1 A" />
                                                            <asp:ListItem Text="1 B" />
                                                            <asp:ListItem Text="1 C" />
                                                            <asp:ListItem Text="2 A" />
                                                            <asp:ListItem Text="2 B" />
                                                            <asp:ListItem Text="2 C" />
                                                            <asp:ListItem Text="3 A" />
                                                            <asp:ListItem Text="3 B" />
                                                            <asp:ListItem Text="3 C" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-4" visible="false" runat="server">
                                                        <label for="ddlSalon" style="color: black;">Salón</label>
                                                        <asp:DropDownList ID="ddlSalon" CssClass="form-control" runat="server">
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
                                            </asp:Panel>

                                            <asp:Panel ID="pnlActivarDatosPadre" Visible="false" runat="server">
                                                <div class="row">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvDatosPadres" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="5" runat="server">
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info">No hay padres</div>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="TUTOR" />
                                                                <asp:BoundField SortExpression="StrTelefono" DataField="StrTelefono" HeaderText="CELULAR" />
                                                                <asp:BoundField SortExpression="StrCorreo" DataField="StrCorreo" HeaderText="CORREO" />
                                                            </Columns>
                                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server">
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
                                                                <label for="FiltroIdentificador" style="color: black;">Identificador</label>
                                                                <asp:TextBox ID="FiltroIdentificador" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroMatricula" style="color: black;">Matricula</label>
                                                                <asp:TextBox ID="FiltroMatricula" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroCorreo" style="color: black;">Correo</label>
                                                                <asp:TextBox ID="FiltroCorreo" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroNombre" style="color: black;">Nombre</label>
                                                                <asp:TextBox ID="FiltroNombre" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="FiltroApePaterno" style="color: black;">ApePaterno</label>
                                                                <asp:TextBox ID="FiltroApePaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-3">
                                                                <label for="FiltroApeMaterno" style="color: black;">ApeMaterno</label>
                                                                <asp:TextBox ID="FiltroApeMaterno" CssClass="form-control" aria-label="Search" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-2">
                                                                <label for="FiltroCelular" style="color: black;">Celular</label>
                                                                <asp:TextBox ID="FiltroCelular" MaxLength="10" TextMode="Phone" CssClass="form-control" aria-label="Search" runat="server" />
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" TargetControlID="FiltroCelular" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-2">
                                                                <label for="FiltroBeca" style="color: black;">¿Tiene beca?</label>
                                                                <asp:DropDownList ID="FiltroBeca" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="TODOS" />
                                                                    <asp:ListItem Text="SI" />
                                                                    <asp:ListItem Text="NO" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-2">
                                                                <label for="FiltroAsociado" style="color: black;">Asociado</label>
                                                                <asp:DropDownList ID="FiltroAsociado" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="TODOS" Value="TODOS" />
                                                                    <asp:ListItem Text="SI" Value="ASOCIADO" />
                                                                    <asp:ListItem Text="NO" Value="NO ASOCIADO" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroEstatus" style="color: black;">Estatus</label>
                                                                <asp:DropDownList ID="FiltroEstatus" AppendDataBoundItems="true" CssClass="form-control" runat="server"></asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-4">
                                                                <label for="FiltroColegiatura" style="color: black;">Colegiatura pendiente</label>
                                                                <asp:DropDownList ID="FiltroColegiatura" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Text="NO IMPORTA" />
                                                                    <asp:ListItem Text="SI" />
                                                                    <asp:ListItem Text="NO" />
                                                                </asp:DropDownList>
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

    <div id="ModalDesasociar" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="Label2" Text="Seleccione el padre a desvincular" runat="server" /></h5>
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>--%>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-body pt-0" style="padding-bottom: 0px;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlAlertMnsjModalDesasociar" Visible="false" runat="server">
                                <div id="divAlertMnsjModalDesasociar" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                    <asp:Label ID="lblMnsjModalDesasociar" runat="server" />
                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvPadres" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="5" runat="server">
                                        <EmptyDataTemplate>
                                            <div class="alert alert-info">No hay padres</div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%--<asp:CheckBox ID="cbTodo" AutoPostBack="true" runat="server" />--%>
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
                                            <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="TUTOR" />
                                            <asp:BoundField SortExpression="StrCorreo" DataField="StrCorreo" HeaderText="CORREO" />
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnDesasociar" OnClick="btnDesasociar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Desvincular
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCanDesasociar" CssClass="btn btn-danger btn-round" data-dismiss="modal" aria-label="Close" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="ModalDialog" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header" style="padding-bottom: 0px; padding-top: 3px;">

                            <div class="row">
                                <div style="width: 100%;">
                                    <img src="../Images/MnsjDialog.jpeg" style="width: 100%; margin: 0 auto; display: block;" height="100" width="100" class="img-fluid align-items-center" alt="Responsive image">
                                </div>
                            </div>
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
                                <div class="text-center">
                                    <h5 class="modal-title" runat="server">
                                        <asp:Label ID="lblTitleDialog" runat="server" />
                                    </h5>
                                    <asp:Label ID="lblMnsjDialog" runat="server" />
                                </div>
                                <br />
                                <ol style="margin-bottom: 0px;">
                                    <asp:Repeater ID="rptSelectPadres" runat="server">
                                        <ItemTemplate>
                                            <li style="clear: both;"><%#Eval("NombreCompleto")%></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ol>
                                <br />
                                <h4 style="font-weight: bold;" class="text-center">¿Esta seguro que desea continuar?</h4>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnSi" OnClick="btnSi_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> SI
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnNo" class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> NO
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
        function showModalDialog() {
            $('#ModalDialog').modal('show');
        }

        function hideModalDialog() {
            $('#ModalDialog').modal('hide');
        }
    </script>
    <script>
        function showModalDesasociar() {
            $('#ModalDesasociar').modal('show');
        }

        function hideModalDesasociar() {
            $('#ModalDesasociar').modal('hide');
        }
    </script>
    <script>
        function hideDialogDesasociar() {
            $('#ModalDialog').modal('hide');
            $('#ModalDesasociar').modal('hide');
        }
    </script>
</asp:Content>
