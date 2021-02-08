using Franquicia.Bussiness;
using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views.Reports
{
    public partial class FormatoClubPago : System.Web.UI.Page
    {
        ClientesServices clientesServices = new ClientesServices();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                rvFormatoClubPago.SizeToReportContent = true;

                rvFormatoClubPago.Width = Unit.Percentage(100);
                rvFormatoClubPago.Height = Unit.Percentage(100);

                rvFormatoClubPago.LocalReport.ReportPath = Server.MapPath("~/Reports/FormatoClubPago.rdlc");

                ReportParameter[] reports = new ReportParameter[6];

                reports[0] = new ReportParameter("Concepto", Session["rdlcConcepto"].ToString());
                reports[1] = new ReportParameter("Escuela", Session["rdlcEscuela"].ToString());
                reports[2] = new ReportParameter("FechaEmision", Session["rdlcFechaEmision"].ToString());
                reports[3] = new ReportParameter("FechaVencimiento", Session["rdlcFechaVencimiento"].ToString());
                reports[4] = new ReportParameter("ImportePago", "1500.00");
                reports[5] = new ReportParameter("Referencia", Session["rdlcReferencia"].ToString());

                rvFormatoClubPago.LocalReport.SetParameters(reports);
                rvFormatoClubPago.LocalReport.DataSources.Add(new ReportDataSource("LogoEscuela", clientesServices.rdlcObtenerLogo(Guid.Parse(Session["rdlcUidPagoColegiatura"].ToString()))));
                rvFormatoClubPago.LocalReport.DataSources.Add(new ReportDataSource("CodigoBarras", clientesServices.rdlcObtenerCodigoBarraClubPago(Session["rdlcCodigoBarras"].ToString())));
                rvFormatoClubPago.LocalReport.DataSources.Add(new ReportDataSource("ComerciosDisponibles", clientesServices.rdlcObtenerComerciosDisponibles()));
                rvFormatoClubPago.LocalReport.Refresh();
            }
        }
    }
}