using Franquicia.Bussiness;
using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views.Reports
{
    public partial class ReciboPagoEfectivo2 : System.Web.UI.Page
    {
        PagosColegiaturasServices pagosColegiaturasServices = new PagosColegiaturasServices();
        DetallesPagosColegiaturasServices detallesPagosColegiaturasServices = new DetallesPagosColegiaturasServices();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                rvReciboPagoCole.SizeToReportContent = true;

                rvReciboPagoCole.Width = Unit.Percentage(100);
                rvReciboPagoCole.Height = Unit.Percentage(100);

                rvReciboPagoCole.LocalReport.ReportPath = Server.MapPath("~/Reports/ReciboPagoEfectivoColegiatura.rdlc");

                ReportParameter[] reports = new ReportParameter[10];

                reports[0] = new ReportParameter("Alumno", Session["rdlcAlumno"].ToString());
                reports[1] = new ReportParameter("Matricula", Session["rdlcMatricula"].ToString());
                reports[2] = new ReportParameter("FechaPago", Session["rdlcFechaPago"].ToString());
                reports[3] = new ReportParameter("FormaPago", Session["rdlcFormaPago"].ToString());
                reports[4] = new ReportParameter("BoolTipoTarjeta", Session["rdlcBoolTipoTarjeta"].ToString());
                reports[5] = new ReportParameter("TipoTarjeta", Session["rdlcTipoTarjeta"].ToString());
                reports[6] = new ReportParameter("BoolPromocion", Session["rdlcBoolPromocion"].ToString());
                reports[7] = new ReportParameter("Promocion", Session["rdlcPromocion"].ToString());
                reports[8] = new ReportParameter("DetallePromocion", Session["rdlcDetallePromocion"].ToString());
                reports[9] = new ReportParameter("DetaPromoImporte", Session["rdlcDetaPromoImporte"].ToString());

                rvReciboPagoCole.LocalReport.SetParameters(reports);
                rvReciboPagoCole.LocalReport.DataSources.Add(new ReportDataSource("DetallePagoCole", detallesPagosColegiaturasServices.rdlcObtenerDetallePagoColegiatura(Guid.Parse(Session["rdlcUidPagoColegiatura"].ToString()))));
                rvReciboPagoCole.LocalReport.DataSources.Add(new ReportDataSource("PagoCole", pagosColegiaturasServices.rdlcObtenerPagoColegiatura(Guid.Parse(Session["rdlcUidPagoColegiatura"].ToString()))));
                rvReciboPagoCole.LocalReport.Refresh();
            }
        }
    }
}