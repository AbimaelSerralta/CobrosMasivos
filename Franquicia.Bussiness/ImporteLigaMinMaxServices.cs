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
    public class ImporteLigaMinMaxServices
    {
        private ImporteLigaMinMaxRepository _importeLigaMinMaxRepository = new ImporteLigaMinMaxRepository();
        public ImporteLigaMinMaxRepository importeLigaMinMaxRepository
        {
            get { return _importeLigaMinMaxRepository; }
            set { _importeLigaMinMaxRepository = value; }
        }

        public List<ImporteLigaMinMax> lsImporteLigaMinMax = new List<ImporteLigaMinMax>();

        public List<ImporteLigaMinMax> CargarImporteLigaMinMax()
        {
            return lsImporteLigaMinMax = importeLigaMinMaxRepository.CargarImporteLigaMinMax();
        }

        public bool RegistrarImporteLigaMinMax(decimal DcmImporteMin, decimal DcmImporteMax)
        {
            Guid UidFranquiciatario = Guid.NewGuid();

            bool result = false;
            if (importeLigaMinMaxRepository.RegistrarImporteLigaMinMax(
                new ImporteLigaMinMax
                {
                    DcmImporteMin = DcmImporteMin,
                    DcmImporteMax = DcmImporteMax
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarImporteLigaMinMax(decimal DcmImporteMin, decimal DcmImporteMax, Guid UidImporteLiga)
        {
            bool result = false;
            if (importeLigaMinMaxRepository.ActualizarImporteLigaMinMax(
                new ImporteLigaMinMax
                {
                    DcmImporteMin = DcmImporteMin,
                    DcmImporteMax = DcmImporteMax,
                    UidImporteLiga = UidImporteLiga
                }))
            {
                result = true;
            }
            return result;
        }
    }
}
