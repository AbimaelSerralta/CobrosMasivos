using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class CicloEscolar : System.Web.UI.Page
    {
        UsuariosCompletosServices usuariosCompletosServices = new UsuariosCompletosServices();
        DireccionesUsuariosServices direccionesUsuariosServices = new DireccionesUsuariosServices();
        TelefonosUsuariosServices telefonosUsuariosServices = new TelefonosUsuariosServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();

        EstatusServices estatusService = new EstatusServices();
        TiposTelefonosServices tiposTelefonosServices = new TiposTelefonosServices();

        PerfilesServices perfilesServices = new PerfilesServices();

        ClienteCuentaServices clienteCuentaServices = new ClienteCuentaServices();

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
                ViewState["gvCicloEscolar"] = SortDirection.Ascending;

                Session["usuariosCompletosServices"] = usuariosCompletosServices;
                //Session["telefonosUsuariosServices"] = telefonosUsuariosServices;
                Session["estatusService"] = estatusService;

                //usuariosCompletosServices.CargarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
                gvCicloEscolar.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                gvCicloEscolar.DataBind();

                estatusService.CargarEstatus();
                ddlEstatus.DataSource = estatusService.lsEstatus;
                ddlEstatus.DataTextField = "VchDescripcion";
                ddlEstatus.DataValueField = "UidEstatus";
                ddlEstatus.DataBind();

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
            }
            else
            {
                usuariosCompletosServices = (UsuariosCompletosServices)Session["usuariosCompletosServices"];
                //telefonosUsuariosServices = (TelefonosUsuariosServices)Session["telefonosUsuariosServices"];

                estatusService = (EstatusServices)Session["estatusService"];
                tiposTelefonosServices = (TiposTelefonosServices)Session["tiposTelefonosServices"];

                lblValidar.Text = string.Empty;

                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //ViewState["txtUsuario.Text"] = string.Empty;
            //ViewState["txtPassword.Text"] = string.Empty;
            //ViewState["txtRepetirPassword.Text"] = string.Empty;

            //#region ValidarCampos

            //if (txtNombre.EmptyTextBox())
            //{
            //    lblValidar.Text = "El campo Nombre es obligatorio";
            //    return;
            //}

            //if (txtApePaterno.EmptyTextBox())
            //{
            //    lblValidar.Text = "El campo Apellido Paterno es obligatorio";
            //    return;
            //}

            //if (txtApeMaterno.EmptyTextBox())
            //{
            //    lblValidar.Text = "El campo Apellido Materno es obligatorio";
            //    return;
            //}

            //if (txtCorreo.EmptyTextBox())
            //{
            //    lblValidar.Text = "El campo Correo Eléctronico es obligatorio";
            //    return;
            //}

            //if (ddlEstatus.EmptyDropDownList())
            //{
            //    lblValidar.Text = "El campo Estatus es obligatorio";
            //    return;
            //}
            //if (txtNumero.EmptyTextBox())
            //{
            //    lblValidar.Text = "El campo Número es obligatorio";
            //    return;
            //}
            //if (!string.IsNullOrEmpty(lblValidar.Text))
            //{
            //    lblValidar.Text = string.Empty;
            //}
            //#endregion

            //List<PermisosMenuModel> permisosMenuModels = (List<PermisosMenuModel>)Session["lsAccesosPermitidos"];
            //permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == new Guid("6D896A85-ABAD-473F-BE0F-2B0CA6F3BC9F")).ToList();
            //foreach (var item in permisosMenuModels)
            //{
            //    if (ViewState["Accion"].ToString() == "Guardar")
            //    {
            //        if (item.Agregar)
            //        {
            //            if (!validacionesServices.ExisteUsuario(ViewState["txtUsuario.Text"].ToString()))
            //            {
            //                if (ViewState["txtPassword.Text"].ToString().Equals(ViewState["txtRepetirPassword.Text"].ToString()))
            //                {
            //                    if (!validacionesServices.ExisteCorreo(txtCorreo.Text))
            //                    {
            //                        Guid UidUsuario = Guid.NewGuid();

            //                        if (usuariosCompletosServices.RegistrarUsuarios(UidUsuario,
            //                        txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), ViewState["txtUsuario.Text"].ToString().Trim().ToUpper(), ViewState["txtPassword.Text"].ToString().Trim(), Guid.Parse("18E9669B-C238-4BCC-9213-AF995644A5A4"),
            //                        txtNumero.Text.Trim(), Guid.Parse("B1055882-BCBA-4AB7-94FA-90E57647E607"), Guid.Parse(ddlPrefijo.SelectedValue), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
            //                        {
            //                            pnlAlert.Visible = true;
            //                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
            //                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

            //                            usuariosCompletosServices.CargarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
            //                            gvCicloEscolar.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            //                            gvCicloEscolar.DataBind();

            //                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        lblValidar.Text = "El correo ingresado ya existe por favor intente con otro.";
            //                    }
            //                }
            //                else
            //                {
            //                    lblValidar.Text = "Las contraseña ingresadas no son iguales por favor reviselo.";
            //                }
            //            }
            //            else
            //            {
            //                lblValidar.Text = "El usuario ingresado ya existe por favor intente con otro.";
            //            }
            //        }
            //        else
            //        {
            //            lblMensajeAlert.Text = "<b>Lo sentimos,</b> no tiene permisos para esta acción.";
            //            divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
            //        }
            //    }
            //    else if (ViewState["Accion"].ToString() == "Actualizar")
            //    {
            //        if (item.Actualizar)
            //        {
            //            bool Actualizar = false;

            //            if (ViewState["ActualizarCorreo"].ToString() != txtCorreo.Text)
            //            {
            //                if (validacionesServices.ExisteCorreo(txtCorreo.Text))
            //                {
            //                    lblValidar.Text = "El correo ingresado ya existe por favor intente con otro.";
            //                    return;
            //                }
            //                else
            //                {
            //                    Actualizar = true;
            //                }
            //            }
            //            else
            //            {
            //                Actualizar = true;
            //            }

            //            if (Actualizar)
            //            {
            //                if (usuariosCompletosServices.ActualizarUsuarios(
            //                new Guid(ViewState["UidRequerido"].ToString()), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), new Guid(ddlEstatus.SelectedValue), "", "", new Guid("18E9669B-C238-4BCC-9213-AF995644A5A4"),
            //                txtNumero.Text.Trim(), new Guid(ddlTipoTelefono.SelectedValue), new Guid(ddlPrefijo.SelectedValue), new Guid(ViewState["UidClienteLocal"].ToString())))
            //                {
            //                    pnlAlert.Visible = true;
            //                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
            //                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");


            //                    usuariosCompletosServices.CargarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
            //                    gvCicloEscolar.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            //                    gvCicloEscolar.DataBind();

            //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            lblMensajeAlert.Text = "<b>Lo sentimos,</b> no tiene permisos para esta acción.";
            //            divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
            //        }
            //    }
            //    else if (ViewState["Accion"].ToString() == "AsosiarUsuario")
            //    {
            //        if (validacionesServices.ExisteUsuarioCliente(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString())))
            //        {
            //            lblValidar.Text = "Lo sentimos, el usuario ya esta asociado.";
            //        }
            //        else
            //        {
            //            if (usuariosCompletosServices.AsociarClienteUsuario(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse(ViewState["usuarioCompleto.UidUsuario"].ToString())))
            //            {
            //                usuariosCompletosServices.CargarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"));
            //                gvCicloEscolar.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            //                gvCicloEscolar.DataBind();

            //                pnlAlert.Visible = true;
            //                lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
            //                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

            //                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
            //            }
            //        }
            //    }
            //}
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
            lblTituloModal.Text = "Registro del Ciclo escolar";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);

        }

        private void BloquearCampos()
        {
            txtNombre.Enabled = false;
            txtFInicio.Enabled = false;
            txtFVencimiento.Enabled = false;
            ddlEstatus.Enabled = false;

            ddlTipoTelefono.Enabled = false;
        }
        private void DesbloquearCampos()
        {
            txtNombre.Enabled = true;
            txtFInicio.Enabled = true;
            txtFVencimiento.Enabled = true;

            if (ViewState["Accion"].ToString() == "Guardar")
            {
                ddlEstatus.Enabled = false;
            }
            else
            {
                ddlEstatus.Enabled = true;
            }

            ddlTipoTelefono.Enabled = true;
        }
        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtFInicio.Text = string.Empty;
            txtFVencimiento.Text = string.Empty;
            ddlEstatus.SelectedIndex = -1;

            ddlTipoTelefono.SelectedIndex = -1;
        }

        protected void gvCicloEscolar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCicloEscolar, "Select$" + e.Row.RowIndex);
            }
        }

        protected void gvCicloEscolar_RowCommand(object sender, GridViewCommandEventArgs e)
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
                GridViewRow Seleccionado = gvCicloEscolar.Rows[index];
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
                GridViewRow Seleccionado = gvCicloEscolar.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);

                lblTituloModal.Text = "Visualización de Usuario";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
        }

        private void ManejoDatos(Guid dataKeys)
        {
            //==================FRANQUICIATARIO============================
            usuariosCompletosServices.ObtenerAdministrador(dataKeys);
            txtNombre.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrNombre;
            txtFInicio.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrApePaterno;
            txtFVencimiento.Text = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrApeMaterno;
            
            ViewState["ActualizarCorreo"] = usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.StrCorreo;
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(usuariosCompletosServices.usuariosCompletosRepository.usuarioCompleto.UidEstatus.ToString()));

            //==================TELÉFONO===================================
            telefonosUsuariosServices.ObtenerTelefonoUsuario(dataKeys);
            
            ddlTipoTelefono.SelectedIndex = ddlTipoTelefono.Items.IndexOf(ddlTipoTelefono.Items.FindByValue(telefonosUsuariosServices.telefonosUsuariosRepository.telefonosUsuarios.UidTipoTelefono.ToString()));
            
        }

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

        protected void gvCicloEscolar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCicloEscolar.PageIndex = e.NewPageIndex;
            gvCicloEscolar.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            gvCicloEscolar.DataBind();
        }

        protected void gvCicloEscolar_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvCicloEscolar"] != null)
            {
                direccion = (SortDirection)ViewState["gvCicloEscolar"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvCicloEscolar"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvCicloEscolar"] = SortDirection.Ascending;
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

                gvCicloEscolar.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
                gvCicloEscolar.DataBind();
            }
        }

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            usuariosCompletosServices.BuscarUsuariosFinales(new Guid(ViewState["UidClienteLocal"].ToString()), new Guid("E39FF705-8A01-4302-829A-7CFB9615CC8F"), FiltroNombre.Text, FiltroApePaterno.Text, FiltroApeMaterno.Text, FiltroCorreo.Text, Guid.Parse(FiltroEstatus.SelectedValue));
            gvCicloEscolar.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            gvCicloEscolar.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        protected void btnLimpiarUsuario_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        //Menu de asinar usuarios EVENTO
        #region Menu
        protected void btn1Activar_Click(object sender, EventArgs e)
        {
            btn1Activar.CssClass = "nav-link active show";
            pnlActivarGeneral.Visible = true;

            btn2Activar.CssClass = "nav-link";
            pnlActivarCronograma.Visible = false;
        }

        protected void btn2Activar_Click(object sender, EventArgs e)
        {
            btn1Activar.CssClass = "nav-link";
            pnlActivarGeneral.Visible = false;

            btn2Activar.CssClass = "nav-link active show";
            pnlActivarCronograma.Visible = true;
        }
        #endregion
    }
}