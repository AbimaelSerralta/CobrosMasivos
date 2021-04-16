using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Sandbox
{
    public partial class Login : System.Web.UI.Page
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

        ManejoSesionSandboxServices manejoSesionSandboxServices { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword.Attributes.Add("onkeypress", "button_click(this,'" + btnLogin.ClientID + "')");
            txtUsuario.Attributes.Add("onkeypress", "button_click(this,'" + btnLogin.ClientID + "')");

            if (Session["manejoSesionSandboxServices"] != null)
            {
                manejoSesionSandboxServices = (ManejoSesionSandboxServices)Session["manejoSesionSandboxServices"];
                if (manejoSesionSandboxServices.BolStatusSesion)
                {
                    if (!IsPostBack)
                    {
                        Guid UidCredencial = Guid.Empty;

                        if (manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidCredencial != null)
                        {
                            UidCredencial = manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidCredencial;
                        }

                        Response.Redirect(manejoSesionSandboxServices.ObtenerHome(UidCredencial));
                        SetFocus(txtUsuario);
                    }
                    else
                    {
                        SetFocus(txtUsuario);
                        divAlert.Visible = false;
                        pnlAlertRecovery.Visible = false;
                        lblMensajeAlertRecovery.Text = "";
                        divAlertRecovery.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
                    }
                }
                else
                {
                    SetFocus(txtUsuario);
                }
            }
            else
            {
                manejoSesionSandboxServices = new ManejoSesionSandboxServices();
                Session["manejoSesionSandboxServices"] = manejoSesionSandboxServices;
                SetFocus(txtUsuario);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text) || !string.IsNullOrEmpty(txtPassword.Text))
            {
                if (!string.IsNullOrEmpty(txtUsuario.Text))
                {
                    lblValiUser.Text = string.Empty;

                    if (!string.IsNullOrEmpty(txtPassword.Text))
                    {
                        lblValiUser.Text = "";
                        lblValiPassword.Text = "";

                        manejoSesionSandboxServices.IniciarSesion(txtUsuario.Text.Trim(), txtPassword.Text.Trim());

                        if (manejoSesionSandboxServices.BolStatusSesion) // Verificar si existieron errores al tratar de iniciar sesión
                        {
                            if (manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidEstatus == Guid.Parse("65E46BC9-1864-4145-AD1A-70F5B5F69739"))
                            {
                                Session["UidIntegracionMaster"] = manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidIntegracion;
                                Session["UidCredencialMaster"] = manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidCredencial;

                                Session["URLBase"] = URLBase;

                                Session["manejoSesionSandboxServices"] = manejoSesionSandboxServices;
                                Response.Redirect(manejoSesionSandboxServices.ObtenerHome(manejoSesionSandboxServices.manejoSesionSandboxRepository.integracionesCompleto.UidCredencial));
                            }
                            else
                            {
                                Session["manejoSesionSandboxServices"] = null;
                                divAlert.Visible = true;
                                lblValiUser.Text = "Lo sentimos su cuenta no tiene acceso, por favor comuniquese con los administradores.";
                            }
                        }
                        else
                        {
                            Session["manejoSesionSandboxServices"] = null;
                            divAlert.Visible = true;
                            lblValiUser.Text = "Usuario y/o contraseña invalida, por favor verifíquelo.";
                        }
                    }
                    else
                    {
                        divAlert.Visible = true;
                        lblValiPassword.Text = "* La contraseña es obligatoria.";
                    }
                }
                else
                {
                    divAlert.Visible = true;
                    lblValiUser.Text = "* El nombre de usuario es obligatorio.";
                }
            }
            else
            {
                divAlert.Visible = true;
                lblValiUser.Text = "* El nombre de usuario es obligatorio.";
                lblValiPassword.Text = "* La contraseña es obligatoria.";
            }
        }
    }
}