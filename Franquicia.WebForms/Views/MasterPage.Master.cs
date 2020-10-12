using AjaxControlToolkit;
using Franquicia.Bussiness;
using Franquicia.WebForms.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        #region Propiedades
        // Nombre del servidor
        string ServerName
        {
            get { return Request.ServerVariables["SERVER_NAME"].ToString(); }
        }
        // Puerto del servidor
        string ServerPort
        {
            get { return Request.ServerVariables["SERVER_PORT"].ToString(); }
        }

        // Obtener URL Base
        public string URLBase
        {
            get
            {
                if (ServerPort != string.Empty && ServerPort.Trim() != "")
                {
                    return "https://" + ServerName + ":" + ServerPort + "/";
                }
                else
                {
                    return "https://" + ServerName + "/";
                }
            }
        }

        ManejoSesionServices manejoSesionServices { get; set; }
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["URLBase"] = URLBase;

            #region Validaciones iniciales antes de cargar la página

            // Verificar que exista el manejador de la sesión
            if (Session["manejoSesionServices"] != null)
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Obtener administrador de la sesión
                manejoSesionServices = (ManejoSesionServices)Session["manejoSesionServices"];
                // Verificar el status de la sesión
                if (manejoSesionServices.BolStatusSesion)
                {
                    if (!IsPostBack)
                    {
                        manejoSesionServices.perfilesRepository.appWebRepository.ObtenerAppWeb(manejoSesionServices.perfilesRepository.perfiles.UidAppWeb);
                        manejoSesionServices.ObtenerAppWeb();

                        //manejoSesionServices.CargarMenu(manejoSesionServices.perfilesRepository.appWebRepository.appWeb.UidAppWeb);
                        //dlMenu.DataSource = manejoSesionServices.lsmodulos;
                        //dlMenu.DataBind();

                        manejoSesionServices.CargarAccesosPermitidos(manejoSesionServices.perfilesRepository.perfiles.UidSegPerfil);
                        Session["lsAccesosPermitidos"] = manejoSesionServices.lsAccesosPermitidos;

                        ValidarAcceso();

                        //manejoSesionServices.ObtenerEmpresasUsuarios();
                        //lblNombreComercial.Text = manejoSesionServices.CUsuario.EEmpresas.StrNombreComercial;

                        if (manejoSesionServices.perfilesRepository.appWebRepository.appWeb.IntGerarquia == 1)
                        {
                            lblTitleMenu.Text = "Panel Administrativo";

                            ViewState["ColorSide"] = "#b9504c";

                            dlMenu.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("514433C7-4439-42F5-ABE4-6BF1C330F0CA")).ToList().OrderBy(x => x.IntGerarquia);
                            dlMenu.DataBind();

                            dlSubMenuFranquicia.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("6d70f88d-3ce0-4c8b-87a1-92666039f5b2")).ToList().OrderBy(x => x.IntGerarquia);
                            dlSubMenuFranquicia.DataBind();

                            dlSubMenuCliente.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("0D910772-AE62-467A-A7A3-79540F0445CB")).ToList().OrderBy(x => x.IntGerarquia);
                            dlSubMenuCliente.DataBind();

                            if (Session["NombreComercial"] == null)
                            {
                                Session["UidFranquiciaMaster"] = Guid.Empty;
                                aMenuFranquicia.Attributes.Add("class", "dropdown-toggle disabled");
                                lblNombreComercial.Text = "&nbsp;seleccione una franquicia";
                            }
                            else if (Session["NombreComercial"] != null)
                            {
                                aMenuFranquicia.Attributes.Add("class", "dropdown-toggle font-weight-bold");
                                lblNombreComercial.Text = Session["NombreComercial"].ToString();
                            }

                            if (Session["NombreClienteMaster"] == null)
                            {
                                Session["UidClienteMaster"] = Guid.Empty;
                                aMenuCliente.Attributes.Add("class", "dropdown-toggle disabled");
                                lblDescripcionCliente.Text = "&nbsp;seleccione un comercio";
                            }
                            else if (Session["NombreClienteMaster"] != null)
                            {
                                aMenuCliente.Attributes.Add("class", "dropdown-toggle font-weight-bold");
                                lblDescripcionCliente.Text = Session["NombreClienteMaster"].ToString();
                            }
                        }

                        else if (manejoSesionServices.perfilesRepository.appWebRepository.appWeb.IntGerarquia == 2)
                        {
                            lblTitleMenu.Text = "Panel Franquicia";

                            ViewState["ColorSide"] = "#024693";

                            lblGvSaldo.Text = "Saldo: $0.00";

                            liMenuFranquicia.Visible = false;
                            dlMenu.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("6d70f88d-3ce0-4c8b-87a1-92666039f5b2")).ToList().OrderBy(x => x.IntGerarquia);
                            dlMenu.DataBind();

                            dlSubMenuCliente.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("0D910772-AE62-467A-A7A3-79540F0445CB")).ToList().OrderBy(x => x.IntGerarquia);
                            dlSubMenuCliente.DataBind();

                            manejoSesionServices.ObtenerFranquiciaUsuario();

                            lblNombreComercial.Text = "<b>FRANQUICIA:</b>&nbsp;" + manejoSesionServices.usuarioCompletoRepository.franquiciatarios.VchNombreComercial;

                            if (Session["NombreClienteMaster"] == null)
                            {
                                Session["NombreComercial"] = lblNombreComercial.Text;
                                Session["UidFranquiciaMaster"] = manejoSesionServices.usuarioCompletoRepository.franquiciatarios.UidFranquiciatarios;
                                lblDescripcionCliente.Text = "&nbsp;seleccione un comercio";
                                aMenuCliente.Attributes.Add("class", "dropdown-toggle disabled");

                                Session["UidClienteMaster"] = Guid.Empty;
                            }
                            else if (Session["NombreClienteMaster"] != null)
                            {
                                lblDescripcionCliente.Text = "&nbsp;" + Session["NombreClienteMaster"].ToString();
                                //string DescripcionSucursal = Session["DescripcionSucursal"].ToString();
                                //string Identificador = Session["Identificador"].ToString();
                                //string IdSucursal = Session["IdSucursal"].ToString();
                            }
                        }

                        else if (manejoSesionServices.perfilesRepository.appWebRepository.appWeb.IntGerarquia == 3)
                        {
                            lblTitleMenu.Text = "Panel Comercio";

                            ViewState["ColorSide"] = "#a33eb7";

                            liMenuFranquicia.Visible = false;
                            liMenuCliente.Visible = false;

                            dlMenu.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("0D910772-AE62-467A-A7A3-79540F0445CB")).ToList().OrderBy(x => x.IntGerarquia);
                            dlMenu.DataBind();

                            manejoSesionServices.ObtenerFranquiciaClienteUsuario();
                            lblNombreComercial.Text = "<b>FRANQUICIA:</b>&nbsp;" + manejoSesionServices.usuarioCompletoRepository.franquiciatarios.VchNombreComercial;
                            lblDescripcionCliente.Text = "&nbsp;<b>COMERCIO:</b>&nbsp;" + manejoSesionServices.usuarioCompletoRepository.clientes.VchNombreComercial;
                            Session["UidClienteMaster"] = manejoSesionServices.usuarioCompletoRepository.clientes.UidCliente;
                            Session["UidUsuarioMaster"] = manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidUsuario;
                            
                            manejoSesionServices.clienteCuentaRepository.ObtenerDineroCuentaCliente(manejoSesionServices.usuarioCompletoRepository.clientes.UidCliente);
                            lblGvSaldo.Text = "Saldo: $" + manejoSesionServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");
                        }
                        else if (manejoSesionServices.perfilesRepository.appWebRepository.appWeb.IntGerarquia == 4)
                        {
                            lblTitleMenu.Text = "Panel Usuario";

                            ViewState["ColorSide"] = "#5b3c64";

                            liMenuFranquicia.Visible = false;
                            liMenuCliente.Visible = false;

                            dlMenu.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("9C8AD059-A37B-42EE-BF37-FEB7ACA84088")).ToList();
                            dlMenu.DataBind();

                            //manejoSesionServices.ObtenerFranquiciaClienteUsuario();
                            //lblNombreComercial.Text = "<b>FRANQUICIA:</b>&nbsp;" + manejoSesionServices.usuarioCompletoRepository.franquiciatarios.VchNombreComercial;
                            //lblDescripcionCliente.Text = "&nbsp;<b>CLIENTE:</b>&nbsp;" + manejoSesionServices.usuarioCompletoRepository.clientes.VchNombreComercial;

                            manejoSesionServices.ObtenerFranquiciaClienteUsuario();
                            Session["UidUsuarioMaster"] = manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidUsuario;

                            PrefijosTelefonicosServices prefijosTelefonicosServices = new PrefijosTelefonicosServices();

                            prefijosTelefonicosServices.CargarPrefijosTelefonicos();
                            ddlPrefijo.DataSource = prefijosTelefonicosServices.lsPrefijosTelefonicos;
                            ddlPrefijo.DataTextField = "VchCompleto";
                            ddlPrefijo.DataValueField = "UidPrefijo";
                            ddlPrefijo.DataBind();

                            bool nuevo = false;

                            foreach (var item in usuariosCompletosServices.ObtenerDatossUsuario(manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidUsuario))
                            {
                                txtNombres.Text = item.StrNombre;
                                txtApePaterno.Text = item.StrApePaterno;
                                txtApeMaterno.Text = item.StrApeMaterno;
                                ddlPrefijo.SelectedIndex = ddlPrefijo.Items.IndexOf(ddlPrefijo.Items.FindByValue(item.UidPrefijo.ToString()));
                                txtTelefono.Text = item.StrTelefono;

                                if (item.UidEstatusCuenta.ToString().ToUpper() == "5E64EF92-5A3E-4C9A-91A4-AC5523D3587C")
                                {
                                    nuevo = true;
                                }
                            }

                            if (nuevo)
                            {
                                mpeActivarUsuario.Show();
                            }
                        }
                    }
                    else
                    {
                    }
                    LblNombreUsuario.Text = manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.StrNombre + " " + manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.StrApePaterno;
                    //Session["UiIdEmpleadoPerfilUsuario"] = manejoSesionServices.CUsuario.UiIdEmpleado;
                    //Session["AppWebGerarquia"] = manejoSesionServices.CPerfil.CAppWeb.STRGerarquia;
                    //LblNombreEmpresa.Text = manejoSesionServices.CUsuario.CEmpresa.NombreEmpresa;
                    //LblNombreEmpresaHeader.Text = manejoSesionServices.CUsuario.CEmpresa.NombreEmpresa;
                    lblversion.Text = "Versión:&nbsp;" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    lblFecha.Text = DateTime.Today.ToLongDateString();

                }
                else
                {
                    Response.Redirect(URLBase + "Views/Login.aspx");
                }
            }
            else
            {
                Response.Redirect(URLBase + "Views/Login.aspx");
            }
            #endregion Validaciones iniciales antes de cargar la página
        }

        public Label GvSaldo
        {
            get { return lblGvSaldo; }
            set { lblGvSaldo = value; }
        }
        public Label NombreComercial
        {
            get { return lblNombreComercial; }
            set { lblNombreComercial = value; }
        }
        public Label NombreCliente
        {
            get { return lblDescripcionCliente; }
            set { lblDescripcionCliente = value; }
        }

        public HtmlAnchor MenuFranquicia
        {
            get { return aMenuFranquicia; }
            set { aMenuFranquicia = value; }
        }
        public HtmlAnchor MenuCliente
        {
            get { return aMenuCliente; }
            set { aMenuCliente = value; }
        }

        public ModalPopupExtender Modal
        {
            get { return MPEAbrirModal; }
            set { MPEAbrirModal = value; }
        }
        public PlaceHolder Caja
        {
            get { return Conte; }
            set { Conte = value; }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            Response.Write("Button Clicked");
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        public void ValidarAcceso()
        {
            string UrlLocal = Request.Url.LocalPath;
            string Url = UrlLocal.Replace("/Views/", "");

            //ActivarMenu(Url);

            if (!manejoSesionServices.lsAccesosPermitidos.Exists(x => x.VchUrl == Url))
            {
                //Response.Redirect(manejoSesionServices.lsmodulos[1].VchUrl);
                if (manejoSesionServices.lsAccesosPermitidos.Exists(x => x.Lectura == true))
                {
                    int i = manejoSesionServices.lsAccesosPermitidos.IndexOf(manejoSesionServices.lsAccesosPermitidos.First(x => x.Lectura == true));
                    Response.Redirect(manejoSesionServices.lsAccesosPermitidos[i].VchUrl);
                }
                else
                {
                    Session.Abandon();
                    Response.Write("<script language='javascript'>window.alert('El acceso ha sido exitoso, pero no hemos encontrado modulos activos, por favor contacte al administrador del sistema.');window.location='Login.aspx';</script>");
                }
            }
            else
            {
                int f = manejoSesionServices.lsAccesosPermitidos.IndexOf(manejoSesionServices.lsAccesosPermitidos.Find(x => x.VchUrl == Url));

                if (!manejoSesionServices.lsAccesosPermitidos[f].Lectura)
                {
                    foreach (var item in manejoSesionServices.lsAccesosPermitidos)
                    {
                        if (manejoSesionServices.lsAccesosPermitidos.Exists(x => x.Lectura == true))
                        {
                            int i = manejoSesionServices.lsAccesosPermitidos.IndexOf(manejoSesionServices.lsAccesosPermitidos.First(x => x.Lectura == true));
                            Response.Redirect(manejoSesionServices.lsAccesosPermitidos[i].VchUrl);
                        }
                        else
                        {
                            Session.Abandon();
                            Response.Write("<script language='javascript'>window.alert('El acceso ha sido exitoso, pero no hemos encontrado modulos activos, por favor contacte al administrador del sistema.');window.location='Login.aspx';</script>");
                        }
                    }
                }
            }
        }

        protected void dlMenu_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            string UrlLocal = Request.Url.LocalPath;
            string Url = UrlLocal.Replace("/Views/", "");

            Label lblUrl = (Label)e.Item.FindControl("lblUrl");
            HtmlGenericControl liMenuActivar = (HtmlGenericControl)e.Item.FindControl("liMenuActivar");
            HtmlAnchor aActive = (HtmlAnchor)e.Item.FindControl("aActive");

            if (Url == lblUrl.Text)
            {
                aActive.Attributes.Add("style", "background-color:" + ViewState["ColorSide"].ToString());
                liMenuActivar.Attributes.Add("class", "nav-item active");
            }

        }

        protected void btnComprar_Click(object sender, EventArgs e)
        {
            TarifasServices tarifasServices = new TarifasServices();

            tarifasServices.CargarTarifas();
            lvTarifa.DataSource = tarifasServices.lsTarifasGridViewModel;
            lvTarifa.DataBind();

            if (tarifasServices.lsTarifasGridViewModel.Count >= 1)
            {
                foreach (var item in tarifasServices.lsTarifasGridViewModel)
                {
                    lblPrecioWhats.Text = item.DcmWhatsapp.ToString("N2");
                    lblPrecioSms.Text = item.DcmSms.ToString("N2");
                }
            }

            lblTituloModal.Text = "<strong>Paso 1</strong> cantidad a comprar.";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalMasDeta", "$('#ModalMasDeta').modal();", true);
            
        }
        public void Calcular()
        {
            decimal totalWhats = decimal.Parse(txtCantWhats.Text) * decimal.Parse(lblPrecioWhats.Text);

            lblTotalWhats.Text = totalWhats.ToString("N2");

            decimal totalSms = decimal.Parse(txtCantSms.Text) * decimal.Parse(lblPrecioSms.Text);

            lblTotalSms.Text = totalSms.ToString("N2");

            decimal sum = totalWhats + totalSms;

            lblSumTotal.Text = sum.ToString("N2");
        }
        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            Calcular();
        }
        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (pnlSeleccion.Visible == true)
            {
                lblTituloModal.Text = "<strong>Paso 2</strong> resumen de compra.";
                

                Calcular();

                pnlSeleccion.Visible = false;
                pnlResumen.Visible = true;
                pnlIframe.Visible = false;

                lblResumenTotal.Text = "Total: $" + lblSumTotal.Text;

                lblResumenDesc.Text = "Usted va a comprar " + txtCantWhats.Text + " de Whatsapp y " + txtCantSms.Text + " de Sms.";
            }
            else if (pnlResumen.Visible == true)
            {
                lblTituloModal.Text = "<strong>Paso 3</strong> pagar compra.";

                //lblUrl.Text = "www.goParkix.net";

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Mu", "myFunction()", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalMata", "myFunction()", true);

                pnlSeleccion.Visible = false;
                pnlResumen.Visible = false;
                pnlIframe.Visible = true;
            }
            else if (pnlIframe.Visible == false)
            {

            }
        }
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            if (pnlResumen.Visible == true)
            {
                lblTituloModal.Text = "<strong>Paso 1</strong> cantidad a comprar.";

                pnlSeleccion.Visible = true;
                pnlResumen.Visible = false;
                pnlIframe.Visible = false;
            }
            else if (pnlIframe.Visible == true)
            {
                lblTituloModal.Text = "<strong>Paso 2</strong> resumen de compra.";

                pnlSeleccion.Visible = false;
                pnlResumen.Visible = true;
                pnlIframe.Visible = false;
            }
        }
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            if (myDIV.Attributes.CssStyle.Value == "sidebar-mini")
            {
                myDIV.Attributes.Add("class", "");
            }
            else
            {
                myDIV.Attributes.Add("class", "sidebar-mini");
            }
        }

        protected void btnActivarCuenta_Click(object sender, EventArgs e)
        {
            if (txtPassword.EmptyTextBox())
            {
                lblValidar.Text = "El campo Contraseña es obligatorio";
                return;
            }
            if (txtRepetir.EmptyTextBox())
            {
                lblValidar.Text = "El campo Repetir Contraseña es obligatorio";
                return;
            }

            if (txtPassword.Text != txtRepetir.Text)
            {
                lblValidar.Text = "Las contraseñas ingresadas no coinciden.";
                return;
            }
            if (txtNombres.EmptyTextBox())
            {
                lblValidar.Text = "El campo Nombre es obligatorio";
                return;
            }

            if (txtApePaterno.EmptyTextBox())
            {
                lblValidar.Text = "El campo Apellido Paterno es obligatorio";
                return;
            }

            if (txtApeMaterno.EmptyTextBox())
            {
                lblValidar.Text = "El campo Apellido Materno es obligatorio";
                return;
            }

            if (txtTelefono.EmptyTextBox())
            {
                lblValidar.Text = "El campo Celular es obligatorio";
                return;
            }
            if (!string.IsNullOrEmpty(lblValidar.Text))
            {
                lblValidar.Text = string.Empty;
            }

            Guid UidUsuario = manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidUsuario;

            if (usuariosCompletosServices.ActivarCuentaUsuario(UidUsuario, txtNombres.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtPassword.Text, txtTelefono.Text, Guid.Parse(ddlPrefijo.SelectedValue)))
            {
                pnlEnProceso.Visible = false;
                pnlActivado.Visible = true;
            }
            else
            {
                pnlEnProceso.Visible = true;
                pnlActivado.Visible = false;
                lblValidar.Text = "Ocurrio un problema inesperado, por favor intentelo más tarde, si el problema persiste comuniquese con los administradores.";
            }
        }
        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            mpeActivarUsuario.Hide();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
        protected void btnAceptarActivacion_Click(object sender, EventArgs e)
        {
            mpeActivarUsuario.Hide();
        }

        protected void btnMiPerfil_Click(object sender, EventArgs e)
        {
            //mpeActivarUsuario.Show();
        }
    }
}