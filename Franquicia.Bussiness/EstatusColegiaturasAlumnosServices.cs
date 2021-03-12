using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class EstatusColegiaturasAlumnosServices
    {
        private EstatusColegiaturasAlumnosRepository estatusColegiaturasAlumnosRepository = new EstatusColegiaturasAlumnosRepository();

        public List<EstatusColegiaturasAlumnos> lsEstatusColegiaturasAlumnos = new List<EstatusColegiaturasAlumnos>();

        public void CargarEstatusColegiaturasAlumnos()
        {
            lsEstatusColegiaturasAlumnos = new List<EstatusColegiaturasAlumnos>();

            lsEstatusColegiaturasAlumnos = estatusColegiaturasAlumnosRepository.CargarEstatusColegiaturasAlumnos();
        }
    }
}
