using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers
{
    public class ContactoController : ApiController
    {
        public string Get(int id)
        {
            string res = "NO";

            if (id == 29)
            {
                res = "SI";
            }

            return res;
        }

        public string RegistrarContacto([FromBody]Contacto contacto)
        {
            Guid par1 = contacto.UidContacto;
            int par2 = contacto.Id;
            string par3 = contacto.StrNombrePropietario;
            string par4 = contacto.StrCorreoElectronico;

            return "Registri Correcto";
        }

        public class Contacto
        {
            public Guid UidContacto { get; set; }
            public int Id { get; set; }
            public string StrInstitucion { get; set; }
            public string StrNombrePropietario { get; set; }
            public string StrCorreoElectronico { get; set; }
            public string StrNoTelefono { get; set; }
        }
    }
}