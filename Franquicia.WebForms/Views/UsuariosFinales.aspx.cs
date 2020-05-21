using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using Franquicia.WebForms.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Vista;

namespace Franquicia.WebForms.Views
{
    public partial class Usuarios : System.Web.UI.Page
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
        PrefijosTelefonicosServices prefijosTelefonicosServices = new PrefijosTelefonicosServices();

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
                ViewState["gvAdministradores"] = SortDirection.Ascending;

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
                Session["prefijosTelefonicosServices"] = prefijosTelefonicosServices;

                usuariosCompletosServices.CargarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                gvAdministradores.DataBind();

                estatusService.CargarEstatus();
                ddlEstatus.DataSource = estatusService.lsEstatus;
                ddlEstatus.DataTextField = "VchDescripcion";
                ddlEstatus.DataValueField = "UidEstatus";
                ddlEstatus.DataBind();

                prefijosTelefonicosServices.CargarPrefijosTelefonicos();
                ddlPrefijo.DataSource = prefijosTelefonicosServices.lsPrefijosTelefonicos;
                ddlPrefijo.DataTextField = "VchCompleto";
                ddlPrefijo.DataValueField = "UidPrefijo";
                ddlPrefijo.DataBind();

                FiltroEstatus.DataSource = estatusService.lsEstatus;
                FiltroEstatus.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
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

                perfilesServices.CargarPerfilesClienteDropDownListModel(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
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
                prefijosTelefonicosServices = (PrefijosTelefonicosServices)Session["prefijosTelefonicosServices"];

                lblValidar.Text = string.Empty;

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

            ViewState["txtUsuario.Text"] = string.Empty;
            ViewState["txtPassword.Text"] = string.Empty;
            ViewState["txtRepetirPassword.Text"] = string.Empty;

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

            if (string.IsNullOrEmpty(txtUsuario.Text) && string.IsNullOrEmpty(txtPassword.Text) && string.IsNullOrEmpty(txtRepetirPassword.Text))
            {
                string[] Descripcion = Regex.Split(txtNombre.Text.Trim().ToUpper(), " ");
                int numMax = Descripcion.Length;
                ViewState["txtUsuario.Text"] = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + txtApePaterno.Text.Trim().ToUpper();

                if (validacionesServices.ExisteUsuario(ViewState["txtUsuario.Text"].ToString()))
                {
                    DateTime dateTime = DateTime.Now;

                    ViewState["txtUsuario.Text"] = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + txtApeMaterno.Text.Trim().ToUpper();

                    if (validacionesServices.ExisteUsuario(ViewState["txtUsuario.Text"].ToString()))
                    {
                        ViewState["txtUsuario.Text"] = Descripcion[numMax - 1].Substring(0, 1).ToString() + "." + txtApePaterno.Text.Trim().ToUpper() + dateTime.ToString("mmssff");
                    }
                }

                Random obj = new Random();
                string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                int longitud = posibles.Length;
                char letra;
                int longitudnuevacadena = 8;
                for (int i = 0; i < longitudnuevacadena; i++)
                {
                    letra = posibles[obj.Next(longitud)];
                    ViewState["txtPassword.Text"] += letra.ToString();
                    ViewState["txtRepetirPassword.Text"] += letra.ToString();
                }
            }
            else
            {
                if (txtUsuario.EmptyTextBox())
                {
                    lblValidar.Text = "El campo Usuario es obligatorio";
                    return;
                }
                else
                {
                    ViewState["txtUsuario.Text"] = txtUsuario.Text;
                }

                if (txtPassword.EmptyTextBox())
                {
                    lblValidar.Text = "El campo Contraseña es obligatorio";
                    return;
                }
                else
                {
                    ViewState["txtPassword.Text"] = txtPassword.Text;
                }

                if (txtRepetirPassword.EmptyTextBox())
                {
                    lblValidar.Text = "El campo Repetir contraseña es obligatorio";
                    return;
                }
                else
                {
                    ViewState["txtRepetirPassword.Text"] = txtRepetirPassword.Text;
                }
            }

            if (txtCorreo.EmptyTextBox())
            {
                lblValidar.Text = "El campo Correo Eléctronico es obligatorio";
                return;
            }

            if (ddlEstatus.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Estatus es obligatorio";
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
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == new Guid("6D896A85-ABAD-473F-BE0F-2B0CA6F3BC9F")).ToList();
            foreach (var item in permisosMenuModels)
            {
                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        if (!validacionesServices.ExisteUsuario(ViewState["txtUsuario.Text"].ToString()))
                        {
                            if (ViewState["txtPassword.Text"].ToString().Equals(ViewState["txtRepetirPassword.Text"].ToString()))
                            {
                                if (!validacionesServices.ExisteCorreo(txtCorreo.Text))
                                {
                                    Guid UidUsuario = Guid.NewGuid();

                                    if (usuariosCompletosServices.RegistrarUsuarios(UidUsuario,
                                    txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), ViewState["txtUsuario.Text"].ToString().Trim().ToUpper(), ViewState["txtPassword.Text"].ToString().Trim(), new Guid("18E9669B-C238-4BCC-9213-AF995644A5A4"),
                                    txtNumero.Text.Trim(), Guid.Parse("B1055882-BCBA-4AB7-94FA-90E57647E607"), new Guid(ddlPrefijo.SelectedValue), new Guid(ViewState["UidClienteLocal"].ToString())))
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
                                                divAlert.Attributes.Add("class", "alert alert-darger alert-dismissible fade show");
                                            }
                                        }
                                        else
                                        {
                                            pnlAlert.Visible = true;
                                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                        }

                                        usuariosCompletosServices.CargarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
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
                                if (usuariosCompletosServices.ActualizarUsuarios(
                                new Guid(ViewState["UidRequerido"].ToString()), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), new Guid(ddlEstatus.SelectedValue), txtUsuario.Text.Trim().ToUpper(), txtPassword.Text.Trim(), new Guid("18E9669B-C238-4BCC-9213-AF995644A5A4"),
                                txtNumero.Text.Trim(), new Guid(ddlTipoTelefono.SelectedValue), new Guid(ddlPrefijo.SelectedValue), new Guid(ViewState["UidClienteLocal"].ToString())))
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
                                                divAlert.Attributes.Add("class", "alert alert-darger alert-dismissible fade show");
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
                                                divAlert.Attributes.Add("class", "alert alert-darger alert-dismissible fade show");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        pnlAlert.Visible = true;
                                        lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                    }

