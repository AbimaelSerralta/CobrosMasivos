using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TelefonosAlumnosServices
    {
        private TelefonosAlumnosRepository _telefonosAlumnosRepository = new TelefonosAlumnosRepository();
        public TelefonosAlumnosRepository telefonosAlumnosRepository
        {
            get { return _telefonosAlumnosRepository; }
            set { _telefonosAlumnosRepository = value; }
        }

        public TelefonosAlumnos ObtenerTelefonosAlumnos(Guid UidAlumno)
        {
            return telefonosAlumnosRepository.ObtenerTelefonosAlumnos(UidAlumno);
        }
    }
}
