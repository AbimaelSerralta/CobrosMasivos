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
    public class TarifasServices
    {
        private TarifasRepository _tarifasRepository = new TarifasRepository();
        public TarifasRepository tarifasRepository
        {
            get { return _tarifasRepository; }
            set { _tarifasRepository = value; }
        }

        public List<TarifasGridViewModel> lsTarifasGridViewModel = new List<TarifasGridViewModel>();

        public List<TarifasGridViewModel> CargarTarifas()
        {
            return lsTarifasGridViewModel = tarifasRepository.CargarTarifas();
        }

        public bool RegistrarTarifas(decimal DcmWhatsapp, decimal DcmSms)
        {
            Guid UidFranquiciatario = Guid.NewGuid();

            bool result = false;
            if (tarifasRepository.RegistrarTarifas(
                new TarifasGridViewModel
                {
                    DcmWhatsapp = DcmWhatsapp,
                    DcmSms = DcmSms
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarTarifas(Guid UidTarifa, decimal DcmWhatsapp, decimal DcmSms)
        {
            bool result = false;
            if (tarifasRepository.ActualizarTarifas(
                new TarifasGridViewModel
                {
                    UidTarifa = UidTarifa,
                    DcmWhatsapp = DcmWhatsapp,
                    DcmSms = DcmSms
                }))
            {
                result = true;
            }
            return result;
        }
    }
}
