using Franquicia.DataAccess.Repository.IntegracionesClubPago;
using Franquicia.Domain.Models.IntegracionesClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.IntegracionesClubPago
{
    public class EndPointClubPagoServices
    {
        private EndPointClubPagoRepository _endPointClubPagoRepository = new EndPointClubPagoRepository();
        public EndPointClubPagoRepository EndPointClubPagoRepository
        {
            get { return _endPointClubPagoRepository; }
            set { _endPointClubPagoRepository = value; }
        }
        
        public void ObtenerEndPointClubPago(Guid UidIntegracion, Guid UidTipoEndPoint)
        {
            EndPointClubPagoRepository.ObtenerEndPointClubPago(UidIntegracion, UidTipoEndPoint);
        }

        public bool RegistrarEndPointClubPago()
        {
            bool result = false;
            if (EndPointClubPagoRepository.RegistrarEndPointClubPago())
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarEndPointClubPago()
        {
            bool result = false;
            if (EndPointClubPagoRepository.ActualizarEndPointClubPago())
            {
                result = true;
            }
            return result;
        }


        public string ObtenerEndPointAUtilizar(string IdReferencia)
        {
            return EndPointClubPagoRepository.ObtenerEndPointAUtilizar(IdReferencia);
        }

        #region Sandbox
        public Tuple<string, bool> ObtenerEndPointClubPagoSandbox(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            return EndPointClubPagoRepository.ObtenerEndPointClubPagoSandbox(IdIntegracion, VchUsuario, VchContrasenia);
        }
        public Tuple<string, bool> ObtenerEndPointClubPagoSandbox(string IdReferencia)
        {
            return EndPointClubPagoRepository.ObtenerEndPointClubPagoSandbox(IdReferencia);
        }
        #endregion

        #region Produccion
        public Tuple<string, bool> ObtenerEndPointClubPagoProduccion(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            return EndPointClubPagoRepository.ObtenerEndPointClubPagoProduccion(IdIntegracion, VchUsuario, VchContrasenia);
        }
        public Tuple<string, bool> ObtenerEndPointClubPagoProduccion(string IdReferencia)
        {
            return EndPointClubPagoRepository.ObtenerEndPointClubPagoProduccion(IdReferencia);
        }
        #endregion


        #region Metodos web
        #region EndPoint
        public List<EndPointClubPago> ObtenerEndPointClubPagoSandboxWeb(int IdIntegracion, Guid UidCredencial)
        {
            return EndPointClubPagoRepository.ObtenerEndPointClubPagoSandboxWeb(IdIntegracion, UidCredencial);
        }
        public List<EndPointClubPago> ObtenerEndPointPragaSandboxWeb(int IdIntegracion, Guid UidCredencial)
        {
            return EndPointClubPagoRepository.ObtenerEndPointPragaSandboxWeb(IdIntegracion, UidCredencial);
        }
        #endregion
        #endregion
    }
}
