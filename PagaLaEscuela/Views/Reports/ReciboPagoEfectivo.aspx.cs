﻿using Franquicia.Bussiness;
using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views.Reports
{
    public partial class ReciboPagoEfectivo : System.Web.UI.Page
    {
        PagosColegiaturasServices pagosColegiaturasServices = new PagosColegiaturasServices();
        DetallesPagosColegiaturasServices DetallesPagosColegiaturasServices = new DetallesPagosColegiaturasServices();

        protected void Page_Load(object sender, EventArgs e)
        {

            rvReciboPagoCole.SizeToReportContent = true;

            rvReciboPagoCole.Width = Unit.Percentage(100);
            rvReciboPagoCole.Height = Unit.Percentage(100);

            rvReciboPagoCole.LocalReport.ReportPath = Server.MapPath("~/Reports/ReciboPagoEfectivoColegiatura.rdlc");

            ReportParameter[] reports = new ReportParameter[3];

            reports[0] = new ReportParameter("Alumno", "Josue Abimael Serralta Robles");
            reports[1] = new ReportParameter("Matricula", "1502133");
            reports[2] = new ReportParameter("FechaPago", "27/11/2020");

            rvReciboPagoCole.LocalReport.SetParameters(reports);
            rvReciboPagoCole.LocalReport.DataSources.Add(new ReportDataSource("DetallePagoCole", DetallesPagosColegiaturasServices.rdlcObtenerDetallePagoColegiatura(Guid.Parse(Session["rdlcUidPagoColegiatura"].ToString()))));
            rvReciboPagoCole.LocalReport.Refresh();
        }
    }
}