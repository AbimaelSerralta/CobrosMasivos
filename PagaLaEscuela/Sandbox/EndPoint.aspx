<%@ Page Title="" Language="C#" MasterPageFile="~/Sandbox/Sandbox.Master" AutoEventWireup="true" CodeBehind="EndPoint.aspx.cs" Inherits="PagaLaEscuela.Sandbox.EndPoint" %>

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

    <div class="content">
        <div class="container-fluid">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="card card-nav-tabs">
                            <div class="card-header card-header-primary" style="background: #b9504c;">
                                <div class="nav-tabs-navigation">
                                    <div class="nav-tabs-wrapper">
                                        <ul class="nav nav-tabs" data-tabs="tabs">
                                            <li id="liActivarComercios" class="nav-item" runat="server">
                                                <asp:LinkButton ID="btnActivarComercios" OnClick="btnActivarComercios_Click" CssClass="nav-link active show" runat="server">
                                                    <i class="material-icons">message</i>Comercios<div class="ripple-container"></div>
                                                </asp:LinkButton>
                                            </li>

                                            <li id="liActivarPagosEnlinea" class="nav-item" runat="server">
                                                <asp:LinkButton ID="btnActivarPagosEnlinea" OnClick="btnActivarPagosEnlinea_Click" CssClass="nav-link" runat="server">
                                                    <i class="material-icons">payment</i>Pago En linea<div class="ripple-container"></div>
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
                                            <asp:Panel ID="pnlAlertMnsjEstatus" Visible="false" runat="server">
                                                <div id="divAlertMnsjEstatus" class="alert alert-danger alert-dismissible fade" role="alert" runat="server">
                                                    <asp:Label runat="server" ID="lblMnsjEstatus" />
                                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <asp:Panel ID="pnlActivarComercios" Visible="true" runat="server">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="margin-bottom: 20px;">
                                                    <div class="col-md-12">
                                                        <label for="txtEntregarReferencia" style="color: black;">Entregar referencia</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text">
                                                                    <i class="material-icons">article</i>
                                                                </span>
                                                            </div>
                                                            <asp:TextBox ID="txtEntregarReferencia" CssClass="form-control" runat="server" />
                                                            <asp:Label ID="UidEntregarReferencia" Visible="false" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label for="txtConsultarReferencia" style="color: black;">Consultar referencia</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text">
                                                                    <i class="material-icons">request_page</i>
                                                                </span>
                                                            </div>
                                                            <asp:TextBox ID="txtConsultarReferencia" CssClass="form-control" runat="server" />
                                                            <asp:Label ID="UidConsultarReferencia" Visible="false" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label for="txtPagarReferencia" style="color: black;">Pagar referencia</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text">
                                                                    <i class="material-icons">attach_money</i>
                                                                </span>
                                                            </div>
                                                            <asp:TextBox ID="txtPagarReferencia" CssClass="form-control" runat="server" />
                                                            <asp:Label ID="UidPagarReferencia" Visible="false" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label for="txtCancelarReferencia" style="color: black;">Cancelar pago</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text">
                                                                    <i class="material-icons">money_off</i>
                                                                </span>
                                                            </div>
                                                            <asp:TextBox ID="txtCancelarReferencia" CssClass="form-control" runat="server" />
                                                            <asp:Label ID="UidCancelarReferencia" Visible="false" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <div class="modal-footer justify-content-center">
                                                            <asp:LinkButton ID="btnGuardarClubPago" OnClick="btnGuardarClubPago_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlActivarPagosEnlinea" Visible="false" runat="server">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="row" style="margin-bottom: 20px;">
                                                    <div class="col-md-12">
                                                        <label for="txtEntregarLiga" style="color: black;">Entregar liga</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text">
                                                                    <i class="material-icons">article</i>
                                                                </span>
                                                            </div>
                                                            <asp:TextBox ID="txtEntregarLiga" CssClass="form-control" runat="server" />
                                                            <asp:Label ID="UidEntregarLiga" Visible="false" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <label for="txtPagarLiga" style="color: black;">Pagar liga</label>
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text">
                                                                    <i class="material-icons">attach_money</i>
                                                                </span>
                                                            </div>
                                                            <asp:TextBox ID="txtPagarLiga" CssClass="form-control" runat="server" />
                                                            <asp:Label ID="UidPagarLiga" Visible="false" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <div class="modal-footer justify-content-center">
                                                            <asp:LinkButton ID="btnGuardarPraga" OnClick="btnGuardarPraga_Click" CssClass="btn btn-success btn-round" runat="server">
                                                                <i class="material-icons">check</i> Guardar
                                                            </asp:LinkButton>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
</asp:Content>
