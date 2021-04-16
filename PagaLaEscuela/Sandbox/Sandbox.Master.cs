using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Sandbox
{
    public partial class Sandbox : System.Web.UI.MasterPage
    {
        string URLBase = string.Empty;
        ManejoSesionSandboxServices manejoSesionSandboxServices { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["URLBase"] != null)
            {
                URLBase = Session["URLBase"].ToString();
            }

            #region Validaciones iniciales antes de cargar la página

            // Verificar que exista el manejador de la sesión
            if (Session["manejoSesionSandboxServices"] != null)
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Obtener administrador de la sesión
                manejoSesionSandboxServices = (ManejoSesionSandboxServices)Session["manejoSesionSandboxServices"];
                // Verificar el status de la sesión
                if (manejoSesionSandboxServices.BolStatusSesion)
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

                        manejoSesionSandboxServices.CargarAccesosPermitidos(manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidCredencial);
                        Session["lsAccesosPermitidos"] = manejoSesionSandboxServices.lsAccesosPermitidos;

                        Session["UidIntegracionMaster"] = manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidIntegracion;
                        Session["UidCredencialMaster"] = manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidCredencial;

                        dlMenu.DataSource = manejoSesionSandboxServices.lsAccesosPermitidos;
                        dlMenu.DataBind();

                        ValidarAcceso();
                    }
                    else
                    {
                    }

                    LblNombreUsuario.Text = manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.VchIdentificador;
                    lblFecha.Text = DateTime.Today.ToLongDateString();

                }
                else
                {
                    Response.Redirect(URLBase + "Login.aspx");
                }
            }
            else
            {
                Response.Redirect(URLBase + "Login.aspx");
            }
            #endregion Validaciones iniciales antes de cargar la página
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
        public void ValidarAcceso()
        {
            string UrlLocal = Request.Url.LocalPath;
            string Url = UrlLocal.Replace("/Sandbox/", "");

            //ActivarMenu(Url);

            if (manejoSesionSandboxServices.lsAccesosPermitidos.Exists(x => x.VchUrl == Url))
            {
                //int i = manejoSesionSandboxServices.lsAccesosPermitidos.IndexOf(manejoSesionSandboxServices.lsAccesosPermitidos.First(x => x.VchUrl == Url));
                //Response.Redirect(manejoSesionSandboxServices.lsAccesosPermitidos[i].VchUrl);
            }
            else
            {
                Session.Abandon();
                Response.Write("<script language='javascript'>window.alert('El acceso ha sido exitoso, pero no hemos encontrado modulos activos, por favor contacte al administrador del sistema.');window.location='Login.aspx';</script>");
            }
        }
        protected void dlMenu_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            string UrlLocal = Request.Url.LocalPath;
            string Url = UrlLocal.Replace("/Sandbox/", "");

            Label lblUrl = (Label)e.Item.FindControl("lblUrl");
            HtmlGenericControl liMenuActivar = (HtmlGenericControl)e.Item.FindControl("liMenuActivar");
            HtmlAnchor aActive = (HtmlAnchor)e.Item.FindControl("aActive");

            if (Url == lblUrl.Text)
            {
                aActive.Attributes.Add("style", "background-color:#b9504c");
                liMenuActivar.Attributes.Add("class", "nav-item active");
            }

        }
    }
}