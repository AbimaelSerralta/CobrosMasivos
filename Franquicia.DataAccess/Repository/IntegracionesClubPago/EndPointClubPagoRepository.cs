using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models.IntegracionesClubPago;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.IntegracionesClubPago
{
    public class EndPointClubPagoRepository : SqlDataRepository
    {
        private EndPointClubPago _endPointClubPago = new EndPointClubPago();
        public EndPointClubPago EndPointClubPago
        {
            get { return _endPointClubPago; }
            set { _endPointClubPago = value; }
        }

        public bool RegistrarEndPointClubPago(Guid UidEndPoint, string VchEndPoint, Guid UidTipoEndPoint, Guid UidPropietario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EndPointClubPagoRegistrar";

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
        public bool ActualizarEndPointClubPago(Guid UidEndPoint, string VchEndPoint)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_EndPointClubPagoActualizar";

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


        public string ObtenerEndPointAUtilizar(string IdReferencia)
        {
            string TipoEndpoint = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pain.* from PagosIntegracion pain, RefClubPago rcp, TiposPagosIntegracion tpi where pain.UidPagoIntegracion = rcp.UidPagoIntegracion and pain.UidTipoPagoIntegracion = tpi.UidTipoPagoIntegracion and rcp.IdReferencia = '" + IdReferencia + "'";

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
        public Tuple<string, bool> ObtenerEndPointClubPagoSandbox(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            string Url = string.Empty;
            bool Respu = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epcp.* from EndPointClubPago epcp, CredenSandbox cs, Integraciones inte where inte.UidIntegracion = cs.UidIntegracion and epcp.UidPropietario = cs.UidCredencial and epcp.UidTipoEndPoint = '5E6CCA56-B547-4F18-ABDD-C50D394923B3' and inte.IdIntegracion = '" + IdIntegracion + "' and cs.VchUsuario = '" + VchUsuario + "' and VchContrasenia = '" + VchContrasenia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Url = item["VchEndPoint"].ToString();
                Respu = true;
            }

            return Tuple.Create(Url, Respu);
        }
        public Tuple<string, bool> ObtenerEndPointClubPagoSandbox(string IdReferencia, Guid UidTipoEndPoint)
        {
            string Url = string.Empty;
            bool Respu = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epcp.* from PagosIntegracion pain, RefClubPago rcp, Integraciones inte, CredenSandbox cs, EndPointClubPago epcp where pain.UidPagoIntegracion = rcp.UidPagoIntegracion and rcp.IdIntegracion = inte.IdIntegracion and inte.UidIntegracion = cs.UidIntegracion and epcp.UidPropietario = cs.UidCredencial and epcp.UidTipoEndPoint = '" + UidTipoEndPoint + "' and rcp.IdReferencia = '" + IdReferencia + "'";

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
        public Tuple<string, bool> ObtenerEndPointClubPagoProduccion(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            string Url = string.Empty;
            bool Respu = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epcp.* from EndPointClubPago epcp, CredenProduccion cp, Integraciones inte where inte.UidIntegracion = cp.UidIntegracion and epcp.UidPropietario = cp.UidCredencial and epcp.UidTipoEndPoint = '5E6CCA56-B547-4F18-ABDD-C50D394923B3' and inte.IdIntegracion = '" + IdIntegracion + "' and cp.VchUsuario = '" + VchUsuario + "' and VchContrasenia = '" + VchContrasenia + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                Url = item["VchEndPoint"].ToString();
                Respu = true;
            }

            return Tuple.Create(Url, Respu);
        }
        public Tuple<string, bool> ObtenerEndPointClubPagoProduccion(string IdReferencia, Guid UidTipoEndPoint)
        {
            string Url = string.Empty;
            bool Respu = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epcp.* from PagosIntegracion pain, RefClubPago rcp, Integraciones inte, CredenProduccion cp, EndPointClubPago epcp where pain.UidPagoIntegracion = rcp.UidPagoIntegracion and rcp.IdIntegracion = inte.IdIntegracion and inte.UidIntegracion = cp.UidIntegracion and epcp.UidPropietario = cp.UidCredencial and epcp.UidTipoEndPoint = '" + UidTipoEndPoint + "' and rcp.IdReferencia = '" + IdReferencia + "'";

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
        #region Emulador de caja
        public void ObtenerEndPointClubPago(Guid UidIntegracion, Guid UidTipoEndPoint)
        {
            EndPointClubPago = new EndPointClubPago();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epcp.* from EndPointClubPago epcp, CredenSandbox cs, Integraciones inte where inte.UidIntegracion = cs.UidIntegracion and cs.UidCredencial = epcp.UidPropietario and inte.UidIntegracion = '" + UidIntegracion + "' and epcp.UidTipoEndPoint = '" + UidTipoEndPoint + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                EndPointClubPago.UidEndPoint = Guid.Parse(item["UidEndPoint"].ToString());
                EndPointClubPago.VchEndPoint = item["VchEndPoint"].ToString();
                EndPointClubPago.UidTipoEndPoint = Guid.Parse(item["UidTipoEndPoint"].ToString());
                EndPointClubPago.UidPropietario = Guid.Parse(item["UidPropietario"].ToString());
            }
        }
        #endregion

        #region EndPoint
        public List<EndPointClubPago> ObtenerEndPointClubPagoSandboxWeb(Guid UidIntegracion, Guid UidCredencial)
        {
            List<EndPointClubPago> lsEndPointClubPago = new List<EndPointClubPago>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select epcp.* from EndPointClubPago epcp, CredenSandbox cs, Integraciones inte where inte.UidIntegracion = cs.UidIntegracion and epcp.UidPropietario = cs.UidCredencial and inte.UidIntegracion = '" + UidIntegracion + "' and cs.UidCredencial = '" + UidCredencial + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEndPointClubPago.Add(new EndPointClubPago
                {
                    UidEndPoint = Guid.Parse(item["UidEndPoint"].ToString()),
                    VchEndPoint = item["VchEndPoint"].ToString(),
                    UidTipoEndPoint = Guid.Parse(item["UidTipoEndPoint"].ToString()),
                    UidPropietario = Guid.Parse(item["UidPropietario"].ToString())
                });
            }

            return lsEndPointClubPago;
        }
        #endregion
        #endregion
    }
}
