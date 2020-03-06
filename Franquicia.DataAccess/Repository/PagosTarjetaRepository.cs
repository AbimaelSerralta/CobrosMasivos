using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class PagosTarjetaRepository : SqlDataRepository
    {
        private PagosTarjeta _pagosTarjeta = new PagosTarjeta();
        public PagosTarjeta pagosTarjeta
        {
            get { return _pagosTarjeta; }
            set { _pagosTarjeta = value; }
        }

        private PagosTarjetaDetalleGridViewModel _pagosTarjetaDetalleGridViewModel = new PagosTarjetaDetalleGridViewModel();

        public PagosTarjetaDetalleGridViewModel pagosTarjetaDetalleGridViewModel
        {
            get { return _pagosTarjetaDetalleGridViewModel; }
            set { _pagosTarjetaDetalleGridViewModel = value; }
        }


        public List<PagosTarjeta> ObtenerEstatusLiga(string Liga)
        {
            List<PagosTarjeta> lspagosTarjetas = new List<PagosTarjeta>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from PagosTarjeta pt, LigasUrls lu where pt.IdReferencia = lu.IdReferencia and lu.VchUrl = '" + Liga + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                pagosTarjeta = new PagosTarjeta()
                {
                    UidPagoTarjeta = new Guid(item["UidPagoTarjeta"].ToString()),
                    IdReferencia = item["IdReferencia"].ToString(),
                    VchEstatus = item["VchEstatus"].ToString(),
                    Autorizacion = item["Autorizacion"].ToString()
                };

                lspagosTarjetas.Add(pagosTarjeta);
            }

            return lspagosTarjetas;
        }

        public List<PagosTarjetaDetalleGridViewModel> DetalleLiga(string IdReferencia)
        {
            List<PagosTarjetaDetalleGridViewModel> lsPagosTarjetaDetalleGridViewModel = new List<PagosTarjetaDetalleGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pt.* from PagosTarjeta pt, LigasUrls lu where pt.IdReferencia = lu.IdReferencia and lu.IdReferencia = '" + IdReferencia + "' order by DtmFechaDeRegistro asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchColor = "#007bff";

                if (!string.IsNullOrEmpty(item["VchEstatus"].ToString()))
                {
                    switch (item["VchEstatus"].ToString())
                    {
                        case "approved":
                            VchColor = "#4caf50 ";
                            break;
                        case "denied":
                            VchColor = "#ff9800 ";
                            break;
                        case "error":
                            VchColor = "#f55145 ";
                            break;
                    }
                }

                pagosTarjetaDetalleGridViewModel = new PagosTarjetaDetalleGridViewModel()
                {
                    UidPagoTarjeta = new Guid(item["UidPagoTarjeta"].ToString()),
                    IdReferencia = item["IdReferencia"].ToString(),
                    DtmFechaDeRegistro = DateTime.Parse(item["DtmFechaDeRegistro"].ToString()),
                    VchEstatus = item["VchEstatus"].ToString(),
                    VchColor = VchColor
                };

                lsPagosTarjetaDetalleGridViewModel.Add(pagosTarjetaDetalleGridViewModel);
            }

            return lsPagosTarjetaDetalleGridViewModel;
        }

    }
}
