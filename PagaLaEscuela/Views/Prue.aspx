<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Prueba.Master" AutoEventWireup="true" CodeBehind="Prue.aspx.cs" Inherits="PagaLaEscuela.Views.Prue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHCaja" runat="server">

    <link href="Tabs/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="Tabs/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="Tabs/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Tabs/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="Tabs/jquery.ui.tabs.js" type="text/javascript"></script>
    <script>
        $(function () {
            $("#tabs").tabs();
        });
    </script>

    <style type="text/css">
        .Link {
            color: Blue;
            font-size: 12px;
            font-family: Arial;
        }
    </style>

    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <asp:Label ID="lblReloj" runat="server" Text="Label" Font-Names="Book Antiqua" Font-Size="XX-Large" ForeColor="#8CC800 "></asp:Label>
            <asp:Timer ID="tmrRelojInterno" runat="server" OnTick="tmrRelojInterno_Tick1" Interval="1000">
            </asp:Timer>
            <!-- End demo -->
            </ContentTemplate>
  </asp:UpdatePanel>




            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:AsyncFileUpload ID="AsyncFileUpload2" ClientIDMode="AutoID" runat="server"
                        CompleteBackColor="Lime" UploaderStyle="Modern"
                        ErrorBackColor="Red" ThrobberID="Throbber" OnUploadedComplete="FileUploadComplete"
                        UploadingBackColor="#66CCFF" />

                    <asp:Label ID="Throbber" runat="server" Style="display: none">
                <img src="../CSSPropio/loader.gif" />
                    </asp:Label>

                    <asp:FileUpload ID="fuSelecionarExcel" Style="display: none;" runat="server" />

                    <script type="text/javascript">
                        function UploadFile(fileUpload) {
                            if (fileUpload.value != '') {
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
                                                        <asp:Label Text="Listado de alumnos" runat="server" />

                                                        <div class="pull-right">
                                                            <asp:LinkButton ID="btnCargarExcel" ToolTip="Importar alumnos." class="btn btn-lg btn-ligh btn-fab btn-fab-mini btn-round" runat="server">
                                                        <i class="material-icons">file_upload</i>
                                                            </asp:LinkButton>
                                                            ||
                                                    <asp:LinkButton ID="btnExportarLista" ToolTip="Exportar alumnos." class="btn btn-lg btn-warning btn-fab btn-fab-mini btn-round" runat="server">
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
                                                    <asp:GridView ID="gvAlumnos" AllowSorting="true" AutoGenerateColumns="false" CssClass="table table-hover" DataKeyNames="UidUsuario" GridLines="None" border="0" AllowPaging="true" PageSize="10" runat="server">
                                                        <EmptyDataTemplate>
                                                            <div class="alert alert-info">No hay alumnos registrados</div>
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
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-8">
                            <div class="card">
                                <div class="card-header card-header-primary">
                                    <h4 class="card-title">Edit Profile</h4>
                                    <p class="card-category">Complete your profile</p>
                                </div>
                                <div class="card-body">
                                    <form>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Company (disabled)</label>
                                                    <input type="text" class="form-control" disabled="">
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Username</label>
                                                    <input type="text" class="form-control">
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Email address</label>
                                                    <input type="email" class="form-control">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Fist Name</label>
                                                    <input type="text" class="form-control">
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Last Name</label>
                                                    <input type="text" class="form-control">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Adress</label>
                                                    <input type="text" class="form-control">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">City</label>
                                                    <input type="text" class="form-control">
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Country</label>
                                                    <input type="text" class="form-control">
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group bmd-form-group">
                                                    <label class="bmd-label-floating">Postal Code</label>
                                                    <input type="text" class="form-control">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>About Me</label>
                                                    <div class="form-group bmd-form-group">
                                                        <label class="bmd-label-floating">Lamborghini Mercy, Your chick she so thirsty, I'm in that two seat Lambo.</label>
                                                        <textarea class="form-control" rows="5"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <button type="submit" class="btn btn-primary pull-right">Update Profile</button>
                                        <div class="clearfix"></div>
                                    </form>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="card card-profile">
                                <div class="card-avatar">
                                    <a href="javascript:;">
                                        <img class="img" src="../assets/img/faces/marc.jpg">
                                    </a>
                                </div>
                                <div class="card-body">
                                    <h6 class="card-category text-gray">CEO / Co-Founder</h6>
                                    <h4 class="card-title">Alec Thompson</h4>
                                    <p class="card-description">
                                        Don't be scared of the truth because we need to restart the human foundation in truth And I love you like Kanye loves Kanye I love Rick Owens’ bed design but the back is...
                                    </p>
                                    <a href="javascript:;" class="btn btn-primary btn-round">Follow</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="form-group">
                <label for="exampleFormControlInput1">Email address</label>
                <input type="email" class="form-control" id="exampleFormControlInput1" placeholder="name@example.com">
            </div>
            <div class="form-group">
                <label for="exampleFormControlSelect1">Example select</label>
                <select class="form-control selectpicker" id="exampleFormControlSelect1">
                    <option>1</option>
                    <option>2</option>
                    <option>3</option>
                    <option>4</option>
                    <option>5</option>
                </select>
            </div>
            <div class="form-group">
                <label for="exampleFormControlSelect2">Example multiple select</label>
                <select multiple class="form-control selectpicker" data-style="btn btn-link" id="exampleFormControlSelect2">
                    <option>1</option>
                    <option>2</option>
                    <option>3</option>
                    <option>4</option>
                    <option>5</option>
                </select>
            </div>
            <div class="form-group">
                <label for="exampleFormControlTextarea1">Example textarea</label>
                <textarea class="form-control" id="exampleFormControlTextarea1" rows="3"></textarea>
            </div>

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

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>



                                        <asp:Label ID="lblUpload" runat="server" Text="hOLA"></asp:Label>
                                    </ContentTemplate>

                                </asp:UpdatePanel>

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-8">
                                                <div class="card">
                                                    <div class="card-header card-header-primary">
                                                        <h4 class="card-title">Edit Profile</h4>
                                                        <p class="card-category">Complete your profile</p>
                                                    </div>
                                                    <div class="card-body">
                                                        <form>
                                                            <div class="row">
                                                                <div class="col-md-5">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">Company (disabled)</label>
                                                                        <input type="text" class="form-control" disabled="">
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">Username</label>
                                                                        <input type="text" class="form-control">
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">Email address</label>
                                                                        <input type="email" class="form-control">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">Fist Name</label>
                                                                        <input type="text" class="form-control">
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">Last Name</label>
                                                                        <input type="text" class="form-control">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">Adress</label>
                                                                        <input type="text" class="form-control">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-4">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">City</label>
                                                                        <input type="text" class="form-control">
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">Country</label>
                                                                        <input type="text" class="form-control">
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="form-group bmd-form-group">
                                                                        <label class="bmd-label-floating">Postal Code</label>
                                                                        <input type="text" class="form-control">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="form-group">
                                                                        <label>About Me</label>
                                                                        <div class="form-group bmd-form-group">
                                                                            <label class="bmd-label-floating">Lamborghini Mercy, Your chick she so thirsty, I'm in that two seat Lambo.</label>
                                                                            <textarea class="form-control" rows="5"></textarea>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <button type="submit" class="btn btn-primary pull-right">Update Profile</button>
                                                            <div class="clearfix"></div>
                                                        </form>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="card card-profile">
                                                    <div class="card-avatar">
                                                        <a href="javascript:;">
                                                            <img class="img" src="../assets/img/faces/marc.jpg">
                                                        </a>
                                                    </div>
                                                    <div class="card-body">
                                                        <h6 class="card-category text-gray">CEO / Co-Founder</h6>
                                                        <h4 class="card-title">Alec Thompson</h4>
                                                        <p class="card-description">
                                                            Don't be scared of the truth because we need to restart the human foundation in truth And I love you like Kanye loves Kanye I love Rick Owens’ bed design but the back is...
                                                        </p>
                                                        <a href="javascript:;" class="btn btn-primary btn-round">Follow</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="form-group col-md-4">
                                                <label for="txtNombre" style="color: black;">Nombre *</label>
                                                <asp:TextBox ID="txtNombre" CssClass="form-control" required="required" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="txtApePaterno" style="color: black;">Apellido Paterno *</label>
                                                <asp:TextBox ID="txtApePaterno" CssClass="form-control" required="required" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="txtApeMaterno" style="color: black;">Apellido Materno *</label>
                                                <asp:TextBox ID="txtApeMaterno" CssClass="form-control" required="required" runat="server" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="txtMatricula" style="color: black;">Matrícula *</label>
                                                <asp:TextBox ID="txtMatricula" CssClass="form-control" required="required" runat="server" />
                                            </div>
                                            <div class="form-group col-md-8">
                                                <label for="txtCorreo" style="color: black;">Correo Eléctronico</label>
                                                <asp:TextBox ID="txtCorreo" CssClass="form-control" runat="server" />
                                                <asp:Label CssClass="text-danger" runat="server" ID="lblExiste" />
                                                <asp:Label CssClass="text-success" runat="server" ID="lblNoExiste" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="ddlPrefijo" style="color: black;">Código pais</label>
                                                <asp:DropDownList ID="ddlPrefijo" CssClass="form-control" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="txtNumero" style="color: black; margin-bottom: 13px;">Celular</label>
                                                    <asp:TextBox ID="txtNumero" TextMode="Phone" CssClass="form-control" runat="server" />
                                                    <asp:RegularExpressionValidator ID="REVNumero" runat="server" ControlToValidate="txtNumero" ErrorMessage="* Valores númericos" ForeColor="Red" ValidationExpression="^[0-9]*"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                            <div class="form-group col-md-4">
                                                <label for="ddlBeca" style="color: black;">¿Tiene beca?</label>
                                                <asp:DropDownList ID="ddlBeca" CssClass="form-control" runat="server">
                                                    <asp:ListItem Text="NO" Value="false" />
                                                    <asp:ListItem Text="SI" Value="true" />
                                                </asp:DropDownList>
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
                                            <div class="form-group col-md-4">
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
                                        <asp:LinkButton ID="btnCerrar" Visible="false" CssClass="btn btn-info btn-round" runat="server">
                            <i class="material-icons">close</i> Cerrar
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnGuardar" CssClass="btn btn-success btn-round" runat="server">
                            <i class="material-icons">check</i> Guardar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancelar" CssClass="btn btn-danger btn-round" runat="server">
                            <i class="material-icons">close</i> Cancelar
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnEditar" CssClass="btn btn-warning btn-round" runat="server">
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
