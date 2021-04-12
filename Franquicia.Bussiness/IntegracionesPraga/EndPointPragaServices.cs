using Franquicia.DataAccess.Repository.IntegracionesPraga;
using Franquicia.Domain.Models.IntegracionesPraga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.IntegracionesPraga
{
    public class EndPointPragaServices
    {
        private EndPointPragaRepository _endPointPragaRepository = new EndPointPragaRepository();
        public EndPointPragaRepository endPointPragaRepository
        {
            get { return _endPointPragaRepository; }
            set { _endPointPragaRepository = value; }
        }

        public List<EndPointPraga> lsEndPointPraga = new List<EndPointPraga>();

        public string ObtenerEndPointAUtilizar(string IdReferencia)
        {
            return endPointPragaRepository.ObtenerEndPointAUtilizar(IdReferencia);
        }

        #region Sandbox
        public Tuple<string, bool> ObtenerEndPointPragaSandbox(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            return endPointPragaRepository.ObtenerEndPointPragaSandbox(IdIntegracion, VchUsuario, VchContrasenia);
        }
        public Tuple<string, bool> ObtenerEndPointPragaSandbox(string IdReferencia)
        {
            return endPointPragaRepository.ObtenerEndPointPragaSandbox(IdReferencia);
        }
        #endregion

        #region Produccion
        public Tuple<string, bool> ObtenerEndPointPragaProduccion(int IdIntegracion, string VchUsuario, string VchContrasenia)
        {
            return endPointPragaRepository.ObtenerEndPointPragaProduccion(IdIntegracion, VchUsuario, VchContrasenia);
        }
        public Tuple<string, bool> ObtenerEndPointPragaProduccion(string IdReferencia)
        {
            return endPointPragaRepository.ObtenerEndPointPragaProduccion(IdReferencia);
        }
        #endregion

        #region Metodos web

        #region EndPoint
        public List<EndPointPraga> ObtenerEndPointPragaSandboxWeb(Guid UidIntegracion, Guid UidCredencial)
        {
            lsEndPointPraga = new List<EndPointPraga>();

            return lsEndPointPraga = endPointPragaRepository.ObtenerEndPointPragaSandboxWeb(UidIntegracion, UidCredencial);
        }
        public bool RegistrarEndPointPraga(string VchEndPoint, Guid UidTipoEndPoint, Guid UidPropietario)
        {
            bool result = false;

            Guid UidEndPoint = Guid.NewGuid();

            if (endPointPragaRepository.RegistrarEndPointPraga(UidEndPoint, VchEndPoint, UidTipoEndPoint, UidPropietario))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarEndPointPraga(Guid UidEndPoint, string VchEndPoint)
        {
            bool result = false;
            if (endPointPragaRepository.ActualizarEndPointPraga(UidEndPoint, VchEndPoint))
            {
                result = true;
            }
            return result;
        }
        #endregion 
        #endregion 
    }
}
