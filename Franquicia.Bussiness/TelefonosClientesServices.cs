using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TelefonosClientesServices
    {
        private TelefonosClientesRepository _telefonosClientesRepository = new TelefonosClientesRepository();
        public TelefonosClientesRepository telefonosClientesRepository
        {
            get { return _telefonosClientesRepository; }
            set { _telefonosClientesRepository = value; }
        }


        public List<TelefonosClientes> lsTelefonosClientes = new List<TelefonosClientes>();

        public TelefonosClientes ObtenerTelefonoCliente(Guid UidUsuario)
        {
            return telefonosClientesRepository.ObtenerTelefonoCliente(UidUsuario);
        }

    }
}
