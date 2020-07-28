using ClosedXML.Excel;
using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using Franquicia.WebForms.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class Colegiatura : System.Web.UI.Page
    {
        ColegiaturasServices colegiaturasServices = new ColegiaturasServices();
        PadresServices padresServices = new PadresServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();
        PeriodicidadesServices periodicidadesServices = new PeriodicidadesServices();
        EstatusServices estatusService = new EstatusServices();
        PrefijosTelefonicosServices prefijosTelefonicosServices = new PrefijosTelefonicosServices();
        AlumnosServices alumnosServices = new AlumnosServices();

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
                fuSelecionarExcel.Attributes["onchange"] = "UploadFile(this)";

                ViewState["gvColegiaturas"] = SortDirection.Ascending;
                ViewState["gvAlumnos"] = SortDirection.Ascending;

                Session["colegiaturasServices"] = colegiaturasServices;
                Session["padresServices"] = padresServices;
                Session["periodicidadesServices"] = periodicidadesServices;
                Session["estatusService"] = estatusService;
                Session["prefijosTelefonicosServices"] = prefijosTelefonicosServices;
                Session["alumnosServices"] = alumnosServices;

                colegiaturasServices.CargarColegiaturas(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                gvColegiaturas.DataSource = colegiaturasServices.lsColegiaturasGridViewModel;
                gvColegiaturas.DataBind();

                FiltroEstatus.DataSource = estatusService.lsEstatus;
                FiltroEstatus.Items.Insert(0, new ListItem("Seleccione", "00000000-0000-0000-0000-000000000000"));
                FiltroEstatus.DataTextField = "VchDescripcion";
                FiltroEstatus.DataValueField = "UidEstatus";
                FiltroEstatus.DataBind();

                periodicidadesServices.CargarPeriodicidades();
                ddlPeriodicidad.DataSource = periodicidadesServices.lsPeriodicidades;
                ddlPeriodicidad.DataTextField = "VchDescripcion";
                ddlPeriodicidad.DataValueField = "UidPeriodicidad";
                ddlPeriodicidad.DataBind();
            }
            else
            {
                colegiaturasServices = (ColegiaturasServices)Session["colegiaturasServices"];
                padresServices = (PadresServices)Session["padresServices"];
                periodicidadesServices = (PeriodicidadesServices)Session["periodicidadesServices"];
                estatusService = (EstatusServices)Session["estatusService"];
                prefijosTelefonicosServices = (PrefijosTelefonicosServices)Session["prefijosTelefonicosServices"];
                alumnosServices = (AlumnosServices)Session["alumnosServices"];

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
            if (txtImporte.EmptyTextBox())
            {
                lblValidar.Text = "El campo Importe es obligatorio";
                return;
            }
            if (bool.Parse(ddlRecargo.SelectedValue))
            {
                if (txtRecargo.EmptyTextBox())
                {
                    lblValidar.Text = "El campo Recargo es obligatorio";
                    return;
                }

                if (ddlTipoRecargo.SelectedValue == "PORCENTAJE")
                {
                    if (decimal.Parse(txtRecargo.Text) > 100)
                    {
                        lblValidar.Text = "Cuando la selección es PORCENTAJE, Solo se admite hasta 100";
                        txtRecargo.BackColor = System.Drawing.Color.FromName("#f2dede");
                        return;
                    }
                }
            }
            else
            {
                txtRecargo.Text = "0";
            }
            if (txtFHInicio.EmptyTextBox())
            {
                lblValidar.Text = "El campo Fecha Inicio es obligatorio";
                return;
            }
            if (cbActivarFHL.Checked)
            {
                if (txtFHLimite.EmptyTextBox())
                {
                    lblValidar.Text = "El campo Fecha Limite es obligatorio";
                    return;
                }
            }
            else
            {
                txtFHLimite.Text = "1/1/0001 12:00:00";
            }
            if (cbActivarFHV.Checked)
            {
                if (txtFHVencimiento.EmptyTextBox())
                {
                    lblValidar.Text = "El campo Fecha Vencimiento es obligatorio";
                    return;
                }
            }
            else
            {
                txtFHVencimiento.Text = "1/1/0001 12:00:00";
            }
            if (txtCantPagos.EmptyTextBox())
            {
                lblValidar.Text = "El campo Cant. de Pagos es obligatorio";
                return;
            }
            if (ddlPeriodicidad.EmptyDropDownList())
            {
                lblValidar.Text = "El campo Periodicidad es obligatorio";
                return;
            }
            if (!string.IsNullOrEmpty(lblValidar.Text))
            {
                lblValidar.Text = string.Empty;
            }
            #endregion

            List<PermisosMenuModel> permisosMenuModels = (List<PermisosMenuModel>)Session["lsAccesosPermitidos"];
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == Guid.Parse("ED42C1B1-1C5F-41ED-90B7-76F150FF5EFB")).ToList();
            foreach (var item in permisosMenuModels)
            {
                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        Guid UidColegiatura = Guid.NewGuid();

                        if (colegiaturasServices.RegistrarColegiatura(UidColegiatura, txtIdentificador.Text.Trim().ToUpper(), decimal.Parse(txtImporte.Text), int.Parse(txtCantPagos.Text), Guid.Parse(ddlPeriodicidad.SelectedValue), DateTime.Parse(txtFHInicio.Text), cbActivarFHL.Checked, DateTime.Parse(txtFHLimite.Text), cbActivarFHV.Checked, DateTime.Parse(txtFHVencimiento.Text), bool.Parse(ddlRecargo.SelectedValue), ddlTipoRecargo.SelectedValue, decimal.Parse(txtRecargo.Text), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                        {
                            RegistrarFechas(UidColegiatura);

                            foreach (var itAlum in alumnosServices.lsSelectAlumnosGridViewModel)
                            {
                                alumnosServices.RegistrarColeAlumnos(UidColegiatura, itAlum.UidAlumno);
                            }

                            colegiaturasServices.CargarColegiaturas(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                            gvColegiaturas.DataSource = colegiaturasServices.lsColegiaturasGridViewModel;
                            gvColegiaturas.DataBind();

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
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
                        if (colegiaturasServices.ActualizarColegiatura(Guid.Parse(ViewState["UidRequerido"].ToString()), txtIdentificador.Text.Trim().ToUpper(), decimal.Parse(txtImporte.Text), int.Parse(txtCantPagos.Text), Guid.Parse(ddlPeriodicidad.SelectedValue), DateTime.Parse(txtFHInicio.Text), cbActivarFHL.Checked, DateTime.Parse(txtFHLimite.Text), cbActivarFHV.Checked, DateTime.Parse(txtFHVencimiento.Text), bool.Parse(ddlRecargo.SelectedValue), ddlTipoRecargo.SelectedValue, decimal.Parse(txtRecargo.Text)))
                        {
                            colegiaturasServices.EliminarColegiaturaFechas(Guid.Parse(ViewState["UidRequerido"].ToString()));
                            RegistrarFechas(Guid.Parse(ViewState["UidRequerido"].ToString()));

                            alumnosServices.EliminarColeAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()));
                            foreach (var itAlum in alumnosServices.lsSelectAlumnosGridViewModel)
                            {
                                alumnosServices.RegistrarColeAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()), itAlum.UidAlumno);
                            }

                            colegiaturasServices.CargarColegiaturas(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                            gvColegiaturas.DataSource = colegiaturasServices.lsColegiaturasGridViewModel;
                            gvColegiaturas.DataBind();

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                        }
                    }
                    else
                    {
                        lblMensajeAlert.Text = "<b>Lo sentimos,</b> no tiene permisos para esta acción.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                    }
                }
            }
        }
        private void RegistrarFechas(Guid UidColegiatura)
        {
            colegiaturasServices.RegistrarColegiaturaFechas(UidColegiatura, DateTime.Parse(txtFHInicio.Text), DateTime.Parse(txtFHLimite.Text), DateTime.Parse(txtFHVencimiento.Text));

            for (int i = 1; i < int.Parse(txtCantPagos.Text); i++)
            {
                if (ddlPeriodicidad.SelectedItem.Text == "SEMANAL")
                {
                    DateTime FHInicio = DateTime.Parse(txtFHInicio.Text).AddDays(7 * i);
                    DateTime FHLimite = DateTime.Parse("01/01/0001 12:00:00 p. m.");
                    DateTime FHVencimiento = DateTime.Parse("01/01/0001 12:00:00 p. m.");

                    if (cbActivarFHL.Checked)
                    {
                        FHLimite = DateTime.Parse(txtFHLimite.Text).AddDays(7 * i);
                    }

                    if (cbActivarFHV.Checked)
                    {
                        FHVencimiento = DateTime.Parse(txtFHVencimiento.Text).AddDays(7 * i);
                    }

                    colegiaturasServices.RegistrarColegiaturaFechas(UidColegiatura, FHInicio, FHLimite, FHVencimiento);
                }
                else if (ddlPeriodicidad.SelectedItem.Text == "MENSUAL")
                {
                    DateTime FHInicio = DateTime.Parse(txtFHInicio.Text).AddMonths(1 * i);
                    DateTime FHLimite = DateTime.Parse("01/01/0001 12:00:00 p. m.");
                    DateTime FHVencimiento = DateTime.Parse("01/01/0001 12:00:00 p. m.");

                    if (cbActivarFHL.Checked)
                    {
                        FHLimite = DateTime.Parse(txtFHLimite.Text).AddMonths(1 * i);
                    }

                    if (cbActivarFHV.Checked)
                    {
                        FHVencimiento = DateTime.Parse(txtFHVencimiento.Text).AddMonths(1 * i);
                    }

                    colegiaturasServices.RegistrarColegiaturaFechas(UidColegiatura, FHInicio, FHLimite, FHVencimiento);
                }
            }
        }
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ViewState["UidRequerido"] = Guid.Empty;

            lblValidar.Text = string.Empty;
            ViewState["Accion"] = "Guardar";
            LimpiarCampos();
            DesbloquearCampos();
            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            lblTituloModal.Text = "Registro de Colegiatura";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);

        }

        private void BloquearCampos()
        {
            txtIdentificador.Enabled = false;
            txtImporte.Enabled = false;
            ddlRecargo.Enabled = false;
            ddlRecargo_SelectedIndexChanged(null, null);
            pnlRecargo.Enabled = false;
            txtCantPagos.Enabled = false;
            txtFHInicio.Enabled = false;
            cbActivarFHL.Enabled = false;
            txtFHLimite.Enabled = false;
            cbActivarFHV.Enabled = false;
            txtFHVencimiento.Enabled = false;
            ddlPeriodicidad.Enabled = false;
        }
        private void DesbloquearCampos()
        {
            txtIdentificador.Enabled = true;
            txtImporte.Enabled = true;
            ddlRecargo.Enabled = true;
            ddlRecargo_SelectedIndexChanged(null, null);
            pnlRecargo.Enabled = true;
            txtCantPagos.Enabled = true;
            txtFHInicio.Enabled = true;
            cbActivarFHL.Enabled = true;
            txtFHLimite.Enabled = true;
            cbActivarFHV.Enabled = true;
            txtFHVencimiento.Enabled = true;
            ddlPeriodicidad.Enabled = true;
        }
        private void LimpiarCampos()
        {
            txtIdentificador.Text = string.Empty;
            txtImporte.Text = string.Empty;
            ddlRecargo.SelectedIndex = -1;
            ddlRecargo_SelectedIndexChanged(null, null);
            ddlTipoRecargo.SelectedIndex = -1;
            txtRecargo.Text = string.Empty;
            txtCantPagos.Text = string.Empty;
            txtFHInicio.Text = string.Empty;
            cbActivarFHL.Checked = false;
            txtFHLimite.Text = string.Empty;
            cbActivarFHV.Checked = false;
            txtFHVencimiento.Text = string.Empty;
            ddlPeriodicidad.SelectedIndex = -1;

            cbActivarFHV_CheckedChanged(null, null);
            cbActivarFHL_CheckedChanged(null, null);
        }

        protected void gvColegiaturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvColegiaturas, "Select$" + e.Row.RowIndex);

                LinkButton btnCargarExcel = e.Row.FindControl("btnCargarExcel") as LinkButton;

                btnCargarExcel.Attributes.Add("onclick", "document.getElementById('" + fuSelecionarExcel.ClientID + "').click(); return false;");
            }
        }

        protected void gvColegiaturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                ViewState["Accion"] = "Actualizar";
                btnCerrar.Visible = false;
                btnCancelar.Visible = true;
                btnGuardar.Visible = true;
                btnEditar.Visible = false;
                lblTituloModal.Text = "Actualizar Colegiatura";
                btnGuardar.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvColegiaturas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);
                DesbloquearCampos();

                alumnosServices.ObtenerColeAlumnos(dataKeys);
                lblCantSeleccionado.Text = alumnosServices.lsSelectAlumnosGridViewModel.Count.ToString();
                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                gvAlumnos.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }

            if (e.CommandName == "Ver")
            {
                btnCerrar.Visible = true;
                btnCancelar.Visible = false;
                btnGuardar.Visible = false;
                btnEditar.Visible = true;
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvColegiaturas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);
                BloquearCampos();

                lblTituloModal.Text = "Visualización de Usuario";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }

            if (e.CommandName == "Detalle")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvColegiaturas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                gvFechas.DataSource = colegiaturasServices.ObtenerFechaColegiatura(dataKeys);
                gvFechas.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalDetalle()", true);
            }

            if (e.CommandName == "Importar")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvColegiaturas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                LinkButton btnAbrirImpor = (LinkButton) row.FindControl("btnAbrirImpor");
                LinkButton btnCargarExcel = (LinkButton) row.FindControl("btnCargarExcel");
                LinkButton btnCancelarExcel = (LinkButton) row.FindControl("btnCancelarExcel");

                btnAbrirImpor.Visible = false;
                btnCargarExcel.Visible = true;
                btnCancelarExcel.Visible = true;
            }
            if (e.CommandName == "CancelarImport")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvColegiaturas.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                LinkButton btnAbrirImpor = (LinkButton) row.FindControl("btnAbrirImpor");
                LinkButton btnCargarExcel = (LinkButton) row.FindControl("btnCargarExcel");
                LinkButton btnCancelarExcel = (LinkButton) row.FindControl("btnCancelarExcel");

                btnAbrirImpor.Visible = true;
                btnCargarExcel.Visible = false;
                btnCancelarExcel.Visible = false;
            }
        }

        private void ManejoDatos(Guid dataKeys)
        {
            colegiaturasServices.ObtenerColegiatura(dataKeys);
            txtIdentificador.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchIdentificador;
            txtImporte.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.DcmImporte.ToString("N2");
            if (colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.BitRecargo)
            {
                ddlRecargo.SelectedIndex = 1;
                ddlTipoRecargo.SelectedValue = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchTipoRecargo;
                txtRecargo.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.DcmRecargo.ToString();
            }
            else
            {
                ddlRecargo.SelectedIndex = 0;
                txtRecargo.Text = string.Empty;
            }
            txtCantPagos.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.IntCantPagos.ToString();
            ddlPeriodicidad.SelectedIndex = ddlPeriodicidad.Items.IndexOf(ddlPeriodicidad.Items.FindByValue(colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.UidPeriodicidad.ToString()));
            txtFHInicio.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.DtFHInicio.ToString("yyyy-MM-dd");
            if (colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHLimite != string.Empty && colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHLimite != "NO TIENE")
            {
                txtFHLimite.Text = DateTime.Parse(colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHLimite).ToString("yyyy-MM-dd");
                cbActivarFHL.Checked = true;
                cbActivarFHL_CheckedChanged(null, null);
            }
            if (colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHVencimiento != string.Empty && colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHVencimiento != "NO TIENE")
            {
                txtFHVencimiento.Text = DateTime.Parse(colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHVencimiento).ToString("yyyy-MM-dd");
                cbActivarFHV.Checked = true;
                cbActivarFHV_CheckedChanged(null, null);
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

        protected void gvColegiaturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvColegiaturas.PageIndex = e.NewPageIndex;
            gvColegiaturas.DataSource = padresServices.lsPadres;
            gvColegiaturas.DataBind();
        }
        protected void gvColegiaturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection direccion;
            string Orden = string.Empty;

            if (ViewState["gvColegiaturas"] != null)
            {
                direccion = (SortDirection)ViewState["gvColegiaturas"];
                if (direccion == SortDirection.Ascending)
                {
                    ViewState["gvColegiaturas"] = SortDirection.Descending;
                    Orden = "ASC";
                }
                else
                {
                    ViewState["gvColegiaturas"] = SortDirection.Ascending;
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
                    case "VchUsuario":
                        if (Orden == "ASC")
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderBy(x => x.VchUsuario).ToList();
                        }
                        else
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderByDescending(x => x.VchUsuario).ToList();
                        }
                        break;
                    case "VchNombrePerfil":
                        if (Orden == "ASC")
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderBy(x => x.VchNombrePerfil).ToList();
                        }
                        else
                        {
                            padresServices.lsPadres = padresServices.lsPadres.OrderByDescending(x => x.VchNombrePerfil).ToList();
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

                gvColegiaturas.DataSource = padresServices.lsPadres;
                gvColegiaturas.DataBind();
            }
        }

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            padresServices.BuscarUsuariosFinales(Guid.Parse(ViewState["UidClienteLocal"].ToString()), Guid.Parse("E39FF705-8A01-4302-829A-7CFB9615CC8F"), FiltroNombre.Text, FiltroApePaterno.Text, FiltroApeMaterno.Text, FiltroCorreo.Text, Guid.Parse(FiltroEstatus.SelectedValue));
            gvColegiaturas.DataSource = padresServices.lsPadres;
            gvColegiaturas.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

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

                        if (dt.Columns.Contains("MATRICULA".Trim()))
                        {
                            alumnosServices.ValidarExcelToList(dt, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                            if (alumnosServices.lsExcelSeleccionar.Count >= 1)
                            {
                                alumnosServices.AsignarColeAlumnos(alumnosServices.lsExcelSeleccionar, alumnosServices.lsSelectAlumnosGridViewModel, Guid.Parse(ViewState["UidClienteLocal"].ToString()), txtFiltroAlumNombre.Text.Trim(), txtFiltroAlumPaterno.Text.Trim(), txtFiltroAlumMaterno.Text.Trim(), txtFiltroAlumMatricula.Text.Trim());
                                //gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel.Where(x => x.blSeleccionado == true);
                                //gvAlumnos.DataBind();

                                alumnosServices.EliminarColeAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()));
                                foreach (var itAlum in alumnosServices.lsAlumnosGridViewModel.Where(x => x.blSeleccionado == true))
                                {
                                    alumnosServices.RegistrarColeAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()), itAlum.UidAlumno);
                                }

                                colegiaturasServices.CargarColegiaturas(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                                gvColegiaturas.DataSource = colegiaturasServices.lsColegiaturasGridViewModel;
                                gvColegiaturas.DataBind();
                                //pnlAlert.Visible = true;
                                //lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                                //divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                            }

                            if (alumnosServices.lsExcelErrores.Count >= 1)
                            {
                                btnDescargarError.Visible = true;
                                btnMasDetalle.Visible = false;
                                pnlAlertImportarError.Visible = true;
                                lblMnsjAlertImportarError.Text = "<strong>!Lo sentimos¡</strong> algunos alumnos no se han podido importar. Posible(s) errores: La matricula puede estar mal escrito.";
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

                        //Response.Write("");
                        //Page.Response.Redirect(Page.Request.Url.ToString(), true); RESUELVE TEMPORALMENTE EL LOADING(REVISAR)
                        //StringBuilder sbScript = new StringBuilder();
                        //sbScript.Append("<script language='JavaScript' type='text/javascript'>\n");
                        //sbScript.Append("<!--\n");
                        //sbScript.Append(this.GetPostBackEventReference(this, "PBArg") + ";\n");
                        //sbScript.Append("// -->\n");
                        //sbScript.Append("</script>\n");
                        //this.RegisterStartupScript("AutoPostBackScript", sbScript.ToString());
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "DoPostBack", "__doPostBack(sender, e)", true);
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
        protected void btnDescargarError_Click(object sender, EventArgs e)
        {
            Session["lsExcelSeleccionar"] = null;
            Session["lsExcelErrores"] = alumnosServices.lsExcelErrores;

            string _open = "window.open('Excel/ExportarExcelAlumnos.aspx', '_blank');";
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
            Session["lsExcelErrores"] = null;
        }

        protected void btnFiltroBuscar_Click(object sender, EventArgs e)
        {
            alumnosServices.AsignarColeAlumnos(alumnosServices.lsExcelSeleccionar, alumnosServices.lsSelectAlumnosGridViewModel, Guid.Parse(ViewState["UidClienteLocal"].ToString()), txtFiltroAlumNombre.Text.Trim(), txtFiltroAlumPaterno.Text.Trim(), txtFiltroAlumMaterno.Text.Trim(), txtFiltroAlumMatricula.Text.Trim());
            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
            gvAlumnos.DataBind();
        }
        protected void btnFiltroLimpiar_Click(object sender, EventArgs e)
        {
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

        protected void cbActivarFHL_CheckedChanged(object sender, EventArgs e)
        {
            if (cbActivarFHL.Checked)
            {
                pnlActivarFHL.Enabled = true;
            }
            else
            {
                pnlActivarFHL.Enabled = false;
            }
        }
        protected void cbActivarFHV_CheckedChanged(object sender, EventArgs e)
        {
            if (cbActivarFHV.Checked)
            {
                pnlActivarFHV.Enabled = true;
            }
            else
            {
                pnlActivarFHV.Enabled = false;
            }
        }

        protected void gvFechas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void gvFechas_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void ddlRecargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bool.Parse(ddlRecargo.SelectedValue))
            {
                pnlRecargo.Visible = true;
            }
            else
            {
                pnlRecargo.Visible = false;
                ddlTipoRecargo.SelectedIndex = -1;
                txtRecargo.Text = string.Empty;
            }
        }
    }
}