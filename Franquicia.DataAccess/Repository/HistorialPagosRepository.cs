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
    public class HistorialPagosRepository : SqlDataRepository
    {
        private HistorialPagos _historialPagos = new HistorialPagos();
        public HistorialPagos historialPagos
        {
            get { return _historialPagos; }
            set { _historialPagos = value; }
        }

        private HistorialPagosGridViewModel _historialPagosGridViewModel = new HistorialPagosGridViewModel();
        public HistorialPagosGridViewModel historialPagosGridViewModel
        {
            get { return _historialPagosGridViewModel; }
            set { _historialPagosGridViewModel = value; }
        }

        #region MetodosFranquicias

        #endregion

        #region MetodosClientes

        public List<HistorialPagosGridViewModel> CargarMovimientos(Guid UidCliente)
        {
            List<HistorialPagosGridViewModel> lsHistorialPagosGridViewModel = new List<HistorialPagosGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select lu.DtRegistro, lu.VchIdentificador, hp.* from LigasUrls lu left join HistorialPagos hp on hp.IdReferencia = lu.IdReferencia left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where lu.IdReferencia = hp.IdReferencia and ap.IdReferencia is null and lu.UidPropietario = '" + UidCliente + "'";
            query.CommandText = "select lu.DtRegistro, lu.VchIdentificador, hp.* from LigasUrls lu left join HistorialPagos hp on hp.IdReferencia = lu.IdReferencia left join AuxiliarPago ap on ap.IdReferencia = lu.IdReferencia where lu.IdReferencia = hp.IdReferencia and ap.IdReferencia is null and lu.UidPropietario = '" + UidCliente + "' UNION select t.DtRegistro, t.VchDescripcion as VchIdentificador, hp.* from Tickets t, HistorialPagos hp where t.UidHistorialPago = hp.UidHistorialPago and t.UidPropietario = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                decimal abono = 0;
                decimal cargo = 0;

                if (item["UidEstatusPago"].ToString().ToUpper() == "7AC8EFFC-8CD8-4643-A4D8-0088AC3072BD")
                {
                    cargo = decimal.Parse(item["DcmOperacion"].ToString());
                }
                else if (item["UidEstatusPago"].ToString().ToUpper() == "13178D2B-9704-4C15-9ED6-22029701E40A")
                {
                    abono = decimal.Parse(item["DcmOperacion"].ToString());
                }


                lsHistorialPagosGridViewModel.Add(new HistorialPagosGridViewModel()
                {
                    DtRegistro = DateTime.Parse(item["DtRegistro"].ToString()),
                    VchIdentificador = item["VchIdentificador"].ToString(),
                    UidHistorialPago = new Guid(item["UidHistorialPago"].ToString()),
                    DcmSaldo = abono,
                    DcmOperacion = cargo,
                    DcmNuevoSaldo = decimal.Parse(item["DcmNuevoSaldo"].ToString()),
                    IdReferencia = item["IdReferencia"].ToString(),
                    UidEstatusPago = Guid.Parse(item["UidEstatusPago"].ToString())
                });
            }

            return lsHistorialPagosGridViewModel.OrderByDescending(x=>x.DtRegistro).ToList();
        }
        public void ObtenerHistorialPago(Guid UidCliente)
        {
            historialPagos = new HistorialPagos();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select cc.* from Usuarios us, ClienteCuenta cc, AuxiliarPago ap where ap.IdReferencia IS NULL and us.UidUsuario = cc.UidUsuario and cc.UidClienteCuenta = ap.UidClienteCuenta and cc.UidUsuario = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                historialPagos.UidHistorialPago = Guid.Parse(item["UidHistorialPago"].ToString());
                historialPagos.DcmSaldo = decimal.Parse(item["DcmSaldo"].ToString());
                historialPagos.DcmOperacion = decimal.Parse(item["DcmOperacion"].ToString());
                historialPagos.DcmNuevoSaldo = decimal.Parse(item["DcmNuevoSaldo"].ToString());
                historialPagos.IdReferencia = item["IdReferencia"].ToString();
            }
        }
        public bool RegistrarHistorialPago(HistorialPagos historialPagos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_HistorialPagosRegistrar";
                
                comando.Parameters.Add("@DcmSaldo", SqlDbType.Decimal);
                comando.Parameters["@DcmSaldo"].Value = historialPagos.DcmSaldo;
                
                comando.Parameters.Add("@DcmOperacion", SqlDbType.Decimal);
                comando.Parameters["@DcmOperacion"].Value = historialPagos.DcmOperacion;
                
                comando.Parameters.Add("@DcmNuevoSaldo", SqlDbType.Decimal);
                comando.Parameters["@DcmNuevoSaldo"].Value = historialPagos.DcmNuevoSaldo;

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = historialPagos.IdReferencia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool ActualizarParametrosEntradaCliente(ParametrosEntrada parametrosEntrada)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosEntradaActualizarCliente";

                comando.Parameters.Add("@IdCompany", SqlDbType.VarChar, 50);
                comando.Parameters["@IdCompany"].Value = parametrosEntrada.IdCompany;

                comando.Parameters.Add("@IdBranch", SqlDbType.VarChar, 50);
                comando.Parameters["@IdBranch"].Value = parametrosEntrada.IdBranch;

                comando.Parameters.Add("@VchModena", SqlDbType.VarChar, 50);
                comando.Parameters["@VchModena"].Value = parametrosEntrada.VchModena;

                comando.Parameters.Add("@VchUsuario", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUsuario"].Value = parametrosEntrada.VchUsuario;

                comando.Parameters.Add("@VchPassword", SqlDbType.VarChar, 50);
                comando.Parameters["@VchPassword"].Value = parametrosEntrada.VchPassword;

                comando.Parameters.Add("@VchCanal", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCanal"].Value = parametrosEntrada.VchCanal;

                comando.Parameters.Add("@VchData0", SqlDbType.VarChar, 50);
                comando.Parameters["@VchData0"].Value = parametrosEntrada.VchData0;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUrl"].Value = parametrosEntrada.VchUrl;

                comando.Parameters.Add("@VchSemillaAES", SqlDbType.VarChar, 50);
                comando.Parameters["@VchSemillaAES"].Value = parametrosEntrada.VchSemillaAES;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = parametrosEntrada.UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool EliminarHistorialPagoLigas(string IdReferencia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_HistorialPagosLigasEliminar";

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar, 100);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion
    }
}
