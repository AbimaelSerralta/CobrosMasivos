using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using PagaLaEscuela.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class Clientes : System.Web.UI.Page
    {
        ClientesServices clientesServices = new ClientesServices();
        DireccionesClientesServices direccionesClientesServices = new DireccionesClientesServices();
        TelefonosClientesServices telefonosClientesServices = new TelefonosClientesServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();

        EstatusServices estatusService = new EstatusServices();
        TiposTelefonosServices tiposTelefonosServices = new TiposTelefonosServices();
        PaisesServices paisesServices = new PaisesServices();
        EstadosServices estadosServices = new EstadosServices();
        MunicipiosServices municipiosServices = new MunicipiosServices();
        CiudadesServices ciudadesServices = new CiudadesServices();
        ColoniasServices coloniasServices = new ColoniasServices();
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();

        SuperPromocionesServices superPromocionesServices = new SuperPromocionesServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        ComisionesTarjetasClientesServices comisionesTarjetasClServices = new ComisionesTarjetasClientesServices();
        ComisionesTarjetasServices comisionesTarjetasServices = new ComisionesTarjetasServices();

        SuperPromocionesPragaServices superPromocionesPragaServices = new SuperPromocionesPragaServices();
        SuperComisionesTarjetasPragaServices superComisionesTarjetasPragaServices = new SuperComisionesTarjetasPragaServices();

        PromocionesPragaServices promocionesPragaServices = new PromocionesPragaServices();
        ComisionesTarjetasPragaServices comisionesTarjetasPragaServices = new ComisionesTarjetasPragaServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideAlert()", true);

                #region Direccion
                Session["paisesServices"] = paisesServices;
                Session["estadosServices"] = estadosServices;
                Session["municipiosServices"] = municipiosServices;
                Session["ciudadesServices"] = ciudadesServices;
                Session["coloniasServices"] = coloniasServices;
                #endregion

                Session["clientesServices"] = clientesServices;
                Session["telefonosClientesServices"] = telefonosClientesServices;
                Session["estatusService"] = estatusService;

                if (Session["UidFranquiciaMaster"] != null)
                {
                    ViewState["UidFranquiciaLocal"] = Session["UidFranquiciaMaster"];
                }
                else
                {
                    ViewState["UidFranquiciaLocal"] = Guid.Empty;
                }

                clientesServices.CargarClientes(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()));
                gvClientes.DataSource = clientesServices.lsClientesGridViewModel;
                gvClientes.DataBind();

                ddlZonaHoraria.DataSource = TimeZoneInfo.GetSystemTimeZones();
                ddlZonaHoraria.DataTextField = "DisplayName";
                ddlZonaHoraria.DataValueField = "Id";
                ddlZonaHoraria.DataBind();

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

                clientesServices = (ClientesServices)Session["clientesServices"];
                telefonosClientesServices = (TelefonosClientesServices)Session["telefonosClientesServices"];

                estatusService = (EstatusServices)Session["estatusService"];
                tiposTelefonosServices = (TiposTelefonosServices)Session["tiposTelefonosServices"];

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Sh", "shot()", true);

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
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
            if (txtCodigoPostal.EmptyTextBox())
            {
                lblValidar.Text = "El campo Código Postal es obligatorio";
                return;
            }
            if (ddlTipoTelefono.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Tipo Telefono es obligatorio";
                return;
            }
            if (txtNumero.EmptyTextBox())
            {
                lblValidar.Text = "El campo Número es obligatorio";
                return;
            }
            if (!string.IsNullOrEmpty(lblValidar.Text))
            {
                lblValidar.Text = string.Empty;
            }
            #endregion

            List<PermisosMenuModel> permisosMenuModels = (List<PermisosMenuModel>)Session["lsAccesosPermitidos"];
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == Guid.Parse("7066023F-E1F9-45A5-AF50-3F6BCFFB1EBA")).ToList();
            foreach (var item in permisosMenuModels)
            {
                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        Guid UidCliente = Guid.NewGuid();

                        if (clientesServices.RegistrarClientes(UidCliente,
                            txtRFC.Text.Trim(), txtRazonSocial.Text.Trim(), txtNombreComercial.Text.Trim(), DateTime.Parse(thisDay.ToString("dd/MM/yyyy HH:mm:ss")), txtCorreo.Text.Trim(), txtIdentificadorWASMS.Text.Trim(), ddlZonaHoraria.SelectedValue, bool.Parse(ddlEscuela.SelectedValue),
                            txtIdentificador.Text.Trim(), Guid.Parse(ddlPais.SelectedValue), Guid.Parse(ddlEstado.SelectedValue), Guid.Parse(ddlMunicipio.SelectedValue), Guid.Parse(ddlCiudad.SelectedValue), Guid.Parse(ddlColonia.SelectedValue), txtCalle.Text.Trim(), txtEntreCalle.Text.Trim(), txtYCalle.Text.Trim(), txtNumeroExterior.Text.Trim(), txtNumeroInterior.Text.Trim(), txtCodigoPostal.Text.Trim(), txtReferencia.Text.Trim(),
                            txtNumero.Text.Trim(), Guid.Parse(ddlTipoTelefono.SelectedValue), Guid.Parse(ViewState["UidFranquiciaLocal"].ToString())
                            ))
                        {

                            superPromocionesServices.CargarSuperPromociones();
                            foreach (var itPromo in superPromocionesServices.lsCBLSuperPromociones)
                            {
                                try
                                {
                                    promocionesServices.RegistrarPromociones(UidCliente, itPromo.UidPromocion, itPromo.DcmComicion, itPromo.DcmApartirDe);
                                }
                                catch (Exception ex)
                                {
                                    var s = ex.Message;
                                }
                            }

                            comisionesTarjetasServices.CargarComisionesTarjeta();
                            foreach (var itComi in comisionesTarjetasServices.lsComisionesTarjetas)
                            {
                                try
                                {
                                    comisionesTarjetasClServices.RegistrarComisionesTarjeta(itComi.BitComision, itComi.DcmComision, UidCliente);
                                }
                                catch (Exception ex)
                                {
                                    var s = ex.Message;
                                }
                            }

                            superPromocionesPragaServices.CargarSuperPromocionesPraga();
                            foreach (var itemPromoPraga in superPromocionesPragaServices.lsCBLSuperPromocionesPraga)
                            {
                                try
                                {
                                    promocionesPragaServices.RegistrarPromocionesCliente(UidCliente, itemPromoPraga.UidPromocion, itemPromoPraga.DcmComicion, itemPromoPraga.DcmApartirDe, itemPromoPraga.UidTipoTarjeta);
                                }
                                catch (Exception ex)
                                {
                                    var s = ex.Message;
                                }
                            }

                            superComisionesTarjetasPragaServices.CargarComisionesTarjeta();
                            foreach (var itemComiPraga in superComisionesTarjetasPragaServices.lsSuperComisionesTarjetasPraga)
                            {
                                try
                                {
                                    comisionesTarjetasPragaServices.RegistrarComisionesTarjeta(itemComiPraga.BitComision, itemComiPraga.DcmComision, itemComiPraga.UidTipoTarjeta, UidCliente);
                                }
                                catch (Exception ex)
                                {
                                    var s = ex.Message;
                                }
                            }

                            clientesServices.CargarClientes(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()));
                            gvClientes.DataSource = clientesServices.lsClientesGridViewModel;
                            gvClientes.DataBind();

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
                        if (clientesServices.ActualizarClientes(
                        Guid.Parse(ViewState["UidFranquiciatario"].ToString()), txtRFC.Text.Trim().ToUpper(), txtRazonSocial.Text.Trim().ToUpper(), txtNombreComercial.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), Guid.Parse(ddlEstatus.SelectedValue), txtIdentificadorWASMS.Text.Trim(), ddlZonaHoraria.SelectedValue, bool.Parse(ddlEscuela.SelectedValue),
                        txtIdentificador.Text.Trim().ToUpper(), Guid.Parse(ddlPais.SelectedValue), Guid.Parse(ddlEstado.SelectedValue), Guid.Parse(ddlMunicipio.SelectedValue), Guid.Parse(ddlCiudad.SelectedValue), Guid.Parse(ddlColonia.SelectedValue), txtCalle.Text.Trim().ToUpper(), txtEntreCalle.Text.Trim().ToUpper(), txtYCalle.Text.Trim().ToUpper(), txtNumeroExterior.Text.Trim().ToUpper(), txtNumeroInterior.Text.Trim().ToUpper(), txtCodigoPostal.Text.Trim(), txtReferencia.Text.Trim().ToUpper(),
                        txtNumero.Text.Trim(), Guid.Parse(ddlTipoTelefono.SelectedValue)))
                        {
                            //Actualizar perfil de usuario para PAGALAESCUELA
                            usuariosCompletosServices.CargarAdminCliente(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse(ViewState["UidFranquiciatario"].ToString()), Guid.Parse("4EF31BD5-95AB-4172-AF94-E25A3CADAD74"));
                            foreach (var itUsu in usuariosCompletosServices.lsActualizarUsuarios)
                            {
                                usuariosCompletosServices.ActualizarAdminClientePerfilEscu(itUsu.UidSegUsuario, bool.Parse(ddlEscuela.SelectedValue), Guid.Parse("85b6ce16-bace-489e-8e75-2280f72605f1"));
                            }

                            clientesServices.CargarClientes(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()));
                            gvClientes.DataSource = clientesServices.lsClientesGridViewModel;
                            gvClientes.DataBind();

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
            lblValidar.Text = string.Empty;
            ViewState["Accion"] = "Guardar";
            LimpiarCampos();
            DesbloquearCampos();
            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            lblTituloModal.Text = "Registro del Comercio";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ddlZonaHoraria.SelectedIndex = ddlZonaHoraria.Items.IndexOf(ddlZonaHoraria.Items.FindByValue("Central Standard Time (Mexico)"));

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
        }

        private void BloquearCampos()
        {
            txtIdentificadorWASMS.Enabled = false;
            txtRFC.Enabled = false;
            txtRazonSocial.Enabled = false;
            txtNombreComercial.Enabled = false;
            txtCorreo.Enabled = false;

            ddlEstatus.Enabled = false;
            ddlZonaHoraria.Enabled = false;
            ddlEscuela.Enabled = false;

            txtIdentificador.Enabled = false;
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
            txtIdentificadorWASMS.Enabled = true;
            txtRFC.Enabled = true;
            txtRazonSocial.Enabled = true;
            txtNombreComercial.Enabled = true;
            txtCorreo.Enabled = true;

            ddlEstatus.Enabled = true;

            ddlZonaHoraria.Enabled = true;
            ddlEscuela.Enabled = true;

            txtIdentificador.Enabled = true;
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
            txtIdentificadorWASMS.Text = string.Empty;
            txtRFC.Text = string.Empty;
            txtRazonSocial.Text = string.Empty;
            txtNombreComercial.Text = string.Empty;
            txtFechaAlta.Text = string.Empty;
            txtCorreo.Text = string.Empty;

            ddlEstatus.SelectedIndex = -1;
            ddlEscuela.SelectedIndex = 1;

            txtIdentificador.Text = string.Empty;
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

        protected void gvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvClientes, "Select$" + e.Row.RowIndex);
            }
        }
        protected void gvClientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                ViewState["Accion"] = "Actualizar";
                DesbloquearCampos();
                btnCerrar.Visible = false;
                btnCancelar.Visible = true;
                btnGuardar.Visible = true;
                btnEditar.Visible = false;
                lblTituloModal.Text = "Actualizar Comercio";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvClientes.Rows[index];
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
                GridViewRow Seleccionado = gvClientes.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidFranquiciatario"] = dataKeys;

                ManejoDatos(dataKeys);

                lblTituloModal.Text = "Visualización del Cliente";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
        }
        protected void gvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid UidCliente = Guid.Parse(gvClientes.SelectedDataKey.Value.ToString());

            if (Guid.Parse(Session["UidClienteMaster"].ToString()) != UidCliente)
            {
                Session["UidClienteMaster"] = UidCliente;

                clientesServices.ObtenerCliente(UidCliente);
                Master.NombreCliente.Text = "<< " + clientesServices.clientes.VchNombreComercial + " >>";
                Session["NombreClienteMaster"] = clientesServices.clientes.VchNombreComercial;
                Master.MenuCliente.Attributes.Add("class", "dropdown-toggle font-weight-bold");
            }
        }
        private void ManejoDatos(Guid dataKeys)
        {
            //==================FRANQUICIATARIO============================
            clientesServices.ObtenerCliente(dataKeys);
            txtIdentificadorWASMS.Text = clientesServices.clientes.VchIdWAySMS;
            txtRFC.Text = clientesServices.clientes.VchRFC;
            txtRazonSocial.Text = clientesServices.clientes.VchRazonSocial;
            txtNombreComercial.Text = clientesServices.clientes.VchNombreComercial;
            txtFechaAlta.Text = clientesServices.clientes.DtFechaAlta.ToString("dd/MM/yyyy");
            txtCorreo.Text = clientesServices.clientes.VchCorreoElectronico;
            ddlZonaHoraria.SelectedIndex = ddlZonaHoraria.Items.IndexOf(ddlZonaHoraria.Items.FindByValue(clientesServices.clientes.VchZonaHoraria));
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(clientesServices.clientes.UidEstatus.ToString()));

            if (clientesServices.clientes.BitEscuela)
            {
                ddlEscuela.SelectedIndex = 1;
            }
            else
            {
                ddlEscuela.SelectedIndex = 0;
            }


            //==================DIRECCIÓN==================================
            direccionesClientesServices.ObtenerDireccionCliente(dataKeys);
            txtIdentificador.Text = direccionesClientesServices.direccionesClientesRepository.direccionesClientes.Identificador;
            ddlPais.SelectedIndex = ddlPais.Items.IndexOf(ddlPais.Items.FindByValue(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidPais.ToString()));
            MuestraEstados(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidPais.ToString());
            ddlEstado.SelectedIndex = ddlEstado.Items.IndexOf(ddlEstado.Items.FindByValue(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidEstado.ToString()));
            MuestraMunicipio(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidEstado.ToString());
            ddlMunicipio.SelectedIndex = ddlMunicipio.Items.IndexOf(ddlMunicipio.Items.FindByValue(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidMunicipio.ToString()));
            MuestraCiudades(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidMunicipio.ToString());
            ddlCiudad.SelectedIndex = ddlCiudad.Items.IndexOf(ddlCiudad.Items.FindByValue(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidCiudad.ToString()));
            MuestraColonia(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidCiudad.ToString());
            ddlColonia.SelectedIndex = ddlColonia.Items.IndexOf(ddlColonia.Items.FindByValue(direccionesClientesServices.direccionesClientesRepository.direccionesClientes.UidColonia.ToString()));
            txtCalle.Text = direccionesClientesServices.direccionesClientesRepository.direccionesClientes.Calle;
            txtEntreCalle.Text = direccionesClientesServices.direccionesClientesRepository.direccionesClientes.EntreCalle;
            txtYCalle.Text = direccionesClientesServices.direccionesClientesRepository.direccionesClientes.YCalle;
            txtNumeroExterior.Text = direccionesClientesServices.direccionesClientesRepository.direccionesClientes.NumeroExterior;
            txtNumeroInterior.Text = direccionesClientesServices.direccionesClientesRepository.direccionesClientes.NumeroInterior;
            txtCodigoPostal.Text = direccionesClientesServices.direccionesClientesRepository.direccionesClientes.CodigoPostal;
            txtReferencia.Text = direccionesClientesServices.direccionesClientesRepository.direccionesClientes.Referencia;
            //==================TELÉFONO===================================
            telefonosClientesServices.ObtenerTelefonoCliente(dataKeys);
            txtNumero.Text = telefonosClientesServices.telefonosClientesRepository.telefonosClientes.VchTelefono;
            ddlTipoTelefono.SelectedIndex = ddlTipoTelefono.Items.IndexOf(ddlTipoTelefono.Items.FindByValue(telefonosClientesServices.telefonosClientesRepository.telefonosClientes.UidTipoTelefono.ToString()));
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

        protected void btnValidarCorreo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCorreo.Text))
            {
                if (validacionesServices.ExisteCorreo(txtCorreo.Text))
                {
                    lblExiste.Text = "Correo existente";
                    lblNoExiste.Text = string.Empty;
                }
                else
                {
                    lblNoExiste.Text = "Correo valido";
                    lblExiste.Text = string.Empty;
                }
            }
            else
            {
                lblNoExiste.Text = string.Empty;
                lblExiste.Text = string.Empty;
            }
        }
    }
}