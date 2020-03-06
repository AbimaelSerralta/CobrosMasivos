using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using Franquicia.WebForms.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Franquicia.WebForms.Views
{
    public partial class Franquicias : System.Web.UI.Page
    {
        FranquiciatariosServices franquiciatariosServices = new FranquiciatariosServices();
        DireccionesFranquiciatariosServices direccionesFranquiciatariosServices = new DireccionesFranquiciatariosServices();
        TelefonosFranquiciatariosServices telefonosFranquiciatariosServices = new TelefonosFranquiciatariosServices();
        //ModulosServices modulosServices = new ModulosServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        EstatusServices estatusService = new EstatusServices();
        TiposTelefonosServices tiposTelefonosServices = new TiposTelefonosServices();
        ParametrosEntradaServices parametrosEntradaServices = new ParametrosEntradaServices();
        PromocionesServices promocionesServices = new PromocionesServices();

        PaisesServices paisesServices = new PaisesServices();
        EstadosServices estadosServices = new EstadosServices();
        MunicipiosServices municipiosServices = new MunicipiosServices();
        CiudadesServices ciudadesServices = new CiudadesServices();
        ColoniasServices coloniasServices = new ColoniasServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            //modulosServices.CargarAccesosPermitidos();

            if (!IsPostBack)
            {
                ViewState["gvFranquiciatarios"] = SortDirection.Ascending;

                #region Direccion
                Session["paisesServices"] = paisesServices;
                Session["estadosServices"] = estadosServices;
                Session["municipiosServices"] = municipiosServices;
                Session["ciudadesServices"] = ciudadesServices;
                Session["coloniasServices"] = coloniasServices;
                #endregion

                Session["franquiciatariosServices"] = franquiciatariosServices;
                Session["telefonosFranquiciatariosServices"] = telefonosFranquiciatariosServices;
                Session["estatusService"] = estatusService;
                Session["parametrosEntradaServices"] = parametrosEntradaServices;
                Session["promocionesServices"] = promocionesServices;

                franquiciatariosServices.CargarFranquiciatarios();
                gvFranquiciatarios.DataSource = franquiciatariosServices.lsFranquiciasGridViewModel;
                gvFranquiciatarios.DataBind();

                estatusService.CargarEstatus();
                ddlEstatus.DataSource = estatusService.lsEstatus;
                ddlEstatus.DataTextField = "VchDescripcion";
                ddlEstatus.DataValueField = "UidEstatus";
                ddlEstatus.DataBind();

                tiposTelefonosServices.CargarTiposTelefonos();
                ddlTipoTelefono.DataSource = tiposTelefonosServices.lsTiposTelefonos;
                ddlTipoTelefono.DataTextField = "VchDescripcion";
                ddlTipoTelefono.DataValueField = "UidTipoTelefono";
                ddlTipoTelefono.DataBind();

                paisesServices.CargarPaises();
                ddlPais.DataSource = paisesServices.lsPaises;
                ddlPais.DataTextField = "VchPais";
                ddlPais.DataValueField = "UidPais";
                ddlPais.DataBind();

                promocionesServices.CargarPromociones();
                cblPromociones.DataSource = promocionesServices.lsPromociones;
                cblPromociones.DataTextField = "VchDescripcion";
                cblPromociones.DataValueField = "UidPromocion";
                cblPromociones.DataBind();
            }
            else
            {
                #region Direccion
                paisesServices = (PaisesServices)Session["paisesServices"];
                estadosServices = (EstadosServices)Session["estadosServices"];
                municipiosServices = (MunicipiosServices)Session["municipiosServices"];
                ciudadesServices = (CiudadesServices)Session["ciudadesServices"];
                coloniasServices = (ColoniasServices)Session["coloniasServices"];
                #endregion

                franquiciatariosServices = (FranquiciatariosServices)Session["franquiciatariosServices"];
                telefonosFranquiciatariosServices = (TelefonosFranquiciatariosServices)Session["telefonosFranquiciatariosServices"];
                estatusService = (EstatusServices)Session["estatusService"];
                tiposTelefonosServices = (TiposTelefonosServices)Session["tiposTelefonosServices"];
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
        #region Dirección
        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            //llena el combo Estados
            estadosServices.CargarEstados(ddlPais.SelectedItem.Value.ToString());
            ddlEstado.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
            ddlEstado.DataSource = estadosServices.lsEstados;
            ddlEstado.DataTextField = "VchEstado";
            ddlEstado.DataValueField = "UidEstado";
            ddlEstado.DataBind();


        }
        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("Municipios");
            ddlMunicipio.Items.Clear();
            ddlColonia.Items.Clear();
            ddlCiudad.Items.Clear();

            //llena el combo Estados
            municipiosServices.CargarMunicipios(ddlEstado.SelectedItem.Value.ToString());
            ddlMunicipio.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
            Session["Municipios"] = municipiosServices;
            ddlMunicipio.DataSource = municipiosServices.lsMunicipios;
            ddlMunicipio.DataTextField = "VchMunicipio";
            ddlMunicipio.DataValueField = "UidMunicipio";
            ddlMunicipio.DataBind();
            txtCodigoPostal.Text = string.Empty;
        }
        protected void ddlMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("Ciudades");
            ddlCiudad.Items.Clear();
            ddlColonia.Items.Clear();

            //llena el combo Municipios
            ciudadesServices.CargarCiudades(ddlMunicipio.SelectedItem.Value.ToString());
            ddlCiudad.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
            Session["Ciudades"] = ciudadesServices;
            ddlCiudad.DataSource = ciudadesServices.lsCiudades;
            ddlCiudad.DataTextField = "VchCiudad";
            ddlCiudad.DataValueField = "UidCiudad";
            ddlCiudad.DataBind();
            txtCodigoPostal.Text = string.Empty;
        }
        protected void ddlCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("Colonias");
            ddlColonia.Items.Clear();

            coloniasServices.CargarColonias(ddlCiudad.SelectedItem.Value.ToString());
            ddlColonia.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
            Session["Colonias"] = coloniasServices.lsColonias;
            ddlColonia.DataSource = coloniasServices.lsColonias;
            ddlColonia.DataTextField = "VchColonia";
            ddlColonia.DataValueField = "UidColonia";
            ddlColonia.DataBind();
            txtCodigoPostal.Text = string.Empty;
        }
        protected void ddlColonia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlColonia.SelectedIndex != 0)
            {
                //coloniasServices.CargarColonias(ddlCiudad.SelectedItem.Value.ToString());
                //ddlColonia.DataSource = coloniasServices.lsColonias;
                //ddlColonia.DataTextField = "VchColonia";
                //ddlColonia.DataValueField = "UidColonia";
                //ddlColonia.DataBind();
                txtCodigoPostal.Text = coloniasServices.lsColonias[ddlColonia.SelectedIndex - 1].VchCodigoPostal;
            }
            else
            {
                txtCodigoPostal.Text = string.Empty;
            }
        }
        #endregion
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DateTime thisDay = DateTime.Now;

            #region ValidarCampos
            if (txtRFC.EmptyTextBox())
            {
                lblValidar.Text = "El campo RFC es obligatorio";
                return;
            }

            if (txtRazonSocial.EmptyTextBox())
            {
                lblValidar.Text = "El campo RazonSocial es obligatorio";
                return;
            }

            if (txtNombreComercial.EmptyTextBox())
            {
                lblValidar.Text = "El campo NombreComercial es obligatorio";
                return;
            }
            if (txtCorreo.EmptyTextBox())
            {
                lblValidar.Text = "El campo Correo Eléctronico es obligatorio";
                return;
            }
            if (txtNumero.EmptyTextBox())
            {
                lblValidar.Text = "El campo Número es obligatorio";
                return;
            }

            //if (txtIdentificador.EmptyTextBox())
            //{
            //    lblValidar.Text = "El campo Identificador es obligatorio";
            //    return;
            //}

            if (ddlPais.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Pais es obligatorio";
                return;
            }
            if (ddlEstado.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Estado es obligatorio";
                return;
            }
            if (ddlMunicipio.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Municipio es obligatorio";
                return;
            }
            if (ddlCiudad.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Ciudad es obligatorio";
                return;
            }
            if (ddlColonia.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Colonia es obligatorio";
                return;
            }
            if (txtCalle.EmptyTextBox())
            {
                lblValidar.Text = "El campo Calle es obligatorio";
                return;
            }
            if (txtEntreCalle.EmptyTextBox())
            {
                lblValidar.Text = "El campo Entre Calle es obligatorio";
                return;
            }
            if (txtYCalle.EmptyTextBox())
            {
                lblValidar.Text = "El campo Y Calle es obligatorio";
                return;
            }
            if (txtNumeroExterior.EmptyTextBox())
            {
                lblValidar.Text = "El campo Numero Exterior es obligatorio";
                return;
            }
            if (txtCodigoPostal.EmptyTextBox())
            {
                lblValidar.Text = "El campo Código Postal es obligatorio";
                return;
            }

            //if (ddlTipoTelefono.EmptyDropDownList())
            //{
            //    lblValidar.Text = "El campo Tipo Telefono es obligatorio";
            //    return;
            //}
            if (!string.IsNullOrEmpty(lblValidar.Text))
            {
                lblValidar.Text = string.Empty;
            }
            #endregion

            List<PermisosMenuModel> permisosMenuModels = (List<PermisosMenuModel>)Session["lsAccesosPermitidos"];
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == new Guid("C3344CA9-32F6-4195-BB26-7753A79BCCF4")).ToList();
            foreach (var item in permisosMenuModels)
            {
                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        if (franquiciatariosServices.RegistrarFranquiciatarios(
                    txtRFC.Text.Trim().ToUpper(), txtRazonSocial.Text.Trim().ToUpper(), txtNombreComercial.Text.Trim().ToUpper(), DateTime.Parse(thisDay.ToString("dd/MM/yyyy HH:mm:ss")), txtCorreo.Text.Trim().ToUpper(),
                    "", new Guid(ddlPais.SelectedValue), new Guid(ddlEstado.SelectedValue), new Guid(ddlMunicipio.SelectedValue), new Guid(ddlCiudad.SelectedValue), new Guid(ddlColonia.SelectedValue), txtCalle.Text.Trim().ToUpper(), txtEntreCalle.Text.Trim().ToUpper(), txtYCalle.Text.Trim().ToUpper(), txtNumeroExterior.Text.Trim().ToUpper(), txtNumeroInterior.Text.Trim().ToUpper(), txtCodigoPostal.Text.Trim(), txtReferencia.Text.Trim().ToUpper(),
                    txtNumero.Text.Trim().ToUpper(), new Guid("B1055882-BCBA-4AB7-94FA-90E57647E607")
                    ))
                        {
                            franquiciatariosServices.CargarFranquiciatarios();
                            gvFranquiciatarios.DataSource = franquiciatariosServices.lsFranquiciasGridViewModel;
                            gvFranquiciatarios.DataBind();

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                        }
                    }
                    else
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>Lo sentimos,</b> no tiene permisos para esta acción.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                    }
                }
                else if (ViewState["Accion"].ToString() == "Actualizar")
                {
                    if (item.Actualizar)
                    {
                        if (franquiciatariosServices.ActualizarFranquiciatarios(
                    new Guid(ViewState["UidFranquiciatario"].ToString()), txtRFC.Text.Trim().ToUpper(), txtRazonSocial.Text.Trim().ToUpper(), txtNombreComercial.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), Guid.Parse(ddlEstatus.SelectedValue),
                    "", new Guid(ddlPais.SelectedValue), new Guid(ddlEstado.SelectedValue), new Guid(ddlMunicipio.SelectedValue), new Guid(ddlCiudad.SelectedValue), new Guid(ddlColonia.SelectedValue), txtCalle.Text.Trim().ToUpper(), txtEntreCalle.Text.Trim().ToUpper(), txtYCalle.Text.Trim().ToUpper(), txtNumeroExterior.Text.Trim().ToUpper(), txtNumeroInterior.Text.Trim().ToUpper(), txtCodigoPostal.Text.Trim(), txtReferencia.Text.Trim().ToUpper(),
                    txtNumero.Text.Trim().ToUpper(), new Guid("B1055882-BCBA-4AB7-94FA-90E57647E607")
                    ))
                        {
                            franquiciatariosServices.CargarFranquiciatarios();
                            gvFranquiciatarios.DataSource = franquiciatariosServices.lsFranquiciasGridViewModel;
                            gvFranquiciatarios.DataBind();

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                        }
                    }
                    else
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<b>Lo sentimos,</b> no tiene permisos para esta acción.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                    }
                }
            }
        }
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lblTituloModal.Text = "Registro de Franquicia";
            lblValidar.Text = string.Empty;
            ViewState["Accion"] = "Guardar";
            LimpiarCampos();
            DesbloquearCampos();
            txtFechaAlta.Text = DateTime.Now.ToShortDateString();
            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);

        }

        private void BloquearCampos()
        {
            txtRFC.Enabled = false;
            txtRazonSocial.Enabled = false;
            txtNombreComercial.Enabled = false;
            txtCorreo.Enabled = false;

            ddlEstatus.Enabled = false;

            ddlPais.Enabled = false;
            ddlEstado.Enabled = false;
            ddlMunicipio.Enabled = false;
            ddlCiudad.Enabled = false;
            ddlColonia.Enabled = false;
            txtCalle.Enabled = false;
            txtEntreCalle.Enabled = false;
            txtYCalle.Enabled = false;
            txtNumeroExterior.Enabled = false;
            txtNumeroInterior.Enabled = false;
            txtCodigoPostal.Enabled = false;
            txtReferencia.Enabled = false;

            ddlTipoTelefono.Enabled = false;
            txtNumero.Enabled = false;
        }
        private void DesbloquearCampos()
        {
            txtRFC.Enabled = true;
            txtRazonSocial.Enabled = true;
            txtNombreComercial.Enabled = true;
            txtCorreo.Enabled = true;

            if (lblTituloModal.Text.Contains("Registro"))
            {
                ddlEstatus.Enabled = false;
            }
            else
            {
                ddlEstatus.Enabled = true;
            }

            ddlPais.Enabled = true;
            ddlEstado.Enabled = true;
            ddlMunicipio.Enabled = true;
            ddlCiudad.Enabled = true;
            ddlColonia.Enabled = true;
            txtCalle.Enabled = true;
            txtEntreCalle.Enabled = true;
            txtYCalle.Enabled = true;
            txtNumeroExterior.Enabled = true;
            txtNumeroInterior.Enabled = true;
            txtCodigoPostal.Enabled = true;
            txtReferencia.Enabled = true;

            ddlTipoTelefono.Enabled = true;
            txtNumero.Enabled = true;
        }
        private void LimpiarCampos()
        {
            txtRFC.Text = string.Empty;
            txtRazonSocial.Text = string.Empty;
            txtNombreComercial.Text = string.Empty;
            txtFechaAlta.Text = string.Empty;
            txtCorreo.Text = string.Empty;

            ddlEstatus.SelectedIndex = -1;

            ddlPais.SelectedIndex = -1;
            ddlEstado.Items.Clear();
            ddlMunicipio.Items.Clear();
            ddlCiudad.Items.Clear();
            ddlColonia.Items.Clear();
            txtCalle.Text = string.Empty;
            txtEntreCalle.Text = string.Empty;
            txtYCalle.Text = string.Empty;
            txtNumeroExterior.Text = string.Empty;
            txtNumeroInterior.Text = string.Empty;
            txtCodigoPostal.Text = string.Empty;
            txtReferencia.Text = string.Empty;

            ddlTipoTelefono.SelectedIndex = -1;
            txtNumero.Text = string.Empty;
        }

        protected void gvFranquiciatarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvFranquiciatarios, "Select$" + e.Row.RowIndex);
            }
        }

        protected void gvFranquiciatarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                ViewState["Accion"] = "Actualizar";
                DesbloquearCampos();
                btnCerrar.Visible = false;
                btnCancelar.Visible = true;
                btnGuardar.Visible = true;
                btnEditar.Visible = false;
                lblTituloModal.Text = "Actualizar Franquicia";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvFranquiciatarios.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidFranquiciatario"] = dataKeys;

                ManejoDatos(dataKeys);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }

            if (e.CommandName == "Ver")
            {
                BloquearCampos();
                btnCerrar.Visible = true;
                btnCancelar.Visible = false;
                btnGuardar.Visible = false;
                btnEditar.Visible = true;

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvFranquiciatarios.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidFranquiciatario"] = dataKeys;

                ManejoDatos(dataKeys);

                lblTituloModal.Text = "Visualización de la Franquicia";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }

            if (e.CommandName == "btnCredencialesLigas")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvFranquiciatarios.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidFranquiciatario"] = dataKeys;

                //lblTituloModal.Text = "Visualización de la Franquicia";

                parametrosEntradaServices.ObtenerParametrosEntrada(dataKeys);
                txtIdCompany.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdCompany;
                txtIdBranch.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.IdBranch;
                txtMoneda.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchModena;
                txtUsuario.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUsuario;
                txtPassword.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchPassword;
                txtCanal.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchCanal;
                txtData.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchData0;
                txtUrl.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchUrl;
                txtSemillaAES.Text = parametrosEntradaServices.parametrosEntradaRepository.parametrosEntrada.VchSemillaAES;

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

        private void ManejoDatos(Guid dataKeys)
        {
            //==================FRANQUICIATARIO============================
            franquiciatariosServices.ObtenerFraquiciatario(dataKeys);
            txtRFC.Text = franquiciatariosServices.franquiciatarios.VchRFC;
            txtRazonSocial.Text = franquiciatariosServices.franquiciatarios.VchRazonSocial;
            txtNombreComercial.Text = franquiciatariosServices.franquiciatarios.VchNombreComercial;
            txtFechaAlta.Text = franquiciatariosServices.franquiciatarios.DtFechaAlta.ToString("dd/MM/yyyy");
            txtCorreo.Text = franquiciatariosServices.franquiciatarios.VchCorreoElectronico;
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(franquiciatariosServices.franquiciatarios.UidEstatus.ToString()));

            //==================DIRECCIÓN==================================
            direccionesFranquiciatariosServices.ObtenerDireccionesFranquiciatarios(dataKeys);
            ddlPais.SelectedIndex = ddlPais.Items.IndexOf(ddlPais.Items.FindByValue(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidPais.ToString()));
            MuestraEstados(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidPais.ToString());
            ddlEstado.SelectedIndex = ddlEstado.Items.IndexOf(ddlEstado.Items.FindByValue(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidEstado.ToString()));
            MuestraMunicipio(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidEstado.ToString());
            ddlMunicipio.SelectedIndex = ddlMunicipio.Items.IndexOf(ddlMunicipio.Items.FindByValue(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidMunicipio.ToString()));
            MuestraCiudades(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidMunicipio.ToString());
            ddlCiudad.SelectedIndex = ddlCiudad.Items.IndexOf(ddlCiudad.Items.FindByValue(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidCiudad.ToString()));
            MuestraColonia(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidCiudad.ToString());
            ddlColonia.SelectedIndex = ddlColonia.Items.IndexOf(ddlColonia.Items.FindByValue(direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.UidColonia.ToString()));
            txtCalle.Text = direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.Calle;
            txtEntreCalle.Text = direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.EntreCalle;
            txtYCalle.Text = direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.YCalle;
            txtNumeroExterior.Text = direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.NumeroExterior;
            txtNumeroInterior.Text = direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.NumeroInterior;
            txtCodigoPostal.Text = direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.CodigoPostal;
            txtReferencia.Text = direccionesFranquiciatariosServices.direccionesFranquiciatariosRepository.direccionesFranquiciatarios.Referencia;
            //==================TELÉFONO===================================
            telefonosFranquiciatariosServices.ObtenerTelefonoFranquiciatario(dataKeys);
            txtNumero.Text = telefonosFranquiciatariosServices.telefonosFranquiciatariosRepository.telefonosFranquiciatarios.VchTelefono;
            ddlTipoTelefono.SelectedIndex = ddlTipoTelefono.Items.IndexOf(ddlTipoTelefono.Items.FindByValue(telefonosFranquiciatariosServices.telefonosFranquiciatariosRepository.telefonosFranquiciatarios.UidTipoTelefono.ToString()));
        }

        #region Combobox de Direcciones

        protected void MuestraEstados(string UidPais)
        {
            estadosServices.CargarEstados(UidPais);
            ddlEstado.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
            ddlEstado.DataSource = estadosServices.lsEstados;
            ddlEstado.DataTextField = "VchEstado";
            ddlEstado.DataValueField = "UidEstado";
            ddlEstado.DataBind();
        }
        protected void MuestraMunicipio(string UidEstado)
        {
            municipiosServices.CargarMunicipios(UidEstado);
            ddlMunicipio.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
            ddlMunicipio.DataSource = municipiosServices.lsMunicipios;
            ddlMunicipio.DataTextField = "VchMunicipio";
            ddlMunicipio.DataValueField = "UidMunicipio";
            ddlMunicipio.DataBind();
        }
        protected void MuestraCiudades(string UidMunicipio)
        {
            ciudadesServices.CargarCiudades(UidMunicipio);
            ddlCiudad.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
            ddlCiudad.DataSource = ciudadesServices.lsCiudades;
            ddlCiudad.DataTextField = "VchCiudad";
            ddlCiudad.DataValueField = "UidCiudad";
            ddlCiudad.DataBind();
        }
        protected void MuestraColonia(string UidCiudad)
        {
            coloniasServices.CargarColonias(UidCiudad);
            ddlColonia.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
            ddlColonia.DataSource = coloniasServices.lsColonias;
            ddlColonia.DataTextField = "VchColonia";
            ddlColonia.DataValueField = "UidColonia";
            ddlColonia.DataBind();
        }
        #endregion

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {

            ViewState["Accion"] = "Actualizar";
            DesbloquearCampos();
            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;

            btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
        }

        protected void gvFranquiciatarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid UidFranquicia = new Guid(gvFranquiciatarios.SelectedDataKey.Value.ToString());

            Session["UidFranquiciaMaster"] = UidFranquicia;

            franquiciatariosServices.ObtenerFraquiciatario(UidFranquicia);
            Master.NombreComercial.Text = "<< " + franquiciatariosServices.franquiciatarios.VchNombreComercial + " >>";
            Session["NombreComercial"] = franquiciatariosServices.franquiciatarios.VchNombreComercial;
            Master.MenuFranquicia.Attributes.Add("class", "dropdown-toggle font-weight-bold");
        }


        protected void btnValidarCorreo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCorreo.Text))
            {
                if (validacionesServices.ExisteCorreo(txtCorreo.Text))
                {
                    //lblExiste.Text = "Correo existente";
                    //lblNoExiste.Text = string.Empty;
                }
                else
                {
                    //lblNoExiste.Text = "Correo valido";
                    //lblExiste.Text = string.Empty;
                }
            }
            else
            {
                //lblNoExiste.Text = string.Empty;
                //lblExiste.Text = string.Empty;
            }
        }

        protected void gvFranquiciatarios_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvFranquiciatarios"] != null)
            {
                direccion = (SortDirection)ViewState["gvFranquiciatarios"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvFranquiciatarios"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvFranquiciatarios"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "VchRFC":
                        if (Orden == "ASC")
                        {
                            franquiciatariosServices.lsFranquiciasGridViewModel = franquiciatariosServices.lsFranquiciasGridViewModel.OrderBy(x => x.VchRFC).ToList();
                        }
                        else
                        {
                            franquiciatariosServices.lsFranquiciasGridViewModel = franquiciatariosServices.lsFranquiciasGridViewModel.OrderByDescending(x => x.VchRFC).ToList();
                        }
                        break;
                    case "VchRazonSocial":
                        if (Orden == "ASC")
                        {
                            franquiciatariosServices.lsFranquiciasGridViewModel = franquiciatariosServices.lsFranquiciasGridViewModel.OrderBy(x => x.VchRazonSocial).ToList();
                        }
                        else
                        {
                            franquiciatariosServices.lsFranquiciasGridViewModel = franquiciatariosServices.lsFranquiciasGridViewModel.OrderByDescending(x => x.VchRazonSocial).ToList();
                        }
                        break;
                    case "VchNombreComercial":
                        if (Orden == "ASC")
                        {
                            franquiciatariosServices.lsFranquiciasGridViewModel = franquiciatariosServices.lsFranquiciasGridViewModel.OrderBy(x => x.VchNombreComercial).ToList();
                        }
                        else
                        {
                            franquiciatariosServices.lsFranquiciasGridViewModel = franquiciatariosServices.lsFranquiciasGridViewModel.OrderByDescending(x => x.VchNombreComercial).ToList();
                        }
                        break;
                    case "UidEstatus":
                        if (Orden == "ASC")
                        {
                            franquiciatariosServices.lsFranquiciasGridViewModel = franquiciatariosServices.lsFranquiciasGridViewModel.OrderBy(x => x.UidEstatus).ToList();
                        }
                        else
                        {
                            franquiciatariosServices.lsFranquiciasGridViewModel = franquiciatariosServices.lsFranquiciasGridViewModel.OrderByDescending(x => x.UidEstatus).ToList();
                        }
                        break;
                }

                gvFranquiciatarios.DataSource = franquiciatariosServices.lsFranquiciasGridViewModel;
                gvFranquiciatarios.DataBind();
            }
        }

        protected void gvFranquiciatarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFranquiciatarios.PageIndex = e.NewPageIndex;
            gvFranquiciatarios.DataSource = franquiciatariosServices.lsFranquiciasGridViewModel;
            gvFranquiciatarios.DataBind();
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
                            if (parametrosEntradaServices.ActualizarParametrosEntrada(txtIdCompany.Text, txtIdBranch.Text, txtMoneda.Text, txtUsuario.Text, txtPassword.Text, txtCanal.Text, txtData.Text, txtUrl.Text, txtSemillaAES.Text, Guid.Parse(ViewState["UidFranquiciatario"].ToString())))
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
                            if (parametrosEntradaServices.RegistrarParametrosEntrada(txtIdCompany.Text, txtIdBranch.Text, txtMoneda.Text, txtUsuario.Text, txtPassword.Text, txtCanal.Text, txtData.Text, txtUrl.Text, txtSemillaAES.Text, Guid.Parse(ViewState["UidFranquiciatario"].ToString())))
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