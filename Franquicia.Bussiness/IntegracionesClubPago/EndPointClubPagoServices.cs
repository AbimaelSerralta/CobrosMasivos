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
        
        public List<EndPointClubPago> lsEndPointClubPago = new List<EndPointClubPago>();

        public void ObtenerEndPointClubPago(Guid UidIntegracion, Guid UidTipoEndPoint)
        {
            EndPointClubPagoRepository.ObtenerEndPointClubPago(UidIntegracion, UidTipoEndPoint);
        }

        public bool RegistrarEndPointClubPago(string VchEndPoint, Guid UidTipoEndPoint, Guid UidPropietario)
        {
            bool result = false;

            Guid UidEndPoint = Guid.NewGuid();

            if (EndPointClubPagoRepository.RegistrarEndPointClubPago(UidEndPoint, VchEndPoint, UidTipoEndPoint, UidPropietario))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarEndPointClubPago(Guid UidEndPoint, string VchEndPoint)
        {
            bool result = false;
            if (EndPointClubPagoRepository.ActualizarEndPointClubPago(UidEndPoint, VchEndPoint))
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
        public Tuple<string, bool> ObtenerEndPointClubPagoSandbox(string IdReferencia, Guid UidTipoEndPoint)
        {
            return EndPointClubPagoRepository.ObtenerEndPointClubPagoSandbox(IdReferencia, UidTipoEndPoint);
        }
        #endregion

        #region Produccion
        public Tuple<string, bool> ObtenerEndPointClubPagoProduccion(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            return EndPointClubPagoRepository.ObtenerEndPointClubPagoProduccion(IdIntegracion, VchUsuario, VchContrasenia);
        }
        public Tuple<string, bool> ObtenerEndPointClubPagoProduccion(string IdReferencia, Guid UidTipoEndPoint)
        {
            return EndPointClubPagoRepository.ObtenerEndPointClubPagoProduccion(IdReferencia, UidTipoEndPoint);
        }
        #endregion


        #region Metodos web
        #region EndPoint
        public List<EndPointClubPago> ObtenerEndPointClubPagoSandboxWeb(Guid UidIntegracion, Guid UidCredencial)
        {
            lsEndPointClubPago = new List<EndPointClubPago>();

            return lsEndPointClubPago = EndPointClubPagoRepository.ObtenerEndPointClubPagoSandboxWeb(UidIntegracion, UidCredencial);
        }
        #endregion
        #endregion
    }
}
