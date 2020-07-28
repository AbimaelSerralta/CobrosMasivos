﻿using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
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
                        if (Session["lblAccMenu"] != null && Session["lblAccMenu"].ToString() != "")
                        {
                            
                            myDIV.Attributes.Add("class", "sidebar-mini");
                        }
                        else
                        {
                            Session["lblAccMenu"] = "";
                            
                        }

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
                            lblTitleMenu.Text = "Panel Escuela";

                            ViewState["ColorSide"] = "#f33527";

                            liMenuFranquicia.Visible = false;
                            liMenuCliente.Visible = false;

                            dlMenu.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("0D910772-AE62-467A-A7A3-79540F0445CB")).ToList().OrderBy(x => x.IntGerarquia);
                            dlMenu.DataBind();

                            manejoSesionServices.ObtenerFranquiciaClienteUsuario();
                            //lblNombreComercial.Text = "<b>FRANQUICIA:</b>&nbsp;" + manejoSesionServices.usuarioCompletoRepository.franquiciatarios.VchNombreComercial;
                            lblDescripcionCliente.Text = "&nbsp;<b>" + manejoSesionServices.usuarioCompletoRepository.clientes.VchNombreComercial + "</b>&nbsp;";
                            Session["UidClienteMaster"] = manejoSesionServices.usuarioCompletoRepository.clientes.UidCliente;
                            Session["UidUsuarioMaster"] = manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidUsuario;

                            manejoSesionServices.clienteCuentaRepository.ObtenerDineroCuentaCliente(manejoSesionServices.usuarioCompletoRepository.clientes.UidCliente);
                            //lblGvSaldo.Text = "Saldo: $" + manejoSesionServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta.ToString("N2");
                        }
                        else if (manejoSesionServices.perfilesRepository.appWebRepository.appWeb.IntGerarquia == 4)
                        {
                            lblTitleMenu.Text = "Panel Tutor";

                            ViewState["ColorSide"] = "#00bcd4";

                            liMenuFranquicia.Visible = false;
                            liMenuCliente.Visible = false;

                            dlMenu.DataSource = manejoSesionServices.lsAccesosPermitidos.Where(x => x.UidAppWeb == new Guid("9C8AD059-A37B-42EE-BF37-FEB7ACA84088")).ToList();
                            dlMenu.DataBind();

                            //manejoSesionServices.ObtenerFranquiciaClienteUsuario();
                            //lblNombreComercial.Text = "<b>FRANQUICIA:</b>&nbsp;" + manejoSesionServices.usuarioCompletoRepository.franquiciatarios.VchNombreComercial;
                            //lblDescripcionCliente.Text = "&nbsp;<b>CLIENTE:</b>&nbsp;" + manejoSesionServices.usuarioCompletoRepository.clientes.VchNombreComercial;

                            manejoSesionServices.ObtenerFranquiciaClienteUsuario();
                            Session["UidUsuarioMaster"] = manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidUsuario;
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
                    lblversion.Text = "Versión: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
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

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            if (Session["lblAccMenu"].ToString() == "sidebar-mini")
            {
                Session["lblAccMenu"] = "";
            }
            else
            {
                Session["lblAccMenu"] = "sidebar-mini";
            }

            //if (myDIV.Attributes.CssStyle.Value == "sidebar-mini")
            //{
            //    myDIV.Attributes.Add("class", "");
            //}
            //else
            //{
            //    myDIV.Attributes.Add("class", "sidebar-mini");
            //}
        }
    }
}