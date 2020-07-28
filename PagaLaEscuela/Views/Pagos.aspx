<%@ Page Title="Pagos" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Pagos.aspx.cs" Inherits="PagaLaEscuela.Views.Pagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">
    <style>
        .cardEfe {
            transition: .5s;
        }

            .cardEfe:hover {
                transform: scale(1.05);
            }
    </style>


    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlComercios" runat="server">
                <div class="row">
                    <asp:Repeater ID="rpComercios" OnItemCommand="rpComercios_ItemCommand" runat="server">
                        <ItemTemplate>
                            <div class="col-lg-3 col-md-6 mb-4">
                                <div class="card cardEfe h-100">
                                    <img class="card-img-top" src="https://placehold.it/500x325" alt="">
                                    <div class="card-body">
                                        <h4 class="card-title text-center" style="font-weight: bold;">
                                            <%#Eval("VchNombreComercial")%>
                                        </h4>
                                        <p class="card-text">
                                            <%#Eval("Direccion")%>
                                            <br />
                                            C.P: <%#Eval("CodigoPostal")%>
                                            <br />
                                            Cel: <%#Eval("VchTelefono")%>
                                        </p>
                                    </div>
                                    <div class="card-footer">
                                        <asp:LinkButton ID="btnIrPagos" CssClass="pull-center" ToolTip="Editar" CommandArgument='<%#Eval("UidCliente")%>' CommandName="Pagos" runat="server">
                                            <asp:Label ID="Label1" class="btn btn-success btn-round" runat="server">
                                        <i class="material-icons">attach_money</i> Pagar
                                            </asp:Label>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPagos" Visible="false" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header card-header-primary" style="background: #00bcd4;">
                                <div class="row">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 20%">
                                                <asp:LinkButton ID="btnRegresar" OnClick="btnRegresar_Click" ToolTip="Editar" runat="server">
                                                    <asp:Label ForeColor="White" runat="server">
                                                        <i class="material-icons">arrow_back</i> Regresar
                                                    </asp:Label>
                                                </asp:LinkButton>
                                            </td>
                                            <td colspan="2" style="width:50%">
                                                <div style="width: 100%;">
                                                    <img src="https://paraisoslatinos.files.wordpress.com/2017/06/fotos-filas-de-moais-chile-isla-pascua-500x3251.jpg" style="width: 30%; margin: 0 auto; display: block;" height="100" width="100" class="img-fluid align-items-center" alt="Responsive image">
                                                </div>
                                                <h4 class="card-title text-center">Nombre Comercio</h4>
                                            </td>

                                            <td style="width: 30%">
                                                <asp:LinkButton ID="btnActualizarLista" ToolTip="Actualizar tabla." class="btn btn-lg btn-success btn-fab btn-fab-mini btn-round pull-right" runat="server">
                                                        <i class="material-icons">sync</i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table">
                                        <thead class=" text-primary">
                                            <tr>
                                                <th>ID
                                                </th>
                                                <th>Name
                                                </th>
                                                <th>Country
                                                </th>
                                                <th>City
                                                </th>
                                                <th>Salary
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>1
                                                </td>
                                                <td>Dakota Rice
                                                </td>
                                                <td>Niger
                                                </td>
                                                <td>Oud-Turnhout
                                                </td>
                                                <td class="text-primary">$36,738
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>2
                                                </td>
                                                <td>Minerva Hooper
                                                </td>
                                                <td>Curaçao
                                                </td>
                                                <td>Sinaai-Waas
                                                </td>
                                                <td class="text-primary">$23,789
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>3
                                                </td>
                                                <td>Sage Rodriguez
                                                </td>
                                                <td>Netherlands
                                                </td>
                                                <td>Baileux
                                                </td>
                                                <td class="text-primary">$56,142
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>4
                                                </td>
                                                <td>Philip Chaney
                                                </td>
                                                <td>Korea, South
                                                </td>
                                                <td>Overland Park
                                                </td>
                                                <td class="text-primary">$38,735
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>5
                                                </td>
                                                <td>Doris Greene
                                                </td>
                                                <td>Malawi
                                                </td>
                                                <td>Feldkirchen in Kärnten
                                                </td>
                                                <td class="text-primary">$63,542
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>6
                                                </td>
                                                <td>Mason Porter
                                                </td>
                                                <td>Chile
                                                </td>
                                                <td>Gloucester
                                                </td>
                                                <td class="text-primary">$78,615
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cBodyBottom" runat="server">
</asp:Content>
