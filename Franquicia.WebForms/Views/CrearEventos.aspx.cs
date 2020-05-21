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
    public partial class CrearEventos : System.Web.UI.Page
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
                { return "https://" + ServerName + "/"; }
            }
        }
        #endregion

        EventosServices eventosServices = new EventosServices();
        PromocionesServices promocionesServices = new PromocionesServices();
        EstatusServices estatusServices = new EstatusServices();

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

            if (Session["UidUsuarioMaster"] != null)
            {
                ViewState["UidUsuarioLocal"] = Session["UidUsuarioMaster"];
            }
            else
            {
                ViewState["UidUsuarioLocal"] = Guid.Empty;
            }

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            txtFHInicio.Attributes.Add("min", hoy.AddDays(-1).ToString("yyyy-MM-ddTHH:mm"));
            txtFHInicio.Attributes.Add("max", hoy.AddDays(89).ToString("yyyy-MM-ddTHH:mm"));

            txtFHFinalizacion.Attributes.Add("min", hoy.AddDays(-1).ToString("yyyy-MM-ddTHH:mm"));
            txtFHFinalizacion.Attributes.Add("max", hoy.AddDays(89).ToString("yyyy-MM-ddTHH:mm"));

            if (!IsPostBack)
            {
                Session["eventosServices"] = eventosServices;
                Session["promocionesServices"] = promocionesServices;

                eventosServices.CargarEventos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                gvEventos.DataSource = eventosServices.lsEventosGridViewModel;
                gvEventos.DataBind();

                promocionesServices.CargarPromocionesClientes(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                ListBoxMultipleMod.DataSource = promocionesServices.lsCBLPromocionesModelCliente;
                ListBoxMultipleMod.DataTextField = "VchDescripcion";
                ListBoxMultipleMod.DataValueField = "UidPromocion";
                ListBoxMultipleMod.DataBind();

                estatusServices.CargarEstatus();
                ddlEstatus.DataSource = estatusServices.lsEstatus;
                ddlEstatus.DataTextField = "VchDescripcion";
                ddlEstatus.DataValueField = "UidEstatus";
                ddlEstatus.DataBind();
            }
            else
            {
                eventosServices = (EventosServices)Session["eventosServices"];
                promocionesServices = (PromocionesServices)Session["promocionesServices"];

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Sh", "shot()", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", "multi()", true);


                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
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
            lblTituloModal.Text = "Registro de Evento";
            btnGuardar.Text = "<i class=" + "material-icons>" + "check </i> Guardar";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
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
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            #region ValidarCampos
            if (txtNombreEvento.EmptyTextBox())
            {
                lblValidar.Text = "El campo Nombre Evento es obligatorio";
                return;
            }
            if (txtFHInicio.EmptyTextBox())
            {
                lblValidar.Text = "El campo F/H Inicio es obligatorio";
                return;
            }
            if (txtFHFinalizacion.EmptyTextBox())
            {
                lblValidar.Text = "El campo F/H Finalización es obligatorio";
                return;
            }
            if (txtConcepto.EmptyTextBox())
            {
                lblValidar.Text = "El campo Concepto es obligatorio";
                return;
            }
            if (!string.IsNullOrEmpty(lblValidar.Text))
            {
                lblValidar.Text = string.Empty;
            }
            #endregion

            List<PermisosMenuModel> permisosMenuModels = (List<PermisosMenuModel>)Session["lsAccesosPermitidos"];
            permisosMenuModels = permisosMenuModels.Where(x => x.UidSegModulo == new Guid("61271096-FD44-4D01-B4C6-7ADA6D12FC38")).ToList();
            foreach (var item in permisosMenuModels)
            {
                if (ViewState["Accion"].ToString() == "Guardar")
                {
                    if (item.Agregar)
                    {
                        DateTime HoraDelServidor = DateTime.Now;
                        DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                        Guid UidEvento = Guid.NewGuid();
                        string URL = "https://cobrosmasivos.com/Evento.aspx?Id=" + UidEvento;

                        if (eventosServices.RegistrarEvento(UidEvento, txtNombreEvento.Text.Trim().ToUpper(), txtDescripcion.Text.Trim().ToUpper(), hoy, DateTime.Parse(txtFHInicio.Text), DateTime.Parse(txtFHFinalizacion.Text), bool.Parse(ddlTipoImporte.SelectedValue), decimal.Parse(txtImporte.Text), txtConcepto.Text.Trim().ToUpper(), bool.Parse(ddlPedirDatos.SelectedValue), URL, Guid.Parse(ViewState["UidClienteLocal"].ToString())))
                        {
                            foreach (ListItem promo in ListBoxMultipleMod.Items)
                            {
                                if (promo.Selected)
                                {
                                    eventosServices.RegistrarPromocionesEvento(UidEvento, Guid.Parse(promo.Value));
                                }
                            }

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            eventosServices.CargarEventos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                            gvEventos.DataSource = eventosServices.lsEventosGridViewModel;
                            gvEventos.DataBind();

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
                        if (eventosServices.ActualizarEvento(Guid.Parse(ViewState["UidRequerido"].ToString()), txtNombreEvento.Text.Trim().ToUpper(), txtDescripcion.Text.Trim().ToUpper(), DateTime.Parse(txtFHInicio.Text), DateTime.Parse(txtFHFinalizacion.Text), bool.Parse(ddlTipoImporte.SelectedValue), decimal.Parse(txtImporte.Text), txtConcepto.Text.Trim().ToUpper(), bool.Parse(ddlPedirDatos.SelectedValue), Guid.Parse(ddlEstatus.SelectedValue)))
                        {
                            eventosServices.EliminarPromocionesEvento(Guid.Parse(ViewState["UidRequerido"].ToString()));

                            foreach (ListItem promo in ListBoxMultipleMod.Items)
                            {
                                if (promo.Selected)
                                {
                                    eventosServices.RegistrarPromocionesEvento(Guid.Parse(ViewState["UidRequerido"].ToString()), Guid.Parse(promo.Value));
                                }
                            }

                            pnlAlert.Visible = true;
                            lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                            divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");

                            eventosServices.CargarEventos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));
                            gvEventos.DataSource = eventosServices.lsEventosGridViewModel;
                            gvEventos.DataBind();

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

        protected void gvEventos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvEventos_RowCommand(object sender, GridViewCommandEventArgs e)
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
                GridViewRow Seleccionado = gvEventos.Rows[index];
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
                GridViewRow Seleccionado = gvEventos.Rows[index];
                GridView valor = (GridView)sender;
                Guid dataKeys = Guid.Parse(valor.DataKeys[Seleccionado.RowIndex].Value.ToString());
                ViewState["UidRequerido"] = dataKeys;

                ManejoDatos(dataKeys);

                lblTituloModal.Text = "Visualización de Evento";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "showModal()", true);
            }

        }
        protected void gvEventos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        private void ManejoDatos(Guid dataKeys)
        {
            int ddlTipoImporte = 1;
            int ddlPedirDatos = 1;

            eventosServices.CargarEventos(Guid.Parse(ViewState["UidClienteLocal"].ToString()));

            eventosServices.ObtenerEvento(dataKeys);
            txtNombreEvento.Text = eventosServices.eventosRepository.eventosGridViewModel.VchNombreEvento;
            txtDescripcion.Text = eventosServices.eventosRepository.eventosGridViewModel.VchDescripcion;
            txtFHInicio.Text = eventosServices.eventosRepository.eventosGridViewModel.DtFHInicio.ToString("yyyy-MM-ddTHH:mm");
            txtFHFinalizacion.Text = eventosServices.eventosRepository.eventosGridViewModel.DtFHFin.ToString("yyyy-MM-ddTHH:mm");

            if (eventosServices.eventosRepository.eventosGridViewModel.BitTipoImporte)
            {
                ddlTipoImporte = 0;
            }
            this.ddlTipoImporte.SelectedIndex = ddlTipoImporte;
            txtImporte.Text = eventosServices.eventosRepository.eventosGridViewModel.DcmImporte.ToString("N2");
            txtConcepto.Text = eventosServices.eventosRepository.eventosGridViewModel.VchConcepto;

            promocionesServices.ObtenerPromocionesEvento(dataKeys);

            if (promocionesServices.lsEventosPromocionesModel.Count >= 1)
            {
                foreach (ListItem item in ListBoxMultipleMod.Items)
                {
                    foreach (var it in promocionesServices.lsEventosPromocionesModel)
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
                foreach (ListItem item in ListBoxMultipleMod.Items)
                {
                    item.Selected = false;
                }
            }

            if (eventosServices.eventosRepository.eventosGridViewModel.BitDatosUsuario)
            {
                ddlPedirDatos = 0;
            }
            this.ddlPedirDatos.SelectedIndex = ddlPedirDatos;
            ddlEstatus.SelectedIndex = ddlEstatus.Items.IndexOf(ddlEstatus.Items.FindByValue(eventosServices.eventosRepository.eventosGridViewModel.UidEstatus.ToString()));
        }

        private void BloquearCampos()
        {
            txtNombreEvento.Enabled = false;
            txtDescripcion.Enabled = false;
            txtFHInicio.Enabled = false;
            txtFHFinalizacion.Enabled = false;
            ddlTipoImporte.Enabled = false;
            txtImporte.Enabled = false;
            txtConcepto.Enabled = false;
            ddlPedirDatos.Enabled = false;
            ddlEstatus.Enabled = false;
        }
        private void DesbloquearCampos()
        {
            txtNombreEvento.Enabled = true;
            txtDescripcion.Enabled = true;
            txtFHInicio.Enabled = true;
            txtFHFinalizacion.Enabled = true;
            ddlTipoImporte.Enabled = true;
            txtImporte.Enabled = true;
            txtConcepto.Enabled = true;
            ddlPedirDatos.Enabled = true;

            if (ViewState["Accion"].ToString() == "Guardar")
            {
                ddlEstatus.Enabled = false;
            }
            else
            {
                ddlEstatus.Enabled = true;
            }
        }
        private void LimpiarCampos()
        {
            txtNombreEvento.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtFHInicio.Text = string.Empty;
            txtFHFinalizacion.Text = string.Empty;
            ddlTipoImporte.SelectedIndex = 0;
            txtImporte.Text = "50";
            txtConcepto.Text = string.Empty;
            ddlPedirDatos.SelectedIndex = 0;
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FormScript", "hideModal()", true);
        }
    }
}