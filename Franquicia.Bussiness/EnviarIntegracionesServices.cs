using Franquicia.Domain.Models.ClubPago;
using Franquicia.Domain.ViewModels.ClubPago;
using Franquicia.Domain.ViewModels.Praga;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class EnviarIntegracionesServices
    {
        public void EnviarPeticionPraga(GenerarLigaPagoIntegraciones generarLigaPagoIntegraciones)
        {
            try
            {
                var client = new RestClient("https://localhost:44352/Pagalaescuela/Service/GenerarLigas");
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject(generarLigaPagoIntegraciones);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }
        }
        public void EnviarPeticionClubPago(GenerarRefereciaPagoIntegraciones generarRefereciaPagoIntegraciones)
        {
            try
            {
                var client = new RestClient("https://localhost:44352/Pagalaescuela/Service/GenerarReferencia");
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject(generarRefereciaPagoIntegraciones);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }
        }
        public AutorizacionPago EnviarPeticionPagoReferenciaClubPago(SolicitudPago solicitudPago, string EndPoint)
        {
            AutorizacionPago autorizacionPago = new AutorizacionPago();

            try
            {
                var client = new RestClient(EndPoint);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject(solicitudPago);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                if (content != string.Empty)
                {
                    autorizacionPago = JsonConvert.DeserializeObject<AutorizacionPago>(content.ToString());
                }

            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }

            return autorizacionPago;
        }
    }
}
