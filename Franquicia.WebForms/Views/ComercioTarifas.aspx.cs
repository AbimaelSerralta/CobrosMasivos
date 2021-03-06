﻿using Franquicia.Bussiness;
using Franquicia.WebForms.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class ComercioTarifas : System.Web.UI.Page
    {
        //SuperPromocionesServices superPromocionesServices = new SuperPromocionesServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        ComisionesTarjetasClientesServices comisionesTarjetasServices = new ComisionesTarjetasClientesServices();
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
    }
}