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
    public partial class PerfilFranquicia : System.Web.UI.Page
    {
        PerfilesServices perfilesServices = new PerfilesServices();
        EstatusServices estatusServices = new EstatusServices();
        //TiposPerfilesFranquiciasServices tiposPerfilesFranquiciasServices = new TiposPerfilesFranquiciasServices();
        ModulosServices modulosServices = new ModulosServices();
        PermisosServices permisosServices = new PermisosServices();
        TiposPerfilesServices tiposPerfilesServices = new TiposPerfilesServices();

        List<string> listPermisosPrincipal = new List<string>();
        List<string> listDenegarPermisos = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["gvPerfiles"] = SortDirection.Ascending;
                ViewState["SoExgvPerfiles"] = "";

                Session["listPermisosPrincipal"] = listPermisosPrincipal;
                Session["listDenegarPermisos"] = listDenegarPermisos;

                Session["perfilesServices"] = perfilesServices;
                Session["estatusServices"] = estatusServices;
                Session["tiposPerfilesServices"] = tiposPerfilesServices;
                Session["modulosServices"] = modulosServices;
                Session["permisosServices"] = permisosServices;

                if (Session["UidFranquiciaMaster"] != null)
                {
                    ViewState["UidFranquiciaLocal"] = Session["UidFranquiciaMaster"];
                }
                else
                {
                    ViewState["UidFranquiciaLocal"] = Guid.Empty;
                }

                perfilesServices.CargarPerfilesFranquiciaGridViewModel(new Guid(ViewState["UidFranquiciaLocal"].ToString()));
                gvPerfiles.DataSource = perfilesServices.lsperfilesGridViewModel;
                gvPerfiles.DataBind();

                estatusServices.CargarEstatus();
                ddlEstatus.DataSource = estatusServices.lsEstatus;
                ddlEstatus.DataTextField = "VchDescripcion";
                ddlEstatus.DataValueField = "UidEstatus";
                ddlEstatus.DataBind();

                tiposPerfilesServices.CargarTipoPerfil(new Guid("6D70F88D-3CE0-4C8B-87A1-92666039F5B2"));
                ddlTipoPerfil.DataSource = tiposPerfilesServices.lsTipoPerfil;
                ddlTipoPerfil.DataTextField = "VchDescripcion";
                ddlTipoPerfil.DataValueField = "UidTipoPerfil";
                ddlTipoPerfil.DataBind();

                ddlTipoPerfil_SelectedIndexChanged(null, null);

                modulosServices.CargarModulosNivelFranquicias(Guid.Parse("18523B2B-C671-44AE-A3F6-F0255C4D11A8"));
                gvModulosFranquicias.DataSource = modulosServices.lsmodulos;
                gvModulosFranquicias.DataBind();

                modulosServices.CargarModulosNivelClientes(Guid.Parse("D2C80D47-C14C-4677-A63D-C46BCB50FE17"));
                gvModulosClientes.DataSource = modulosServices.lsmodulos;
                gvModulosClientes.DataBind();

                modulosServices.CargarModulosNivelUsuarios(Guid.Parse("18E9669B-C238-4BCC-9213-AF995644A5A4"));
                gvModulosUsuarios.DataSource = modulosServices.lsmodulos;
                gvModulosUsuarios.DataBind();
            }
            else
            {
                perfilesServices = (PerfilesServices)Session["perfilesServices"];
                estatusServices = (EstatusServices)Session["estatusServices"];
                tiposPerfilesServices = (TiposPerfilesServices)Session["tiposPerfilesServices"];
                modulosServices = (ModulosServices)Session["modulosServices"];
                permisosServices = (PermisosServices)Session["permisosServices"];

                listPermisosPrincipal = (List<string>)Session["listPermisosPrincipal"];
                listDenegarPermisos = (List<string>)Session["listDenegarPermisos"];

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guid UidSegPerfil = Guid.NewGuid();
            //Guid UidAppWeb = Guid.Empty;
            Guid UidAppWeb = new Guid("6d70f88d-3ce0-4c8b-87a1-92666039f5b2");

            //switch (ddlTipoPerfil.SelectedItem.ToString())
            //{
            //    case "FRANQUICIA ADMIN":
            //        UidAppWeb = new Guid("6d70f88d-3ce0-4c8b-87a1-92666039f5b2");
            //        break;
            //    case "CLIENTE ADMIN":
            //        UidAppWeb = new Guid("0d910772-ae62-467a-a7a3-79540f0445cb");
            //        break;
            //}

            #region ValidarCampos
            if (txtNombre.EmptyTextBox())
            {
                lblValidar.Text = "El campo Nombre es obligatorio";
                return;
            }
            if (ddlTipoPerfil.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Tipo Perfil es obligatorio";
                return;
            }
            if (ddlEstatus.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Estatus es obligatorio";
                return;
            }
            if (!string.IsNullOrEmpty(lblValidar.Text))
            {
                lblValidar.Text = string.Empty;
            }
            #endregion

            List<PermisosMenuModel> permisosMenuModels = (List<PermisosMenuModel>)Session["lsAccesosPermitidos"];
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == new Guid("398F9537-A3CF-43DD-AE59-B2339CB76BAC")).ToList();
            foreach (var item in permisosMenuModels)
            {
                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        if (perfilesServices.RegistrarPerfilesFranquicia(UidSegPerfil, txtNombre.Text.Trim().ToUpper(), UidAppWeb, new Guid(ViewState["UidFranquiciaLocal"].ToString()), new Guid(ddlModuloInicial.SelectedValue), new Guid("8490c81d-5979-49ac-92cc-e34a50a497d5")))
                        {
                            permisosServices.RegistrarModulosPermisos(UidSegPerfil, listPermisosPrincipal, permisosServices.lsModulosPermisos);

                            perfilesServices.CargarPerfilesFranquiciaGridViewModel(new Guid(ViewState["UidFranquiciaLocal"].ToString()));
                            gvPerfiles.DataSource = perfilesServices.lsperfilesGridViewModel;
                            gvPerfiles.DataBind();

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
                        if (perfilesServices.ActualizarPerfilesFranquicia(new Guid(ViewState["dataKeysRequerido"].ToString()), txtNombre.Text.Trim().ToUpper(), UidAppWeb, new Guid(ddlEstatus.SelectedValue), new Guid(ddlModuloInicial.SelectedValue), new Guid("8490c81d-5979-49ac-92cc-e34a50a497d5")))
                        {
                            permisosServices.ActualizarModulosPermisos(new Guid(ViewState["dataKeysRequerido"].ToString()), listDenegarPermisos, permisosServices.lsModulosPermisos, listPermisosPrincipal);

                            perfilesServices.CargarPerfilesFranquiciaGridViewModel(new Guid(ViewState["UidFranquiciaLocal"].ToString()));
                            gvPerfiles.DataSource = perfilesServices.lsperfilesGridViewModel;
                            gvPerfiles.DataBind();

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
            MarcarTodosPermisos();
            lblValidar.Text = string.Empty;
            ViewState["Accion"] = "Guardar";
            LimpiarCampos();
            DesbloquearCampos();
            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            lblTituloModal.Text = "Registro del Acceso";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            listPermisosPrincipal.Clear();
            listDenegarPermisos.Clear();

            permisosServices.CargarModulosPermisosFranquicias(Guid.Parse("18523B2B-C671-44AE-A3F6-F0255C4D11A8"));

            foreach (var item in permisosServices.lsModulosPermisos)
            {
                listPermisosPrincipal.Add(item.UidPermiso.ToString());
            }
            foreach (ListItem item in cblPermisosFranquicias.Items)
            {
                item.Selected = true;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
        }

        private void BloquearCampos()
        {
            txtNombre.Enabled = false;
            ddlEstatus.Enabled = false;
            ddlTipoPerfil.Enabled = false;
        }
        private void DesbloquearCampos()
        {
            txtNombre.Enabled = true;
            ddlEstatus.Enabled = true;
            ddlTipoPerfil.Enabled = true;
        }
        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            ddlEstatus.SelectedIndex = -1;
            ddlTipoPerfil.SelectedIndex = -1;
        }

        protected void gvPerfiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvPerfiles, "Select$" + e.Row.RowIndex);

                Label Estatus = e.Row.FindControl("lblEstatus") as Label;

                if (e.Row.Cells[3].Text == "65e46bc9-1864-4145-ad1a-70f5b5f69739")
                {
                    Estatus.Text = "check";

                    Estatus.ToolTip = "Activo";
                }
                if (e.Row.Cells[3].Text == "3b0db4df-5687-4d80-8d32-3a2ac76af453")
                {
                    Estatus.Text = "close";
                    Estatus.ToolTip = "Inactivo";
                }
            }
        }
        protected void gvPerfiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                ViewState["Accion"] = "Actualizar";
                DesbloquearCampos();
                btnCerrar.Visible = false;
                btnCancelar.Visible = true;
                btnGuardar.Visible = true;
                btnEditar.Visible = false;
                lblTituloModal.Text = "Actualizar Acceso";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvPerfiles.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["dataKeysRequerido"] = dataKeys;

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
                GridViewRow Seleccionado = gvPerfiles.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["dataKeysRequerido"] = dataKeys;

                ManejoDatos(dataKeys);

                lblTituloModal.Text = "Visualización del Acceso";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
        }
        protected void gvPerfiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPerfiles.PageIndex = e.NewPageIndex;
            gvPerfiles.DataSource = perfilesServices.lsperfilesGridViewModel;
            gvPerfiles.DataBind();
        }
        protected void gvPerfiles_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            ViewState["SoExgvPerfiles"] = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvPerfiles"] != null)
            {
                direccion = (SortDirection)ViewState["gvPerfiles"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvPerfiles"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvPerfiles"] = SortDirection.Ascending;
                    Orden = "DESC";
                }

                switch (SortExpression)
                {
                    case "VchNombre":
                        if (Orden == "ASC")
                        {
                            perfilesServices.lsperfilesGridViewModel = perfilesServices.lsperfilesGridViewModel.OrderBy(x => x.VchNombre).ToList();
                        }
                        else
                        {
                            perfilesServices.lsperfilesGridViewModel = perfilesServices.lsperfilesGridViewModel.OrderByDescending(x => x.VchNombre).ToList();
                        }
                        break;
                    case "VchPerfil":
                        if (Orden == "ASC")
                        {
                            perfilesServices.lsperfilesGridViewModel = perfilesServices.lsperfilesGridViewModel.OrderBy(x => x.VchPerfil).ToList();
                        }
                        else
                        {
                            perfilesServices.lsperfilesGridViewModel = perfilesServices.lsperfilesGridViewModel.OrderByDescending(x => x.VchPerfil).ToList();
                        }
                        break;
                    case "UidEstatus":
                        if (Orden == "ASC")
                        {
                            perfilesServices.lsperfilesGridViewModel = perfilesServices.lsperfilesGridViewModel.OrderBy(x => x.UidEstatus).ToList();
                        }
                        else
                        {
                            perfilesServices.lsperfilesGridViewModel = perfilesServices.lsperfilesGridViewModel.OrderByDescending(x => x.UidEstatus).ToList();
                        }
                        break;
                }

                gvPerfiles.DataSource = perfilesServices.lsperfilesGridViewModel;
                gvPerfiles.DataBind();
            }
        }
        protected void gvPerfiles_RowCreated(object sender, GridViewRowEventArgs e)
        {
            SortDirection direccion = (SortDirection)ViewState["gvPerfiles"];
            string SortExpression = ViewState["SoExgvPerfiles"].ToString();

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
            listPermisosPrincipal.Clear();
            listDenegarPermisos.Clear();

            //==================FRANQUICIATARIO============================
            perfilesServices.ObtenerPerfil(dataKeys);
            txtNombre.Text = perfilesServices.perfilesRepository.perfilesGridViewModel.VchNombre;
            ddlTipoPerfil.SelectedIndex = ddlTipoPerfil.Items.IndexOf(ddlTipoPerfil.Items.FindByValue(perfilesServices.perfilesRepository.perfilesGridViewModel.UidTipoPerfilFranquicia.ToString()));
            ddlTipoPerfil_SelectedIndexChanged(null, null);
            ddlModuloInicial.SelectedIndex = ddlModuloInicial.Items.IndexOf(ddlModuloInicial.Items.FindByValue(perfilesServices.perfilesRepository.perfilesGridViewModel.UidModuloInicial.ToString()));
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(perfilesServices.perfilesRepository.perfilesGridViewModel.UidEstatus.ToString()));

            //==================PermisosAccesos============================
            DesmarcarTodosPermisos();

            permisosServices.CargarModulosPermisos(Guid.Parse("17BB8F08-9D5F-425C-9B9B-1CA230C07C7F"));
            permisosServices.CargarAccesosModulosPermisos(dataKeys);

            foreach (var item in permisosServices.lsAccesosModulosPermisos)
            {
                listPermisosPrincipal.Add(item.UidPermiso.ToString());
            }

            foreach (ListItem item in cblPermisosFranquicias.Items)
            {
                for (int i = 0; i < listPermisosPrincipal.Count; i++)
                {
                    if (item.Value == listPermisosPrincipal[i])
                    {
                        item.Selected = true;
                    }
                }
            }
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

        protected void gvModulosFranquicias_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid dataKey = new Guid(gvModulosFranquicias.SelectedDataKey.Value.ToString());

            permisosServices.ObtenerModulosPermisos(dataKey);
            cblPermisosFranquicias.DataSource = permisosServices.lsModulosCheckBoxListModel;
            cblPermisosFranquicias.DataTextField = "VchDescripcion";
            cblPermisosFranquicias.DataValueField = "UidPermiso";
            cblPermisosFranquicias.DataBind();

            foreach (ListItem item in cblPermisosFranquicias.Items)
            {
                for (int i = 0; i < listPermisosPrincipal.Count; i++)
                {
                    if (item.Value == listPermisosPrincipal[i])
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        protected void gvModulosFranquicias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvModulosFranquicias, "Select$" + e.Row.RowIndex);
            }
        }
        protected void cblPermisosFranquicias_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in cblPermisosFranquicias.Items)
            {
                if (listPermisosPrincipal.Exists(x => x == item.Value))
                {
                    if (item.Selected == false)
                    {
                        listPermisosPrincipal.Remove(item.Value);
                        listDenegarPermisos.Add(item.Value);
                    }
                }
                else
                {
                    if (item.Selected == true)
                    {
                        listPermisosPrincipal.Add(item.Value);
                        listDenegarPermisos.Remove(item.Value);
                    }
                }
            }
        }

        protected void gvModulosClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid dataKey = new Guid(gvModulosClientes.SelectedDataKey.Value.ToString());

            permisosServices.ObtenerModulosPermisos(dataKey);
            cblPermisosClientes.DataSource = permisosServices.lsModulosCheckBoxListModel;
            cblPermisosClientes.DataTextField = "VchDescripcion";
            cblPermisosClientes.DataValueField = "UidPermiso";
            cblPermisosClientes.DataBind();

            foreach (ListItem item in cblPermisosClientes.Items)
            {
                for (int i = 0; i < listPermisosPrincipal.Count; i++)
                {
                    if (item.Value == listPermisosPrincipal[i])
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        protected void gvModulosClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvModulosClientes, "Select$" + e.Row.RowIndex);
            }
        }
        protected void cblPermisosClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in cblPermisosClientes.Items)
            {
                if (listPermisosPrincipal.Exists(x => x == item.Value))
                {
                    if (item.Selected == false)
                    {
                        listPermisosPrincipal.Remove(item.Value);
                        listDenegarPermisos.Add(item.Value);
                    }
                }
                else
                {
                    if (item.Selected == true)
                    {
                        listPermisosPrincipal.Add(item.Value);
                        listDenegarPermisos.Remove(item.Value);
                    }
                }
            }
        }

        protected void gvModulosUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid dataKey = new Guid(gvModulosUsuarios.SelectedDataKey.Value.ToString());

            permisosServices.ObtenerModulosPermisos(dataKey);
            cblPermisosUsuarios.DataSource = permisosServices.lsModulosCheckBoxListModel;
            cblPermisosUsuarios.DataTextField = "VchDescripcion";
            cblPermisosUsuarios.DataValueField = "UidPermiso";
            cblPermisosUsuarios.DataBind();

            foreach (ListItem item in cblPermisosUsuarios.Items)
            {
                for (int i = 0; i < listPermisosPrincipal.Count; i++)
                {
                    if (item.Value == listPermisosPrincipal[i])
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        protected void gvModulosUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvModulosUsuarios, "Select$" + e.Row.RowIndex);
            }
        }
        protected void cblPermisosUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in cblPermisosUsuarios.Items)
            {
                if (listPermisosPrincipal.Exists(x => x == item.Value))
                {
                    if (item.Selected == false)
                    {
                        listPermisosPrincipal.Remove(item.Value);
                        listDenegarPermisos.Add(item.Value);
                    }
                }
                else
                {
                    if (item.Selected == true)
                    {
                        listPermisosPrincipal.Add(item.Value);
                        listDenegarPermisos.Remove(item.Value);
                    }
                }
            }
        }

        private void MarcarTodosPermisos()
        {
            foreach (ListItem li in cblPermisosFranquicias.Items)
            {
                li.Selected = true;
            }
            foreach (ListItem li in cblPermisosClientes.Items)
            {
                li.Selected = true;
            }
        }
        private void DesmarcarTodosPermisos()
        {
            foreach (ListItem li in cblPermisosFranquicias.Items)
            {
                li.Selected = false;
            }
            foreach (ListItem li in cblPermisosClientes.Items)
            {
                li.Selected = false;
            }
        }
        protected void ddlTipoPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {            
            modulosServices.CargarModulosNivelFranquicias(Guid.Parse("18523B2B-C671-44AE-A3F6-F0255C4D11A8"));
            ddlModuloInicial.DataSource = modulosServices.lsmodulos;
            ddlModuloInicial.DataTextField = "VchNombre";
            ddlModuloInicial.DataValueField = "UidSegModulo";
            ddlModuloInicial.DataBind();

            //switch (ddlTipoPerfil.SelectedItem.ToString())
            //{
            //    case "FRANQUICIA ADMIN":
            //        modulosServices.CargarModulosNivelFranquicias();
            //        ddlModuloInicial.DataSource = modulosServices.lsmodulos;
            //        ddlModuloInicial.DataTextField = "VchNombre";
            //        ddlModuloInicial.DataValueField = "UidSegModulo";
            //        ddlModuloInicial.DataBind();

            //        liFranquicias.Visible = true;
            //        aFranquicias.Attributes.Add("class", "nav-link active");
            //        //aCliente.Attributes.Add("class", "nav-link");

            //        break;
            //    case "CLIENTE ADMIN":
            //        modulosServices.CargarModulosNivelClientes();
            //        ddlModuloInicial.DataSource = modulosServices.lsmodulos;
            //        ddlModuloInicial.DataTextField = "VchNombre";
            //        ddlModuloInicial.DataValueField = "UidSegModulo";
            //        ddlModuloInicial.DataBind();

            //        //liCliente.Visible = false;
            //        //aCliente.Attributes.Add("class", "nav-link");
            //        //aFranquicias.Attributes.Add("class", "nav-link active");
            //        break;
            //}
        }

    }
}