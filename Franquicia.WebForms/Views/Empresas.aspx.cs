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

                promocionesServices.CargarPromociones();
                cblPromociones.DataSource = promocionesServices.lsPromociones;
                cblPromociones.DataTextField = "VchDescripcion";
                cblPromociones.DataValueField = "UidPromocion";
                cblPromociones.DataBind();
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
            }
        }

        protected void btnGuardarCredenciales_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdCompany.Text) && !string.IsNullOrEmpty(txtIdBranch.Text) && !string.IsNullOrEmpty(txtMoneda.Text) && !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtCanal.Text) && !string.IsNullOrEmpty(txtData.Text) && !string.IsNullOrEmpty(txtUrl.Text) && !string.IsNullOrEmpty(txtSemillaAES.Text))
            {
                if (validacionesServices.isUrl(txtUrl.Text))
                {
                    switch (ViewState["ActualizarParametros"].ToString())
                    {

                        case "ActualizarParametros":
                            if (parametrosEntradaServices.ActualizarParametrosEntradaCliente(txtIdCompany.Text, txtIdBranch.Text, txtMoneda.Text, txtUsuario.Text, txtPassword.Text, txtCanal.Text, txtData.Text, txtUrl.Text, txtSemillaAES.Text, Guid.Parse(ViewState["UidCliente"].ToString())))
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
                            if (parametrosEntradaServices.RegistrarParametrosEntradaCliente(txtIdCompany.Text, txtIdBranch.Text, txtMoneda.Text, txtUsuario.Text, txtPassword.Text, txtCanal.Text, txtData.Text, txtUrl.Text, txtSemillaAES.Text, Guid.Parse(ViewState["UidCliente"].ToString())))
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

                parametrosEntradaServices.ObtenerParametrosEntradaCliente(dataKeys);
                txtIdCompany.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdCompany;
                txtIdBranch.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdBranch;
                txtMoneda.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchModena;
                txtUsuario.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUsuario;
                txtPassword.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchPassword;
                txtCanal.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchCanal;
                txtData.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchData0;
                txtUrl.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUrl;
                txtSemillaAES.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchSemillaAES;

                promocionesServices.CargarPromociones(dataKeys);
                foreach (ListItem li in cblPromociones.Items)
                {
                    foreach (var item in promocionesServices.lsCBLPromocionesModel)
                    {
                        if (li.Value == item.UidPromocion.ToString())
                        {
                            li.Selected = true;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(txtIdCompany.Text) && !string.IsNullOrEmpty(txtIdBranch.Text))
                {
                    ViewState["ActualizarParametros"] = "ActualizarParametros";
                    btnGuardarCredenciales.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                }
                else
                {
                    ViewState["ActualizarParametros"] = "GuardarParametros";
                    btnGuardarCredenciales.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalCredenciales()", true);
            }
        }

        protected void ddlPromociones_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlPromociones.Text)
            {
                case "SI":
                    pnlPromociones.Visible = true;
                    break;
                case "NO":
                    pnlPromociones.Visible = false;
                    break;
            }
        }
    }
}