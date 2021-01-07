using ClosedXML.Excel;
using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using PagaLaEscuela.Util;
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
        PromocionesServices promocionesServices = new PromocionesServices();

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

            txtFHInicio.Attributes.Add("onchange", "button_click(this,'" + btnCalcular.ClientID + "')");
            txtFHLimite.Attributes.Add("onchange", "button_click(this,'" + btnCalcularFHV.ClientID + "')");

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

                promocionesServices.CargarPromocionesClientes(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                ListBoxPromociones.DataSource = promocionesServices.lsCBLPromocionesModelCliente;
                ListBoxPromociones.DataTextField = "VchDescripcion";
                ListBoxPromociones.DataValueField = "UidPromocion";
                ListBoxPromociones.DataBind();

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

                periodicidadesServices.CargarPeriodicidades();
                ddlPeriodicidad.DataSource = periodicidadesServices.lsPeriodicidades;
                ddlPeriodicidad.DataTextField = "VchDescripcion";
                ddlPeriodicidad.DataValueField = "UidPeriodicidad";
                ddlPeriodicidad.DataBind();

                FiltroPeriodicidad.DataSource = periodicidadesServices.lsPeriodicidades;
                FiltroPeriodicidad.Items.Insert(0, new ListItem("TODOS", "00000000-0000-0000-0000-000000000000"));
                FiltroPeriodicidad.DataTextField = "VchDescripcion";
                FiltroPeriodicidad.DataValueField = "UidPeriodicidad";
                FiltroPeriodicidad.DataBind();
            }
            else
            {
                colegiaturasServices = (ColegiaturasServices)Session["colegiaturasServices"];
                padresServices = (PadresServices)Session["padresServices"];
                periodicidadesServices = (PeriodicidadesServices)Session["periodicidadesServices"];
                estatusService = (EstatusServices)Session["estatusService"];
                prefijosTelefonicosServices = (PrefijosTelefonicosServices)Session["prefijosTelefonicosServices"];
                alumnosServices = (AlumnosServices)Session["alumnosServices"];

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", "multi()", true);

                lblValidar.Text = string.Empty;

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertMnsjEstatus.Visible = false;
                lblMnsjEstatus.Text = "";
                divAlertMnsjEstatus.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string MontoMin = "50.00";
            string MontoMax = "15000.00";

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
            if (decimal.Parse(txtImporte.Text) >= decimal.Parse(MontoMin) && decimal.Parse(txtImporte.Text) <= decimal.Parse(MontoMax))
            {

            }
            else
            {
                txtImporte.BackColor = System.Drawing.Color.FromName("#f2dede");
                lblValidar.Text = "El importe mínimo es de $50.00 y el máximo es de $15,000.00.";
                return;
            }
            if (cbActivarRL.Checked)
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
            if (cbActivarRP.Checked)
            {
                if (txtRecargoP.EmptyTextBox())
                {
                    lblValidar.Text = "El campo Recargo es obligatorio";
                    return;
                }

                if (ddlTipoRecargoP.SelectedValue == "PORCENTAJE")
                {
                    if (decimal.Parse(txtRecargoP.Text) > 100)
                    {
                        lblValidar.Text = "Cuando la selección es PORCENTAJE, Solo se admite hasta 100";
                        txtRecargoP.BackColor = System.Drawing.Color.FromName("#f2dede");
                        return;
                    }
                }
            }
            else
            {
                txtRecargoP.Text = "0";
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

                if (DateTime.Parse(txtFHLimite.Text) >= DateTime.Parse(ViewState["FHMin"].ToString()) && DateTime.Parse(txtFHLimite.Text) <= DateTime.Parse(ViewState["FHMax"].ToString()))
                {

                }
                else
                {
                    lblValidar.Text = "El campo Fecha Limite no puede estar fuera del rango de la periodicidad.";
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
                if (DateTime.Parse(txtFHVencimiento.Text) >= DateTime.Parse(ViewState["FHMin"].ToString()) && DateTime.Parse(txtFHVencimiento.Text) <= DateTime.Parse(ViewState["FHMax"].ToString()))
                {

                }
                else
                {
                    lblValidar.Text = "El campo Fecha Vencimiento no puede estar fuera del rango de la periodicidad.";
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

                        if (colegiaturasServices.RegistrarColegiatura(UidColegiatura, txtIdentificador.Text.Trim().ToUpper(), decimal.Parse(txtImporte.Text), int.Parse(txtCantPagos.Text), Guid.Parse(ddlPeriodicidad.SelectedValue), DateTime.Parse(txtFHInicio.Text), cbActivarFHL.Checked, DateTime.Parse(txtFHLimite.Text), cbActivarFHV.Checked, DateTime.Parse(txtFHVencimiento.Text), cbActivarRL.Checked, ddlTipoRecargo.SelectedValue, decimal.Parse(txtRecargo.Text), cbActivarRP.Checked, ddlTipoRecargoP.SelectedValue, decimal.Parse(txtRecargoP.Text), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                        {
                            RegistrarFechas(UidColegiatura);

                            colegiaturasServices.ObtenerFechasColegiaturasVicular(UidColegiatura);
                            foreach (var itFechaColegiatura in colegiaturasServices.lsFechasColegiaturas)
                            {
                                foreach (var itAlumno in alumnosServices.lsSelectAlumnosGridViewModel)
                                {
                                    colegiaturasServices.RegistrarFechasColegiaturasAlumnos(itFechaColegiatura.UidFechaColegiatura, itAlumno.UidAlumno, decimal.Parse(txtImporte.Text), Guid.Parse("76C8793B-4493-44C8-B274-696A61358BDF"));
                                }
                            }

                            foreach (ListItem promo in ListBoxPromociones.Items)
                            {
                                if (promo.Selected)
                                {
                                    colegiaturasServices.RegistrarPromocionesColegiatura(UidColegiatura, Guid.Parse(promo.Value));
                                }
                            }

                            foreach (var itAlum in alumnosServices.lsSelectAlumnosGridViewModel)
                            {
                                alumnosServices.RegistrarColeAlumnos(UidColegiatura, itAlum.UidAlumno);
                            }

                            ViewState["NewPageIndex"] = null;
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
                        if (colegiaturasServices.ActualizarColegiatura(Guid.Parse(ViewState["UidRequerido"].ToString()), txtIdentificador.Text.Trim().ToUpper(), decimal.Parse(txtImporte.Text), int.Parse(txtCantPagos.Text), Guid.Parse(ddlPeriodicidad.SelectedValue), DateTime.Parse(txtFHInicio.Text), cbActivarFHL.Checked, DateTime.Parse(txtFHLimite.Text), cbActivarFHV.Checked, DateTime.Parse(txtFHVencimiento.Text), cbActivarRL.Checked, ddlTipoRecargo.SelectedValue, decimal.Parse(txtRecargo.Text), cbActivarRP.Checked, ddlTipoRecargoP.SelectedValue, decimal.Parse(txtRecargoP.Text)))
                        {
                            colegiaturasServices.ObtenerFechasColegiaturasVicular(Guid.Parse(ViewState["UidRequerido"].ToString()));
                            foreach (var itFechaColegiatura in colegiaturasServices.lsFechasColegiaturas)
                            {
                                colegiaturasServices.EliminarFechasColegiaturasAlumnos(itFechaColegiatura.UidFechaColegiatura);
                            }

                            colegiaturasServices.EliminarColegiaturaFechas(Guid.Parse(ViewState["UidRequerido"].ToString()));
                            RegistrarFechas(Guid.Parse(ViewState["UidRequerido"].ToString()));

                            colegiaturasServices.ObtenerFechasColegiaturasVicular(Guid.Parse(ViewState["UidRequerido"].ToString()));
                            foreach (var itFechaColegiatura in colegiaturasServices.lsFechasColegiaturas)
                            {
                                foreach (var itAlumno in alumnosServices.lsSelectAlumnosGridViewModel)
                                {
                                    colegiaturasServices.RegistrarFechasColegiaturasAlumnos(itFechaColegiatura.UidFechaColegiatura, itAlumno.UidAlumno, decimal.Parse(txtImporte.Text), Guid.Parse("76C8793B-4493-44C8-B274-696A61358BDF"));
                                }
                            }

                            colegiaturasServices.EliminarPromocionesColegiatura(Guid.Parse(ViewState["UidRequerido"].ToString()));
                            foreach (ListItem promo in ListBoxPromociones.Items)
                            {
                                if (promo.Selected)
                                {
                                    colegiaturasServices.RegistrarPromocionesColegiatura(Guid.Parse(ViewState["UidRequerido"].ToString()), Guid.Parse(promo.Value));
                                }
                            }

                            alumnosServices.EliminarColeAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()));
                            foreach (var itAlum in alumnosServices.lsSelectAlumnosGridViewModel)
                            {
                                alumnosServices.RegistrarColeAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()), itAlum.UidAlumno);
                            }

                            ViewState["NewPageIndex"] = null;
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
                else if (ViewState["Accion"].ToString() == "ActualizarEstatus")
                {
                    if (colegiaturasServices.ActualizarEstatusColegiatura(Guid.Parse(ViewState["UidRequerido"].ToString()), Guid.Parse(ddlEstatus.SelectedValue)))
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha actualizado exitosamente.";
                        divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                    }
                    else
                    {
                        lblMensajeAlert.Text = "<strong>Lo sentimos</strong> no se ha podido actualizar.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                    }

                    ViewState["NewPageIndex"] = null;
                    colegiaturasServices.CargarColegiaturas(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                    gvColegiaturas.DataSource = colegiaturasServices.lsColegiaturasGridViewModel;
                    gvColegiaturas.DataBind();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                }
            }
        }
        private void RegistrarFechas(Guid UidColegiatura)
        {
            DateTime FHFinPerido = DateTime.Parse(ViewState["FHFinPerido"].ToString());

            colegiaturasServices.RegistrarColegiaturaFechas(UidColegiatura, 1, DateTime.Parse(txtFHInicio.Text), DateTime.Parse(txtFHLimite.Text), DateTime.Parse(txtFHVencimiento.Text), FHFinPerido);

            int num = 2;

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

                    FHFinPerido = DateTime.Parse(ViewState["FHFinPerido"].ToString()).AddDays(7 * i);

                    colegiaturasServices.RegistrarColegiaturaFechas(UidColegiatura, num++, FHInicio, FHLimite, FHVencimiento, FHFinPerido);
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

                    FHFinPerido = DateTime.Parse(ViewState["FHFinPerido"].ToString()).AddMonths(1 * i);

                    colegiaturasServices.RegistrarColegiaturaFechas(UidColegiatura, num++, FHInicio, FHLimite, FHVencimiento, FHFinPerido);
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

            btnFiltroLimpiar_Click(null, null);
            alumnosServices.lsAlumnosGridViewModel.Clear();
            gvAlumnos.DataBind();
            alumnosServices.lsSelectAlumnosGridViewModel.Clear();
            lblCantSeleccionado.Text = alumnosServices.lsSelectAlumnosGridViewModel.Count().ToString();

            btnCerrar.Visible = false;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            lblTituloModal.Text = "Registro de Colegiatura";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
            txtFHInicio.Text = hoy.ToString("yyyy-MM-dd");
            btnCalcular_Click(null, null);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "showTab()", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);

        }

        private void BloquearCampos()
        {
            txtIdentificador.Enabled = false;
            txtImporte.Enabled = false;
            cbActivarRL.Enabled = false;
            cbActivarRP.Enabled = false;
            pnlRecargo.Enabled = false;
            txtCantPagos.Enabled = false;
            txtFHInicio.Enabled = false;
            cbActivarFHL.Enabled = false;
            txtFHLimite.Enabled = false;
            cbActivarFHV.Enabled = false;
            txtFHVencimiento.Enabled = false;
            ddlPeriodicidad.Enabled = false;
            ListBoxPromociones.Enabled = false;
        }
        private void DesbloquearCampos()
        {
            txtIdentificador.Enabled = true;
            txtImporte.Enabled = true;
            cbActivarRL.Enabled = true;
            cbActivarRP.Enabled = true;
            pnlRecargo.Enabled = true;
            txtCantPagos.Enabled = true;
            txtFHInicio.Enabled = true;
            cbActivarFHL.Enabled = true;
            txtFHLimite.Enabled = true;
            cbActivarFHV.Enabled = true;
            txtFHVencimiento.Enabled = true;
            ddlPeriodicidad.Enabled = true;
            ListBoxPromociones.Enabled = true;
        }
        private void LimpiarCampos()
        {
            txtIdentificador.Text = string.Empty;
            txtImporte.Text = string.Empty;
            cbActivarRL.Checked = false;
            cbActivarRP.Checked = false;
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
            cbActivarRL_CheckedChanged(null, null);
            cbActivarRP_CheckedChanged(null, null);

            ListBoxPromociones.SelectedIndex = -1;
        }

        protected void gvColegiaturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvColegiaturas, "Select$" + e.Row.RowIndex);

                LinkButton btnCargarExcel = e.Row.FindControl("btnCargarExcel") as LinkButton;

                btnCargarExcel.Attributes.Add("onclick", "document.getElementById('" + fuSelecionarExcel.ClientID + "').click(); return false;");
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblPaginado = (Label)e.Row.FindControl("lblPaginado");

                int PageSize = gvColegiaturas.PageSize;
                int antNum = 0;

                int numTotal = colegiaturasServices.lsColegiaturasGridViewModel.Count;

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
        protected void gvColegiaturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
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

                alumnosServices.ObtenerColeAlumnos(dataKeys);
                lblCantSeleccionado.Text = alumnosServices.lsSelectAlumnosGridViewModel.Count.ToString();
                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                gvAlumnos.DataBind();

                if (colegiaturasServices.lsColegiaturasGridViewModel.Find(x => x.UidColegiatura == dataKeys).blEditar)
                {
                    ViewState["Accion"] = "Actualizar";
                    DesbloquearCampos();
                }
                else
                {
                    pnlAlertMnsjEstatus.Visible = true;
                    lblMnsjEstatus.Text = "Solo podra modificar el estatus, debido a que hay pagos asociado a las fechas de la colegiatura.";
                    divAlertMnsjEstatus.Attributes.Add("class", "alert alert-info alert-dismissible fade show");

                    ViewState["Accion"] = "ActualizarEstatus";
                    BloquearCampos();
                }

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

                LinkButton btnAbrirImpor = (LinkButton)row.FindControl("btnAbrirImpor");
                LinkButton btnCargarExcel = (LinkButton)row.FindControl("btnCargarExcel");
                LinkButton btnCancelarExcel = (LinkButton)row.FindControl("btnCancelarExcel");

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

                LinkButton btnAbrirImpor = (LinkButton)row.FindControl("btnAbrirImpor");
                LinkButton btnCargarExcel = (LinkButton)row.FindControl("btnCargarExcel");
                LinkButton btnCancelarExcel = (LinkButton)row.FindControl("btnCancelarExcel");

                btnAbrirImpor.Visible = true;
                btnCargarExcel.Visible = false;
                btnCancelarExcel.Visible = false;
            }
        }
        protected void gvColegiaturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvColegiaturas.PageIndex = e.NewPageIndex;
            ViewState["NewPageIndex"] = e.NewPageIndex;
            gvColegiaturas.DataSource = colegiaturasServices.lsColegiaturasGridViewModel;
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
                    case "VchIdentificador":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.VchIdentificador).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.VchIdentificador).ToList();
                        }
                        break;
                    case "DcmImporte":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.DcmImporte).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.DcmImporte).ToList();
                        }
                        break;
                    case "IntCantPagos":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.IntCantPagos).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.IntCantPagos).ToList();
                        }
                        break;
                    case "VchDescripcion":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.VchDescripcion).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.VchDescripcion).ToList();
                        }
                        break;
                    case "DtFHInicio":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.DtFHInicio).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.DtFHInicio).ToList();
                        }
                        break;
                    case "VchFHLimite":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.VchFHLimite).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.VchFHLimite).ToList();
                        }
                        break;
                    case "VchFHVencimiento":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.VchFHVencimiento).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.VchFHVencimiento).ToList();
                        }
                        break;
                    case "DcmRecargo":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.DcmRecargo).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.DcmRecargo).ToList();
                        }
                        break;
                    case "DcmRecargoPeriodo":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.DcmRecargoPeriodo).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.DcmRecargoPeriodo).ToList();
                        }
                        break;
                    case "UidEstatus":
                        if (Orden == "ASC")
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderBy(x => x.UidEstatus).ToList();
                        }
                        else
                        {
                            colegiaturasServices.lsColegiaturasGridViewModel = colegiaturasServices.lsColegiaturasGridViewModel.OrderByDescending(x => x.UidEstatus).ToList();
                        }
                        break;
                }

                ViewState["NewPageIndex"] = int.Parse(ViewState["NewPageIndex"].ToString()) - 1;
                gvColegiaturas.DataSource = colegiaturasServices.lsColegiaturasGridViewModel;
                gvColegiaturas.DataBind();
            }
        }

        private void ManejoDatos(Guid dataKeys)
        {
            colegiaturasServices.ObtenerColegiatura(dataKeys);
            txtIdentificador.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchIdentificador;
            txtImporte.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.DcmImporte.ToString("N2");
            if (colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.BitRecargo)
            {
                ddlTipoRecargo.SelectedValue = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchTipoRecargo;
                txtRecargo.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.DcmRecargo.ToString();
                cbActivarRL.Checked = true;
                cbActivarRL_CheckedChanged(null, null);
            }
            else
            {
                txtRecargo.Text = string.Empty;
                cbActivarRL.Checked = false;
                cbActivarRL_CheckedChanged(null, null);
            }
            if (colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.BitRecargoPeriodo)
            {
                ddlTipoRecargoP.SelectedValue = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchTipoRecargoPeriodo;
                txtRecargoP.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.DcmRecargoPeriodo.ToString();
                cbActivarRP.Checked = true;
                cbActivarRP_CheckedChanged(null, null);
            }
            else
            {
                txtRecargoP.Text = string.Empty;
                cbActivarRP.Checked = false;
                cbActivarRP_CheckedChanged(null, null);
            }
            txtCantPagos.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.IntCantPagos.ToString();
            ddlPeriodicidad.SelectedIndex = ddlPeriodicidad.Items.IndexOf(ddlPeriodicidad.Items.FindByValue(colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.UidPeriodicidad.ToString()));
            txtFHInicio.Text = colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.DtFHInicio.ToString("yyyy-MM-dd");
            ddlPeriodicidad_SelectedIndexChanged(null, null);
            if (colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHLimite != string.Empty && colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHLimite != "NO TIENE")
            {
                txtFHLimite.Text = DateTime.Parse(colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHLimite).ToString("yyyy-MM-dd");
                cbActivarFHL.Checked = true;
                cbActivarFHL_CheckedChanged(null, null);
            }
            else
            {
                cbActivarFHL.Checked = false;
            }
            if (colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHVencimiento != string.Empty && colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHVencimiento != "NO TIENE")
            {
                txtFHVencimiento.Text = DateTime.Parse(colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.VchFHVencimiento).ToString("yyyy-MM-dd");
                cbActivarFHV.Checked = true;
                cbActivarFHV_CheckedChanged(null, null);
            }
            else
            {
                cbActivarFHV.Checked = false;
            }

            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(colegiaturasServices.colegiaturasRepository.colegiaturasGridViewModel.UidEstatus.ToString()));

            promocionesServices.ObtenerPromocionesColegiatura(dataKeys);
            if (promocionesServices.lsPromocionesColegiaturaModel.Count >= 1)
            {
                foreach (ListItem item in ListBoxPromociones.Items)
                {
                    foreach (var it in promocionesServices.lsPromocionesColegiaturaModel)
                    {
                        if (item.Value == it.UidPromocion.ToString())
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            else
            {
                foreach (ListItem item in ListBoxPromociones.Items)
                {
                    item.Selected = false;
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

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            decimal ImporteMayor = 0;
            decimal ImporteMenor = 0;

            if (FiltroImporteMayor.Text != string.Empty)
            {
                switch (ddlImporteMayor.SelectedValue)
                {
                    case ">":
                        ImporteMayor = Convert.ToDecimal(FiltroImporteMayor.Text) + 1;
                        break;
                    case ">=":
                        ImporteMayor = Convert.ToDecimal(FiltroImporteMayor.Text);
                        break;
                }
            }
            if (FiltroImporteMenor.Text != string.Empty)
            {
                switch (ddlImporteMenor.SelectedValue)
                {
                    case "<":
                        ImporteMenor = Convert.ToDecimal(FiltroImporteMenor.Text) - 1;
                        break;
                    case "<=":
                        ImporteMenor = Convert.ToDecimal(FiltroImporteMenor.Text);
                        break;
                }
            }

            ViewState["NewPageIndex"] = null;
            colegiaturasServices.BuscarColegiatura(FiltroIdentificador.Text, ImporteMayor, ImporteMenor, FiltroCantPagos.Text, Guid.Parse(FiltroPeriodicidad.SelectedValue), FiltroInicioDesde.Text, FiltroInicioHasta.Text, FiltroFechaLimite.Text, FiltroFechaVencimiento.Text, FiltroRecargoLimite.Text, FiltroRecargoPeriodo.Text, Guid.Parse(FiltroEstatus.SelectedValue), Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            gvColegiaturas.DataSource = colegiaturasServices.lsColegiaturasGridViewModel;
            gvColegiaturas.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            FiltroIdentificador.Text = string.Empty;
            FiltroImporteMenor.Text = string.Empty;
            FiltroImporteMayor.Text = string.Empty;
            ddlImporteMayor.SelectedIndex = -1;
            ddlImporteMenor.SelectedIndex = -1;
            FiltroCantPagos.Text = string.Empty;
            FiltroPeriodicidad.SelectedIndex = -1;
            FiltroInicioDesde.Text = string.Empty;
            FiltroInicioHasta.Text = string.Empty;
            FiltroFechaLimite.SelectedIndex = -1;
            FiltroFechaVencimiento.SelectedIndex = -1;
            FiltroRecargoLimite.SelectedIndex = -1;
            FiltroRecargoPeriodo.SelectedIndex = -1;
            FiltroEstatus.SelectedIndex = -1;
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
                                alumnosServices.AsignarColeAlumnos(alumnosServices.lsExcelSeleccionar, alumnosServices.lsSelectAlumnosGridViewModel, Guid.Parse(ViewState["UidClienteLocal"].ToString()), txtFiltroAlumIdentificador.Text, txtFiltroAlumNombre.Text.Trim(), txtFiltroAlumPaterno.Text.Trim(), txtFiltroAlumMaterno.Text.Trim(), txtFiltroAlumMatricula.Text.Trim());
                                //gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel.Where(x => x.blSeleccionado == true);
                                //gvAlumnos.DataBind();

                                alumnosServices.EliminarColeAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()));
                                foreach (var itAlum in alumnosServices.lsAlumnosGridViewModel.Where(x => x.blSeleccionado == true))
                                {
                                    alumnosServices.RegistrarColeAlumnos(Guid.Parse(ViewState["UidRequerido"].ToString()), itAlum.UidAlumno);
                                }

                                ViewState["NewPageIndex"] = null;
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
            alumnosServices.AsignarColeAlumnos(alumnosServices.lsExcelSeleccionar, alumnosServices.lsSelectAlumnosGridViewModel, Guid.Parse(ViewState["UidClienteLocal"].ToString()), txtFiltroAlumIdentificador.Text, txtFiltroAlumNombre.Text.Trim(), txtFiltroAlumPaterno.Text.Trim(), txtFiltroAlumMaterno.Text.Trim(), txtFiltroAlumMatricula.Text.Trim());
            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
            gvAlumnos.DataBind();
        }
        protected void btnFiltroLimpiar_Click(object sender, EventArgs e)
        {
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

        protected void cbActivarFHL_CheckedChanged(object sender, EventArgs e)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
            string FHI = txtFHInicio.Text;
            if (string.IsNullOrEmpty(FHI))
            {
                FHI = hoy.ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrEmpty(txtFHLimite.Text))
            {
                txtFHLimite.Text = hoy.ToString("yyyy-MM-dd");
            }

            if (cbActivarFHL.Checked)
            {

                pnlActivarFHL.Enabled = true;

                if (ddlPeriodicidad.SelectedItem.Text == "SEMANAL")
                {
                    txtFHVencimiento.Attributes.Add("min", DateTime.Parse(txtFHLimite.Text).ToString("yyyy-MM-dd"));
                    txtFHVencimiento.Text = DateTime.Parse(FHI).AddDays(6).ToString("yyyy-MM-dd");
                }
                else if (ddlPeriodicidad.SelectedItem.Text == "MENSUAL")
                {
                    txtFHVencimiento.Attributes.Add("min", DateTime.Parse(txtFHLimite.Text).ToString("yyyy-MM-dd"));
                    txtFHVencimiento.Text = DateTime.Parse(FHI).AddMonths(1).ToString("yyyy-MM-dd");
                }

            }
            else
            {
                pnlActivarFHL.Enabled = false;
                txtFHVencimiento.Attributes.Add("min", DateTime.Parse(FHI).ToString("yyyy-MM-dd"));
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

        protected void cbActivarRL_CheckedChanged(object sender, EventArgs e)
        {
            if (cbActivarRL.Checked)
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
        protected void cbActivarRP_CheckedChanged(object sender, EventArgs e)
        {
            if (cbActivarRP.Checked)
            {
                pnlRecargoP.Visible = true;
            }
            else
            {
                pnlRecargoP.Visible = false;
                ddlTipoRecargoP.SelectedIndex = -1;
                txtRecargoP.Text = string.Empty;
            }
        }

        protected void ddlPeriodicidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCalcular_Click(null, null);
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            DateTime FHI = DateTime.Parse(txtFHInicio.Text);

            txtFHLimite.Text = string.Empty;
            txtFHVencimiento.Text = string.Empty;

            if (ddlPeriodicidad.SelectedItem.Text == "SEMANAL")
            {
                ViewState["FHMin"] = FHI.ToString("yyyy-MM-dd");
                txtFHLimite.Attributes.Add("min", FHI.ToString("yyyy-MM-dd"));
                //txtFHVencimiento.Attributes.Add("min", FHI.AddDays(2).ToString("yyyy-MM-dd"));

                txtFHLimite.Attributes.Add("max", FHI.AddDays(6).ToString("yyyy-MM-dd"));
                txtFHVencimiento.Attributes.Add("max", FHI.AddDays(6).ToString("yyyy-MM-dd"));
                ViewState["FHMax"] = FHI.AddDays(6).ToString("yyyy-MM-dd");

                ViewState["FHFinPerido"] = FHI.AddDays(6).ToString("yyyy-MM-dd");

                //Asigna el mismo valor que FHInicio
                txtFHLimite.Text = FHI.ToString("yyyy-MM-dd");
                //Asigna el valor de fin de periodo a vencimiento
                txtFHVencimiento.Text = FHI.AddDays(6).ToString("yyyy-MM-dd");

                //Sacar rango para FHVencimiento entre FHLimite y fin de periodo
                if (cbActivarFHL.Checked)
                {
                    txtFHVencimiento.Attributes.Add("min", DateTime.Parse(txtFHLimite.Text).ToString("yyyy-MM-dd"));
                }
                else
                {
                    txtFHVencimiento.Attributes.Add("min", FHI.ToString("yyyy-MM-dd"));
                }
            }
            else if (ddlPeriodicidad.SelectedItem.Text == "MENSUAL")
            {
                ViewState["FHMin"] = FHI.AddDays(1).ToString("yyyy-MM-dd");
                txtFHLimite.Attributes.Add("min", FHI.ToString("yyyy-MM-dd"));
                //txtFHVencimiento.Attributes.Add("min", FHI.AddDays(2).ToString("yyyy-MM-dd"));

                txtFHLimite.Attributes.Add("max", FHI.AddMonths(1).ToString("yyyy-MM-dd"));
                txtFHVencimiento.Attributes.Add("max", FHI.AddMonths(1).ToString("yyyy-MM-dd"));
                ViewState["FHMax"] = FHI.AddMonths(1).ToString("yyyy-MM-dd");

                ViewState["FHFinPerido"] = FHI.AddMonths(1).ToString("yyyy-MM-dd");

                //Asigna el mismo valor que FHInicio
                txtFHLimite.Text = FHI.ToString("yyyy-MM-dd");
                //Asigna el valor de fin de periodo a vencimiento
                txtFHVencimiento.Text = FHI.AddMonths(1).ToString("yyyy-MM-dd");

                //Sacar rango para FHVencimiento entre FHLimite y fin de periodo
                if (cbActivarFHL.Checked)
                {
                    txtFHVencimiento.Attributes.Add("min", DateTime.Parse(txtFHLimite.Text).ToString("yyyy-MM-dd"));
                }
                else
                {
                    txtFHVencimiento.Attributes.Add("min", FHI.ToString("yyyy-MM-dd"));
                }
            }
        }

        protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DDL DENTRO DE UN GRIDVIEW

            DropDownList DropDownList = (DropDownList)sender;
            GridViewRow gr = (GridViewRow)DropDownList.Parent.Parent;
            Guid dataKey = Guid.Parse(gvColegiaturas.DataKeys[gr.RowIndex].Value.ToString());

            DropDownList ddlEstatus = (DropDownList)gr.FindControl("ddlEstatus");

            if (colegiaturasServices.ActualizarEstatusColegiatura(dataKey, Guid.Parse(ddlEstatus.SelectedValue)))
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha actualizado exitosamente.";
                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            }
            else
            {
                lblMensajeAlert.Text = "<strong>Lo sentimos</strong> no se ha podido actualizar.";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
            }
        }

        protected void btnCalcularFHV_Click(object sender, EventArgs e)
        {
            DateTime FHI = DateTime.Parse(txtFHInicio.Text);

            //Sacar rango para FHVencimiento entre FHLimite y fin de periodo

            if (cbActivarFHL.Checked)
            {
                if (ddlPeriodicidad.SelectedItem.Text == "SEMANAL")
                {
                    txtFHVencimiento.Attributes.Add("min", DateTime.Parse(txtFHLimite.Text).ToString("yyyy-MM-dd"));
                    txtFHVencimiento.Text = FHI.AddDays(6).ToString("yyyy-MM-dd");
                }
                else if (ddlPeriodicidad.SelectedItem.Text == "MENSUAL")
                {
                    txtFHVencimiento.Attributes.Add("min", DateTime.Parse(txtFHLimite.Text).ToString("yyyy-MM-dd"));
                    txtFHVencimiento.Text = FHI.AddMonths(1).ToString("yyyy-MM-dd");
                }
            }
        }
    }
}