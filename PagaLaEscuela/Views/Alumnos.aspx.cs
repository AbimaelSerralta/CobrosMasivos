using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using Franquicia.WebForms.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class Alumnos : System.Web.UI.Page
    {
        AlumnosServices alumnosServices = new AlumnosServices();
        TelefonosAlumnosServices telefonosAlumnosServices = new TelefonosAlumnosServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();

        EstatusServices estatusService = new EstatusServices();
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
                ViewState["gvAlumnos"] = SortDirection.Ascending;

                Session["alumnosServices"] = alumnosServices;
                Session["telefonosAlumnosServices"] = telefonosAlumnosServices;
                Session["estatusService"] = estatusService;
                Session["prefijosTelefonicosServices"] = prefijosTelefonicosServices;

                alumnosServices.CargarAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                gvAlumnos.DataBind();

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
            }
            else
            {
                alumnosServices = (AlumnosServices)Session["alumnosServices"];
                telefonosAlumnosServices = (TelefonosAlumnosServices)Session["telefonosAlumnosServices"];

                estatusService = (EstatusServices)Session["estatusService"];
                prefijosTelefonicosServices = (PrefijosTelefonicosServices)Session["prefijosTelefonicosServices"];

                lblValidar.Text = string.Empty;

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            #region ValidarCampos

            if (txtIdentificador.EmptyTextBox())
            {
                lblValidar.Text = "El campo Identificador es obligatorio";
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

            if (txtMatricula.EmptyTextBox())
            {
                lblValidar.Text = "El campo Matricula es obligatorio";
                return;
            }

            if (bool.Parse(ddlBeca.SelectedValue))
            {
                if (txtBeca.EmptyTextBox())
                {
                    lblValidar.Text = "El campo Beca es obligatorio";
                    return;
                }

                if (ddlTipoBeca.SelectedValue == "PORCENTAJE")
                {
                    if (decimal.Parse(txtBeca.Text) > 100)
                    {
                        lblValidar.Text = "Cuando la selección es PORCENTAJE, Solo se admite hasta 100";
                        txtBeca.BackColor = System.Drawing.Color.FromName("#f2dede");
                        return;
                    }
                }
            }
            else
            {

                txtBeca.Text = "0";
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
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == Guid.Parse("C1E64F99-F72E-4C68-89DE-6E32BD5C0525")).ToList();
            foreach (var item in permisosMenuModels)
            {
                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        Guid UidAlumno = Guid.NewGuid();

                        if (alumnosServices.RegistrarUsuarios(UidAlumno, txtIdentificador.Text.Trim().ToUpper(), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtMatricula.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), bool.Parse(ddlBeca.SelectedValue), ddlTipoBeca.SelectedValue, decimal.Parse(txtBeca.Text), txtNumero.Text.Trim(), Guid.Parse(ddlPrefijo.SelectedValue), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                        {
                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            alumnosServices.CargarAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                            gvAlumnos.DataBind();

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
                        if (alumnosServices.ActualizarAlumno(Guid.Parse(ViewState["UidRequerido"].ToString()), txtIdentificador.Text.Trim().ToUpper(), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtMatricula.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), bool.Parse(ddlBeca.SelectedValue), ddlTipoBeca.SelectedValue, decimal.Parse(txtBeca.Text), Guid.Parse(ddlEstatus.SelectedValue), txtNumero.Text.Trim(), Guid.Parse(ddlPrefijo.SelectedValue)))
                        {
                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            alumnosServices.CargarAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                            gvAlumnos.DataBind();

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
            lblTituloModal.Text = "Registro de Alumno";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);

        }

        private void BloquearCampos()
        {
            txtIdentificador.Enabled = false;
            txtNombre.Enabled = false;
            txtApePaterno.Enabled = false;
            txtApeMaterno.Enabled = false;
            txtMatricula.Enabled = false;
            txtCorreo.Enabled = false;
            ddlBeca.Enabled = false;
            ddlBeca_SelectedIndexChanged(null, null);
            pnlBeca.Enabled = false;
            ddlEstatus.Enabled = false;

            txtNumero.Enabled = false;
            ddlPrefijo.Enabled = false;
        }
        private void DesbloquearCampos()
        {
            txtIdentificador.Enabled = true;
            txtNombre.Enabled = true;
            txtApePaterno.Enabled = true;
            txtApeMaterno.Enabled = true;
            txtMatricula.Enabled = true;
            txtCorreo.Enabled = true;
            ddlBeca.Enabled = true;
            ddlBeca_SelectedIndexChanged(null, null);
            pnlBeca.Enabled = true;

            if (ViewState["Accion"].ToString() == "Guardar")
            {
                ddlEstatus.Enabled = false;
            }
            else
            {
                ddlEstatus.Enabled = true;
            }

            txtNumero.Enabled = true;
            ddlPrefijo.Enabled = true;
        }
        private void LimpiarCampos()
        {
            txtIdentificador.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApePaterno.Text = string.Empty;
            txtApeMaterno.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            ddlBeca.SelectedIndex = -1;
            ddlBeca_SelectedIndexChanged(null, null);
            ddlTipoBeca.SelectedIndex = -1;
            txtBeca.Text = string.Empty;
            ddlEstatus.SelectedIndex = -1;

            txtNumero.Text = string.Empty;
            ddlPrefijo.SelectedIndex = ddlPrefijo.Items.IndexOf(ddlPrefijo.Items.FindByValue("abb854c4-e7ed-420f-8561-aa4b61bf5b0f"));
        }

        protected void gvAlumnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvAlumnos, "Select$" + e.Row.RowIndex);
            }
        }

        protected void gvAlumnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                ViewState["Accion"] = "Actualizar";
                btnCerrar.Visible = false;
                btnCancelar.Visible = true;
                btnGuardar.Visible = true;
                btnEditar.Visible = false;
                lblTituloModal.Text = "Actualizar Alumno";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvAlumnos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);
                DesbloquearCampos();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }

            if (e.CommandName == "Ver")
            {
                btnCerrar.Visible = true;
                btnCancelar.Visible = false;
                btnGuardar.Visible = false;
                btnEditar.Visible = true;
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvAlumnos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);
                BloquearCampos();

                lblTituloModal.Text = "Visualización del Alumno";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }
        }

        private void ManejoDatos(Guid dataKeys)
        {
            //==================ALUMNO============================
            alumnosServices.ObtenerAlumno(dataKeys);
            txtIdentificador.Text = alumnosServices.alumnosRepository.alumnosGridViewModel.VchIdentificador;
            txtNombre.Text = alumnosServices.alumnosRepository.alumnosGridViewModel.VchNombres;
            txtApePaterno.Text = alumnosServices.alumnosRepository.alumnosGridViewModel.VchApePaterno;
            txtApeMaterno.Text = alumnosServices.alumnosRepository.alumnosGridViewModel.VchApeMaterno;
            txtMatricula.Text = alumnosServices.alumnosRepository.alumnosGridViewModel.VchMatricula;
            txtCorreo.Text = alumnosServices.alumnosRepository.alumnosGridViewModel.VchCorreo;
            if (alumnosServices.alumnosRepository.alumnosGridViewModel.BitBeca)
            {
                ddlBeca.SelectedIndex = 1;
                ddlTipoBeca.SelectedValue = alumnosServices.alumnosRepository.alumnosGridViewModel.VchTipoBeca;
                txtBeca.Text = alumnosServices.alumnosRepository.alumnosGridViewModel.DcmBeca.ToString();
            }
            else
            {
                ddlBeca.SelectedIndex = 0;
                txtBeca.Text = string.Empty;
            }
            ViewState["ActualizarCorreo"] = alumnosServices.alumnosRepository.alumnosGridViewModel.VchCorreo;
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(alumnosServices.alumnosRepository.alumnosGridViewModel.UidEstatus.ToString()));

            //==================TELÉFONO===================================
            telefonosAlumnosServices.ObtenerTelefonosAlumnos(dataKeys);
            txtNumero.Text = telefonosAlumnosServices.telefonosAlumnosRepository.telefonosAlumnos.VchTelefono;
            ddlPrefijo.SelectedIndex = ddlPrefijo.Items.IndexOf(ddlPrefijo.Items.FindByValue(telefonosAlumnosServices.telefonosAlumnosRepository.telefonosAlumnos.UidPrefijo.ToString()));
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

        protected void gvAlumnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAlumnos.PageIndex = e.NewPageIndex;
            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
            gvAlumnos.DataBind();
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
                    case "VchBeca":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderBy(x => x.VchBeca).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderByDescending(x => x.VchBeca).ToList();
                        }
                        break;
                    case "VchCorreo":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderBy(x => x.VchCorreo).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderByDescending(x => x.VchCorreo).ToList();
                        }
                        break;
                    case "UidEstatus":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderBy(x => x.UidEstatus).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderByDescending(x => x.UidEstatus).ToList();
                        }
                        break;
                }

                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                gvAlumnos.DataBind();
            }
        }

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //usuariosCompletosServices.BuscarUsuariosFinales(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"), FiltroNombre.Text, FiltroApePaterno.Text, FiltroApeMaterno.Text, FiltroCorreo.Text, Guid.Parse(FiltroEstatus.SelectedValue));
            //gvAlumnos.DataSource = usuariosCompletosServices.lsUsuariosCompletos;
            //gvAlumnos.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlBeca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bool.Parse(ddlBeca.SelectedValue))
            {
                pnlBeca.Visible = true;
            }
            else
            {
                pnlBeca.Visible = false;
                ddlTipoBeca.SelectedIndex = -1;
                txtBeca.Text = string.Empty;
            }
        }

        protected void btnExportarLista_Click(object sender, EventArgs e)
        {
            Session["lsExcelSeleccionar"] = alumnosServices.lsAlumnosGridViewModel;
            Session["lsExcelErrores"] = null;
            string _open = "window.open('Excel/ExportarExcelAlumnos.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }
    }
}