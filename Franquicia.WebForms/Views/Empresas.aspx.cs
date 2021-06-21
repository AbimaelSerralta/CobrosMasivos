using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class Empresas : System.Web.UI.Page
    {
        ClientesServices clientesServices = new ClientesServices();
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        ImporteLigaMinMaxServices importeLigaMinMaxServices = new ImporteLigaMinMaxServices();

        decimal ImporteMin = 0;
        decimal ImporteMax = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["clientesServices"] = clientesServices;
                Session["parametrosEntradaServices"] = parametrosEntradaServices;
                Session["promocionesServices"] = promocionesServices;
                Session["importeLigaMinMaxServices"] = importeLigaMinMaxServices;

                clientesServices.CargarTodosClientes();
                gvEmpresas.DataSource = clientesServices.lsClientesGridViewEmpresasModel;
                gvEmpresas.DataBind();

                importeLigaMinMaxServices.CargarImporteLigaMinMax();
                AsignarImporteLigaMinMax();
            }
            else
            {
                clientesServices = (ClientesServices)Session["clientesServices"];
                parametrosEntradaServices = (ParametrosEntradaServices)Session["parametrosEntradaServices"];
                promocionesServices = (PromocionesServices)Session["promocionesServices"];
                importeLigaMinMaxServices = (ImporteLigaMinMaxServices)Session["importeLigaMinMaxServices"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertCredenciales.Visible = false;
                lblMnsjAlertCredenciales.Text = "";
                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertPromociones.Visible = false;
                lblMnsjAlertPromociones.Text = "";
                divAlertPromociones.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                AsignarImporteLigaMinMax();
            }
        }

        private void AsignarImporteLigaMinMax()
        {
            //Asigna los importes min y max del sistema
            foreach (var item in importeLigaMinMaxServices.lsImporteLigaMinMax)
            {
                ImporteMin = item.DcmImporteMin;
                ImporteMax = item.DcmImporteMax;
            }
        }

        protected void btnGuardarCredenciales_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdCompany.Text) && !string.IsNullOrEmpty(txtIdBranch.Text) && !string.IsNullOrEmpty(txtMoneda.Text) && !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtCanal.Text) && !string.IsNullOrEmpty(txtData.Text) && !string.IsNullOrEmpty(txtUrl.Text) && !string.IsNullOrEmpty(txtSemillaAES.Text))
            {
                if (validacionesServices.isUrl(txtUrl.Text))
                {
                    ValidacionesServices validacionesServices = new ValidacionesServices();
                    if (!validacionesServices.IsNumeric(txtImpMin.Text))
                    {
                        pnlAlertCredenciales.Visible = true;
                        lblMnsjAlertCredenciales.Text = "El Importe Mínimo no es un formato correcto.";
                        divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }
                    if (!validacionesServices.IsNumeric(txtImpMax.Text))
                    {
                        pnlAlertCredenciales.Visible = true;
                        lblMnsjAlertCredenciales.Text = "El Importe Máximo no es un formato correcto.";
                        divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }

                    if (decimal.Parse(txtImpMin.Text) >= ImporteMin)
                    {
                    }
                    else
                    {
                        pnlAlertCredenciales.Visible = true;
                        lblMnsjAlertCredenciales.Text = "El importe mínimo es de $" + ImporteMin.ToString("N2");
                        divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        return;
                    }

                    switch (ViewState["AccionParametros"].ToString())
                    {

                        case "ActualizarParametros":
                            if (parametrosEntradaServices.ActualizarParametrosEntradaClienteCM(txtIdCompany.Text, txtIdBranch.Text, txtMoneda.Text, txtUsuario.Text, txtPassword.Text, txtCanal.Text, txtData.Text, txtUrl.Text, txtSemillaAES.Text, Guid.Parse(ViewState["UidCliente"].ToString()), cbActivarImp.Checked, decimal.Parse(txtImpMin.Text), decimal.Parse(txtImpMax.Text)))
                            {
                                pnlAlert.Visible = true;
                                lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalCredenciales()", true);
                            }
                            else
                            {
                                pnlAlertCredenciales.Visible = true;
                                lblMnsjAlertCredenciales.Text = "<b>¡Lo sentimos! </b> ha ocurrido un error inesperado, por favor intentelo más tarde.";
                                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                            break;
                        case "GuardarParametros":
                            if (parametrosEntradaServices.RegistrarParametrosEntradaClienteCM(txtIdCompany.Text, txtIdBranch.Text, txtMoneda.Text, txtUsuario.Text, txtPassword.Text, txtCanal.Text, txtData.Text, txtUrl.Text, txtSemillaAES.Text, Guid.Parse(ViewState["UidCliente"].ToString()), cbActivarImp.Checked, decimal.Parse(txtImpMin.Text), decimal.Parse(txtImpMax.Text)))
                            {
                                pnlAlert.Visible = true;
                                lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalCredenciales()", true);
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
                }
            }
            else
            {
                pnlAlertCredenciales.Visible = true;
                lblMnsjAlertCredenciales.Text = "Los campos con * son obligatorios.";
                divAlertCredenciales.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
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

                parametrosEntradaServices.ObtenerParametrosEntradaClienteCM(dataKeys);
                txtIdCompany.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdCompany;
                txtIdBranch.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdBranch;
                txtUsuario.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUsuario;
                txtPassword.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchPassword;
                cbActivarImp.Checked = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.BitImporteLiga;
                txtImpMin.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.DcmImporteMin.ToString("N2");
                txtImpMax.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.DcmImporteMax.ToString("N2");

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
                    if (decimal.Parse(txtImpMin.Text) != 0 && decimal.Parse(txtImpMax.Text) != 0)
                    {
                        ViewState["AccionParametros"] = "ActualizarParametros";
                        btnGuardarCredenciales.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                    }
                    else
                    {
                        ViewState["AccionParametros"] = "GuardarParametros";
                        btnGuardarCredenciales.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                    }
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalPromociones()", true);
            }
        }

        protected void gvEmpresas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmpresas.PageIndex = e.NewPageIndex;
            gvEmpresas.DataSource = clientesServices.lsClientesGridViewEmpresasModel;
            gvEmpresas.DataBind();
        }
    }
}