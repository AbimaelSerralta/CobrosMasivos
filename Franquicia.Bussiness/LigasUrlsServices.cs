using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class LigasUrlsServices
    {
        private LigasUrlsRepository _ligasUrlsRepository = new LigasUrlsRepository();
        public LigasUrlsRepository ligasUrlsRepository
        {
            get { return _ligasUrlsRepository; }
            set { _ligasUrlsRepository = value; }
        }

        public List<LigasUrlsListViewModel> lsLigasUrlsListViewModel = new List<LigasUrlsListViewModel>();
        public List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

        public List<LigasUrlsListViewModel> TotalEnvioLigas(Guid UidCliente)
        {
            lsLigasUrlsListViewModel = new List<LigasUrlsListViewModel>();

            return lsLigasUrlsListViewModel = ligasUrlsRepository.TotalEnvioLigas(UidCliente);
        }

        public List<LigasUrlsGridViewModel> ConsultarEstatusLiga(Guid UidCliente)
        {
            lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            return lsLigasUrlsGridViewModel = ligasUrlsRepository.ConsultarEstatusLiga(UidCliente);
        }
        public List<LigasUrlsGridViewModel> BuscarLigas(Guid UidCliente, string Identificador, string Usuario, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string VencimientoDesde, string VencimientoHasta)
        {
            lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            return lsLigasUrlsGridViewModel = ligasUrlsRepository.BuscarLigas(UidCliente, Identificador, Usuario, Asunto, Concepto, ImporteMayor, ImporteMenor, VencimientoDesde, VencimientoHasta);
        }
    }
}
