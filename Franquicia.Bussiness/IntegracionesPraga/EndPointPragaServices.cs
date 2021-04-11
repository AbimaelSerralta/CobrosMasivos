using Franquicia.DataAccess.Repository.IntegracionesPraga;
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

        public bool RegistrarEndPointPraga()
        {
            bool result = false;
            if (endPointPragaRepository.RegistrarEndPointPraga())
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarEndPointPraga()
        {
            bool result = false;
            if (endPointPragaRepository.ActualizarEndPointPraga())
            {
                result = true;
            }
            return result;
        }
    }
}
