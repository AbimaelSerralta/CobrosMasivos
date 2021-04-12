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
        protected void Page_Load(object sender, EventArgs e)
        {

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

                        if (txtUsuario.Text == "MjfKgFrPIx" && txtPassword.Text == "pR#BHdbc^c") // Verificar si existieron errores al tratar de iniciar sesión
                        {

                            Response.Redirect("CheckReference.aspx");
                        }
                        else
                        {
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