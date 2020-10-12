using ClosedXML.Excel;
using Franquicia.Bussiness;
using Franquicia.Domain.ViewModels;
using Franquicia.WebForms.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
        PadresServices padresServices = new PadresServices();

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

                ViewState["gvAlumnos"] = SortDirection.Ascending;

                Session["alumnosServices"] = alumnosServices;
                Session["telefonosAlumnosServices"] = telefonosAlumnosServices;
                Session["estatusService"] = estatusService;
                Session["prefijosTelefonicosServices"] = prefijosTelefonicosServices;
                Session["padresServices"] = padresServices;

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
                FiltroEstatus.Items.Insert(0, new ListItem("TODOS", "00000000-0000-0000-0000-000000000000"));
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
                padresServices = (PadresServices)Session["padresServices"];

                lblValidar.Text = string.Empty;

                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");

                pnlAlertMnsjModalDesasociar.Visible = false;
                lblMnsjModalDesasociar.Text = "";
                divAlertMnsjModalDesasociar.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
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
                txtMatricula.BackColor = System.Drawing.Color.Transparent;

                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        Guid UidAlumno = Guid.NewGuid();

                        if (validacionesServices.ExisteMatricula(txtMatricula.Text))
                        {
                            txtMatricula.BackColor = System.Drawing.Color.FromName("#f2dede");
                            lblValidar.Text = "La matricula ingresado ya existe por favor intente con otro.";
                        }
                        else
                        {
                            if (alumnosServices.RegistrarAlumno(UidAlumno, txtIdentificador.Text.Trim().ToUpper(), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtMatricula.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), bool.Parse(ddlBeca.SelectedValue), ddlTipoBeca.SelectedValue, decimal.Parse(txtBeca.Text), txtNumero.Text.Trim(), Guid.Parse(ddlPrefijo.SelectedValue), Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                            {
                                pnlAlert.Visible = true;
                                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha registrado exitosamente.";
                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                                ViewState["NewPageIndex"] = null;
                                alumnosServices.CargarAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                                gvAlumnos.DataBind();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                            }
                        }
                    }
                    else
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<strong>Lo sentimos,</strong> no tiene permisos para esta acción.";
                        divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                    }
                }
                else if (ViewState["Accion"].ToString() == "Actualizar")
                {
                    if (item.Actualizar)
                    {
                        bool Actualizar = true;

                        if (ViewState["ActualizarMatricula"].ToString() != txtMatricula.Text)
                        {
                            if (validacionesServices.ExisteMatricula(txtMatricula.Text))
                            {
                                txtMatricula.BackColor = System.Drawing.Color.FromName("#f2dede");
                                lblValidar.Text = "La matricula que desea actualizar ya existe por favor intente con otro.";
                                Actualizar = false;
                            }
                        }

                        if (Actualizar)
                        {
                            if (alumnosServices.ActualizarAlumno(Guid.Parse(ViewState["UidRequerido"].ToString()), txtIdentificador.Text.Trim().ToUpper(), txtNombre.Text.Trim().ToUpper(), txtApePaterno.Text.Trim().ToUpper(), txtApeMaterno.Text.Trim().ToUpper(), txtMatricula.Text.Trim().ToUpper(), txtCorreo.Text.Trim().ToUpper(), bool.Parse(ddlBeca.SelectedValue), ddlTipoBeca.SelectedValue, decimal.Parse(txtBeca.Text), Guid.Parse(ddlEstatus.SelectedValue), txtNumero.Text.Trim(), Guid.Parse(ddlPrefijo.SelectedValue)))
                            {
                                pnlAlert.Visible = true;
                                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha actualizado exitosamente.";
                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                                ViewState["NewPageIndex"] = null;
                                alumnosServices.CargarAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                                gvAlumnos.DataBind();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
                            }
                        }
                    }
                    else
                    {
                        pnlAlert.Visible = true;
                        lblMensajeAlert.Text = "<strong>Lo sentimos,</strong> no tiene permisos para esta acción.";
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

            btnActivarGeneral_Click(null, null);
            liActivarDatosPadre.Visible = false;

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

                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[e.Row.RowIndex].Value.ToString());

                if (ViewState["UidRequerido"] != null)
                {
                    if (dataKeys == Guid.Parse(ViewState["UidRequerido"].ToString()))
                    {
                        e.Row.BackColor = Color.FromName("#dff0d8");
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblPaginado = (Label)e.Row.FindControl("lblPaginado");

                int PageSize = gvAlumnos.PageSize;
                int antNum = 0;

                int numTotal = alumnosServices.lsAlumnosGridViewModel.Count;

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

                padresServices.ObtenerAlumnoPadres(dataKeys);
                gvDatosPadres.DataSource = padresServices.lsPadresAlumnosViewModel;
                gvDatosPadres.DataBind();

                if (padresServices.lsPadresAlumnosViewModel.Count >= 1)
                {
                    liActivarDatosPadre.Visible = true;
                }
                else
                {
                    btnActivarGeneral_Click(null, null);
                    liActivarDatosPadre.Visible = false;
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
                GridViewRow Seleccionado = gvAlumnos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);
                BloquearCampos();

                lblTituloModal.Text = "Visualización del Alumno";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }

            if (e.CommandName == "Desasociar")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow Seleccionado = gvAlumnos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                padresServices.ObtenerAlumnoPadres(dataKeys);
                gvPadres.DataSource = padresServices.lsPadresAlumnosViewModel;
                gvPadres.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalDesasociar()", true);
            }
        }
        protected void gvAlumnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAlumnos.PageIndex = e.NewPageIndex;
            ViewState["NewPageIndex"] = e.NewPageIndex;
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
                    case "IntCantPadres":
                        if (Orden == "ASC")
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderBy(x => x.IntCantPadres).ToList();
                        }
                        else
                        {
                            alumnosServices.lsAlumnosGridViewModel = alumnosServices.lsAlumnosGridViewModel.OrderByDescending(x => x.IntCantPadres).ToList();
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

                ViewState["NewPageIndex"] = int.Parse(ViewState["NewPageIndex"].ToString()) - 1;
                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                gvAlumnos.DataBind();
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
            ViewState["ActualizarMatricula"] = alumnosServices.alumnosRepository.alumnosGridViewModel.VchMatricula;
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

        protected void btnFiltros_Click(object sender, EventArgs e)
        {
            FiltroEstatus.SelectedIndex = FiltroEstatus.Items.IndexOf(FiltroEstatus.Items.FindByValue("65e46bc9-1864-4145-ad1a-70f5b5f69739"));

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalBusqueda()", true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["NewPageIndex"] = null;

            alumnosServices.BuscarAlumnos(FiltroIdentificador.Text, FiltroMatricula.Text, FiltroCorreo.Text, FiltroNombre.Text, FiltroApePaterno.Text, FiltroApeMaterno.Text, FiltroCelular.Text, FiltroAsociado.SelectedValue, FiltroBeca.Text, Guid.Parse(FiltroEstatus.SelectedValue), FiltroColegiatura.SelectedValue, Guid.Parse(ViewState["UidClienteLocal"].ToString()));
            gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
            gvAlumnos.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalBusqueda()", true);
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            FiltroIdentificador.Text = string.Empty;
            FiltroMatricula.Text = string.Empty;
            FiltroCorreo.Text = string.Empty;
            FiltroNombre.Text = string.Empty;
            FiltroApePaterno.Text = string.Empty;
            FiltroApeMaterno.Text = string.Empty;
            FiltroCelular.Text = string.Empty;
            FiltroAsociado.SelectedIndex = 0;
            FiltroBeca.SelectedIndex = 0;
            FiltroEstatus.SelectedIndex = 0;
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

        #region IMPORTACION DE ALUMNOS
        protected void btnExportarLista_Click(object sender, EventArgs e)
        {
            Session["lsAlumnosGridViewModel"] = alumnosServices.lsAlumnosGridViewModel;
            Session["lsExcelErrores"] = null;
            string _open = "window.open('Excel/ExportarExcelAlumnos.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
        }
        protected void btnDescargarError_Click(object sender, EventArgs e)
        {
            Session["lsAlumnosGridViewModel"] = null;
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

                        if (dt.Columns.Contains("IDENTIFICADOR".Trim()) && dt.Columns.Contains("MATRICULA".Trim()) && dt.Columns.Contains("NOMBRE(S)".Trim()) && dt.Columns.Contains("APEPATERNO".Trim()) && dt.Columns.Contains("APEMATERNO".Trim()) && dt.Columns.Contains("CORREO".Trim()) && dt.Columns.Contains("CELULAR".Trim()) && dt.Columns.Contains("BECA".Trim()) && dt.Columns.Contains("TIPO BECA".Trim()) && dt.Columns.Contains("CANTIDAD".Trim()) && dt.Columns.Contains("ESTATUS".Trim()))
                        {
                            alumnosServices.ValidarAlumnosExcelToList(dt);

                            if (alumnosServices.lsExcelInsertar.Count >= 1)
                            {
                                alumnosServices.AccionAlumnosExcelToList(alumnosServices.lsExcelInsertar, Guid.Parse(ViewState["UidClienteLocal"].ToString()));

                                pnlAlert.Visible = true;
                                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> " + alumnosServices.lsExcelInsertar.Count() + " alumno(s) se ha registrado/actualizado exitosamente.";
                                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                                ViewState["NewPageIndex"] = null;
                                alumnosServices.CargarAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                                gvAlumnos.DataBind();
                            }

                            if (alumnosServices.lsExcelErrores.Count >= 1)
                            {
                                btnDescargarError.Visible = true;
                                btnMasDetalle.Visible = false;
                                pnlAlertImportarError.Visible = true;
                                lblMnsjAlertImportarError.Text = "<strong>!Lo sentimos¡</strong>" + alumnosServices.lsExcelErrores.Count() + " alumno(s) no se han importado.";
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

        #region Proceso Desasociar Padres
        protected void btnDesasociar_Click(object sender, EventArgs e)
        {
            bool Seleccionado = false;

            foreach (var item in padresServices.lsPadresAlumnosViewModel)
            {
                if (item.blSeleccionado)
                {
                    Seleccionado = true;
                }
            }

            if (Seleccionado)
            {
                lblMnsjDialog.Text = "Esta apunto de desvincular <strong>los(el) siguiente(s) Tutor(es).</strong>";

                rptSelectPadres.DataSource = padresServices.lsPadresAlumnosViewModel.Where(x => x.blSeleccionado == true);
                rptSelectPadres.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModalDialog()", true);
            }
            else
            {
                pnlAlertMnsjModalDesasociar.Visible = true;
                lblMnsjModalDesasociar.Text = "Por favor seleccione al menos un tutor.";
                divAlertMnsjModalDesasociar.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            }
        }
        protected void cbSeleccionado_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)checkBox.Parent.Parent;
            Guid dataKey = Guid.Parse(gvPadres.DataKeys[gr.RowIndex].Value.ToString());

            CheckBox cbSeleccionado = (CheckBox)gr.FindControl("cbSeleccionado");

            if (cbSeleccionado.Checked)
            {
                padresServices.ActualizarLsDesasociarPadres(padresServices.lsPadresAlumnosViewModel, dataKey, true);
            }
            else
            {
                padresServices.ActualizarLsDesasociarPadres(padresServices.lsPadresAlumnosViewModel, dataKey, false);
            }

            //lblCantSeleccionado.Text = alumnosServices.padresServices.lsPadresSelectAlumnosViewModel.Count.ToString();
            gvPadres.DataSource = padresServices.lsPadresAlumnosViewModel;
            gvPadres.DataBind();
        }
        protected void btnSi_Click(object sender, EventArgs e)
        {
            int error = 0;

            foreach (var item in padresServices.lsPadresAlumnosViewModel.Where(x => x.blSeleccionado == true))
            {
                if (alumnosServices.DesasociarPadreAlumno(item.UidUsuario, Guid.Parse(ViewState["UidRequerido"].ToString())))
                {
                }
                else
                {
                    error++;
                }
            }

            if (error != 0)
            {
                pnlAlertMnsjModalDesasociar.Visible = true;
                lblMnsjModalDesasociar.Text = "<strong>¡Lo sentimos! </strong> algunos padres no se han podido desasociado, por favor intente mas tarde.";
                divAlertMnsjModalDesasociar.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            }
            else
            {
                pnlAlertMnsjModalDesasociar.Visible = true;
                lblMnsjModalDesasociar.Text = "<strong>¡Felicidades! </strong> se ha desasociado exitosamente.";
                divAlertMnsjModalDesasociar.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            }

            padresServices.ObtenerAlumnoPadres(Guid.Parse(ViewState["UidRequerido"].ToString()));
            gvPadres.DataSource = padresServices.lsPadresAlumnosViewModel;
            gvPadres.DataBind();

            if (padresServices.lsPadresAlumnosViewModel.Count >= 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModalDialog()", true);
            }
            else
            {
                pnlAlert.Visible = true;
                lblMensajeAlert.Text = "<strong>¡Felicidades! </strong> se ha desasociado exitosamente.";
                divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                ViewState["NewPageIndex"] = null;
                alumnosServices.CargarAlumnos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                gvAlumnos.DataSource = alumnosServices.lsAlumnosGridViewModel;
                gvAlumnos.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideDialogDesasociar()", true);
            }
        }
        #endregion

        #region Pestañas(Menu)
        protected void btnActivarGeneral_Click(object sender, EventArgs e)
        {
            btnActivarGeneral.CssClass = "nav-link active show";
            pnlActivarGeneral.Visible = true;

            btnActivarDatosPadre.CssClass = "nav-link";
            pnlActivarDatosPadre.Visible = false;
        }
        protected void btnActivarDatosPadre_Click(object sender, EventArgs e)
        {
            btnActivarGeneral.CssClass = "nav-link";
            pnlActivarGeneral.Visible = false;

            btnActivarDatosPadre.CssClass = "nav-link active show";
            pnlActivarDatosPadre.Visible = true;
        }
        #endregion
    }
}