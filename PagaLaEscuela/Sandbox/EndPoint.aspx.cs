using Franquicia.Bussiness;
using Franquicia.Bussiness.IntegracionesClubPago;
using Franquicia.Bussiness.IntegracionesPraga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Sandbox
{
    public partial class EndPoint : System.Web.UI.Page
    {
        EndPointClubPagoServices endPointClubPagoServices = new EndPointClubPagoServices();
        EndPointPragaServices endPointPragaServices = new EndPointPragaServices();
        ValidacionesServices validacionesServices = new ValidacionesServices();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UidIntegracionMaster"] != null)
            {
                ViewState["UidIntegracionLocal"] = Session["UidIntegracionMaster"];
            }
            else
            {
                ViewState["UidIntegracionLocal"] = Guid.Empty;
            }

            if (Session["UidCredencialMaster"] != null)
            {
                ViewState["UidCredencialLocal"] = Session["UidCredencialMaster"];
            }
            else
            {
                ViewState["UidCredencialLocal"] = Guid.Empty;
            }

            if (!IsPostBack)
            {
                endPointClubPagoServices.ObtenerEndPointClubPagoSandboxWeb(Guid.Parse(ViewState["UidIntegracionLocal"].ToString()), Guid.Parse("DA801181-6975-41C1-A615-CF9F50580369"));
                foreach (var item in endPointClubPagoServices.lsEndPointClubPago)
                {
                    if (Guid.Parse("5E6CCA56-B547-4F18-ABDD-C50D394923B3") == item.UidTipoEndPoint)
                    {
                        txtEntregarReferencia.Text = item.VchEndPoint;
                        UidEntregarReferencia.Text = item.UidEndPoint.ToString();
                    }
                    if (Guid.Parse("609302B6-0DEE-4680-B6BF-D547FD09ED12") == item.UidTipoEndPoint)
                    {
                        txtConsultarReferencia.Text = item.VchEndPoint;
                        UidConsultarReferencia.Text = item.UidEndPoint.ToString();
                    }
                    if (Guid.Parse("65341240-E22B-49B4-88B5-792B80E13F97") == item.UidTipoEndPoint)
                    {
                        txtPagarReferencia.Text = item.VchEndPoint;
                        UidPagarReferencia.Text = item.UidEndPoint.ToString();
                    }
                    if (Guid.Parse("D8903F55-B478-452D-ACC9-A7F0524C687B") == item.UidTipoEndPoint)
                    {
                        txtCancelarReferencia.Text = item.VchEndPoint;
                        UidCancelarReferencia.Text = item.UidEndPoint.ToString();
                    }
                }

                if (endPointClubPagoServices.lsEndPointClubPago.Count >= 1)
                {
                    ViewState["AccionEndPointClubPago"] = "ActualizarEndPointClubPago";
                    btnGuardarClubPago.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                }
                else
                {
                    ViewState["AccionEndPointClubPago"] = "GuardarEndPointClubPago";
                    btnGuardarClubPago.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                }

                endPointPragaServices.ObtenerEndPointPragaSandboxWeb(Guid.Parse(ViewState["UidIntegracionLocal"].ToString()), Guid.Parse("DA801181-6975-41C1-A615-CF9F50580369"));
                foreach (var itemPraga in endPointPragaServices.lsEndPointPraga)
                {
                    if (Guid.Parse("1F3A8ECB-806A-4970-958C-5360E2BB1009") == itemPraga.UidTipoEndPoint)
                    {
                        txtEntregarLiga.Text = itemPraga.VchEndPoint;
                        UidEntregarLiga.Text = itemPraga.UidEndPoint.ToString();
                    }
                    if (Guid.Parse("2C1854EA-00BC-474E-BF3E-F8395819DE53") == itemPraga.UidTipoEndPoint)
                    {
                        txtPagarLiga.Text = itemPraga.VchEndPoint;
                        UidPagarLiga.Text = itemPraga.UidEndPoint.ToString();
                    }
                }

                if (endPointPragaServices.lsEndPointPraga.Count >= 1)
                {
                    ViewState["AccionEndPointPraga"] = "ActualizarEndPointPraga";
                    btnGuardarPraga.Text = "<i class=" + "material-icons>" + "refresh </i> Actualizar";
                }
                else
                {
                    ViewState["AccionEndPointPraga"] = "GuardarEndPointPraga";
                    btnGuardarPraga.Text = "<i class=" + "material-icons>" + "check </i> Guardar";
                }

                ValidarMenu();
            }
            else
            {
                pnlAlert.Visible = false;
                lblMensajeAlert.Text = "";
                divAlert.Attributes.Add("class", "alert alert-danger alert-dismissible fade");
            }
        }

        protected void btnGuardarPraga_Click(object sender, EventArgs e)
        {
            switch (ViewState["AccionEndPointPraga"].ToString())
            {
                case "ActualizarEndPointPraga":

                    endPointPragaServices.ActualizarEndPointPraga(Guid.Parse(UidEntregarLiga.Text), txtEntregarLiga.Text.Trim());
                    endPointPragaServices.ActualizarEndPointPraga(Guid.Parse(UidPagarLiga.Text), txtPagarLiga.Text.Trim());

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                    break;

                case "GuardarEndPointPraga":
                    endPointPragaServices.RegistrarEndPointPraga(txtEntregarLiga.Text.Trim(), Guid.Parse("1F3A8ECB-806A-4970-958C-5360E2BB1009"), Guid.Parse(ViewState["UidCredencial"].ToString()));
                    endPointPragaServices.RegistrarEndPointPraga(txtPagarLiga.Text.Trim(), Guid.Parse("2C1854EA-00BC-474E-BF3E-F8395819DE53"), Guid.Parse(ViewState["UidCredencial"].ToString()));

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                    break;
            }
        }

        protected void btnGuardarClubPago_Click(object sender, EventArgs e)
        {
            switch (ViewState["AccionEndPointClubPago"].ToString())
            {
                case "ActualizarEndPointClubPago":

                    endPointClubPagoServices.ActualizarEndPointClubPago(Guid.Parse(UidEntregarReferencia.Text), txtEntregarReferencia.Text.Trim());
                    endPointClubPagoServices.ActualizarEndPointClubPago(Guid.Parse(UidConsultarReferencia.Text), txtConsultarReferencia.Text.Trim());
                    endPointClubPagoServices.ActualizarEndPointClubPago(Guid.Parse(UidPagarReferencia.Text), txtPagarReferencia.Text.Trim());
                    endPointClubPagoServices.ActualizarEndPointClubPago(Guid.Parse(UidCancelarReferencia.Text), txtCancelarReferencia.Text.Trim());

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha actualizado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                    break;

                case "GuardarEndPointClubPago":
                    endPointClubPagoServices.RegistrarEndPointClubPago(txtEntregarReferencia.Text.Trim(), Guid.Parse("5E6CCA56-B547-4F18-ABDD-C50D394923B3"), Guid.Parse(ViewState["UidCredencial"].ToString()));
                    endPointClubPagoServices.RegistrarEndPointClubPago(txtConsultarReferencia.Text.Trim(), Guid.Parse("609302B6-0DEE-4680-B6BF-D547FD09ED12"), Guid.Parse(ViewState["UidCredencial"].ToString()));
                    endPointClubPagoServices.RegistrarEndPointClubPago(txtPagarReferencia.Text.Trim(), Guid.Parse("65341240-E22B-49B4-88B5-792B80E13F97"), Guid.Parse(ViewState["UidCredencial"].ToString()));
                    endPointClubPagoServices.RegistrarEndPointClubPago(txtCancelarReferencia.Text.Trim(), Guid.Parse("D8903F55-B478-452D-ACC9-A7F0524C687B"), Guid.Parse(ViewState["UidCredencial"].ToString()));

                    pnlAlert.Visible = true;
                    lblMensajeAlert.Text = "<b>¡Felicidades! </b> se ha registrado exitosamente.";
                    divAlert.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                    break;
            }
        }

        #region Menu
        private void ValidarMenu()
        {
            //Iniciamos oculto todas las opciones de menus
            liActivarComercios.Visible = false;
            pnlActivarComercios.Visible = false;

            liActivarPagosEnlinea.Visible = false;
            pnlActivarPagosEnlinea.Visible = false;

            //Validamos las opciones de menu a mostrar

            if (validacionesServices.ValidarPermisoMenu(Guid.Parse("D0E8AE95-4EDC-4A8A-A412-87B63B678FB3"), Guid.Parse(ViewState["UidIntegracionLocal"].ToString())))
            {
                liActivarComercios.Visible = true;
            }

            if (validacionesServices.ValidarPermisoMenu(Guid.Parse("1981690D-B9FA-43BA-9B97-BF624EBEEC2E"), Guid.Parse(ViewState["UidIntegracionLocal"].ToString())))
            {
                liActivarPagosEnlinea.Visible = true;
            }


            //Iniciamos activo la primera opcion del menu
            if (liActivarComercios.Visible == true)
            {
                btnActivarComercios.CssClass = "nav-link active show";
                pnlActivarComercios.Visible = true;
            }
            else if (liActivarPagosEnlinea.Visible == true)
            {
                btnActivarPagosEnlinea.CssClass = "nav-link active show";
                pnlActivarPagosEnlinea.Visible = true;
            }
        }
        protected void btnActivarComercios_Click(object sender, EventArgs e)
        {
            btnActivarComercios.CssClass = "nav-link active show";
            pnlActivarComercios.Visible = true;

            btnActivarPagosEnlinea.CssClass = "nav-link";
            pnlActivarPagosEnlinea.Visible = false;
        }
        protected void btnActivarPagosEnlinea_Click(object sender, EventArgs e)
        {
            btnActivarComercios.CssClass = "nav-link";
            pnlActivarComercios.Visible = false;

            btnActivarPagosEnlinea.CssClass = "nav-link active show";
            pnlActivarPagosEnlinea.Visible = true;
        }
        #endregion
    }
}