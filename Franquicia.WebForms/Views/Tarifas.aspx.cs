using Franquicia.Bussiness;
using Franquicia.WebForms.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class Tarifas : System.Web.UI.Page
    {
        TarifasServices tarifasServices = new TarifasServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["tarifasServices"] = tarifasServices;

                tarifasServices.CargarTarifas();
                gvTarifas.DataSource = tarifasServices.lsTarifasGridViewModel;
                gvTarifas.DataBind();

                if (tarifasServices.lsTarifasGridViewModel.Count >= 1)
                {
                    btnNuevo.Visible = false;
                }
                else
                {
                    btnNuevo.Visible = true;
                }
            }
            else
            {
                tarifasServices = (TarifasServices)Session["tarifasServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lblValidar.Text = string.Empty;
            ViewState["Accion"] = "Guardar";
            LimpiarCampos();
            DesbloquearCampos();
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            lblTituloModal.Text = "Registro de Tarifa";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtWhats.EmptyTextBox())
            {
                lblValidar.Text = "El campo Whatsapp es obligatorio";
                return;
            }
            if (txtSms.EmptyTextBox())
            {
                lblValidar.Text = "El campo Sms es obligatorio";
                return;
            }

            if (tarifasServices.RegistrarTarifas(decimal.Parse(txtWhats.Text), decimal.Parse(txtSms.Text)))
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                tarifasServices.CargarTarifas();
                gvTarifas.DataSource = tarifasServices.lsTarifasGridViewModel;
                gvTarifas.DataBind();
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> no se ha podido registar, por favor intentelo de nuevo, si el problema persiste, favor de contactar a los admnistradores.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
        }

        public void LimpiarCampos()
        {
            txtWhats.Text = string.Empty;
            txtSms.Text = string.Empty;
        }

        public void BloquearCampos()
        {
            txtWhats.Enabled = false;
            txtSms.Enabled = false;
        }
        public void DesbloquearCampos()
        {
            txtWhats.Enabled = true;
            txtSms.Enabled = true;
        }

        protected void gvTarifas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnEditar")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvTarifas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                LinkButton btnAceptar = (LinkButton)row.FindControl("btnAceptar");
                LinkButton btnCancelar = (LinkButton)row.FindControl("btnCancelar");
                LinkButton btnEditar = (LinkButton)row.FindControl("btnEditar");

                TextBox txtGvWhatsapp = (TextBox)row.FindControl("txtGvWhatsapp");
                TextBox txtGvSms = (TextBox)row.FindControl("txtGvSms");

                ViewState["GvWhatsapp"] = txtGvWhatsapp.Text;
                ViewState["GvSms"] = txtGvSms.Text;

                btnAceptar.Visible = true;
                btnCancelar.Visible = true;
                btnEditar.Visible = false;

                txtGvWhatsapp.Enabled = true;
                txtGvSms.Enabled = true;

                txtGvWhatsapp.BorderStyle = BorderStyle.Solid;
                txtGvSms.BorderStyle = BorderStyle.Solid;

            }

            if (e.CommandName == "btnAceptar")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvTarifas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                LinkButton btnAceptar = (LinkButton)row.FindControl("btnAceptar");
                LinkButton btnCancelar = (LinkButton)row.FindControl("btnCancelar");
                LinkButton btnEditar = (LinkButton)row.FindControl("btnEditar");

                TextBox txtGvWhatsapp = (TextBox)row.FindControl("txtGvWhatsapp");
                TextBox txtGvSms = (TextBox)row.FindControl("txtGvSms");

                btnAceptar.Visible = false;
                btnCancelar.Visible = false;
                btnEditar.Visible = true;

                txtGvWhatsapp.Enabled = false;
                txtGvSms.Enabled = false;

                txtGvWhatsapp.BorderStyle = BorderStyle.None;
                txtGvSms.BorderStyle = BorderStyle.None;

                if (tarifasServices.ActualizarTarifas(dataKey, decimal.Parse(txtGvWhatsapp.Text), decimal.Parse(txtGvSms.Text)))
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                    tarifasServices.CargarTarifas();
                    gvTarifas.DataSource = tarifasServices.lsTarifasGridViewModel;
                    gvTarifas.DataBind();
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> no se ha podido actualizar, por favor intentelo de nuevo, si el problema persiste, favor de contactar a los admnistradores.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }

            if (e.CommandName == "btnCancelar")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvTarifas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKey = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());

                LinkButton btnAceptar = (LinkButton)row.FindControl("btnAceptar");
                LinkButton btnCancelar = (LinkButton)row.FindControl("btnCancelar");
                LinkButton btnEditar = (LinkButton)row.FindControl("btnEditar");

                TextBox txtGvWhatsapp = (TextBox)row.FindControl("txtGvWhatsapp");
                TextBox txtGvSms = (TextBox)row.FindControl("txtGvSms");

                btnAceptar.Visible = false;
                btnCancelar.Visible = false;
                btnEditar.Visible = true;

                txtGvWhatsapp.Enabled = false;
                txtGvSms.Enabled = false;

                txtGvWhatsapp.BorderStyle = BorderStyle.None;
                txtGvSms.BorderStyle = BorderStyle.None;

                txtGvWhatsapp.Text = ViewState["GvWhatsapp"].ToString();
                txtGvSms.Text = ViewState["GvSms"].ToString();
            }
        }
    }
}