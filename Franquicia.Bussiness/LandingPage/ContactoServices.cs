using Franquicia.DataAccess.Repository.LandingPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.LandingPage
{
    public class ContactoServices
    {
        ContactoRepository _contactoRepository = new ContactoRepository();
        public ContactoRepository contactoRepository
        {
            get { return _contactoRepository; }
            set { _contactoRepository = value; }
        }

        public bool Registrar(Guid UidPosibleCliente, string VchInstituto,
            string VchContactoPropietario, string VchCorreoElectronico,
            string VchNoTelefono, int IdFranquicia)
        {
           return contactoRepository.Registrar( UidPosibleCliente,  VchInstituto,
             VchContactoPropietario,  VchCorreoElectronico,
             VchNoTelefono,  IdFranquicia);
        }
        public bool RegistrarCita(Guid UidPosibleCliente, string VchFecha,
           string VchHora)
        {
            return contactoRepository.RegistrarCita(UidPosibleCliente, VchFecha,
              VchHora);
        }
        public string ObtenerCorreoFranquiciatario(int idFranqui)
        {
            return contactoRepository.ObtenerCorreoFranquiciatario(idFranqui);
        }
        public string ValidarFranquiciatario( int IdFranquicia) 
        {
            return contactoRepository.ValidarFranquiciatario( IdFranquicia);
        }


    }
}
