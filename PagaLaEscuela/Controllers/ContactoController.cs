using Franquicia.Bussiness.LandingPage;
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
        ContactoServices contactoservices = new ContactoServices();
        public string Get(int id)
        {
          string res =   contactoservices.ValidarFranquiciatario(id).ToString();
            return res;
        }
        public Contacto RegistrarContacto([FromBody]Contacto contacto)
        {
            Guid par1 = contacto.UidPosibleCliente;
            int par2 = contacto.IdFranquicia;
            string par3 = contacto.VchContactoPropietario;
            string par4 = contacto.VchCorreoElectronico;
            string par5 = contacto.VchNoTelefono;
            string par6 = contacto.VchInstituto;


            contactoservices.Registrar(par1,par6,par3,par4,par5,par2);

            return contacto;
        }
        public string Registrar(Guid Uid ,int Id, string StrInstitucion, string StrNombrePropietario, string StrCorreoElectronico, string StrNoTelefono)
        {
            contactoservices.Registrar(Uid,  StrInstitucion,  StrNombrePropietario,  StrCorreoElectronico,  StrNoTelefono,Id);

            return "Registro Correcto";
        }
        public string RegistrarCita([FromBody]CitaContacto contacto)
        {
            contactoservices.RegistrarCita(contacto.UidPosibleCliente, contacto.VchFecha, contacto.VchHora);
            return "Registro Correcto";
        }
        public string ObtenerCorreoFranquiciatario([FromBody]int idFranqui)
        {
            return contactoservices.ObtenerCorreoFranquiciatario(idFranqui);
        }
        public string RegistrarCita(Guid UidPosibleCliente, string StrFecha, string StrHora)
        {
            contactoservices.RegistrarCita(UidPosibleCliente, StrFecha, StrHora);
            return "Registro Correcto";
        }
        public class Contacto
        {
            public Guid UidPosibleCliente { get; set; }
            public string VchInstituto { get; set; }
            public string VchContactoPropietario { get; set; }
            public string VchCorreoElectronico { get; set; }
            public string VchNoTelefono { get; set; }
            public int IdFranquicia { get; set; }
        }
        public class CitaContacto
        {
            public Guid UidPosibleCliente { get; set; }
            public string VchFecha { get; set; }
            public string VchHora { get; set; }
        }

    }
}