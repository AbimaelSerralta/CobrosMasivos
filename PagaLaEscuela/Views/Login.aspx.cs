using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
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

        ManejoSesionServices manejoSesionServices { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword.Attributes.Add("onkeypress", "button_click(this,'" + btnLogin.ClientID + "')");
            txtUsuario.Attributes.Add("onkeypress", "button_click(this,'" + btnLogin.ClientID + "')");

            if (Session["manejoSesionServices"] != null)
            {
                manejoSesionServices = (ManejoSesionServices)Session["manejoSesionServices"];
                if (manejoSesionServices.BolStatusSesion)
                {
                    if (!IsPostBack)
                    {
                        Response.Redirect(manejoSesionServices.ObtenerHome());
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
                manejoSesionServices = new ManejoSesionServices();
                Session["manejoSesionServices"] = manejoSesionServices;
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

                        manejoSesionServices.IniciarSesionEscuela(txtUsuario.Text.Trim(), txtPassword.Text.Trim());

                        if (manejoSesionServices.BolStatusSesion) // Verificar si existieron errores al tratar de iniciar sesión
                        {
                            if (manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidSegPerfil != Guid.Empty)
                            {
                                manejoSesionServices.ObtenerAppWeb();
                                if (manejoSesionServices.perfilesRepository.appWebRepository.appWeb.IntGerarquia == 2)
                                {
                                    manejoSesionServices.ObtenerFranquiciaUsuario();
                                    Session["UidFranquiciaMaster"] = manejoSesionServices.usuarioCompletoRepository.franquiciatarios.UidFranquiciatarios;
                                }

                                else if (manejoSesionServices.perfilesRepository.appWebRepository.appWeb.IntGerarquia == 3)
                                {
                                    manejoSesionServices.ObtenerFranquiciaClienteUsuario();
                                    Session["UidClienteMaster"] = manejoSesionServices.usuarioCompletoRepository.clientes.UidCliente;
                                    Session["UidUsuarioMaster"] = manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidUsuario;
                                }
                                else if (manejoSesionServices.perfilesRepository.appWebRepository.appWeb.IntGerarquia == 4)
                                {
                                    Session["UidUsuarioMaster"] = manejoSesionServices.usuarioCompletoRepository.usuarioCompleto.UidUsuario;
                                }
                                Session["manejoSesionServices"] = manejoSesionServices;
                                Response.Redirect(manejoSesionServices.ObtenerHome());
                            }
                            else
                            {
                                Session["manejoSesionServices"] = null;
                                divAlert.Visible = true;
                                lblValiUser.Text = "Lo sentimos su cuenta no esta activada.";
                            }

                            Session["URLBase"] = URLBase;
                        }
                        else
                        {
                            Session["manejoSesionServices"] = null;
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

        protected void btnRecovery_Click(object sender, EventArgs e)
        {
            Session["AccountRecovery"] = txtUsuario.Text;

            Response.Redirect(URLBase + "Views/AccountRecovery.aspx");

            //pnlAlertRecovery.Visible = false;
            //lblMensajeAlertRecovery.Text = "";
            //divAlertRecovery.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

            //txtDatos.Text = string.Empty;
            //btnEnviar.Visible = true;
            //btnCancelar.CssClass = "btn btn-danger";
            //btnCancelar.Text = "Cancelar";

            //SetFocus(txtDatos);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "FormModalsScript", "showModal()", true);
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
                    btnEnviar.Visible = false;
                    btnCancelar.CssClass = "btn btn-info";
                    btnCancelar.Text = "Aceptar";

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
    }
}