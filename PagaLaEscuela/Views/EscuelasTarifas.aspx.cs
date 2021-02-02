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
    public partial class EscuelasTarifas : System.Web.UI.Page
    {
        //SuperPromocionesServices superPromocionesServices = new SuperPromocionesServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        ComisionesTarjetasClientesServices comisionesTarjetasServices = new ComisionesTarjetasClientesServices();

        TiposTarjetasServices tiposTarjetasServices = new TiposTarjetasServices();
        PromocionesTerminalServices promocionesTerminalServices = new PromocionesTerminalServices();
        ComisionesTarjetasClientesTerminalServices comisionesTarjetasTerminalServices = new ComisionesTarjetasClientesTerminalServices();

        TiposTarjetasClubPagoServices tiposTarjetasClubPagoServices = new TiposTarjetasClubPagoServices();
        PromocionesClubPagoServices promocionesClubPagoServices = new PromocionesClubPagoServices();
        ComisionesTarjetasClubPagoServices comisionesTarjetasClubPagoServices = new ComisionesTarjetasClubPagoServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UidClienteMaster"] != null)
            {
                ViewState["UidClienteLocal"] = Session["UidClienteMaster"];
            }
            else
            {
                ViewState["UidClienteLocal"] = Guid.Empty;
            }

            if (!IsPostBack)
            {
                //Session["superPromocionesServices"] = superPromocionesServices;
                Session["promocionesServices"] = promocionesServices;

                CargarSuperPromociones();
                CargarComision(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                CargarTipoTarjeta();
                CargarTipoTarjetaClubPago();
            }
            else
            {
                //superPromocionesServices = (SuperPromocionesServices)Session["superPromocionesServices"];
                promocionesServices = (PromocionesServices)Session["promocionesServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        #region SuperPromociones
        private void CargarSuperPromociones()
        {
            promocionesServices.CargarPromociones();

            promocionesServices.lsCBLPromocionesModel.Clear();
            promocionesServices.CargarPromociones(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            if (promocionesServices.lsCBLPromocionesModel.Count >= 1)
            {
                ViewState["AccionPromociones"] = "ActualizarPromociones";
                btnGuardarPromociones.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
            }
            else
            {
                ViewState["AccionPromociones"] = "GuardarPromociones";
                btnGuardarPromociones.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
            }

            gvPromociones.DataSource = promocionesServices.lsPromociones;
            gvPromociones.DataBind();
        }
        protected void gvPromociones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);

                CheckBox cbPromocion = e.Row.FindControl("cbPromocion") as CheckBox;
                TextBox txtComision = e.Row.FindControl("txtComision") as TextBox;
                TextBox txtDcmApartirDe = e.Row.FindControl("txtApartirDe") as TextBox;


                foreach (var item in promocionesServices.lsCBLPromocionesModel)
                {
                    if (e.Row.Cells[0].Text == item.UidPromocion.ToString())
                    {
                        cbPromocion.Checked = true;
                        txtComision.Text = item.DcmComicion.ToString();
                        txtDcmApartirDe.Text = item.DcmApartirDe.ToString("N2");
                    }
                }
            }
        }
        protected void btnGuardarPromociones_Click(object sender, EventArgs e)
        {
            //Reutilizamos el sp de eliminar e insertar del modulo empresa

            promocionesServices.EliminarPromociones(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            foreach (GridViewRow row in gvPromociones.Rows)
            {
                CheckBox cbPromocion = row.FindControl("cbPromocion") as CheckBox;
                TextBox txtComision = row.FindControl("txtComision") as TextBox;
                TextBox txtApartirDe = row.FindControl("txtApartirDe") as TextBox;

                if (cbPromocion.Checked)
                {
                    Guid UidPromocion = Guid.Parse(row.Cells[0].Text);
                    decimal DcmComicion = 0;
                    decimal DcmApartirDe = 0;

                    if (!string.IsNullOrEmpty(txtComision.Text.Trim()))
                    {
                        DcmComicion = decimal.Parse(txtComision.Text.Trim());
                    }

                    if (!string.IsNullOrEmpty(txtApartirDe.Text.Trim()))
                    {
                        DcmApartirDe = decimal.Parse(txtApartirDe.Text.Trim());
                    }

                    if (promocionesServices.RegistrarPromociones(Guid.Parse(ViewState["UidClienteLocal"].ToString()), UidPromocion, DcmComicion, DcmApartirDe))
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
            }

            CargarSuperPromociones();
        }
        protected void cbPromocion_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)checkBox.Parent.Parent;
            Guid dataKey = Guid.Parse(gvPromociones.DataKeys[gr.RowIndex].Value.ToString());

            CheckBox cbPromocion = (CheckBox)gr.FindControl("cbPromocion");
            TextBox txtComision = (TextBox)gr.FindControl("txtComision");
            TextBox txtApartirDe = (TextBox)gr.FindControl("txtApartirDe");

            if (cbPromocion.Checked)
            {
                txtComision.Enabled = true;
                txtApartirDe.Enabled = true;
            }
            else
            {
                txtComision.Enabled = false;
                txtApartirDe.Enabled = false;
            }
        }
        #endregion

        #region Comision
        private void CargarComision(Guid UidCliente)
        {
            comisionesTarjetasServices.CargarComisionesTarjeta(UidCliente);

            foreach (var item in comisionesTarjetasServices.lsComisionesTarjetasClientes)
            {
                ViewState["item.UidComicionTarjetaCliente"] = item.UidComicionTarjetaCliente;

                cbActivarComision.Checked = false;
                if (item.BitComision)
                {
                    cbActivarComision.Checked = true;
                }

                txtComisionTarjeta.Text = item.DcmComision.ToString("N2");
            }

            if (comisionesTarjetasServices.lsComisionesTarjetasClientes.Count >= 1)
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
                if (comisionesTarjetasServices.RegistrarComisionesTarjeta(cbActivarComision.Checked, decimal.Parse(txtComisionTarjeta.Text), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                    CargarComision(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
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
                if (comisionesTarjetasServices.ActualizarComisionesTarjeta(cbActivarComision.Checked, decimal.Parse(txtComisionTarjeta.Text), Guid.Parse(ViewState["item.UidComicionTarjetaCliente"].ToString())))
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                    CargarComision(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
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

        #region TerminalBancaria
        private void CargarTipoTarjeta()
        {
            tiposTarjetasServices.CargarTiposTarjetas();
            gvTipoTarjeta.DataSource = tiposTarjetasServices.lsTiposTarjetas;
            gvTipoTarjeta.DataBind();
        }

        #region GridViewTipoTarjeta
        protected void gvTipoTarjeta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);

                CheckBox cbComisionTerminal = e.Row.FindControl("cbComisionTerminal") as CheckBox;
                TextBox txtComisionTipoTarjeta = e.Row.FindControl("txtComisionTipoTarjeta") as TextBox;
                GridView gvPromocionesTerminal = e.Row.FindControl("gvPromocionesTerminal") as GridView;


                comisionesTarjetasTerminalServices.CargarComisionesTarjetaTerminal(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                foreach (var item in comisionesTarjetasTerminalServices.lsComisionesTarjetasClientesTerminal)
                {

                    if (Guid.Parse(e.Row.Cells[0].Text) == item.UidTipoTarjeta)
                    {
                        ViewState["item.UidComicionTarjetaCliente"] = item.UidComicionTarjetaClienteTerminal;

                        cbComisionTerminal.Checked = false;
                        if (item.BitComision)
                        {
                            cbComisionTerminal.Checked = true;
                        }

                        txtComisionTipoTarjeta.Text = item.DcmComision.ToString("N2");
                    }
                }

                //if (promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Count >= 1)
                //{
                ViewState["AccionPromocionesTerminal"] = "ActualizarPromocionesTerminal";
                btnGuardarPromocionesTerminal.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                //}
                //else
                //{
                //    ViewState["AccionPromocionesTerminal"] = "GuardarPromocionesTerminal";
                //    btnGuardarPromocionesTerminal.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                //}

                foreach (var itemTT in tiposTarjetasServices.lsTiposTarjetas)
                {
                    if (itemTT.BitPromociones && Guid.Parse(e.Row.Cells[0].Text) == itemTT.UidTipoTarjeta)
                    {
                        promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Clear();
                        promocionesTerminalServices.CargarPromocionesTerminalCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), itemTT.UidTipoTarjeta);
                        gvPromocionesTerminal.DataSource = promocionesTerminalServices.lsCBLPromocionesTerminalViewModel;
                        gvPromocionesTerminal.DataBind();
                    }
                }
            }
        }
        #endregion

        #region GridViewPromocionesTerminal
        protected void gvPromocionesTerminal_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void cbPromocionTerminal_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion
        protected void btnGuardarPromocionesTerminal_Click(object sender, EventArgs e)
        {
            comisionesTarjetasTerminalServices.EliminarComisionesTarjetaTerminal(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            promocionesTerminalServices.EliminarPromocionesTerminalCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            foreach (GridViewRow row in gvTipoTarjeta.Rows)
            {
                Guid UidTipoTarjeta = Guid.Parse(row.Cells[0].Text);
                CheckBox cbComisionTerminal = row.FindControl("cbComisionTerminal") as CheckBox;
                TextBox txtComisionTipoTarjeta = row.FindControl("txtComisionTipoTarjeta") as TextBox;
                GridView gvPromocionesTerminal = row.FindControl("gvPromocionesTerminal") as GridView;

                if (cbComisionTerminal.Checked)
                {
                    if (txtComisionTipoTarjeta.EmptyTextBox())
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "El campo comisión es obligatorio.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }

                    ValidacionesServices validacionesServices = new ValidacionesServices();
                    if (!validacionesServices.IsNumeric(txtComisionTipoTarjeta.Text))
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "La comisión no es un formato correcto.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }
                }
                else
                {
                    txtComisionTipoTarjeta.Text = "0";
                }
                comisionesTarjetasTerminalServices.RegistrarComisionesTarjetaTerminal(cbComisionTerminal.Checked, decimal.Parse(txtComisionTipoTarjeta.Text), UidTipoTarjeta, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                foreach (GridViewRow rowPT in gvPromocionesTerminal.Rows)
                {
                    Guid UidPromocionTerminal = Guid.Parse(rowPT.Cells[0].Text);
                    CheckBox cbPromocionTerminal = rowPT.FindControl("cbPromocionTerminal") as CheckBox;
                    TextBox txtComisionTerminal = rowPT.FindControl("txtComisionTerminal") as TextBox;
                    TextBox txtApartirDeTerminal = rowPT.FindControl("txtApartirDeTerminal") as TextBox;

                    if (cbPromocionTerminal.Checked)
                    {
                        decimal DcmComicion = 0;
                        decimal DcmApartirDe = 0;

                        if (!string.IsNullOrEmpty(txtComisionTerminal.Text.Trim()))
                        {
                            DcmComicion = decimal.Parse(txtComisionTerminal.Text.Trim());
                        }

                        if (!string.IsNullOrEmpty(txtApartirDeTerminal.Text.Trim()))
                        {
                            DcmApartirDe = decimal.Parse(txtApartirDeTerminal.Text.Trim());
                        }

                        promocionesTerminalServices.RegistrarPromocionesTerminalCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), UidPromocionTerminal, DcmComicion, DcmApartirDe, UidTipoTarjeta);
                    }
                }

                switch (ViewState["AccionPromocionesTerminal"].ToString())
                {
                    case "ActualizarPromocionesTerminal":
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;

                    case "GuardarPromocionesTerminal":
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;
                }
            }

            CargarTipoTarjeta();
        }
        #endregion

        #region PagosEnEfectivo

        #region ClubPago
        private void CargarTipoTarjetaClubPago()
        {
            tiposTarjetasClubPagoServices.CargarTiposTarjetas();
            gvTipoTarjetaClubPago.DataSource = tiposTarjetasClubPagoServices.lsTiposTarjetasClubPago;
            gvTipoTarjetaClubPago.DataBind();
        }

        #region GridViewTipoTarjeta
        protected void gvTipoTarjetaClubPago_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);

                CheckBox cbComisionClubPago = e.Row.FindControl("cbComisionClubPago") as CheckBox;
                TextBox txtComisionTipoTarjetaClubPago = e.Row.FindControl("txtComisionTipoTarjetaClubPago") as TextBox;
                GridView gvPromocionesClubPago = e.Row.FindControl("gvPromocionesClubPago") as GridView;


                comisionesTarjetasClubPagoServices.CargarComisionesTarjeta(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                foreach (var item in comisionesTarjetasClubPagoServices.lsComisionesTarjetasClubPago)
                {

                    if (Guid.Parse(e.Row.Cells[0].Text) == item.UidTipoTarjeta)
                    {
                        ViewState["item.UidComicionTarjetaCliente"] = item.UidComicionTarjeta;

                        cbComisionClubPago .Checked = false;
                        if (item.BitComision)
                        {
                            cbComisionClubPago.Checked = true;
                        }

                        txtComisionTipoTarjetaClubPago.Text = item.DcmComision.ToString("N2");
                    }
                }

                //if (promocionesTerminalServices.lsCBLPromocionesTerminalViewModel.Count >= 1)
                //{
                ViewState["AccionPromocionesClubPago"] = "ActualizarPromocionesClubPago";
                btnGuardarPromocionesClubPago.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                //}
                //else
                //{
                //    ViewState["AccionPromocionesClubPago"] = "GuardarPromocionesClubPago";
                //    btnGuardarPromocionesTerminal.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                //}

                foreach (var itemTT in tiposTarjetasClubPagoServices.lsTiposTarjetasClubPago)
                {
                    if (itemTT.BitPromociones && Guid.Parse(e.Row.Cells[0].Text) == itemTT.UidTipoTarjeta)
                    {
                        promocionesClubPagoServices.lsCBLPromocionesClubPagoViewModel.Clear();
                        promocionesClubPagoServices.CargarPromocionesCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), itemTT.UidTipoTarjeta);
                        gvPromocionesClubPago.DataSource = promocionesClubPagoServices.lsCBLPromocionesClubPagoViewModel;
                        gvPromocionesClubPago.DataBind();
                    }
                }
            }
        }
        #endregion

        #region GridViewPromociones
        protected void gvPromocionesClubPago_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void cbPromocionClubPago_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion
        protected void btnGuardarPromocionesClubPago_Click(object sender, EventArgs e)
        {
            comisionesTarjetasClubPagoServices.EliminarComisionesTarjeta(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            promocionesClubPagoServices.EliminarPromocionesCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            foreach (GridViewRow row in gvTipoTarjetaClubPago.Rows)
            {
                Guid UidTipoTarjeta = Guid.Parse(row.Cells[0].Text);
                CheckBox cbComisionClubPago = row.FindControl("cbComisionClubPago") as CheckBox;
                TextBox txtComisionTipoTarjetaClubPago = row.FindControl("txtComisionTipoTarjetaClubPago") as TextBox;
                GridView gvPromocionesClubPago = row.FindControl("gvPromocionesClubPago") as GridView;

                if (cbComisionClubPago.Checked)
                {
                    if (txtComisionTipoTarjetaClubPago.EmptyTextBox())
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "El campo comisión es obligatorio.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }

                    ValidacionesServices validacionesServices = new ValidacionesServices();
                    if (!validacionesServices.IsNumeric(txtComisionTipoTarjetaClubPago.Text))
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "La comisión no es un formato correcto.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }
                }
                else
                {
                    txtComisionTipoTarjetaClubPago.Text = "0";
                }
                comisionesTarjetasClubPagoServices.RegistrarComisionesTarjeta(cbComisionClubPago.Checked, decimal.Parse(txtComisionTipoTarjetaClubPago.Text), UidTipoTarjeta, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                foreach (GridViewRow rowPT in gvPromocionesClubPago.Rows)
                {
                    Guid UidPromocion = Guid.Parse(rowPT.Cells[0].Text);
                    CheckBox cbPromocionClubPago = rowPT.FindControl("cbPromocionClubPago") as CheckBox;
                    TextBox txtComisionClubPago = rowPT.FindControl("txtComisionClubPago") as TextBox;
                    TextBox txtApartirDeClubPago = rowPT.FindControl("txtApartirDeClubPago") as TextBox;

                    if (cbPromocionClubPago.Checked)
                    {
                        decimal DcmComicion = 0;
                        decimal DcmApartirDe = 0;

                        if (!string.IsNullOrEmpty(txtComisionClubPago.Text.Trim()))
                        {
                            DcmComicion = decimal.Parse(txtComisionClubPago.Text.Trim());
                        }

                        if (!string.IsNullOrEmpty(txtApartirDeClubPago.Text.Trim()))
                        {
                            DcmApartirDe = decimal.Parse(txtApartirDeClubPago.Text.Trim());
                        }

                        promocionesClubPagoServices.RegistrarPromocionesCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), UidPromocion, DcmComicion, DcmApartirDe, UidTipoTarjeta);
                    }
                }

                switch (ViewState["AccionPromocionesClubPago"].ToString())
                {
                    case "ActualizarPromocionesClubPago":
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;

                    case "GuardarPromocionesClubPago":
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;
                }
            }

            CargarTipoTarjetaClubPago();
        }
        #endregion
        #endregion
    }
}