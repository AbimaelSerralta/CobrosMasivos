using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class PagosManualesRepository : SqlDataRepository
    {
        PagosManuales _pagosManuales = new PagosManuales();
        public PagosManuales PagosManuales
        {
            get { return _pagosManuales; }
            set { _pagosManuales = value; }
        }

        #region Metodos PagosManuales
        public bool RegistrarPagoManual(PagosManuales pagosManuales)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosManualesRegistrar";

                comando.Parameters.Add("@UidBanco", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidBanco"].Value = pagosManuales.UidBanco;

                comando.Parameters.Add("@VchCuenta", SqlDbType.VarChar);
                comando.Parameters["@VchCuenta"].Value = pagosManuales.VchCuenta;

                comando.Parameters.Add("@DtFHPago", SqlDbType.DateTime);
                comando.Parameters["@DtFHPago"].Value = pagosManuales.DtFHPago;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = pagosManuales.DcmImporte;

                comando.Parameters.Add("@VchFolio", SqlDbType.VarChar);
                comando.Parameters["@VchFolio"].Value = pagosManuales.VchFolio;

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = pagosManuales.UidPagoColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Resultado;
        }
        public bool ActualizarPagoManual(Alumnos Alumnos, TelefonosAlumnos telefonosAlumnos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_AlumnosActualizar";

                
                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarPagoManual(Guid UidPagoColegiatura)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PagosColegiaturasEliminar";

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = UidPagoColegiatura;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        #endregion

        #region Metodos Clientes
        #region Pagos

        #endregion

        #region Metodos ReporteLigasPadres
        public List<PagosManualesReporteEscuelaViewModel> ObtenerPagoManual(Guid UidPagoColegiatura)
        {
            List<PagosManualesReporteEscuelaViewModel> lsPagosManualesReporteEscuelaViewModel = new List<PagosManualesReporteEscuelaViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select fp.UidEstatusFechaPago, pm.*, ba.VchDescripcion as VchBanco, fop.VchDescripcion as VchFormaPago from FechasPagos fp, PagosColegiaturas pc, PagosManuales pm, Bancos ba, FormasPagos fop where fp.UidPagoColegiatura = pc.UidPagoColegiatura and pm.UidPagoColegiatura = pc.UidPagoColegiatura and fop.UidFormaPago = fp.UidFormaPago and ba.UidBanco = pm.UidBanco and pc.UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosManualesReporteEscuelaViewModel.Add(new PagosManualesReporteEscuelaViewModel()
                {
                    UidPagoManual = Guid.Parse(item["UidPagoManual"].ToString()),
                    VchCuenta = item["VchCuenta"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    VchFolio = item["VchFolio"].ToString(),
                    VchBanco = item["VchBanco"].ToString(),
                    UidEstatusFechaPago = Guid.Parse(item["UidEstatusFechaPago"].ToString()),
                    VchFormaPago = item["VchFormaPago"].ToString()
                });
            }

            return lsPagosManualesReporteEscuelaViewModel;
        }
        public List<PagosManualesReporteEscuelaViewModel> ConsultarDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            List<PagosManualesReporteEscuelaViewModel> lsPagosManualesReporteEscuelaViewModel = new List<PagosManualesReporteEscuelaViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select fp.UidEstatusFechaPago, pm.*, ba.VchDescripcion as VchBanco, fop.VchDescripcion as VchFormaPago from FechasPagos fp, PagosColegiaturas pc, PagosManuales pm, Bancos ba, FormasPagos fop where fp.UidPagoColegiatura = pc.UidPagoColegiatura and pm.UidPagoColegiatura = pc.UidPagoColegiatura and fop.UidFormaPago = fp.UidFormaPago and ba.UidBanco = pm.UidBanco and pc.UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosManualesReporteEscuelaViewModel.Add(new PagosManualesReporteEscuelaViewModel()
                {
                    UidPagoManual = Guid.Parse(item["UidPagoManual"].ToString()),
                    VchCuenta = item["VchCuenta"].ToString(),
                    DtFHPago = DateTime.Parse(item["DtFHPago"].ToString()),
                    DcmImporte = decimal.Parse(item["DcmImporte"].ToString()),
                    VchFolio = item["VchFolio"].ToString(),
                    VchBanco = item["VchBanco"].ToString(),
                    UidEstatusFechaPago = Guid.Parse(item["UidEstatusFechaPago"].ToString()),
                    VchFormaPago = item["VchFormaPago"].ToString()
                });
            }

            return lsPagosManualesReporteEscuelaViewModel;
        }
        #endregion
        #endregion

        #region Metodos Colegiaturas

        #endregion

    }
}
