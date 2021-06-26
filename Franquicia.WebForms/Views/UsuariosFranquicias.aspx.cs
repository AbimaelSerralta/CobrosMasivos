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
    public partial class AdministradoresFranquicias : System.Web.UI.Page
    {
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();
        DireccionesUsuariosServices direccionesUsuariosServices = new DireccionesUsuariosServices();
        TelefonosUsuariosServices telefonosUsuariosServices = new TelefonosUsuariosServices();
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

                if (Session["UidFranquiciaMaster"] != null)
                {
                    ViewState["UidFranquiciaLocal"] = Session["UidFranquiciaMaster"];
                }
                else
                {
                    ViewState["UidFranquiciaLocal"] = Guid.Empty;
                }

                usuariosCompletosServices.CargarAdministradoresFranquicia(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("8490C81D-5979-49AC-92CC-E34A50A497D5"));
                gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                gvAdministradores.DataBind();

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

                perfilesServices.CargarPerfilesFranquiciaDropDownListModel(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("8490C81D-5979-49AC-92CC-E34A50A497D5"));
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
            ddlEstado.Items.Clear();
            ddlMunicipio.Items.Clear();
            ddlColonia.Items.Clear();
            ddlCiudad.Items.Clear();

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
            string Identificador = string.Empty;
            Guid Pais = Guid.Empty;
            Guid Estado = Guid.Empty;
            Guid Municipio = Guid.Empty;
            Guid Ciudad = Guid.Empty;
            Guid Colonia = Guid.Empty;
            string Calle = string.Empty;
            string EntreCalle = string.Empty;
            string YCalle = string.Empty;
            string NumeroExterior = string.Empty;
            string NumeroInterior = string.Empty;
            string CodigoPostal = string.Empty;
            string Referencia = string.Empty;

            #region ValidarCampos

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
            if (txtNumero.EmptyTextBox())
            {
                lblValidar.Text = "El campo Número es obligatorio";
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
            if (ddlPerfil.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Estado es obligatorio";
                return;
            }
            if (ddlEstatus.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Municipio es obligatorio";
                return;
            }

            if (ddlIncluirDir.SelectedValue == "SI")
            {
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

                Identificador = string.Empty;
                Pais = Guid.Parse(ddlPais.SelectedValue);
                Estado = Guid.Parse(ddlEstado.SelectedValue);
                Municipio = Guid.Parse(ddlMunicipio.SelectedValue);
                Ciudad = Guid.Parse(ddlCiudad.SelectedValue);
                Colonia = Guid.Parse(ddlColonia.SelectedValue);
                Calle = txtCalle.Text.Trim().ToUpper();
                EntreCalle = txtEntreCalle.Text.Trim().ToUpper();
                YCalle = txtYCalle.Text.Trim().ToUpper();
                NumeroExterior = txtNumeroExterior.Text.Trim().ToUpper();
                NumeroInterior = txtNumeroInterior.Text.Trim().ToUpper();
                CodigoPostal = txtCodigoPostal.Text.Trim().ToUpper();
                Referencia = txtReferencia.Text.Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(lblValidar.Text))
            {
                lblValidar.Text = string.Empty;
            }
            #endregion

            List<PermisosMenuModel> permisosMenuModels = (List<PermisosMenuModel>)Session["lsAccesosPermitidos"];
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == Guid.Parse("67172786-B5FE-4E05-8FFE-AF8B2A31491C")).ToList();
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
                                    Guid UidUsuario = Guid.NewGuid();

                                    if (usuariosCompletosServices.RegistrarAdministradoresFranquicia(UidUsuario,
                                    txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), txtUsuario.Text.Trim().ToUpper(), txtPassword.Text.Trim(), Guid.Parse(ddlPerfil.SelectedValue), Guid.Parse(ddlPerfil.SelectedValue),
                                    txtNumero.Text.Trim(), Guid.Parse(ddlTipoTelefono.SelectedValue), Guid.Parse(Session["UidFranquiciaMaster"].ToString())))
                                    {
                                        if (ddlIncluirDir.SelectedValue == "SI")
                                        {
                                            if (usuariosCompletosServices.RegistrarDireccionUsuarios(UidUsuario, Identificador, Pais, Estado, Municipio, Ciudad, Colonia, Calle, EntreCalle, YCalle, NumeroExterior, NumeroInterior, CodigoPostal, Referencia))
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                            }
                                            else
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<b>Lo sentimos, </b> el usuario se ha registrado exitosamente, sin embargo la dirección no se pudo registrar.";
                                                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                                            }
                                        }
                                        else
                                        {
                                            pnlAlert.Visible = true;
                                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                        }

                                        usuariosCompletosServices.CargarAdministradoresFranquicia(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("8490C81D-5979-49AC-92CC-E34A50A497D5"));
                                        gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                                        gvAdministradores.DataBind();

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
                                if (usuariosCompletosServices.ActualizarAdministradoresFranquicia(
                                Guid.Parse(ViewState["UidRequerido"].ToString()), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), Guid.Parse(ddlEstatus.SelectedValue), txtUsuario.Text.Trim().ToUpper(), txtPassword.Text.Trim(), Guid.Parse(ddlPerfil.SelectedValue), Guid.Parse(ddlPerfil.SelectedValue),
                                txtNumero.Text.Trim(), Guid.Parse(ddlTipoTelefono.SelectedValue), Guid.Parse(Session["UidFranquiciaMaster"].ToString())))
                                {
                                    if (ddlIncluirDir.SelectedValue == "SI")
                                    {
                                        if (validacionesServices.ExisteDireccionUsuario(Guid.Parse(ViewState["UidRequerido"].ToString())))
                                        {
                                            if (usuariosCompletosServices.ActualizarDireccionUsuarios(Guid.Parse(ViewState["UidRequerido"].ToString()), Identificador, Pais, Estado, Municipio, Ciudad, Colonia, Calle, EntreCalle, YCalle, NumeroExterior, NumeroInterior, CodigoPostal, Referencia))
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                            }
                                            else
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<b>Lo sentimos, </b> los datos del usuario se ha actualizado exitosamente, sin embargo la dirección no se pudo registrar.";
                                                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                                            }
                                        }
                                        else
                                        {
                                            if (usuariosCompletosServices.RegistrarDireccionUsuarios(Guid.Parse(ViewState["UidRequerido"].ToString()), Identificador, Pais, Estado, Municipio, Ciudad, Colonia, Calle, EntreCalle, YCalle, NumeroExterior, NumeroInterior, CodigoPostal, Referencia))
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                            }
                                            else
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<b>Lo sentimos, </b> el usuario se ha registrado exitosamente, sin embargo la dirección no se pudo registrar.";
                                                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        pnlAlert.Visible = true;
                                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                    }

                                    usuariosCompletosServices.CargarAdministradoresFranquicia(Guid.Parse(ViewState["UidFranquiciaLocal"].ToString()), Guid.Parse("8490C81D-5979-49AC-92CC-E34A50A497D5"));
                                    gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                                    gvAdministradores.DataBind();

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
            ddlIncluirDir.SelectedIndex = 0;
            ddlIncluirDir.Enabled = true;
            pnlIncluirDir.Visible = false;

            lblValidar.Text = string.Empty;
            ViewState["Accion"] = "Guardar";
            LimpiarCampos();
            DesbloquearCampos();
            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            lblTituloModal.Text = "Registro de Usuario";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
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
            txtUsuario.Enabled = true;
            txtPassword.Enabled = true;
            txtRepetirPassword.Enabled = true;
            ddlPerfil.Enabled = true;
            ddlEstatus.Enabled = true;

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
            txtNombre.Text = string.Empty;
            txtApePaterno.Text = string.Empty;
            txtApeMaterno.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            lblExiste.Text = "";
            lblNoExiste.Text = "";

            txtUsuario.Text = string.Empty;
            lblExisteUsuario.Text = "";
            lblNoExisteUsuario.Text = "";
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
                lblTituloModal.Text = "Actualizar Usuario";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvAdministradores.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);

                if (!string.IsNullOrEmpty(txtCalle.Text) && !string.IsNullOrEmpty(txtEntreCalle.Text))
                {
                    ddlIncluirDir.SelectedIndex = 1;
                    ddlIncluirDir.Enabled = false;
                    pnlIncluirDir.Visible = true;
                }
                else
                {
                    ddlIncluirDir.SelectedIndex = 0;
                    ddlIncluirDir.Enabled = true;
                    pnlIncluirDir.Visible = false;
                }

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

                if (!string.IsNullOrEmpty(txtCalle.Text) && !string.IsNullOrEmpty(txtEntreCalle.Text))
                {
                    ddlIncluirDir.SelectedIndex = 1;
                    ddlIncluirDir.Enabled = false;
                    pnlIncluirDir.Visible = true;
                }
                else
                {
                    ddlIncluirDir.SelectedIndex = 0;
                    ddlIncluirDir.Enabled = true;
                    pnlIncluirDir.Visible = false;
                }

                lblTituloModal.Text = "Visualización de Usuario";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
        }
        protected void gvAdministradores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdministradores.PageIndex = e.NewPageIndex;
            gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            gvAdministradores.DataBind();
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
                    case "VchNombrePerfil":
                        if (Orden == "ASC")
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderBy(x => x.VchNombrePerfil).ToList();
                        }
                        else
                        {
                            usuariosCompletosServices.lsUsuariosCompletos = usuariosCompletosServices.lsUsuariosCompletos.OrderByDescending(x => x.VchNombrePerfil).ToList();
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

        private void ManejoDatos(Guid dataKeys)
        {
            lblExiste.Text = "";
            lblNoExiste.Text = "";

            lblExisteUsuario.Text = "";
            lblNoExisteUsuario.Text = "";

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

        protected void ddlIncluirDir_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlIncluirDir.SelectedValue == "SI")
            {
                pnlIncluirDir.Visible = true;
            }
            else
            {
                pnlIncluirDir.Visible = false;
            }
        }
    }
}