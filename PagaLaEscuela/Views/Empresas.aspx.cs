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
    public partial class Empresas : System.Web.UI.Page
    {
        ClientesServices clientesServices = new ClientesServices();
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        PromocionesServices promocionesServices = new PromocionesServices();

        TiposTarjetasPragaServices tiposTarjetasPragaServices = new TiposTarjetasPragaServices();
        ComisionesTarjetasPragaServices comisionesTarjetasPragaServices = new ComisionesTarjetasPragaServices();
        PromocionesPragaServices promocionesPragaServices = new PromocionesPragaServices();

        ParametrosPragaServices parametrosPragaServices = new ParametrosPragaServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["clientesServices"] = clientesServices;
                Session["parametrosEntradaServices"] = parametrosEntradaServices;
                Session["promocionesServices"] = promocionesServices;

                clientesServices.CargarTodosClientes();
                gvEmpresas.DataSource = clientesServices.lsClientesGridViewEmpresasModel;
                gvEmpresas.DataBind();

            }
            else
            {
                clientesServices = (ClientesServices)Session["clientesServices"];
                parametrosEntradaServices = (ParametrosEntradaServices)Session["parametrosEntradaServices"];
                promocionesServices = (PromocionesServices)Session["promocionesServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertCredenciales.Visible = false;
                lblMnsjAlertCredenciales.Text = "";
                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertPromociones.Visible = false;
                lblMnsjAlertPromociones.Text = "";
                divAlertPromociones.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        protected void btnGuardarCredenciales_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdCompany.Text) && !string.IsNullOrEmpty(txtIdBranch.Text) && !string.IsNullOrEmpty(txtMoneda.Text) && !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtCanal.Text) && !string.IsNullOrEmpty(txtData.Text) && !string.IsNullOrEmpty(txtUrl.Text) && !string.IsNullOrEmpty(txtSemillaAES.Text))
            {
                if (validacionesServices.isUrl(txtUrl.Text))
                {
                    switch (ViewState["AccionParametros"].ToString())
                    {
                        case "ActualizarParametros":
                            if (parametrosEntradaServices.ActualizarParametrosEntradaCliente(txtIdCompany.Text, txtIdBranch.Text, txtMoneda.Text, txtUsuario.Text, txtPassword.Text, txtCanal.Text, txtData.Text, txtUrl.Text, txtSemillaAES.Text, Guid.Parse(ViewState["UidCliente"].ToString())))
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                            }
                            else
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Lo sentimos! </b> ha ocurrido un error inesperado, por favor intentelo más tarde.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                            break;
                        case "GuardarParametros":
                            if (parametrosEntradaServices.RegistrarParametrosEntradaCliente(txtIdCompany.Text, txtIdBranch.Text, txtMoneda.Text, txtUsuario.Text, txtPassword.Text, txtCanal.Text, txtData.Text, txtUrl.Text, txtSemillaAES.Text, Guid.Parse(ViewState["UidCliente"].ToString())))
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                            }
                            else
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Lo sentimos! </b> ha ocurrido un error inesperado, por favor intentelo más tarde.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                            break;
                    }
                }
                else
                {
                    pnlAlertCredenciales.Visible = true;
                    lblMnsjAlertCredenciales.Text = "Lo sentimos el campo URL no tiene el formato correcto. <br/>=> http(s)://www.cobroscontarjeta.com";
                    divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    return;
                }
            }
            else
            {
                pnlAlertCredenciales.Visible = true;
                lblMnsjAlertCredenciales.Text = "Los campos con * son obligatorios.";
                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
        }
        protected void btnGuardarCredencialesPraga_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdBusinesPartner.Text) && !string.IsNullOrEmpty(txtClaveUsuarioWSREST.Text) && !string.IsNullOrEmpty(txtMonedaPraga.Text) && !string.IsNullOrEmpty(txtUrlPraga.Text) && !string.IsNullOrEmpty(txtClaveEncripcionWSREST.Text) && !string.IsNullOrEmpty(txtAPIKey.Text))
            {
                if (validacionesServices.isUrl(txtUrlPraga.Text))
                {
                    switch (ViewState["AccionParametrosPraga"].ToString())
                    {

                        case "ActualizarParametrosPraga":
                            if (parametrosPragaServices.ActualizarParametrosPraga(txtIdBusinesPartner.Text, txtUrlPraga.Text, txtClaveUsuarioWSREST.Text, txtClaveEncripcionWSREST.Text, txtAPIKey.Text, txtMonedaPraga.Text, Guid.Parse(ViewState["UidCliente"].ToString())))
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                            }
                            else
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Lo sentimos! </b> ha ocurrido un error inesperado, por favor intentelo más tarde.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                            break;
                        case "GuardarParametrosPraga":
                            if (parametrosPragaServices.RegistrarParametrosPraga(txtIdBusinesPartner.Text, txtUrlPraga.Text, txtClaveUsuarioWSREST.Text, txtClaveEncripcionWSREST.Text, txtAPIKey.Text, txtMonedaPraga.Text, Guid.Parse(ViewState["UidCliente"].ToString())))
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                            }
                            else
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Lo sentimos! </b> ha ocurrido un error inesperado, por favor intentelo más tarde.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                            break;
                    }
                }
                else
                {
                    pnlAlertCredenciales.Visible = true;
                    lblMnsjAlertCredenciales.Text = "Lo sentimos el campo URL no tiene el formato correcto. <br/>=> http(s)://www.cobroscontarjeta.com";
                    divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    return;
                }
            }
            else
            {
                pnlAlertCredenciales.Visible = true;
                lblMnsjAlertCredenciales.Text = "Los campos con * son obligatorios.";
                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                return;
            }
        }

        protected void gvEmpresas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnCredencialesLigas")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvEmpresas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidCliente"] = dataKeys;

                //lblTituloModal.Text = "Visualización de la Franquicia";

                parametrosEntradaServices.ObtenerParametrosEntradaCliente(dataKeys);
                txtIdCompany.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdCompany;
                txtIdBranch.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdBranch;
                txtUsuario.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUsuario;
                txtPassword.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchPassword;

                if (!string.IsNullOrEmpty(txtIdCompany.Text) && !string.IsNullOrEmpty(txtIdBranch.Text))
                {
                    txtMoneda.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchModena;
                    txtCanal.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchCanal;
                    txtData.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchData0;
                    txtUrl.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUrl;
                    txtSemillaAES.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchSemillaAES;

                    ViewState["AccionParametros"] = "ActualizarParametros";
                    btnGuardarCredenciales.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                }
                else
                {
                    ViewState["AccionParametros"] = "GuardarParametros";
                    btnGuardarCredenciales.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                }

                bool Accion = parametrosPragaServices.ObtenerParametrosPragaBl(dataKeys);
                txtIdBusinesPartner.Text = "";

                if (Accion)
                {
                    txtIdBusinesPartner.Text = parametrosPragaServices.parametrosPragaRepository.parametrosPraga.BusinessId.ToString();
                    txtClaveUsuarioWSREST.Text = parametrosPragaServices.parametrosPragaRepository.parametrosPraga.UserCode.ToString();
                    txtMonedaPraga.Text = parametrosPragaServices.parametrosPragaRepository.parametrosPraga.Currency.ToString();
                    txtUrlPraga.Text = parametrosPragaServices.parametrosPragaRepository.parametrosPraga.VchUrl.ToString();
                    txtClaveEncripcionWSREST.Text = parametrosPragaServices.parametrosPragaRepository.parametrosPraga.WSEncryptionKey.ToString();
                    txtAPIKey.Text = parametrosPragaServices.parametrosPragaRepository.parametrosPraga.APIKey.ToString();

                    ViewState["AccionParametrosPraga"] = "ActualizarParametrosPraga";
                    btnGuardarCredencialesPraga.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                }
                else
                {
                    ViewState["AccionParametrosPraga"] = "GuardarParametrosPraga";
                    btnGuardarCredencialesPraga.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalCredenciales()", true);
            }

            if (e.CommandName == "btnPromociones")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvEmpresas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidCliente"] = dataKeys;

                //lblTituloModal.Text = "Visualización de la Franquicia";

                promocionesServices.CargarPromociones();

                promocionesServices.lsCBLPromocionesModel.Clear();
                promocionesServices.CargarPromociones(dataKeys);

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

                CargarTipoTarjetaPraga();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalPromociones()", true);
            }
        }

        protected void gvPromociones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);

                CheckBox cbPromocion = e.Row.FindControl("cbPromocion") as CheckBox;
                TextBox txtComicion = e.Row.FindControl("txtComicion") as TextBox;
                TextBox txtDcmApartirDe = e.Row.FindControl("txtApartirDe") as TextBox;


                foreach (var item in promocionesServices.lsCBLPromocionesModel)
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
            promocionesServices.EliminarPromociones(Guid.Parse(ViewState["UidCliente"].ToString()));

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

                    if (promocionesServices.RegistrarPromociones(Guid.Parse(ViewState["UidCliente"].ToString()), UidPromocion, DcmComicion, DcmApartirDe))
                    {
                    }
                }

                switch (ViewState["AccionPromociones"].ToString())
                {
                    case "ActualizarPromociones":
                        pnlAlertPromociones.Visible = true;
                        lblMnsjAlertPromociones.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                        divAlertPromociones.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;

                    case "GuardarPromociones":
                        pnlAlertPromociones.Visible = true;
                        lblMnsjAlertPromociones.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                        divAlertPromociones.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;
                }
            }
        }

        protected void gvEmpresas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmpresas.PageIndex = e.NewPageIndex;
            gvEmpresas.DataSource = clientesServices.lsClientesGridViewEmpresasModel;
            gvEmpresas.DataBind();
        }

        #region CobrosEnLinea

        #region Praga
        private void CargarTipoTarjetaPraga()
        {
            tiposTarjetasPragaServices.CargarTiposTarjetas();
            gvTipoTarjetaPraga.DataSource = tiposTarjetasPragaServices.lsTiposTarjetasPraga;
            gvTipoTarjetaPraga.DataBind();
        }

        #region GridViewTipoTarjetaPraga
        protected void gvTipoTarjetaPraga_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);

                CheckBox cbComisionPraga = e.Row.FindControl("cbComisionPraga") as CheckBox;
                TextBox txtComisionTipoTarjetaPraga = e.Row.FindControl("txtComisionTipoTarjetaPraga") as TextBox;
                GridView gvPromocionesPraga = e.Row.FindControl("gvPromocionesPraga") as GridView;


                comisionesTarjetasPragaServices.CargarComisionesTarjeta(Guid.Parse(ViewState["UidCliente"].ToString()));

                foreach (var item in comisionesTarjetasPragaServices.lsComisionesTarjetasPraga)
                {

                    if (Guid.Parse(e.Row.Cells[0].Text) == item.UidTipoTarjeta)
                    {
                        ViewState["item.UidComicionTarjetaCliente"] = item.UidComicionTarjeta;

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
                //    ViewState["AccionPromocionesPraga"] = "GuardarPromocionesTerminal";
                //    btnGuardarPromocionesTerminal.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                //}

                foreach (var itemTT in tiposTarjetasPragaServices.lsTiposTarjetasPraga)
                {
                    if (Guid.Parse(e.Row.Cells[0].Text) == itemTT.UidTipoTarjeta)
                    {
                        promocionesPragaServices.lsCBLPromocionesPragaViewModel.Clear();
                        promocionesPragaServices.CargarPromocionesCliente(Guid.Parse(ViewState["UidCliente"].ToString()), itemTT.UidTipoTarjeta);
                        gvPromocionesPraga.DataSource = promocionesPragaServices.lsCBLPromocionesPragaViewModel;
                        gvPromocionesPraga.DataBind();
                    }
                }
            }
        }
        #endregion

        #region GridViewPromocionesPraga
        protected void gvPromocionesPraga_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DGVCajas, "Select$" + e.Row.RowIndex);
            }
        }

        protected void cbPromocionPraga_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion
        protected void btnGuardarPromocionesPraga_Click(object sender, EventArgs e)
        {
            comisionesTarjetasPragaServices.EliminarComisionesTarjeta(Guid.Parse(ViewState["UidCliente"].ToString()));
            promocionesPragaServices.EliminarPromocionesCliente(Guid.Parse(ViewState["UidCliente"].ToString()));

            foreach (GridViewRow row in gvTipoTarjetaPraga.Rows)
            {
                Guid UidTipoTarjeta = Guid.Parse(row.Cells[0].Text);
                CheckBox cbComisionPraga = row.FindControl("cbComisionPraga") as CheckBox;
                TextBox txtComisionTipoTarjetaPraga = row.FindControl("txtComisionTipoTarjetaPraga") as TextBox;
                GridView gvPromocionesPraga = row.FindControl("gvPromocionesPraga") as GridView;

                if (cbComisionPraga.Checked)
                {
                    if (txtComisionTipoTarjetaPraga.EmptyTextBox())
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "El campo comisión es obligatorio.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }

                    ValidacionesServices validacionesServices = new ValidacionesServices();
                    if (!validacionesServices.IsNumeric(txtComisionTipoTarjetaPraga.Text))
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "La comisión no es un formato correcto.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }
                }
                else
                {
                    txtComisionTipoTarjetaPraga.Text = "0";
                }
                comisionesTarjetasPragaServices.RegistrarComisionesTarjeta(cbComisionPraga.Checked, decimal.Parse(txtComisionTipoTarjetaPraga.Text), UidTipoTarjeta, Guid.Parse(ViewState["UidCliente"].ToString()));

                foreach (GridViewRow rowPT in gvPromocionesPraga.Rows)
                {
                    Guid UidPromocion = Guid.Parse(rowPT.Cells[0].Text);
                    CheckBox cbPromocionPraga = rowPT.FindControl("cbPromocionPraga") as CheckBox;
                    TextBox txtComisionPraga = rowPT.FindControl("txtComisionPraga") as TextBox;
                    TextBox txtApartirDePraga = rowPT.FindControl("txtApartirDePraga") as TextBox;

                    if (cbPromocionPraga.Checked)
                    {
                        decimal DcmComicion = 0;
                        decimal DcmApartirDe = 0;

                        if (!string.IsNullOrEmpty(txtComisionPraga.Text.Trim()))
                        {
                            DcmComicion = decimal.Parse(txtComisionPraga.Text.Trim());
                        }

                        if (!string.IsNullOrEmpty(txtApartirDePraga.Text.Trim()))
                        {
                            DcmApartirDe = decimal.Parse(txtApartirDePraga.Text.Trim());
                        }

                        promocionesPragaServices.RegistrarPromocionesCliente(Guid.Parse(ViewState["UidCliente"].ToString()), UidPromocion, DcmComicion, DcmApartirDe, UidTipoTarjeta);
                    }
                }

                switch (ViewState["AccionPromocionesPraga"].ToString())
                {
                    case "ActualizarPromocionesPraga":
                        pnlAlertPromociones.Visible = true;
                        lblMnsjAlertPromociones.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                        divAlertPromociones.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;

                    case "GuardarPromocionesPraga":
                        pnlAlertPromociones.Visible = true;
                        lblMnsjAlertPromociones.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                        divAlertPromociones.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                        break;
                }
            }

            CargarTipoTarjetaPraga();
        }
        #endregion

        #endregion
    }
}