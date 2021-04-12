using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models.IntegracionesPraga;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.IntegracionesPraga
{
    public class EndPointPragaRepository : SqlDataRepository
    {
        private EndPointPraga _endPointPraga = new EndPointPraga();
        public EndPointPraga endPointPraga
        {
            get { return _endPointPraga; }
            set { _endPointPraga = value; }
        }

        public string ObtenerEndPointAUtilizar(string IdReferencia)
        {
            string TipoEndpoint = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pain.* from PagosIntegracion pain, LigasUrlsPragaIntegracion lupi, TiposPagosIntegracion tpi where pain.UidPagoIntegracion = lupi.UidPagoIntegracion and pain.UidTipoPagoIntegracion = tpi.UidTipoPagoIntegracion and lupi.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                TipoEndpoint = item["UidTipoPagoIntegracion"].ToString();

                if (Guid.Parse(item["UidTipoPagoIntegracion"].ToString()) == Guid.Parse("3F792D20-B3B6-41D3-AF88-1BCB20D99BBE"))
                {
                    TipoEndpoint = "SANDBOX";
                }
                else if (Guid.Parse(item["UidTipoPagoIntegracion"].ToString()) == Guid.Parse("D87454C9-12EF-4459-9CED-36E8401E4033"))
                {
                    TipoEndpoint = "PRODUCCION";
                }

            }

            return TipoEndpoint;
        }

        #region Sandbox
        public Tuple<string, bool> ObtenerEndPointPragaSandbox(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            string Url = string.Empty;
            bool Respu = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epp.* from EndPointPraga epp, CredenSandbox cs, Integraciones inte where inte.UidIntegracion = cs.UidIntegracion and epp.UidPropietario = cs.UidCredencial and epp.UidTipoEndPoint = '1F3A8ECB-806A-4970-958C-5360E2BB1009' and inte.IdIntegracion = '" + IdIntegracion + "' and cs.VchUsuario = '" + VchUsuario + "' and VchContrasenia = '" + VchContrasenia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Url = item["VchEndPoint"].ToString();
                Respu = true;
            }

            return Tuple.Create(Url, Respu);
        }
        public Tuple<string, bool> ObtenerEndPointPragaSandbox(string IdReferencia)
        {
            string Url = string.Empty;
            bool Respu = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epp.* from PagosIntegracion pain, LigasUrlsPragaIntegracion lupi, Integraciones inte, CredenSandbox cs, EndPointPraga epp where pain.UidPagoIntegracion = lupi.UidPagoIntegracion and lupi.IdIntegracion = inte.IdIntegracion and inte.UidIntegracion = cs.UidIntegracion and epp.UidPropietario = cs.UidCredencial and epp.UidTipoEndPoint = '2C1854EA-00BC-474E-BF3E-F8395819DE53' and lupi.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Url = item["VchEndPoint"].ToString();
                Respu = true;
            }

            return Tuple.Create(Url, Respu);
        }
        #endregion

        #region Produccion
        public Tuple<string, bool> ObtenerEndPointPragaProduccion(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            string Url = string.Empty;
            bool Respu = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epp.* from EndPointPraga epp, CredenProduccion cp, Integraciones inte where inte.UidIntegracion = cp.UidIntegracion and epp.UidPropietario = cp.UidCredencial and epp.UidTipoEndPoint = '1F3A8ECB-806A-4970-958C-5360E2BB1009' and inte.IdIntegracion = '" + IdIntegracion + "' and cp.VchUsuario = '" + VchUsuario + "' and VchContrasenia = '" + VchContrasenia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Url = item["VchEndPoint"].ToString();
                Respu = true;
            }

            return Tuple.Create(Url, Respu);
        }
        public Tuple<string, bool> ObtenerEndPointPragaProduccion(string IdReferencia)
        {
            string Url = string.Empty;
            bool Respu = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epp.* from PagosIntegracion pain, LigasUrlsPragaIntegracion lupi, Integraciones inte, CredenProduccion cp, EndPointPraga epp where pain.UidPagoIntegracion = lupi.UidPagoIntegracion and lupi.IdIntegracion = inte.IdIntegracion and inte.UidIntegracion = cp.UidIntegracion and epp.UidPropietario = cp.UidCredencial and epp.UidTipoEndPoint = '2C1854EA-00BC-474E-BF3E-F8395819DE53' and lupi.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Url = item["VchEndPoint"].ToString();
                Respu = true;
            }

            return Tuple.Create(Url, Respu);
        }
        #endregion

        #region Metodos web

        #region EndPoint
        public List<EndPointPraga> ObtenerEndPointPragaSandboxWeb(Guid UidIntegracion, Guid UidCredencial)
        {
            List<EndPointPraga> lsEndPointPraga = new List<EndPointPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epp.* from EndPointPraga epp, CredenSandbox cs, Integraciones inte where inte.UidIntegracion = cs.UidIntegracion and epp.UidPropietario = cs.UidCredencial and inte.UidIntegracion = '" + UidIntegracion + "' and cs.UidCredencial = '" + UidCredencial + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEndPointPraga.Add(new EndPointPraga
                {
                    UidEndPoint = Guid.Parse(item["UidEndPoint"].ToString()),
                    VchEndPoint = item["VchEndPoint"].ToString(),
                    UidTipoEndPoint = Guid.Parse(item["UidTipoEndPoint"].ToString()),
                    UidPropietario = Guid.Parse(item["UidPropietario"].ToString())
                });
            }

            return lsEndPointPraga;
        }
        //Estos metod no se han creado
        public bool RegistrarEndPointPraga(Guid UidEndPoint, string VchEndPoint, Guid UidTipoEndPoint, Guid UidPropietario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EndPointPragaRegistrar";
                
                comando.Parameters.Add("@UidEndPoint", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEndPoint"].Value = UidEndPoint;

                comando.Parameters.Add("@VchEndPoint", SqlDbType.VarChar);
                comando.Parameters["@VchEndPoint"].Value = VchEndPoint;

                comando.Parameters.Add("@UidTipoEndPoint", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoEndPoint"].Value = UidTipoEndPoint;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool ActualizarEndPointPraga(Guid UidEndPoint, string VchEndPoint)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EndPointPragaActualizar";

                comando.Parameters.Add("@UidEndPoint", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEndPoint"].Value = UidEndPoint;

                comando.Parameters.Add("@VchEndPoint", SqlDbType.VarChar);
                comando.Parameters["@VchEndPoint"].Value = VchEndPoint;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion
        #endregion
    }
}
