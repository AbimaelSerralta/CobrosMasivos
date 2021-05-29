using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class AccountRecovery : System.Web.UI.Page
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

        ManejoSesionServices manejoSesionServices = new ManejoSesionServices();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            txtDatos.Attributes.Add("onkeypress", "button_click(this,'" + btnEnviar.ClientID + "')");

            if (!IsPostBack)
            {
                SetFocus(txtDatos);
                if (Session["AccountRecovery"] != null)
                {
                    txtDatos.Text = Session["AccountRecovery"].ToString();
                }
                else
                {
                    txtDatos.Text = string.Empty;
                }
            }
            else
            {
                SetFocus(txtDatos);
                pnlAlertRecovery.Visible = false;
                lblMensajeAlertRecovery.Text = "";
                divAlertRecovery.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            ValidacionesServices validacionesServices = new ValidacionesServices();
            CorreosEscuelaServices correosEscuelaServices = new CorreosEscuelaServices();

            string dato = string.Empty;
            string parametro = string.Empty;
            string LigaUrl = URLBase + "Views/Login.aspx";

            if (!string.IsNullOrEmpty(txtDatos.Text))
            {
                if (validacionesServices.isEmail(txtDatos.Text))
                {
                    dato = "correo";
                    parametro = "us.VchCorreo";
                }
                else
                {
                    dato = "usuario";
                    parametro = "su.VchUsuario";
                }

                manejoSesionServices.RecoveryPasswordEscuela(parametro, txtDatos.Text);

                if (manejoSesionServices.lsRecoveryPassword != null && manejoSesionServices.lsRecoveryPassword.Count != 0)
                {
                    foreach (var item in manejoSesionServices.lsRecoveryPassword)
                    {
                        correosEscuelaServices.CorreoRecoveryPassword(item.NombreCompleto, "Recuperar contraseña", item.StrCorreo, item.VchContrasenia, LigaUrl, item.StrCorreo);
                    }

                    txtDatos.Text = string.Empty;

                    pnlAlertRecovery.Visible = true;
                    lblMensajeAlertRecovery.Text = "<strong>Felicidades,</strong> le hemos enviado sus credenciales a su correo.";
                    divAlertRecovery.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                }
                else
                {
                    pnlAlertRecovery.Visible = true;
                    lblMensajeAlertRecovery.Text = "<strong>Lo sentimos,</strong> no hemos encontrado una cuenta con este " + dato + ".";
                    divAlertRecovery.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
            else
            {
                pnlAlertRecovery.Visible = true;
                lblMensajeAlertRecovery.Text = "Por favor ingrese su correo o usuario.";
                divAlertRecovery.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect(URLBase + "Views/Login.aspx");
        }
    }
}