<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Promociones.aspx.cs" Inherits="Franquicia.WebForms.Views.Promociones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../bootstrap4.0.0/css/bootstrap.min.css" rel="stylesheet" />

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <title>Promociones</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="pricing-header px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
            <h1 class="display-4">¡Lo sentimos!</h1>
            <p class="lead"><asp:Label ID="lblMnsj" runat="server" /></p>
        </div>
        <div class="container">
            <div class="card-deck mb-3 text-center">
                <div class="form-group col-sm-12">
                    <div class="row">
                        <asp:Repeater ID="rpPromociones" runat="server">
                            <ItemTemplate>
                                <div class="form-group col-sm-4">
                                    <div class="card mb-4 box-shadow">
                                        <div class="card-header">
                                            <h4 class="my-0 font-weight-normal"><%# Eval("VchDescripcion") %></h4>
                                        </div>
                                        <div class="card-body">
                                            <h1 class="card-title pricing-card-title">$<%# Eval("DcmImporte") %></h1>
                                            <ul class="list-unstyled mt-3 mb-4">
                                                <li><%# Eval("IdReferencia") %></li>
                                                <li><%# Eval("VchConcepto") %></li>
                                            </ul>
                                            <asp:LinkButton href='<%# Eval("VchUrl") %>' class="btn btn-lg btn-block btn-outline-primary" Text="Pagar" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:DataList ID="dtPromociones" Style="width: 100%;" RepeatDirection="Horizontal" runat="server">
                            <ItemTemplate>
                                
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>


            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
</body>
</html>
