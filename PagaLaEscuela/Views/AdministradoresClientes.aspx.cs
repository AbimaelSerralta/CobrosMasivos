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
    public partial class AdministradoresClientes : System.Web.UI.Page
    {
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();
        DireccionesUsuariosServices direccionesUsuariosServices = new DireccionesUsuariosServices();
        TelefonosUsuariosServices telefonosUsuariosServices = new TelefonosUsuariosServices();
        ClientesServices clientesServices = new ClientesServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();

        EstatusServices estatusService = new EstatusServices();
        TiposTelefonosServices tiposTelefonosServices = new TiposTelefonosServices();
        PaisesServices paisesServices = new PaisesServices();
        EstadosServices estadosServices = new EstadosServices();
        MunicipiosServices municipiosServices = new MunicipiosServices();
        CiudadesServices ciudadesServices = new CiudadesServices();
        ColoniasServices coloniasServices = new ColoniasServices();

        PerfilesServices perfilesServices = new PerfilesServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["gvAdministradores"] = SortDirection.Ascending;
                ViewState["SoExgvAdministradores"] = "";

                #region Direccion
                Session["paisesServices"] = paisesServices;
                Session["estadosServices"] = estadosServices;
                Session["municipiosServices"] = municipiosServices;
                Session["ciudadesServices"] = ciudadesServices;
                Session["coloniasServices"] = coloniasServices;
                #endregion

                Session["usuariosCompletosServices"] = usuariosCompletosServices;
                //Session["telefonosUsuariosServices"] = telefonosUsuariosServices;
                Session["estatusService"] = estatusService;
                Session["clientesServices"] = clientesServices;

                if (Session["UidFranquiciaMaster"] != null)
                {
                    ViewState["UidFranquiciaLocal"] = Session["UidFranquiciaMaster"];
                }
                else
                {
                    ViewState["UidFranquiciaLocal"] = Guid.Empty;
                }

                usuariosCompletosServices.CargarAdministradoresCliente(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("4EF31BD5-95AB-4172-AF94-E25A3CADAD74"));
                gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                gvAdministradores.DataBind();

                estatusService.CargarEstatus();
                ddlEstatus.DataSource = estatusService.lsEstatus;
                ddlEstatus.DataTextField = "VchDescripcion";
                ddlEstatus.DataValueField = "UidEstatus";
                ddlEstatus.DataBind();

                FiltroEstatus.DataSource = estatusService.lsEstatus;
                FiltroEstatus.Items.Insert(0, new ListItem("TODOS", "00000000-0000-0000-0000-000000000000"));
                FiltroEstatus.DataTextField = "VchDescripcion";
                FiltroEstatus.DataValueField = "UidEstatus";
                FiltroEstatus.DataBind();

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

                perfilesServices.CargarPerfilesFranquiciaDropDownListModel(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("4EF31BD5-95AB-4172-AF94-E25A3CADAD74"));
                ddlPerfil.DataSource = perfilesServices.lsPerfilesDropDownListModel;
                ddlPerfil.DataTextField = "VchNombre";
                ddlPerfil.DataValueField = "UidSegPerfil";
                ddlPerfil.DataBind();
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

                usuariosCompletosServices = (UsuariosCompletosServices)Session["usuariosCompletosServices"];
                //telefonosUsuariosServices = (TelefonosUsuariosServices)Session["telefonosUsuariosServices"];

                estatusService = (EstatusServices)Session["estatusService"];
                tiposTelefonosServices = (TiposTelefonosServices)Session["tiposTelefonosServices"];
                clientesServices = (ClientesServices)Session["clientesServices"];

                lblValidar.Text = string.Empty;

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                txtPassword.Attributes.Add("value", txtPassword.Text);
                txtRepetirPassword.Attributes.Add("value", txtRepetirPassword.Text);
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
            #region ValidarCampos
            if (FiltroRFC.EmptyTextBox())
            {
                lblValidar.Text = "El campo RFC es obligatorio";
                return;
            }
            if (FiltroRazonSocial.EmptyTextBox())
            {
                lblValidar.Text = "El campo Razon Social es obligatorio";
                return;
            }
            if (FiltroNombreComercial.EmptyTextBox())
            {
                lblValidar.Text = "El campo Nombre Comercial es obligatorio";
                return;
            }

            if (Guid.Parse(ViewState["UidCliente"].ToString()) == Guid.Empty)
            {
                lblValidar.Text = "Por favor seleccione un comercio";
                return;
            }

            if (txtNombre.EmptyTextBox())
            {
                lblValidar.Text = "El campo Nombre es obligatorio";
                return;
            }

            if (txtApePaterno.EmptyTextBox())
            {
                lblValidar.Text = "El campo Apellido Paterno es obligatorio";
                return;
            }

            if (txtApeMaterno.EmptyTextBox())
            {
                lblValidar.Text = "El campo Apellido Materno es obligatorio";
                return;
            }

            if (txtCorreo.EmptyTextBox())
            {
                lblValidar.Text = "El campo Correo Eléctronico es obligatorio";
                return;
            }

            if (txtUsuario.EmptyTextBox())
            {
                lblValidar.Text = "El campo Usuario es obligatorio";
                return;
            }

            if (txtPassword.EmptyTextBox())
            {
                lblValidar.Text = "El campo Contraseña es obligatorio";
                return;
            }
            if (txtRepetirPassword.EmptyTextBox())
            {
                lblValidar.Text = "El campo Repetir contraseña es obligatorio";
                return;
            }

            if (ddlEstatus.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Municipio es obligatorio";
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
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == Guid.Parse("34C2AD6C-0F91-4DF5-9A35-501E6279003D")).ToList();
            foreach (var item in permisosMenuModels)
            {
                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        if (!validacionesServices.ExisteUsuario(txtUsuario.Text.Trim()))
                        {
                            if (txtPassword.Text.Equals(txtRepetirPassword.Text.Trim()))
                            {
                                if (!validacionesServices.ExisteCorreo(txtCorreo.Text.Trim()))
                                {
                                    if (usuariosCompletosServices.RegistrarAdministradoresCliente(
                                    txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), txtUsuario.Text.Trim().ToUpper(), txtPassword.Text.Trim(), bool.Parse(ViewState["BitEstatus"].ToString()), Guid.Parse("d2c80d47-c14c-4677-a63d-c46bcb50fe17"), Guid.Parse("85b6ce16-bace-489e-8e75-2280f72605f1"),
                                    txtIdentificador.Text.Trim().ToUpper(), Guid.Parse(ddlPais.SelectedValue), Guid.Parse(ddlEstado.SelectedValue), Guid.Parse(ddlMunicipio.SelectedValue), Guid.Parse(ddlCiudad.SelectedValue), Guid.Parse(ddlColonia.SelectedValue), txtCalle.Text.Trim().ToUpper(), txtEntreCalle.Text.Trim().ToUpper(), txtYCalle.Text.Trim().ToUpper(), txtNumeroExterior.Text.Trim().ToUpper(), txtNumeroInterior.Text.Trim().ToUpper(), txtCodigoPostal.Text.Trim().ToUpper(), txtReferencia.Text.Trim().ToUpper(),
                                    txtNumero.Text.Trim(), Guid.Parse(ddlTipoTelefono.SelectedValue), Guid.Parse(ViewState["UidCliente"].ToString())))
                                    {
                                        usuariosCompletosServices.CargarAdministradoresCliente(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("4EF31BD5-95AB-4172-AF94-E25A3CADAD74"));
                                        gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                                        gvAdministradores.DataBind();

                                        pnlAlert.Visible = true;
                                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                                    }
                                }
                                else
                                {
                                    lblValidar.Text = "El correo ingresado ya existe por favor intente con otro.";
                                }
                            }
                            else
                            {
                                lblValidar.Text = "Las contraseña ingresadas no son iguales por favor reviselo.";
                            }
                        }
                        else
                        {
                            lblValidar.Text = "El usuario ingresado ya existe por favor intente con otro.";
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
                        bool Actualizar = false;

                        if (txtPassword.Text.Equals(txtRepetirPassword.Text))
                        {
                            if (ViewState["ActualizarCorreo"].ToString() != txtCorreo.Text)
                            {
                                if (validacionesServices.ExisteCorreo(txtCorreo.Text))
                                {
                                    lblValidar.Text = "El correo ingresado ya existe por favor intente con otro.";
                                    return;
                                }
                                else
                                {
                                    Actualizar = true;
                                }
                            }
                            else
                            {
                                Actualizar = true;
                            }

                            if (Actualizar)
                            {
                                if (usuariosCompletosServices.ActualizarAdministradoresCliente(
                                Guid.Parse(ViewState["UidRequerido"].ToString()), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), Guid.Parse(ddlEstatus.SelectedValue), txtUsuario.Text.Trim().ToUpper(), txtPassword.Text.Trim(), bool.Parse(ViewState["BitEstatus"].ToString()), Guid.Parse("d2c80d47-c14c-4677-a63d-c46bcb50fe17"), Guid.Parse("85b6ce16-bace-489e-8e75-2280f72605f1"),
                                txtIdentificador.Text.Trim().ToUpper(), Guid.Parse(ddlPais.SelectedValue), Guid.Parse(ddlEstado.SelectedValue), Guid.Parse(ddlMunicipio.SelectedValue), Guid.Parse(ddlCiudad.SelectedValue), Guid.Parse(ddlColonia.SelectedValue), txtCalle.Text.Trim().ToUpper(), txtEntreCalle.Text.Trim().ToUpper(), txtYCalle.Text.Trim().ToUpper(), txtNumeroExterior.Text.Trim().ToUpper(), txtNumeroInterior.Text.Trim().ToUpper(), txtCodigoPostal.Text.Trim().ToUpper(), txtReferencia.Text.Trim().ToUpper(),
                                txtNumero.Text.Trim(), Guid.Parse(ddlTipoTelefono.SelectedValue), Guid.Parse(ViewState["UidCliente"].ToString())))
                                {
                                    usuariosCompletosServices.CargarAdministradoresCliente(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("4EF31BD5-95AB-4172-AF94-E25A3CADAD74"));
                                    gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                                    gvAdministradores.DataBind();

                                    pnlAlert.Visible = true;
                                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                                }
                            }
                        }
                        else
                        {
                            lblValidar.Text = "Las contraseña ingresadas no son iguales por favor reviselo.";
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
            lblTituloModal.Text = "Registro de Administrador";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
        }
        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }
        protected void btnFiltroBuscar_Click(object sender, EventArgs e)
        {
            usuariosCompletosServices.BuscarAdministradoresCliente(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("4EF31BD5-95AB-4172-AF94-E25A3CADAD74"), FiltroNombre.Text, FiltroApePaterno.Text, FiltroApeMaterno.Text, FiltroCorreo.Text, FiltroEscuela.Text, Guid.Parse(FiltroEstatus.SelectedValue));
            gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            gvAdministradores.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }
        protected void btnFiltroLimpiar_Click(object sender, EventArgs e)
        {
            FiltroNombre.Text = string.Empty;
            FiltroApePaterno.Text = string.Empty;
            FiltroApeMaterno.Text = string.Empty;
            FiltroCorreo.Text = string.Empty;
            FiltroEscuela.Text = string.Empty;
            FiltroEstatus.SelectedIndex = 0;
        }
        private void BloquearCampos()
        {
            txtNombre.Enabled = false;
            txtApePaterno.Enabled = false;
            txtApeMaterno.Enabled = false;
            txtCorreo.Enabled = false;
            txtUsuario.Enabled = false;
            txtPassword.Enabled = false;
            txtRepetirPassword.Enabled = false;
            ddlPerfil.Enabled = false;
            ddlEstatus.Enabled = false;

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
            txtNombre.Enabled = true;
            txtApePaterno.Enabled = true;
            txtApeMaterno.Enabled = true;
            txtCorreo.Enabled = true;
            txtPassword.Enabled = true;
            txtRepetirPassword.Enabled = true;
            ddlPerfil.Enabled = true;

            if (ViewState["Accion"].ToString() == "Actualizar")
            {
                txtUsuario.Enabled = false;
            }
            else
            {
                txtUsuario.Enabled = true;
            }

            if (ViewState["Accion"].ToString() == "Guardar")
            {
                ddlEstatus.Enabled = false;
            }
            else
            {
                ddlEstatus.Enabled = true;
            }

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
            FiltroRFC.Text = string.Empty;
            FiltroRazonSocial.Text = string.Empty;
            FiltroNombreComercial.Text = string.Empty;
            ViewState["UidCliente"] = Guid.Empty;

            txtNombre.Text = string.Empty;
            txtApePaterno.Text = string.Empty;
            txtApeMaterno.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtPassword.Attributes.Add("value", string.Empty);
            txtRepetirPassword.Attributes.Add("value", string.Empty);
            ddlPerfil.SelectedIndex = -1;
            ddlEstatus.SelectedIndex = -1;

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

        protected void gvAdministradores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvAdministradores, "Select$" + e.Row.RowIndex);
            }
        }
        protected void gvAdministradores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                ViewState["Accion"] = "Actualizar";
                DesbloquearCampos();
                btnCerrar.Visible = false;
                btnCancelar.Visible = true;
                btnGuardar.Visible = true;
                btnEditar.Visible = false;
                lblTituloModal.Text = "Actualizar Administrador";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvAdministradores.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

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
                GridViewRow Seleccionado = gvAdministradores.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);

                lblTituloModal.Text = "Visualización de Administrador";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
        }
        protected void gvAdministradores_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            ViewState["SoExgvAdministradores"] = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvAdministradores"] != null)
            {
                direccion = (SortDirection)ViewState["gvAdministradores"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvAdministradores"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvAdministradores"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "NombreCompleto":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderByDescending(x => x.NombreCompleto).ToList();
                        }
                        break;
                    case "StrCorreo":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderBy(x => x.StrCorreo).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderByDescending(x => x.StrCorreo).ToList();
                        }
                        break;
                    case "VchUsuario":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderBy(x => x.VchUsuario).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderByDescending(x => x.VchUsuario).ToList();
                        }
                        break;
                    case "VchNombreComercial":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderBy(x => x.VchNombreComercial).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderByDescending(x => x.VchNombreComercial).ToList();
                        }
                        break;
                    case "UidEstatus":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderBy(x => x.UidEstatus).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderByDescending(x => x.UidEstatus).ToList();
                        }
                        break;
                }

                gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                gvAdministradores.DataBind();
            }
        }
        protected void gvAdministradores_RowCreated(object sender, GridViewRowEventArgs e)
        {
            SortDirection direccion = (SortDirection)ViewState["gvAdministradores"];
            string SortExpression = ViewState["SoExgvAdministradores"].ToString();

            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        // Buscar el enlace de la cabecera
                        LinkButton lnk = tc.Controls[0] as LinkButton;
                        if (lnk != null && SortExpression == lnk.CommandArgument)
                        {
                            // Verificar que se está ordenando por el campo indicado en el comando de ordenación
                            // Crear una imagen
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            img.Height = 20;
                            img.Width = 20;
                            // Ajustar dinámicamente el icono adecuado
                            img.ImageUrl = "~/Images/SortingGv/" + (direccion == SortDirection.Ascending ? "desc" : "asc") + ".png";
                            img.ImageAlign = ImageAlign.AbsMiddle;
                            // Le metemos un espacio delante de la imagen para que no se pegue al enlace
                            tc.Controls.Add(new LiteralControl(""));
                            tc.Controls.Add(img);
                        }
                    }
                }
            }
        }
        protected void gvAdministradores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdministradores.PageIndex = e.NewPageIndex;
            gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            gvAdministradores.DataBind();
        }

        private void ManejoDatos(Guid dataKeys)
        {
            //===================EMPRESA===================================
            clientesServices.ObtenerClientes(dataKeys);
            ViewState["UidCliente"] = clientesServices.clientesRepository.clientesGridViewModel.UidCliente;
            FiltroRFC.Text = clientesServices.clientesRepository.clientesGridViewModel.VchRFC;
            FiltroRazonSocial.Text = clientesServices.clientesRepository.clientesGridViewModel.VchRazonSocial;
            FiltroNombreComercial.Text = clientesServices.clientesRepository.clientesGridViewModel.VchNombreComercial;
            ViewState["BitEstatus"] = clientesServices.clientesRepository.clientesGridViewModel.BitEscuela;

            //==================FRANQUICIATARIO============================
            usuariosCompletosServices.ObtenerAdministrador(dataKeys);
            txtNombre.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrNombre;
            txtApePaterno.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrApePaterno;
            txtApeMaterno.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrApeMaterno;
            txtCorreo.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrCorreo;
            ViewState["ActualizarCorreo"] = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrCorreo;
            txtUsuario.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.VchUsuario;
            txtPassword.Attributes.Add("value", usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.VchContrasenia);
            txtRepetirPassword.Attributes.Add("value", usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.VchContrasenia);
            ddlPerfil.SelectedIndex = ddlPerfil.Items.IndexOf(ddlPerfil.Items.FindByValue(usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidSegPerfil.ToString()));
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidEstatus.ToString()));

            //==================DIRECCIÓN==================================
            direccionesUsuariosServices.ObtenerDireccionesUsuarios(dataKeys);
            txtIdentificador.Text = direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.Identificador;
            ddlPais.SelectedIndex = ddlPais.Items.IndexOf(ddlPais.Items.FindByValue(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidPais.ToString()));
            MuestraEstados(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidPais.ToString());
            ddlEstado.SelectedIndex = ddlEstado.Items.IndexOf(ddlEstado.Items.FindByValue(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidEstado.ToString()));
            MuestraMunicipio(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidEstado.ToString());
            ddlMunicipio.SelectedIndex = ddlMunicipio.Items.IndexOf(ddlMunicipio.Items.FindByValue(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidMunicipio.ToString()));
            MuestraCiudades(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidMunicipio.ToString());
            ddlCiudad.SelectedIndex = ddlCiudad.Items.IndexOf(ddlCiudad.Items.FindByValue(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidCiudad.ToString()));
            MuestraColonia(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidCiudad.ToString());
            ddlColonia.SelectedIndex = ddlColonia.Items.IndexOf(ddlColonia.Items.FindByValue(direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.UidColonia.ToString()));
            txtCalle.Text = direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.Calle;
            txtEntreCalle.Text = direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.EntreCalle;
            txtYCalle.Text = direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.YCalle;
            txtNumeroExterior.Text = direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.NumeroExterior;
            txtNumeroInterior.Text = direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.NumeroInterior;
            txtCodigoPostal.Text = direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.CodigoPostal;
            txtReferencia.Text = direccionesUsuariosServices.direccionesUsuariosRepository.direccionesUsuarios.Referencia;
            //==================TELÉFONO===================================
            telefonosUsuariosServices.ObtenerTelefonoUsuarioSinPrefijo(dataKeys);
            txtNumero.Text = telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.VchTelefono;
            ddlTipoTelefono.SelectedIndex = ddlTipoTelefono.Items.IndexOf(ddlTipoTelefono.Items.FindByValue(telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.UidTipoTelefono.ToString()));
        }

        protected void gvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvClientes, "Select$" + e.Row.RowIndex);
            }
        }
        protected void gvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvClientes.SelectedRow;

            Guid UidCliente = Guid.Parse(gvClientes.SelectedDataKey.Value.ToString());

            ViewState["UidCliente"] = UidCliente;

            FiltroRFC.Text = clientesServices.lsClientesGridViewModel[row.RowIndex].VchRFC;
            FiltroRazonSocial.Text = clientesServices.lsClientesGridViewModel[row.RowIndex].VchRazonSocial;
            FiltroNombreComercial.Text = clientesServices.lsClientesGridViewModel[row.RowIndex].VchNombreComercial;
            ViewState["BitEstatus"] = clientesServices.lsClientesGridViewModel[row.RowIndex].BitEscuela;

            //FiltroRFC.Text = HttpUtility.HtmlDecode(gvClientes.Rows[gvClientes.SelectedRow.RowIndex].Cells[1].Text);
            //FiltroRazonSocial.Text = HttpUtility.HtmlDecode(gvClientes.Rows[gvClientes.SelectedRow.RowIndex].Cells[2].Text);
            //FiltroNombreComercial.Text = HttpUtility.HtmlDecode(gvClientes.Rows[gvClientes.SelectedRow.RowIndex].Cells[3].Text);

            panelFranquicias.Visible = false;
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            clientesServices.CargarClientes(Guid.Parse(Session["UidFranquiciaMaster"].ToString()));
            gvClientes.DataSource = clientesServices.lsClientesGridViewModel;
            gvClientes.DataBind();

            panelFranquicias.Visible = true;
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            FiltroRFC.Text = string.Empty;
            FiltroRazonSocial.Text = string.Empty;
            FiltroNombreComercial.Text = string.Empty;
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

        protected void btnValidarUsuario_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text))
            {
                if (validacionesServices.ExisteUsuario(txtUsuario.Text))
                {
                    lblExisteUsuario.Text = "Usuario existente";
                    lblNoExisteUsuario.Text = string.Empty;
                }
                else
                {
                    lblNoExisteUsuario.Text = "Usuario valido";
                    lblExisteUsuario.Text = string.Empty;
                }
            }
            else
            {
                lblNoExisteUsuario.Text = string.Empty;
                lblExisteUsuario.Text = string.Empty;
            }
        }
    }
}