using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models.IntegracionesPraga;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.IntegracionesPraga
{
    public class PagosIntegracionRepository : SqlDataRepository
    {
        PagosIntegracion _pagosIntegracion = new PagosIntegracion();
        public PagosIntegracion pagosIntegracion
        {
            get { return _pagosIntegracion; }
            set { _pagosIntegracion = value; }
        }

        public bool RegistrarPagosIntegracion(Guid UidPagoIntegracion, int IdEscuela, decimal DcmImporte, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid UidFormaPago, Guid UidEstatusFechaPago, Guid UidTipoPagoIntegracion)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosIntegracionRegistrar";

                comando.Parameters.Add("@UidPagoIntegracion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoIntegracion"].Value = UidPagoIntegracion;

                comando.Parameters.Add("@IdEscuela", SqlDbType.BigInt);
                comando.Parameters["@IdEscuela"].Value = IdEscuela;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = DcmImporte;

                comando.Parameters.Add("@DcmImportePagado", SqlDbType.Decimal);
                comando.Parameters["@DcmImportePagado"].Value = DcmImportePagado;

                comando.Parameters.Add("@DcmImporteNuevo", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteNuevo"].Value = DcmImporteNuevo;

                comando.Parameters.Add("@UidFormaPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFormaPago"].Value = UidFormaPago;

                comando.Parameters.Add("@UidEstatusFechaPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatusFechaPago"].Value = UidEstatusFechaPago;

                comando.Parameters.Add("@UidTipoPagoIntegracion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoPagoIntegracion"].Value = UidTipoPagoIntegracion;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        #region Metodos Integraciones

        #region Praga        
        public List<PagosIntegracion> ObtenerPagoIntegracion(string IdReferencia)
        {
            List<PagosIntegracion> lsPagosIntegracion = new List<PagosIntegracion>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from LigasUrlsPragaIntegracion lupi, PagosIntegracion pai where lupi.UidPagoIntegracion = pai.UidPagoIntegracion and lupi.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosIntegracion.Add(new PagosIntegracion
                {
                    UidPagoIntegracion = Guid.Parse(item["UidPagoIntegracion"].ToString()),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                    DcmImporteNuevo = decimal.Parse(item["DcmImporteNuevo"].ToString()),
                    UidEstatusFechaPago = Guid.Parse(item["UidEstatusFechaPago"].ToString())
                });
            }

            return lsPagosIntegracion;
        }
        public bool ActualizarPagoIntegracion(Guid UidPagoIntegracion, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid UidEstatusFechaPago)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosIntegracionActualizar";

                comando.Parameters.Add("@UidPagoIntegracion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoIntegracion"].Value = UidPagoIntegracion;

                comando.Parameters.Add("@DcmImportePagado", SqlDbType.Decimal);
                comando.Parameters["@DcmImportePagado"].Value = DcmImportePagado;

                comando.Parameters.Add("@DcmImporteNuevo", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteNuevo"].Value = DcmImporteNuevo;

                comando.Parameters.Add("@UidEstatusFechaPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatusFechaPago"].Value = UidEstatusFechaPago;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion

        #region ClubPago
        public List<PagosIntegracion> ObtenerPagoClubPagoIntegracion(string IdReferencia)
        {
            List<PagosIntegracion> lsPagosIntegracion = new List<PagosIntegracion>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from RefClubPago rcp, PagosIntegracion pai where rcp.UidPagoIntegracion = pai.UidPagoIntegracion and rcp.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosIntegracion.Add(new PagosIntegracion
                {
                    UidPagoIntegracion = Guid.Parse(item["UidPagoIntegracion"].ToString()),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    DcmImportePagado = decimal.Parse(item["DcmImportePagado"].ToString()),
                    DcmImporteNuevo = decimal.Parse(item["DcmImporteNuevo"].ToString()),
                    UidEstatusFechaPago = Guid.Parse(item["UidEstatusFechaPago"].ToString())
                });
            }

            return lsPagosIntegracion;
        }
        public bool ActualizarPagoClubPagoIntegracion(Guid UidPagoIntegracion, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid UidEstatusFechaPago)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosIntegracionActualizar";

                comando.Parameters.Add("@UidPagoIntegracion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoIntegracion"].Value = UidPagoIntegracion;

                comando.Parameters.Add("@DcmImportePagado", SqlDbType.Decimal);
                comando.Parameters["@DcmImportePagado"].Value = DcmImportePagado;

                comando.Parameters.Add("@DcmImporteNuevo", SqlDbType.Decimal);
                comando.Parameters["@DcmImporteNuevo"].Value = DcmImporteNuevo;

                comando.Parameters.Add("@UidEstatusFechaPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatusFechaPago"].Value = UidEstatusFechaPago;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion

        #region Metodos web
        #region ReportGenerated
        public List<ReporteGeneradosGridViewModel> CargarReporteGenerado(Guid UidCredencial)
        {
            List<ReporteGeneradosGridViewModel> lsReporteGeneradosGridViewModel = new List<ReporteGeneradosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pain.UidPagoIntegracion, rcp.DtRegistro, rcp.IdReferencia, rcp.DcmImporte, rcp.DtVencimiento, fp.VchDescripcion as FormaPago from PagosIntegracion pain, RefClubPago rcp, TiposPagosIntegracion tpi, FormasPagos fp, Integraciones inte, CredenSandbox cs where tpi.UidTipoPagoIntegracion = '3F792D20-B3B6-41D3-AF88-1BCB20D99BBE' and pain.UidPagoIntegracion = rcp.UidPagoIntegracion and tpi.UidTipoPagoIntegracion = pain.UidTipoPagoIntegracion and fp.UidFormaPago = pain.UidFormaPago and inte.IdIntegracion = rcp.IdIntegracion and cs.UidIntegracion = inte.UidIntegracion and cs.UidCredencial = '" + UidCredencial + "' union select pain.UidPagoIntegracion, lupi.DtRegistro, lupi.IdReferencia, lupi.DcmImporte, lupi.DtVencimiento, fp.VchDescripcion as FormaPago from PagosIntegracion pain, LigasUrlsPragaIntegracion lupi, TiposPagosIntegracion tpi, FormasPagos fp, Integraciones inte, CredenSandbox cs where tpi.UidTipoPagoIntegracion = '3F792D20-B3B6-41D3-AF88-1BCB20D99BBE' and pain.UidPagoIntegracion = lupi.UidPagoIntegracion and tpi.UidTipoPagoIntegracion = pain.UidTipoPagoIntegracion and fp.UidFormaPago = pain.UidFormaPago and inte.IdIntegracion = lupi.IdIntegracion and cs.UidIntegracion = inte.UidIntegracion and cs.UidCredencial = '" + UidCredencial + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsReporteGeneradosGridViewModel.Add(new ReporteGeneradosGridViewModel
                {
                    UidPagoIntegracion = Guid.Parse(item["UidPagoIntegracion"].ToString()),
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    IdReferencia = item["IdReferencia"].ToString(),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    DtVencimiento = DateTime.Parse(item["DtVencimiento"].ToString()),
                    VchFormaPago = item["FormaPago"].ToString()
                });
            }

            return lsReporteGeneradosGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
        }
        public List<ReporteGeneradosGridViewModel> BuscarReporteGenerado(string FHDesde, string FHHasta, string IdReferencia, Guid UidCredencial)
        {
            List<ReporteGeneradosGridViewModel> lsReporteGeneradosGridViewModel = new List<ReporteGeneradosGridViewModel>();

            SqlCommand comando = new SqlCommand();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "sp_ReporteGeneradoBuscar";
            try
            {
                if (FHDesde != string.Empty)
                {
                    comando.Parameters.Add("@FHDesde", SqlDbType.DateTime);
                    comando.Parameters["@FHDesde"].Value = FHDesde;
                }
                if (FHHasta != string.Empty)
                {
                    comando.Parameters.Add("@FHHasta", SqlDbType.Date);
                    comando.Parameters["@FHHasta"].Value = FHHasta;
                }
                if (IdReferencia != string.Empty)
                {
                    comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                    comando.Parameters["@IdReferencia"].Value = IdReferencia;
                }

                comando.Parameters.Add("@UidCredencial", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCredencial"].Value = UidCredencial;

                foreach (DataRow item in this.Busquedas(comando).Rows)
                {
                    lsReporteGeneradosGridViewModel.Add(new ReporteGeneradosGridViewModel
                    {
                        UidPagoIntegracion = Guid.Parse(item["UidPagoIntegracion"].ToString()),
                        DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                        IdReferencia = item["IdReferencia"].ToString(),
                        DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                        DtVencimiento = DateTime.Parse(item["DtVencimiento"].ToString()),
                        VchFormaPago = item["FormaPago"].ToString()
                    });
                }

                return lsReporteGeneradosGridViewModel.OrderByDescending(x => x.DtRegistro).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion

        #endregion
    }
}
