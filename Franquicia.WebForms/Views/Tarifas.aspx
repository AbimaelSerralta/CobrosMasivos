<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Tarifas.aspx.cs" Inherits="Franquicia.WebForms.Views.Tarifas" %>

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
                                <div class="card-header card-header-tabs card-header-primary" style="background: #b9504c; padding-top: 0px; padding-bottom: 0px;">
                                    <div class="nav-tabs-navigation">
                                        <div class="nav-tabs-wrapper">
                                            <div class="form-group">

                                                <asp:Label Text="Tarifa" runat="server" />

                                                <div class="pull-right">
                                                    <asp:LinkButton ID="btnNuevo" Visible="false" OnClick="btnNuevo_Click" class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                            <i class="material-icons">add</i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="form-group col-sm-12 col-md-12 col-lg-6">
                                            <div class="card">
                                                <div class="card-body">
                                                    <div class="row">
                                                        <div class="col-4">
                                                            <img class="card-img-top" style="height: 100px; width: 100px" src="../Images/icoWhats.png" alt="whatsapp">
                                                        </div>
                                                        <div class="col-8">
                                                            <h5 class="card-title">Whatsapp</h5>
                                                            <div class="form-group col-md-12" style="padding-left: 0px;">
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;padding-right:5px;">
                                                                            <i class="material-icons">$</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:TextBox ID="txtWhats" Text="0.00" CssClass="form-control" TextMode="Phone" runat="server" />
                                                                </div>

                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtWhats" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-sm-12 col-md-12 col-lg-6">
                                            <div class="card" style="padding-top: 0px;padding-bottom: 0px;">
                                                <div class="card-body">
                                                    <div class="row">
                                                        <div class="col-4">
                                                            <img class="card-img-top" style="height: 100px; width: 100px" src="../Images/icoSms.jpg" alt="sms">
                                                        </div>
                                                        <div class="col-8">
                                                            <h5 class="card-title">SMS</h5>
                                                            <div class="form-group col-md-12" style="padding-left: 0px;">
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px;padding-right:5px;">
                                                                            <i class="material-icons">$</i>
                                                                        </span>
                                                                    </div>
                                                                    <asp:TextBox ID="txtSms" Text="0.00" CssClass="form-control" TextMode="Phone" runat="server" />
                                                                </div>

                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtSms" runat="server" />
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
                                                <asp:LinkButton ID="btnGuardar" OnClick="btnGuardar_Click" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnCancelar" OnClick="btnCancelar_Click" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEditar" OnClick="btnEditar_Click" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">refresh</i> Editar
                                                </asp:LinkButton>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvTarifas" Visible="false" OnRowCommand="gvTarifas_RowCommand" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidTarifa" GridLines="None" border="0" runat="server">
                                                <EmptyDataTemplate>
                                                    <div class="alert alert-info">No hay tarifa</div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:ButtonField CommandName="Select" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:TemplateField SortExpression="DcmWhatsapp" HeaderText="WHATSAPP">
                                                        <ItemTemplate>
                                                            <div class="form-group col-md-12" style="padding-left: 0px; padding-right: 0px;">
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px; padding-right: 0px;">$
                                                                        </span>
                                                                    </div>
                                                                    <asp:TextBox ID="txtGvWhatsapp" CssClass="form-control" Text='<%#Eval("DcmWhatsapp", "{0:N2}")%>' Enabled="false" BorderStyle="None" runat="server" />
                                                                </div>
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtGvWhatsapp" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="DcmSms" HeaderText="SMS">
                                                        <ItemTemplate>
                                                            <div class="form-group col-md-12" style="padding-left: 0px; padding-right: 0px;">
                                                                <div class="input-group">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text" style="padding-left: 0px; padding-right: 0px;">$
                                                                        </span>
                                                                    </div>
                                                                    <asp:TextBox ID="txtGvSms" CssClass="form-control" Text='<%#Eval("DcmSms", "{0:N2}")%>' Enabled="false" BorderStyle="None" runat="server" />
                                                                </div>
                                                                <asp:FilteredTextBoxExtender FilterType="Numbers, Custom" ValidChars=".," TargetControlID="txtGvSms" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField SortExpression="UidEstatus" HeaderText="ESTATUS">
                                                        <ItemTemplate>
                                                            <div class="col-md-6">
                                                                <asp:Label ToolTip='<%#Eval("VchEstatus")%>' runat="server">
                                                                <i class="large material-icons">
                                                                    <%#Eval("VchIcono")%>
                                                                </i>
                                                                </asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tbody>
                                                                    <tr style="background: transparent;">
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnEditar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnEditar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label ID="Label1" class="btn btn-sm btn-info btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">edit</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>

                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnAceptar" Visible="false" ToolTip="Aceptar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnAceptar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-success btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">check</i>
                                                                                </asp:Label>
                                                                            </asp:LinkButton>
                                                                        </td>
                                                                        <td style="border: none; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px;">
                                                                            <asp:LinkButton ID="btnCancelar" Visible="false" ToolTip="Cancelar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="btnCancelar" Style="margin-left: 5px;" runat="server">
                                                                                <asp:Label class="btn btn-sm btn-danger btn-fab btn-fab-mini btn-round" runat="server">
                                                                                        <i class="material-icons">close</i>
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
                </div>
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

        function hideAlert() {
            $(".alert").alert('close')
        }
    </script>
</asp:Content>
