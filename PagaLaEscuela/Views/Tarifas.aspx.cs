using Franquicia.Bussiness;
using PagaLaEscuela.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class Tarifas : System.Web.UI.Page
    {
        TarifasServices tarifasServices = new TarifasServices();
        SuperPromocionesServices superPromocionesServices = new SuperPromocionesServices();
        ComisionesTarjetasServices comisionesTarjetasServices = new ComisionesTarjetasServices();

        TiposTarjetasPragaServices tiposTarjetasPragaServices = new TiposTarjetasPragaServices();
        SuperComisionesTarjetasPragaServices superComisionesTarjetasPragaServices = new SuperComisionesTarjetasPragaServices();
        PromocionesPragaServices promocionesPragaServices = new PromocionesPragaServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["tarifasServices"] = tarifasServices;

                CargarTarifas();

                CargarSuperPromociones();

                CargarComision();

                CargarTipoTarjetaPraga();
            }
            else
            {
                tarifasServices = (TarifasServices)Session["tarifasServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        #region WhatsApp y SMS
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
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "El campo Whatsapp es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
            if (txtSms.EmptyTextBox())
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "El campo Sms es obligatorio.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }

            if (ViewState["Accion"].ToString() == "Guardar")
            {
                if (tarifasServices.RegistrarTarifas(decimal.Parse(txtWhats.Text), decimal.Parse(txtSms.Text)))
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                    CargarTarifas();
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> no se ha podido registar, por favor intentelo de nuevo, si el problema persiste, favor de contactar a los admnistradores.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
            else if (ViewState["Accion"].ToString() == "Actualizar")
            {
                if (tarifasServices.ActualizarTarifas(Guid.Parse(ViewState["item.UidTarifa"].ToString()), decimal.Parse(txtWhats.Text), decimal.Parse(txtSms.Text)))
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                    CargarTarifas();
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> no se ha podido actualizar, por favor intentelo de nuevo, si el problema persiste, favor de contactar a los admnistradores.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
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
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            btnGuardar.Visible = false;
            btnCancelar.Visible = false;
            btnEditar.Visible = true;

            txtWhats.Enabled = false;
            txtSms.Enabled = false;
        }
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            btnGuardar.Visible = true;
            btnCancelar.Visible = true;
            btnEditar.Visible = false;

            txtWhats.Enabled = true;
            txtSms.Enabled = true;
        }
        private void CargarTarifas()
        {
            tarifasServices.CargarTarifas();
            //gvTarifas.DataSource = tarifasServices.lsTarifasGridViewModel;
            //gvTarifas.DataBind();

            foreach (var item in tarifasServices.lsTarifasGridViewModel)
            {
                ViewState["item.UidTarifa"] = item.UidTarifa;
                txtWhats.Text = item.DcmWhatsapp.ToString("N2");
                txtSms.Text = item.DcmSms.ToString("N2");
            }

            if (tarifasServices.lsTarifasGridViewModel.Count >= 1)
            {
                ViewState["Accion"] = "Actualizar";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                btnGuardar.Visible = false;
                btnCancelar.Visible = false;
                btnEditar.Visible = true;

                txtWhats.Enabled = false;
                txtSms.Enabled = false;
            }
            else
            {
                ViewState["Accion"] = "Guardar";
                btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                btnGuardar.Visible = true;
                btnCancelar.Visible = false;
                btnEditar.Visible = false;

                txtWhats.Enabled = true;
                txtSms.Enabled = true;
            }
        }
        #endregion

        #region SuperPromociones
        private void CargarSuperPromociones()
        {
            superPromocionesServices.CargarPromociones();

            superPromocionesServices.lsCBLSuperPromociones.Clear();
            superPromocionesServices.CargarSuperPromociones();

            if (superPromocionesServices.lsCBLSuperPromociones.Count >= 1)
            {
                ViewState["AccionPromociones"] = "ActualizarPromociones";
                btnGuardarPromociones.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
            }
            else
            {
                ViewState["AccionPromociones"] = "GuardarPromociones";
                btnGuardarPromociones.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
            }

            gvPromociones.DataSource = superPromocionesServices.lsPromociones;
            gvPromociones.DataBind();
        }
        protected void gvPromociones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);

                CheckBox cbPromocion = e.Row.FindControl("cbPromocion") as CheckBox;
                TextBox txtComicion = e.Row.FindControl("txtComicion") as TextBox;
                TextBox txtDcmApartirDe = e.Row.FindControl("txtApartirDe") as TextBox;


                foreach (var item in superPromocionesServices.lsCBLSuperPromociones)
                {
                    if (e.Row.Cells[0].Text == item.UidPromocion.ToString())
                    {
                        cbPromocion.Checked = true;
                        txtComicion.Text = item.DcmComicion.ToString();
                        txtDcmApartirDe.Text = item.DcmApartirDe.ToString("N2");
                    }
                }
            }
        }
        protected void btnGuardarPromociones_Click(object sender, EventArgs e)
        {
            superPromocionesServices.EliminarSuperPromociones();

            foreach (GridViewRow row in gvPromociones.Rows)
            {
                CheckBox cbPromocion = row.FindControl("cbPromocion") as CheckBox;
                TextBox txtComicion = row.FindControl("txtComicion") as TextBox;
                TextBox txtApartirDe = row.FindControl("txtApartirDe") as TextBox;

                if (cbPromocion.Checked)
                {
                    Guid UidPromocion = Guid.Parse(row.Cells[0].Text);
                    decimal DcmComicion = 0;
                    decimal DcmApartirDe = 0;

                    if (!string.IsNullOrEmpty(txtComicion.Text))
                    {
                        DcmComicion = decimal.Parse(txtComicion.Text);
                    }

                    if (!string.IsNullOrEmpty(txtApartirDe.Text))
                    {
                        DcmApartirDe = decimal.Parse(txtApartirDe.Text);
                    }

                    if (superPromocionesServices.RegistrarSuperPromociones(UidPromocion, DcmComicion, DcmApartirDe))
                    {
                    }
                }

                switch (ViewState["AccionPromociones"].ToString())
                {
                    case "ActualizarPromociones":
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;

                    case "GuardarPromociones":
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;
                }

                CargarSuperPromociones();
            }
        }
        protected void cbPromocion_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)checkBox.Parent.Parent;
            Guid dataKey = Guid.Parse(gvPromociones.DataKeys[gr.RowIndex].Value.ToString());

            CheckBox cbPromocion = (CheckBox)gr.FindControl("cbPromocion");
            TextBox txtComicion = (TextBox)gr.FindControl("txtComicion");
            TextBox txtApartirDe = (TextBox)gr.FindControl("txtApartirDe");

            if (cbPromocion.Checked)
            {
                txtComicion.Enabled = true;
                txtApartirDe.Enabled = true;
            }
            else
            {
                txtComicion.Enabled = false;
                txtApartirDe.Enabled = false;
            }
        }
        #endregion

        #region Comision
        private void CargarComision()
        {
            comisionesTarjetasServices.CargarComisionesTarjeta();

            foreach (var item in comisionesTarjetasServices.lsComisionesTarjetas)
            {
                ViewState["item.UidComicionTarjeta"] = item.UidComicionTarjeta;

                cbActivarComision.Checked = false;
                if (item.BitComision)
                {
                    cbActivarComision.Checked = true;
                }

                txtComisionTarjeta.Text = item.DcmComision.ToString("N2");
            }

            if (comisionesTarjetasServices.lsComisionesTarjetas.Count >= 1)
            {
                ViewState["Accion"] = "Actualizar";
                btnGuardarComision.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
            }
            else
            {
                ViewState["Accion"] = "Guardar";
                btnGuardarComision.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
            }

            cbActivarComision_CheckedChanged(null, null);
        }
        protected void btnGuardarComision_Click(object sender, EventArgs e)
        {
            if (cbActivarComision.Checked)
            {
                if (txtComisionTarjeta.EmptyTextBox())
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "El campo comisión es obligatorio.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    return;
                }

                ValidacionesServices validacionesServices = new ValidacionesServices();
                if (!validacionesServices.IsNumeric(txtComisionTarjeta.Text))
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "La comisión no es un formato correcto.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    return;
                }
            }
            else
            {
                txtComisionTarjeta.Text = "0";
            }

            if (ViewState["Accion"].ToString() == "Guardar")
            {
                if (comisionesTarjetasServices.RegistrarComisionesTarjeta(cbActivarComision.Checked, decimal.Parse(txtComisionTarjeta.Text)))
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                    CargarComision();
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> no se ha podido registar, por favor intentelo de nuevo, si el problema persiste, favor de contactar a los admnistradores.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
            else if (ViewState["Accion"].ToString() == "Actualizar")
            {
                if (comisionesTarjetasServices.ActualizarComisionesTarjeta(cbActivarComision.Checked, decimal.Parse(txtComisionTarjeta.Text), Guid.Parse(ViewState["item.UidComicionTarjeta"].ToString())))
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                    CargarComision();
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Lo sentimos! </b> no se ha podido actualizar, por favor intentelo de nuevo, si el problema persiste, favor de contactar a los admnistradores.";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
        }
        protected void cbActivarComision_CheckedChanged(object sender, EventArgs e)
        {
            txtComisionTarjeta.Enabled = cbActivarComision.Checked;
        }
        #endregion

        #region CobrosEnLinea

        #region Praga
        private void CargarTipoTarjetaPraga()
        {
            tiposTarjetasPragaServices.CargarTiposTarjetas();
            gvTipoTarjetaPraga.DataSource = tiposTarjetasPragaServices.lsTiposTarjetasPraga;
            gvTipoTarjetaPraga.DataBind();
        }

        #region GridViewTipoTarjeta
        protected void gvTipoTarjetaPraga_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);

                CheckBox cbComisionPraga = e.Row.FindControl("cbComisionPraga") as CheckBox;
                TextBox txtComisionTipoTarjetaPraga = e.Row.FindControl("txtComisionTipoTarjetaPraga") as TextBox;
                GridView gvPromocionesPraga = e.Row.FindControl("gvPromocionesPraga") as GridView;


                superComisionesTarjetasPragaServices.CargarComisionesTarjeta();

                foreach (var item in superComisionesTarjetasPragaServices.lsSuperComisionesTarjetasPraga)
                {

                    if (Guid.Parse(e.Row.Cells[0].Text) == item.UidTipoTarjeta)
                    {
                        ViewState["item.UidSuperComicionTarjeta"] = item.UidSuperComicionTarjeta;

                        cbComisionPraga.Checked = false;
                        if (item.BitComision)
                        {
                            cbComisionPraga.Checked = true;
                        }

                        txtComisionTipoTarjetaPraga.Text = item.DcmComision.ToString("N2");
                    }
                }

                //if (promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Count >= 1)
                //{
                ViewState["AccionPromocionesPraga"] = "ActualizarPromocionesPraga";
                btnGuardarPromocionesPraga.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                //}
                //else
                //{
                //    ViewState["AccionPromocionesTerminal"] = "GuardarPromocionesTerminal";
                //    btnGuardarPromocionesTerminal.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                //}

                foreach (var itemTT in tiposTarjetasPragaServices.lsTiposTarjetasPraga)
                {
                    if (Guid.Parse(e.Row.Cells[0].Text) == itemTT.UidTipoTarjeta)
                    {
                        promocionesPragaServices.lsCBLSuperPromocionesPragaViewModel.Clear();
                        promocionesPragaServices.CargarSuperPromociones(itemTT.UidTipoTarjeta);
                        gvPromocionesPraga.DataSource = promocionesPragaServices.lsCBLSuperPromocionesPragaViewModel;
                        gvPromocionesPraga.DataBind();
                    }
                }
            }
        }
        #endregion

        #region GridViewPromocionesTerminal
        protected void gvPromocionesPraga_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);

                //CheckBox cbPromocionTerminal = e.Row.FindControl("cbPromocionTerminal") as CheckBox;
                //TextBox txtComisionTerminal = e.Row.FindControl("txtComisionTerminal") as TextBox;
                //TextBox txtApartirDeTerminal = e.Row.FindControl("txtApartirDeTerminal") as TextBox;


                //foreach (var item in promocionesTerminalServices.lsCBLPromocionesTerminalViewModel)
                //{
                //    if (Guid.Parse(e.Row.Cells[0].Text) == item.UidPromocionTerminal)
                //    {
                //        cbPromocionTerminal.Checked = true;
                //        txtComisionTerminal.Text = item.DcmComicion.ToString();
                //        txtApartirDeTerminal.Text = item.DcmApartirDe.ToString("N2");
                //    }
                //}
            }
        }

        protected void cbPromocionPraga_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion
        protected void btnGuardarPromocionesPraga_Click(object sender, EventArgs e)
        {
            //comisionesTarjetasTerminalServices.EliminarComisionesTarjetaTerminal(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            //promocionesTerminalServices.EliminarPromocionesTerminalCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            //foreach (GridViewRow row in gvTipoTarjeta.Rows)
            //{
            //    Guid UidTipoTarjeta = Guid.Parse(row.Cells[0].Text);
            //    CheckBox cbComisionTerminal = row.FindControl("cbComisionTerminal") as CheckBox;
            //    TextBox txtComisionTipoTarjeta = row.FindControl("txtComisionTipoTarjeta") as TextBox;
            //    GridView gvPromocionesTerminal = row.FindControl("gvPromocionesTerminal") as GridView;

            //    if (cbComisionTerminal.Checked)
            //    {
            //        if (txtComisionTipoTarjeta.EmptyTextBox())
            //        {
            //            pnlAlert.Visible = true;
            //            lblMensajeAlert.Text = "El campo comisión es obligatorio.";
            //            divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            //            return;
            //        }

            //        ValidacionesServices validacionesServices = new ValidacionesServices();
            //        if (!validacionesServices.IsNumeric(txtComisionTipoTarjeta.Text))
            //        {
            //            pnlAlert.Visible = true;
            //            lblMensajeAlert.Text = "La comisión no es un formato correcto.";
            //            divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        txtComisionTipoTarjeta.Text = "0";
            //    }
            //    comisionesTarjetasTerminalServices.RegistrarComisionesTarjetaTerminal(cbComisionTerminal.Checked, decimal.Parse(txtComisionTipoTarjeta.Text), UidTipoTarjeta, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            //    foreach (GridViewRow rowPT in gvPromocionesTerminal.Rows)
            //    {
            //        Guid UidPromocionTerminal = Guid.Parse(rowPT.Cells[0].Text);
            //        CheckBox cbPromocionTerminal = rowPT.FindControl("cbPromocionTerminal") as CheckBox;
            //        TextBox txtComisionTerminal = rowPT.FindControl("txtComisionTerminal") as TextBox;
            //        TextBox txtApartirDeTerminal = rowPT.FindControl("txtApartirDeTerminal") as TextBox;

            //        if (cbPromocionTerminal.Checked)
            //        {
            //            decimal DcmComicion = 0;
            //            decimal DcmApartirDe = 0;

            //            if (!string.IsNullOrEmpty(txtComisionTerminal.Text.Trim()))
            //            {
            //                DcmComicion = decimal.Parse(txtComisionTerminal.Text.Trim());
            //            }

            //            if (!string.IsNullOrEmpty(txtApartirDeTerminal.Text.Trim()))
            //            {
            //                DcmApartirDe = decimal.Parse(txtApartirDeTerminal.Text.Trim());
            //            }

            //            promocionesTerminalServices.RegistrarPromocionesTerminalCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), UidPromocionTerminal, DcmComicion, DcmApartirDe, UidTipoTarjeta);
            //        }
            //    }

            //    switch (ViewState["AccionPromocionesTerminal"].ToString())
            //    {
            //        case "ActualizarPromocionesTerminal":
            //            pnlAlert.Visible = true;
            //            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
            //            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            //            break;

            //        case "GuardarPromocionesTerminal":
            //            pnlAlert.Visible = true;
            //            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
            //            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            //            break;
            //    }
            //}

            //CargarTipoTarjeta();
        }
        #endregion

        #endregion
    }
}