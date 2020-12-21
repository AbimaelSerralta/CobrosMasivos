using ClosedXML.Excel;
using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using PagaLaEscuela.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class Padres : System.Web.UI.Page
    {
        #region Propiedades
        // Nombre del servidor
        string ServerName
        {
            get { return Request.ServerVariables["SERVER_NAME"].ToString(); }
        }
        // Puerto del servidor
        string ServerPort
        {
            get { return Request.ServerVariables["SERVER_PORT"].ToString(); }
        }

        // Obtener URL Base
        public string URLBase
        {
            get
            {
                if (ServerPort != string.Empty && ServerPort.Trim() != "")
                { return "https://" + ServerName + ":" + ServerPort + "/"; }
                else
                { return "https://" + ServerName + "/"; }
            }
        }
        #endregion

        PadresServices padresServices = new PadresServices();
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
        AlumnosServices alumnosServices = new AlumnosServices();
        CorreosEscuelaServices correosEscuelaServices = new CorreosEscuelaServices();

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
                btnCargarExcel.Attributes.Add("onclick", "document.getElementById('" + fuSelecionarExcel.ClientID + "').click(); return false;");
                fuSelecionarExcel.Attributes["onchange"] = "UploadFile(this)";

                ViewState["gvPadres"] = SortDirection.Ascending;
                ViewState["gvAlumnos"] = SortDirection.Ascending;

                #region Direccion
                Session["paisesServices"] = paisesServices;
                Session["estadosServices"] = estadosServices;
                Session["municipiosServices"] = municipiosServices;
                Session["ciudadesServices"] = ciudadesServices;
                Session["coloniasServices"] = coloniasServices;
                #endregion

                Session["padresServices"] = padresServices;
                //Session["telefonosUsuariosServices"] = telefonosUsuariosServices;
                Session["estatusService"] = estatusService;
                Session["prefijosTelefonicosServices"] = prefijosTelefonicosServices;
                Session["alumnosServices"] = alumnosServices;

                padresServices.CargarPadres(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                gvPadres.DataSource = padresServices.lsPadres;
                gvPadres.DataBind();

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

                paisesServices.CargarPaises();
                ddlPais.DataSource = paisesServices.lsPaises;
                ddlPais.DataTextField = "VchPais";
                ddlPais.DataValueField = "UidPais";
                ddlPais.DataBind();

                perfilesServices.CargarPerfilesClienteDropDownListModel(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
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

                padresServices = (PadresServices)Session["padresServices"];
                //telefonosUsuariosServices = (TelefonosUsuariosServices)Session["telefonosUsuariosServices"];

                estatusService = (EstatusServices)Session["estatusService"];
                tiposTelefonosServices = (TiposTelefonosServices)Session["tiposTelefonosServices"];
                prefijosTelefonicosServices = (PrefijosTelefonicosServices)Session["prefijosTelefonicosServices"];
                alumnosServices = (AlumnosServices)Session["alumnosServices"];

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
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == Guid.Parse("4F6FDB13-85D0-4720-949F-6315FBC23B72")).ToList();
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

                                    if (padresServices.RegistrarPadres(UidUsuario,
                                    txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), ViewState["txtUsuario.Text"].ToString().Trim(), ViewState["txtPassword.Text"].ToString().Trim(), Guid.Parse("18E9669B-C238-4BCC-9213-AF995644A5A4"), Guid.Parse("A4B4F919-FDD2-4076-BD4A-59E4011E71C8"),
                                    txtNumero.Text.Trim(), Guid.Parse(ddlPrefijo.SelectedValue), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                                    {
                                        string NombreComercial = validacionesServices.ObtenerNombreClienteCompleto(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                                        CorreoEnvioCredenciales(txtNombre.Text.Trim().ToUpper() + " " + txtApePaterno.Text.Trim().ToUpper() + " " + txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), ViewState["txtPassword.Text"].ToString().Trim(), txtCorreo.Text.Trim(), NombreComercial);

                                        if (ddlIncluirDir.SelectedValue == "SI")
                                        {
                                            if (padresServices.RegistrarDireccionUsuarios(UidUsuario, Identificador, Pais, Estado, Municipio, Ciudad, Colonia, Calle, EntreCalle, YCalle, NumeroExterior, NumeroInterior, CodigoPostal, Referencia))
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha registrado exitosamente.";
                                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                            }
                                            else
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<strong>Lo sentimos, </strong> el usuario se ha registrado exitosamente, sin embargo la dirección no se pudo registrar.";
                                                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                                            }
                                        }
                                        else
                                        {
                                            pnlAlert.Visible = true;
                                            lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha registrado exitosamente.";
                                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                        }

                                        foreach (var itAlum in alumnosServices.lsSelectAlumnosGridViewModel)
                                        {
                                            alumnosServices.RegistrarClienteAlumnos(UidUsuario, itAlum.UidAlumno);
                                        }

                                        ViewState["NewPageIndex"] = null;
                                        padresServices.CargarPadres(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                                        gvPadres.DataSource = padresServices.lsPadres;
                                        gvPadres.DataBind();

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
                        lblMensajeAlert.Text = "<strong>Lo sentimos,</strong> no tiene permisos para esta acción.";
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
                                if (padresServices.ActualizarPadres(
                                Guid.Parse(ViewState["UidRequerido"].ToString()), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), Guid.Parse(ddlEstatus.SelectedValue), txtUsuario.Text.Trim().ToUpper(), txtPassword.Text.Trim(), Guid.Parse("18E9669B-C238-4BCC-9213-AF995644A5A4"),
                                txtNumero.Text.Trim(), Guid.Parse(ddlPrefijo.SelectedValue), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                                {
                                    if (ddlIncluirDir.SelectedValue == "SI")
                                    {
                                        if (validacionesServices.ExisteDireccionUsuario(Guid.Parse(ViewState["UidRequerido"].ToString())))
                                        {
                                            if (padresServices.ActualizarDireccionUsuarios(Guid.Parse(ViewState["UidRequerido"].ToString()), Identificador, Pais, Estado, Municipio, Ciudad, Colonia, Calle, EntreCalle, YCalle, NumeroExterior, NumeroInterior, CodigoPostal, Referencia))
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha actualizado exitosamente.";
                                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                            }
                                            else
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<strong>Lo sentimos, </strong> los datos del usuario se ha actualizado exitosamente, sin embargo la dirección no se pudo registrar.";
                                                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                                            }
                                        }
                                        else
                                        {
                                            if (padresServices.RegistrarDireccionUsuarios(Guid.Parse(ViewState["UidRequerido"].ToString()), Identificador, Pais, Estado, Municipio, Ciudad, Colonia, Calle, EntreCalle, YCalle, NumeroExterior, NumeroInterior, CodigoPostal, Referencia))
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha registrado exitosamente.";
                                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                            }
                                            else
                                            {
                                                pnlAlert.Visible = true;
                                                lblMensajeAlert.Text = "<strong>Lo sentimos, </strong> el usuario se ha registrado exitosamente, sin embargo la dirección no se pudo registrar.";
                                                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        pnlAlert.Visible = true;
                                        lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha actualizado exitosamente.";
                                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                    }

                                    alumnosServices.EliminarClienteAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["UidRequerido"].ToString()));
                                    foreach (var itAlum in alumnosServices.lsSelectAlumnosGridViewModel)
                                    {
                                        alumnosServices.RegistrarClienteAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()), itAlum.UidAlumno);
                                    }

                                    ViewState["NewPageIndex"] = null;
                                    padresServices.CargarPadres(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                                    gvPadres.DataSource = padresServices.lsPadres;
                                    gvPadres.DataBind();

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
                        lblMensajeAlert.Text = "<strong>Lo sentimos,</strong> no tiene permisos para esta acción.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                    }
                }
                else if (ViewState["Accion"].ToString() == "AsosiarUsuario")
                {
                    string MnsjCuenta = string.Empty;

                    if (validacionesServices.EstatusCuentaPadre(Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString())).ToUpper() != "2C859517-9507-4B5B-BB3E-C7341F6630DD")
                    {
                        MnsjCuenta = "<strong>La cuenta del padre esta en proceso de activación</strong>, sin embargo ";
                    }

                    if (validacionesServices.ExisteUsuarioCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString())))
                    {
                        lblValidar.Text = "Lo sentimos, el usuario ya esta asociado.";

                        if (padresServices.ActualizarAsociarClienteUsuario(Guid.Parse(ViewState["usuarioCompleto.UidSegUsuario"].ToString())))
                        {
                            alumnosServices.EliminarClienteAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString()));
                            foreach (var itAlum in alumnosServices.lsSelectAlumnosGridViewModel)
                            {
                                alumnosServices.RegistrarClienteAlumnos(Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString()), itAlum.UidAlumno);
                            }

                            ViewState["NewPageIndex"] = null;
                            padresServices.CargarPadres(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                            gvPadres.DataSource = padresServices.lsPadres;
                            gvPadres.DataBind();

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<strong>¡Felicidades! </strong>" + MnsjCuenta +  "se ha asociado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                        }
                    }
                    else
                    {
                        if (padresServices.AsociarClienteUsuario(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString())))
                        {
                            if (padresServices.ActualizarAsociarClienteUsuario(Guid.Parse(ViewState["usuarioCompleto.UidSegUsuario"].ToString())))
                            {
                                alumnosServices.EliminarClienteAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString()));
                                foreach (var itAlum in alumnosServices.lsSelectAlumnosGridViewModel)
                                {
                                    alumnosServices.RegistrarClienteAlumnos(Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString()), itAlum.UidAlumno);
                                }
                            }

                            ViewState["NewPageIndex"] = null;
                            padresServices.CargarPadres(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                            gvPadres.DataSource = padresServices.lsPadres;
                            gvPadres.DataBind();

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<strong>¡Felicidades! </strong>" + MnsjCuenta + "se ha asociado exitosamente.";
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

            ViewState["UidRequerido"] = Guid.Empty;

            lblValidar.Text = string.Empty;
            ViewState["Accion"] = "Guardar";
            LimpiarCampos();
            DesbloquearCampos();

            btnFiltroLimpiar_Click(null, null);
            alumnosServices.lsAlumnosGridViewModel.Clear();
            gvAlumnos.DataBind();
            alumnosServices.lsSelectAlumnosGridViewModel.Clear();
            lblCantSeleccionado.Text = alumnosServices.lsSelectAlumnosGridViewModel.Count().ToString();

            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            pnlAsosiarUsuario.Visible = true;
            lblTituloModal.Text = "Registro del Tutor";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "showTab()", true);
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

            txtNumero.Text = string.Empty;
            ddlPrefijo.SelectedIndex = ddlPrefijo.Items.IndexOf(ddlPrefijo.Items.FindByValue("abb854c4-e7ed-420f-8561-aa4b61bf5b0f"));
        }

        protected void gvPadres_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvPadres, "Select$" + e.Row.RowIndex);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblPaginado = (Label)e.Row.FindControl("lblPaginado");

                int PageSize = gvPadres.PageSize;
                int antNum = 0;

                int numTotal = padresServices.lsPadres.Count;

                if (numTotal >= 1)
                {
                    if (ViewState["NewPageIndex"] != null)
                    {
                        int gh = int.Parse(ViewState["NewPageIndex"].ToString());
                        ViewState["NewPageIndex"] = gh + 1;

                        int r1 = int.Parse(ViewState["NewPageIndex"].ToString()) * PageSize;
                        antNum = r1 - (PageSize - 1);
                    }
                    else
                    {
                        ViewState["NewPageIndex"] = 1;
                        antNum = 1;
                    }

                    int NewPageIndex = int.Parse(ViewState["NewPageIndex"].ToString());

                    int newNum = NewPageIndex * PageSize;

                    if (numTotal >= newNum)
                    {
                        lblPaginado.Text = "Del " + antNum + " al " + newNum + " de " + numTotal;
                    }
                    else
                    {
                        lblPaginado.Text = "Del " + antNum + " al " + numTotal + " de " + numTotal;
                    }

                    ViewState["lblPaginado"] = lblPaginado.Text;
                }
                else
                {
                    lblPaginado.Text = ViewState["lblPaginado"].ToString();
                }
            }
        }
        protected void gvPadres_RowCommand(object sender, GridViewCommandEventArgs e)
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
                lblTituloModal.Text = "Actualizar Tutor";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPadres.Rows[index];
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

                alumnosServices.ObtenerClienteAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()), dataKeys);
                lblCantSeleccionado.Text = alumnosServices.lsSelectAlumnosGridViewModel.Count.ToString();
                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                gvAlumnos.DataBind();

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
                GridViewRow Seleccionado = gvPadres.Rows[index];
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

                lblTituloModal.Text = "Visualización del Tutor";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }

            if (e.CommandName == "Enviar")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPadres.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                padresServices.ObtenerPadre(dataKeys);
                string Nombre = padresServices.padresRepository.padres.NombreCompleto;
                string Usuario = padresServices.padresRepository.padres.VchUsuario;
                string Contrasenia = padresServices.padresRepository.padres.VchContrasenia;
                string Correo = padresServices.padresRepository.padres.StrCorreo;
                string NombreComercial = padresServices.padresRepository.padres.VchNombreComercial;

                CorreoEnvioCredenciales(Nombre, Correo, Contrasenia, Correo, NombreComercial);
            }
        }
        protected void gvPadres_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPadres.PageIndex = e.NewPageIndex;
            ViewState["NewPageIndex"] = e.NewPageIndex;
            gvPadres.DataSource = padresServices.lsPadres;
            gvPadres.DataBind();
        }
        protected void gvPadres_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvPadres"] != null)
            {
                direccion = (SortDirection)ViewState["gvPadres"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvPadres"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvPadres"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "NombreCompleto":
                        if (Orden == "ASC")
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderByDescending(x => x.NombreCompleto).ToList();
                        }
                        break;
                    case "StrCorreo":
                        if (Orden == "ASC")
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderBy(x => x.StrCorreo).ToList();
                        }
                        else
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderByDescending(x => x.StrCorreo).ToList();
                        }
                        break;
                    case "StrTelefono":
                        if (Orden == "ASC")
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderBy(x => x.StrTelefono).ToList();
                        }
                        else
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderByDescending(x => x.StrTelefono).ToList();
                        }
                        break;
                    case "IntCantAlumnos":
                        if (Orden == "ASC")
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderBy(x => x.IntCantAlumnos).ToList();
                        }
                        else
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderByDescending(x => x.IntCantAlumnos).ToList();
                        }
                        break;
                    case "UidEstatus":
                        if (Orden == "ASC")
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderBy(x => x.UidEstatus).ToList();
                        }
                        else
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderByDescending(x => x.UidEstatus).ToList();
                        }
                        break;
                }

                ViewState["NewPageIndex"] = int.Parse(ViewState["NewPageIndex"].ToString()) - 1;
                gvPadres.DataSource = padresServices.lsPadres;
                gvPadres.DataBind();
            }
        }

        private void CorreoEnvioCredenciales(string Nombre, string Usuario, string Contrasenia, string Correo, string NombreComercial)
        {
            string Asunto = "Acceso al sistema PagaLaEscuela";
            string LigaUrl = URLBase + "Views/Login.aspx";

            string mnsj = correosEscuelaServices.CorreoEnvioCredenciales(Nombre, Asunto, Usuario, Contrasenia, LigaUrl, Correo, NombreComercial);

            if (mnsj == string.Empty)
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha enviado las credenciales de acceso exitosamente.";
                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<strong>Lo sentimos, </strong> no se ha podido enviar las credenciales de acceso, por favor intentelo más tarde. Si el error persiste comuniquese con los administradores.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }

        private void ManejoDatos(Guid dataKeys)
        {
            //==================FRANQUICIATARIO============================
            padresServices.ObtenerPadre(dataKeys);
            if (padresServices.padresRepository.padres.UidEstatusCuenta == Guid.Parse("2C859517-9507-4B5B-BB3E-C7341F6630DD"))
            {
                BloquearCampos();
            }
            else if (padresServices.padresRepository.padres.VchAccion == "CREADO")
            {
                DesbloquearCampos();
            }
            else if (padresServices.padresRepository.padres.VchAccion == "ASOCIADO")
            {
                BloquearCampos();
            }

            txtNombre.Text = padresServices.padresRepository.padres.StrNombre;
            txtApePaterno.Text = padresServices.padresRepository.padres.StrApePaterno;
            txtApeMaterno.Text = padresServices.padresRepository.padres.StrApeMaterno;
            txtCorreo.Text = padresServices.padresRepository.padres.StrCorreo;
            ViewState["ActualizarCorreo"] = padresServices.padresRepository.padres.StrCorreo;
            txtUsuario.Text = padresServices.padresRepository.padres.VchUsuario;
            txtPassword.Attributes.Add("value", padresServices.padresRepository.padres.VchContrasenia);
            txtRepetirPassword.Attributes.Add("value", padresServices.padresRepository.padres.VchContrasenia);
            ddlPerfil.SelectedIndex = ddlPerfil.Items.IndexOf(ddlPerfil.Items.FindByValue(padresServices.padresRepository.padres.UidSegPerfil.ToString()));
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(padresServices.padresRepository.padres.UidEstatus.ToString()));

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

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            FiltroEstatus.SelectedIndex = FiltroEstatus.Items.IndexOf(FiltroEstatus.Items.FindByValue("65e46bc9-1864-4145-ad1a-70f5b5f69739"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["NewPageIndex"] = null;

            string FiltroAlumno = FiltroAlumnos.Text;

            if (string.IsNullOrEmpty(FiltroAlumno))
            {
                FiltroAlumno = "todos";
            }

            padresServices.BuscarPadres(FiltroNombre.Text, FiltroApePaterno.Text, FiltroApeMaterno.Text, FiltroCorreo.Text, FiltroCelular.Text, FiltroAlumno, Guid.Parse(FiltroEstatus.SelectedValue), FiltroColegiatura.SelectedValue, Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
            gvPadres.DataSource = padresServices.lsPadres;
            gvPadres.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            FiltroNombre.Text = string.Empty;
            FiltroApePaterno.Text = string.Empty;
            FiltroApeMaterno.Text = string.Empty;
            FiltroCorreo.Text = string.Empty;
            FiltroCelular.Text = string.Empty;
            FiltroAlumnos.Text = string.Empty;
            FiltroEstatus.SelectedIndex = -1;
        }

        protected void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            if (FiltroCorreoUsuario.EmptyTextBox())
            {
                lblValidar.Text = "Por favor ingrese un correo.";
                return;
            }

            padresServices.AsociarUsuariosFinales(FiltroCorreoUsuario.Text);

            //==================FRANQUICIATARIO============================
            txtNombre.Text = padresServices.padresRepository.padres.StrNombre;
            txtApePaterno.Text = padresServices.padresRepository.padres.StrApePaterno;
            txtApeMaterno.Text = padresServices.padresRepository.padres.StrApeMaterno;
            txtCorreo.Text = padresServices.padresRepository.padres.StrCorreo;
            txtUsuario.Text = padresServices.padresRepository.padres.VchUsuario;
            txtPassword.Attributes.Add("value", padresServices.padresRepository.padres.VchContrasenia);
            txtRepetirPassword.Attributes.Add("value", padresServices.padresRepository.padres.VchContrasenia);
            ddlPerfil.SelectedIndex = ddlPerfil.Items.IndexOf(ddlPerfil.Items.FindByValue(padresServices.padresRepository.padres.UidSegPerfil.ToString()));
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(padresServices.padresRepository.padres.UidEstatus.ToString()));

            //==================DIRECCIÓN==================================
            direccionesUsuariosServices.ObtenerDireccionesUsuarios(padresServices.padresRepository.padres.UidUsuario);
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
            telefonosUsuariosServices.ObtenerTelefonoUsuario(padresServices.padresRepository.padres.UidUsuario);
            txtNumero.Text = telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.VchTelefono;

            if (padresServices.padresRepository.padres.UidUsuario != Guid.Empty && padresServices.padresRepository.padres.UidUsuario != null)
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

                ViewState["usuarioCompleto.UidSegUsuario"] = padresServices.padresRepository.padres.UidSegUsuario;
                ViewState["usuarioCompleto.UidUsuario"] = padresServices.padresRepository.padres.UidUsuario;
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

        protected void btnFiltroBuscar_Click(object sender, EventArgs e)
        {
            alumnosServices.AsignarAlumnos(alumnosServices.lsSelectAlumnosGridViewModel, Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["UidRequerido"].ToString()), ddlFiltroAsociado.SelectedValue, txtFiltroAlumIdentificador.Text, txtFiltroAlumNombre.Text.Trim(), txtFiltroAlumPaterno.Text.Trim(), txtFiltroAlumMaterno.Text.Trim(), txtFiltroAlumMatricula.Text.Trim());
            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
            gvAlumnos.DataBind();
        }
        protected void btnFiltroLimpiar_Click(object sender, EventArgs e)
        {
            ddlFiltroAsociado.SelectedIndex = -1;
            txtFiltroAlumIdentificador.Text = string.Empty;
            txtFiltroAlumNombre.Text = string.Empty;
            txtFiltroAlumPaterno.Text = string.Empty;
            txtFiltroAlumMaterno.Text = string.Empty;
            txtFiltroAlumMatricula.Text = string.Empty;
        }
        protected void gvAlumnos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvAlumnos"] != null)
            {
                direccion = (SortDirection)ViewState["gvAlumnos"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvAlumnos"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvAlumnos"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "VchIdentificador":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderBy(x => x.VchIdentificador).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderByDescending(x => x.VchIdentificador).ToList();
                        }
                        break;
                    case "VchMatricula":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderBy(x => x.VchMatricula).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderByDescending(x => x.VchMatricula).ToList();
                        }
                        break;
                    case "NombreCompleto":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderBy(x => x.NombreCompleto).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderByDescending(x => x.NombreCompleto).ToList();
                        }
                        break;
                }

                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                gvAlumnos.DataBind();
            }
        }
        protected void gvAlumnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAlumnos.PageIndex = e.NewPageIndex;
            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
            gvAlumnos.DataBind();
        }
        protected void gvAlumnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox cbTodo = (CheckBox)e.Row.FindControl("cbTodo");

                int count = alumnosServices.lsAlumnosGridViewModel.Count(x => x.blSeleccionado == false);

                if (count >= 1)
                {
                    cbTodo.Checked = false;
                }
                else
                {
                    cbTodo.Checked = true;
                }
            }
        }
        protected void cbTodo_CheckedChanged(object sender, EventArgs e)
        {
            bool cbTodo = ((CheckBox)gvAlumnos.HeaderRow.FindControl("cbTodo")).Checked;

            if (cbTodo)
            {
                alumnosServices.ActualizarLsAsignarAlumnosTodo(alumnosServices.lsAlumnosGridViewModel, true);
            }
            else
            {
                alumnosServices.ActualizarLsAsignarAlumnosTodo(alumnosServices.lsAlumnosGridViewModel, false);
            }

            lblCantSeleccionado.Text = alumnosServices.lsSelectAlumnosGridViewModel.Count.ToString();
            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
            gvAlumnos.DataBind();
        }
        protected void cbSeleccionado_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)checkBox.Parent.Parent;
            Guid dataKey = Guid.Parse(gvAlumnos.DataKeys[gr.RowIndex].Value.ToString());

            CheckBox cbSeleccionado = (CheckBox)gr.FindControl("cbSeleccionado");

            if (cbSeleccionado.Checked)
            {
                alumnosServices.ActualizarLsAsignarAlumnos(alumnosServices.lsAlumnosGridViewModel, dataKey, true);
            }
            else
            {
                alumnosServices.ActualizarLsAsignarAlumnos(alumnosServices.lsAlumnosGridViewModel, dataKey, false);
            }

            lblCantSeleccionado.Text = alumnosServices.lsSelectAlumnosGridViewModel.Count.ToString();
            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
            gvAlumnos.DataBind();
        }

        #region IMPORTACION DE PADRES
        protected void btnExportarLista_Click(object sender, EventArgs e)
        {
            Session["lsPadres"] = padresServices.lsPadres;
            Session["lsPadresExcelErrores"] = null;
            string _open = "window.open('Excel/ExportarExcelPadres.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }
        protected void btnDescargarError_Click(object sender, EventArgs e)
        {
            Session["lsPadres"] = null;
            Session["lsPadresExcelErrores"] = padresServices.lsExcelErrores;
            string _open = "window.open('Excel/ExportarExcelPadres.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }
        protected void btnMasDetalle_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalMasDetalle()", true);
        }
        protected void btnCloseAlertImportarError_Click(object sender, EventArgs e)
        {
            pnlAlertImportarError.Visible = false;
            lblMnsjAlertImportarError.Text = "";
            divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            Session["lsLigasUsuariosGridViewModelError"] = null;
        }

        protected void btnImportarExcel_Click(object sender, EventArgs e)
        {
            if (fuSelecionarExcel.HasFile)
            {
                if (".xlsx" == Path.GetExtension(fuSelecionarExcel.FileName))
                {
                    try
                    {
                        byte[] buffer = new byte[fuSelecionarExcel.FileBytes.Length];
                        fuSelecionarExcel.FileContent.Seek(0, SeekOrigin.Begin);
                        fuSelecionarExcel.FileContent.Read(buffer, 0, Convert.ToInt32(fuSelecionarExcel.FileContent.Length));

                        Stream stream2 = new MemoryStream(buffer);

                        DataTable dt = new DataTable();
                        using (XLWorkbook workbook = new XLWorkbook(stream2))
                        {
                            IXLWorksheet sheet = workbook.Worksheet(1);
                            bool FirstRow = true;
                            string readRange = "1:1";
                            foreach (IXLRow row in sheet.RowsUsed())
                            {
                                //If Reading the First Row (used) then add them as column name  
                                if (FirstRow)
                                {
                                    //Checking the Last cellused for column generation in datatable  
                                    readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Columns.Add(cell.Value.ToString());
                                    }
                                    FirstRow = false;
                                }
                                else
                                {
                                    //Adding a Row in datatable  
                                    dt.Rows.Add();
                                    int cellIndex = 0;
                                    //Updating the values of datatable  
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                        cellIndex++;
                                    }
                                }
                            }
                        }

                        if (dt.Columns.Contains("NOMBRE(S)".Trim()) && dt.Columns.Contains("APEPATERNO".Trim()) && dt.Columns.Contains("APEMATERNO".Trim())
                            && dt.Columns.Contains("CORREO".Trim()) && dt.Columns.Contains("CELULAR".Trim()) && dt.Columns.Contains("MATRICULA(S)".Trim()))
                        {
                            padresServices.ValidarExcelToList(dt, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                            if (padresServices.lsExcelInsertar.Count >= 1)
                            {
                                if (padresServices.lsExcelErrores.Count == 0)
                                {
                                    pnlAlert.Visible = true;
                                    lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> " + padresServices.lsExcelInsertar.Count() + " padre(s) se ha registrado/actualizado exitosamente.";
                                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                                }

                                ViewState["NewPageIndex"] = null;
                                padresServices.CargarPadres(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                                gvPadres.DataSource = padresServices.lsPadres;
                                gvPadres.DataBind();
                            }

                            if (padresServices.lsExcelErrores.Count >= 1)
                            {
                                btnDescargarError.Visible = true;
                                btnMasDetalle.Visible = false;
                                pnlAlertImportarError.Visible = true;
                                lblMnsjAlertImportarError.Text = "<strong>!Lo sentimos¡</strong> " + padresServices.lsExcelErrores.Count() + " padre(s) no se han importado.";
                                divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                            }
                        }
                        else
                        {
                            btnDescargarError.Visible = false;
                            btnMasDetalle.Visible = true;
                            pnlAlertImportarError.Visible = true;
                            lblMnsjAlertImportarError.Text = "<strong>!Lo sentimos¡</strong> el archivo no tiene las columnas correctas.";
                            divAlertImportarError.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        }
                    }
                    catch (Exception ex)
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = ex.Message;
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                    }
                }
                else
                {
                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "Solo se admite los formatos xlsx";
                    divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                }
            }
        }
        #endregion
    }
}