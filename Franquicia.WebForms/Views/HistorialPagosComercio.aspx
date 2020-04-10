<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="HistorialPagosComercio.aspx.cs" Inherits="Franquicia.WebForms.Views.HistorialPagosComercio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
                                                <asp:LinkButton ID="btnFiltros" ToolTip="Filtros de busqueda." BackColor="#4db6ac" class="btn btn-lg btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">search</i>
                                                </asp:LinkButton>
                                                <asp:Label Text="Historial de pagos" runat="server" />

                                                <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" ToolTip="Nueva recarga." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">add</i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvHistorial" DataKeyNames="UidHistorial" AutoGenerateColumns="false" CssClass="table table-hover" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">Su historial esta vacio</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField SortExpression="NombreCompleto" DataField="NombreCompleto" HeaderText="NOMBRE COMPLETO" />
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
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" runat="server">
                                <asp:Label ID="lblTituloModal" Text="Selección" runat="server" /></h5>
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
                                <asp:Label ID="lblValidar" runat="server" />
                                <asp:Panel ID="pnlSeleccion" runat="server">
                                    <div class="row">
                                        <asp:ListView ID="lvTarifa" runat="server">
                                            <ItemTemplate>

                                                <div class="col-12 col-sm-6 col-md-6 col-lg-6">
                                                    <div class="card card-stats">
                                                        <div class="card-header card-header-success card-header-icon">
                                                            <div class="card-icon">
                                                                <img class="card-img-top" style="height: 50px; width: 50px" src="https://is5-ssl.mzstatic.com/image/thumb/Purple113/v4/b8/d8/7a/b8d87ae1-b9df-1cb6-bbce-c71932ac9ce7/AppIcon-0-0-1x_U007emarketing-0-0-0-6-0-0-85-220.png/246x0w.png" alt="whatsapp">
                                                            </div>
                                                            <p class="card-category">Whatsapp</p>
                                                            <h3 class="card-title">
                                                                <asp:Label ID="lblDcmWhatsapp" runat="server"> <%# Eval("DcmWhatsapp", "{0:C}") %></asp:Label></h3>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-12 col-sm-6 col-md-6 col-lg-6">
                                                    <div class="card card-stats">
                                                        <div class="card-header card-header-warning card-header-icon">
                                                            <div class="card-icon">
                                                                <img class="card-img-top" style="height: 50px; width: 50px" src="https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcTZ5jK9GXD0SBrKIlY3X4RuVYp9xgvF1dpHPyvGmaYwq3bVXpWg&usqp=CAU" alt="sms">
                                                            </div>
                                                            <p class="card-category">Sms</p>
                                                            <h3 class="card-title"><%# Eval("DcmSms", "{0:C}") %></h3>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </div>
                                    <asp:Panel ID="pnlForm" runat="server">
                                        <div class="row justify-content-center align-items-center h-100">
                                            <div class="col col-sm-12 col-md-12 col-lg-8 col-xl-8">
                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                    <label for="txtIdentificador" style="color: black;">Identificador</label>
                                                    <asp:TextBox ID="txtIdentificador" Enabled="false" Text="Recarga" CssClass="form-control" runat="server" />
                                                </div>
                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                    <label for="txtConcepto" style="color: black;">Concepto</label>
                                                    <asp:TextBox ID="txtConcepto" Enabled="false" Text="Recarga para Whatsapp y sms" CssClass="form-control" runat="server" />
                                                </div>
                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                    <label for="txtVencimiento" style="color: black;">Vencimiento</label>
                                                    <asp:TextBox ID="txtVencimiento" Enabled="false" TextMode="Date" CssClass="form-control" runat="server" />
                                                </div>
                                                <div class="form-group col-md-12" style="padding-left: 0px;">
                                                    <label for="txtImporte" style="color: black;">Importe *</label>
                                                    <div class="input-group">
                                                        <div class="input-group-prepend">
                                                            <span class="input-group-text" style="padding-left: 0px; padding-right: 5px;">
                                                                <i class="material-icons">$</i>
                                                            </span>
                                                        </div>
                                                        <asp:TextBox ID="txtImporte" CssClass="form-control" TextMode="Phone" runat="server" />
                                                    </div>

                                                    <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtImporte" runat="server" />
                                                </div>

                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>

                                <asp:Panel ID="pnlIframe" Visible="false" runat="server">
                                    <div class="row">
                                        <div style="width: 100%;">
                                            <iframe id="ifrLiga" style="width: 80%; margin: 0 auto; display: block;" width="450px" height="479px" class="centrado" src="https://u.mitec.com.mx/p/i/EHXYPEKR" frameborder="0" seamless="seamless" runat="server"></iframe>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>

                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upRegistro">
                    <ContentTemplate>
                        <div class="modal-footer justify-content-center">
                            <asp:LinkButton ID="btnGenerar" OnClick="btnGenerar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">link</i> Generar liga
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnCancelar" class="close" data-dismiss="modal" aria-label="Close" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGenerar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
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
                            <asp:LinkButton ID="btnBuscar" CssClass="btn btn-primary btn-round" runat="server">
                            <i class="material-icons">search</i> Buscar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpiar" CssClass="btn btn-warning btn-round" runat="server">
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
            $('#ModalNuevo').modal({ backdrop: 'static', keyboard: false, show: true });
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
