using Franquicia.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class PagosServices
    {
        private PagosRepository _pagosRepository = new PagosRepository();
        public PagosRepository pagosRepository
        {
            get { return _pagosRepository; }
            set { _pagosRepository = value; }
        }

        public bool AgregarInformacionTarjeta(string Autorizacion, string reference, DateTime HoraTransaccion, string response, string cc_type, string tp_operation, string nb_company, string nb_merchant, string id_url, string cd_error, string nb_error, string cc_number, string cc_mask, string FolioPago, decimal Monto, DateTime DtFechaOperacion)
        {
            return pagosRepository.AgregarInformacionTarjeta(Autorizacion, reference, HoraTransaccion, response, cc_type, tp_operation, nb_company, nb_merchant, id_url, cd_error, nb_error, cc_number, cc_mask, FolioPago, Monto, DtFechaOperacion);
        }
    }
}