                                    usuariosCompletosServices.CargarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
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
                        lblMensajeAlert.Text = "<b>Lo sentimos,</b> no tiene permisos para esta acción.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                    }
                }
                else if (ViewState["Accion"].ToString() == "AsosiarUsuario")
                {
                    if (validacionesServices.ExisteUsuarioCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString())))
                    {
                        lblValidar.Text = "Lo sentimos, el usuario ya esta asociado.";
                    }
                    else
                    {
                        if (usuariosCompletosServices.AsociarClienteUsuario(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString())))
                        {
                            usuariosCompletosServices.CargarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                            gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                            gvAdministradores.DataBind();

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                        }
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
            pnlAsosiarUsuario.Visible = true;
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
            ddlPrefijo.Enabled = false;
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
            ddlPrefijo.Enabled = true;
        }
        private void LimpiarCampos()
        {
            FiltroCorreoUsuario.Text = string.Empty;

            txtNombre.Text = string.Empty;
            txtApePaterno.Text = string.Empty;
            txtApeMaterno.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtPassword.Attributes.Add("value", "");
            txtRepetirPassword.Attributes.Add("value", "");
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
            ddlPrefijo.SelectedIndex = ddlPrefijo.Items.IndexOf(ddlPrefijo.Items.FindByValue("abb854c4-e7ed-420f-8561-aa4b61bf5b0f"));
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
                pnlAsosiarUsuario.Visible = false;
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
                pnlAsosiarUsuario.Visible = false;
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

        private void ManejoDatos(Guid dataKeys)
        {
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
            telefonosUsuariosServices.ObtenerTelefonoUsuario(dataKeys);
            txtNumero.Text = telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.VchTelefono;
            ddlTipoTelefono.SelectedIndex = ddlTipoTelefono.Items.IndexOf(ddlTipoTelefono.Items.FindByValue(telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.UidTipoTelefono.ToString()));
            ddlPrefijo.SelectedIndex = ddlPrefijo.Items.IndexOf(ddlPrefijo.Items.FindByValue(telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.UidPrefijo.ToString()));
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
            pnlAsosiarUsuario.Visible = false;
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

        protected void gvAdministradores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdministradores.PageIndex = e.NewPageIndex;
            gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            gvAdministradores.DataBind();
        }

        protected void gvAdministradores_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
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

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            usuariosCompletosServices.BuscarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"), FiltroNombre.Text, FiltroApePaterno.Text, FiltroApeMaterno.Text, FiltroCorreo.Text, Guid.Parse(FiltroEstatus.SelectedValue));
            gvAdministradores.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            gvAdministradores.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        protected void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            if (FiltroCorreoUsuario.EmptyTextBox())
            {
                lblValidar.Text = "Por favor ingrese un correo.";
                return;
            }

            usuariosCompletosServices.AsociarUsuariosFinales(FiltroCorreoUsuario.Text);

            //==================FRANQUICIATARIO============================
            txtNombre.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrNombre;
            txtApePaterno.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrApePaterno;
            txtApeMaterno.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrApeMaterno;
            txtCorreo.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrCorreo;
            txtUsuario.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.VchUsuario;
            txtPassword.Attributes.Add("value", usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.VchContrasenia);
            txtRepetirPassword.Attributes.Add("value", usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.VchContrasenia);
            ddlPerfil.SelectedIndex = ddlPerfil.Items.IndexOf(ddlPerfil.Items.FindByValue(usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidSegPerfil.ToString()));
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidEstatus.ToString()));

            //==================DIRECCIÓN==================================
            direccionesUsuariosServices.ObtenerDireccionesUsuarios(usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario);
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
            telefonosUsuariosServices.ObtenerTelefonoUsuario(usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario);
            txtNumero.Text = telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.VchTelefono;
            ddlTipoTelefono.SelectedIndex = ddlTipoTelefono.Items.IndexOf(ddlTipoTelefono.Items.FindByValue(telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.UidTipoTelefono.ToString()));

            if (usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario != Guid.Empty && usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario != null)
            {
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

                ViewState["usuarioCompleto.UidUsuario"] = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidUsuario;
                ViewState["Accion"] = "AsosiarUsuario";
                btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Asociar";
                BloquearCampos();
            }
            else
            {
                ViewState["Accion"] = "Guardar";
                lblValidar.Text = "Lo sentimos no hemos encontado ningun usuario.";
                DesbloquearCampos();
            }
        }

        protected void btnLimpiarUsuario_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
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