using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class DireccionesClientesServices
    {
        private DireccionesClientesRepository _direccionesClientesRepository = new DireccionesClientesRepository();
        public DireccionesClientesRepository direccionesClientesRepository
        {
            get { return _direccionesClientesRepository; }
            set { _direccionesClientesRepository = value; }
        }


        public List<DireccionesClientesRepository> lsDireccionesFranquiciatariosRepository = new List<DireccionesClientesRepository>();

        public DireccionesClientes ObtenerDireccionCliente(Guid UidCliente)
        {
            return direccionesClientesRepository.ObtenerDireccionCliente(UidCliente);
        }
    }
}
