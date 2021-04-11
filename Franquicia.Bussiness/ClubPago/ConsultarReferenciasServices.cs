using Franquicia.DataAccess.Repository.ClubPago;
using Franquicia.Domain.Models.ClubPago;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.ClubPago
{
    public class ConsultarReferenciasServices
    {
        private ConsultarReferenciasRepository _consultarReferenciasRepository = new ConsultarReferenciasRepository();
        public ConsultarReferenciasRepository consultarReferenciasRepository
        {
            get { return _consultarReferenciasRepository; }
            set { _consultarReferenciasRepository = value; }
        }

        public ConsultarReferencia ConsultarReferenciaClubPago(string IdReferencia, DateTime day)
        {
            return consultarReferenciasRepository.ConsultarReferenciaClubPago(IdReferencia, day);
        }

        #region Metodos Integraciones
        public ConsultarReferencia ApiConsultarReferencia(string EndPoint, string IdReferencia)
        {
            ConsultarReferencia consultarReferencia = new ConsultarReferencia();

            try
            {
                var client = new RestClient(EndPoint + "/?r=" + IdReferencia);
                var request = new RestRequest(Method.GET);         
                request.AddHeader("content-type", "application/json");                
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                if (content != string.Empty)
                {
                    consultarReferencia = JsonConvert.DeserializeObject<ConsultarReferencia>(content.ToString());

                }

                return consultarReferencia;
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;

                return consultarReferencia;
            }
        }
        
        
        public ConsultarReferencia ApiConsultarReferenciaWeb(string EndPoint, string IdReferencia)
        {
            ConsultarReferencia consultarReferencia = new ConsultarReferencia();

            try
            {
                var client = new RestClient(EndPoint + "/?r=" + IdReferencia);
                client.UserAgent = "CPAPI_AGNT_V1";
                var request = new RestRequest(Method.GET);
                request.AddHeader("X-Origin", "Njg3NzZERjgxNEFTV1JT");
                request.AddHeader("content-type", "application/json");
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                if (content != string.Empty)
                {
                    consultarReferencia = JsonConvert.DeserializeObject<ConsultarReferencia>(content.ToString());

                }

                return consultarReferencia;
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;

                return consultarReferencia;
            }
        }
        public ConsultarReferencia ApiPagarReferenciaWeb(string EndPoint, string transaccion, string fecha, string monto, string IdReferencia)
        {
            ConsultarReferencia consultarReferencia = new ConsultarReferencia();

            try
            {
                var client = new RestClient(EndPoint);
                client.UserAgent = "CPAPI_AGNT_V1";
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-Origin", "Njg3NzZERjgxNEFTV1JT");
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\n\t\"transaccion\":\"" + transaccion + "\",\n\t\"fecha\":\"" + fecha + "\",\n\t\"monto\":\"" + monto + "\",\n\t\"referencia\":\"" + IdReferencia + "\"\n}", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                var content = response.Content;

                if (content != string.Empty)
                {
                    consultarReferencia = JsonConvert.DeserializeObject<ConsultarReferencia>(content.ToString());

                }

                return consultarReferencia;
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;

                return consultarReferencia;
            }
        }
        #endregion
    }
}
