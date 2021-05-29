using Franquicia.DataAccess.Repository;
using Franquicia.Domain;
using Franquicia.Domain.Models.Praga;
using Franquicia.Domain.ViewModels.Praga;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class GenerarLigaPraga
    {
        private ParametrosPragaRepository _parametrosPragaRepository = new ParametrosPragaRepository();
        public ParametrosPragaRepository parametrosPragaRepository
        {
            get { return _parametrosPragaRepository; }
            set { _parametrosPragaRepository = value; }
        }

        string BusinessId = "";
        string Url = $"";
        string UserCode = "";
        string WSEncryptionKey = "";
        string APIKey = "";
        string Currency = "";

        public GenerarLigaPraga() { }

        public GenerarLigaPraga(Guid UidPropietario)
        {
            parametrosPragaRepository.ObtenerParametrosPraga(UidPropietario);

            BusinessId = parametrosPragaRepository.parametrosPraga.BusinessId;
            if (string.IsNullOrEmpty(parametrosPragaRepository.parametrosPraga.VchUrl))
            {
                Url = $"" + "https://www.praga.io/praga-ws/url/generateUrlV3";
            }
            else
            {
                Url = $"" + parametrosPragaRepository.parametrosPraga.VchUrl;
            }
            UserCode = parametrosPragaRepository.parametrosPraga.UserCode;
            WSEncryptionKey = parametrosPragaRepository.parametrosPraga.WSEncryptionKey;
            APIKey = parametrosPragaRepository.parametrosPraga.APIKey;
            Currency = parametrosPragaRepository.parametrosPraga.Currency;
        }

        public List<UrlV3PaymentResponse> ApiGenerarURL(decimal Ammount, string EffectiveDate, string Id, string PaymentTypes, string Reference, string Station, string concepto)
        {
            UrlV3PaymentRequest urlV3 = new UrlV3PaymentRequest();
            AESCryptoPraga aesCryptoPraga = new AESCryptoPraga();

            List<UrlV3PaymentResponse> lsUrlV3PaymentResponse = new List<UrlV3PaymentResponse>();

            urlV3.ammount = Ammount;
            urlV3.businessId = BusinessId;
            urlV3.currency = Currency;
            urlV3.effectiveDate = EffectiveDate;
            urlV3.id = Id;
            urlV3.paymentTypes = PaymentTypes;
            urlV3.reference = Reference;
            urlV3.station = Station;
            urlV3.userCode = UserCode;
            urlV3.valuePairs = new ValuePair[1] { new ValuePair { label = "Concepto:", value = concepto } };

            string Mnsj = string.Empty;

            var url = Url;
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = JsonConvert.SerializeObject(urlV3);

            string encryptedString = aesCryptoPraga.encrypt(json, WSEncryptionKey);

            string json2 = encryptedString;

            request.Headers.Add("Authorization", APIKey);
            request.Method = "post";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json2);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader != null)
                        {
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();

                                if (responseBody != string.Empty)
                                {
                                    UrlV3PaymentResponse obtenerRefereciaPago = JsonConvert.DeserializeObject<UrlV3PaymentResponse>(responseBody);

                                    lsUrlV3PaymentResponse.Add(obtenerRefereciaPago);
                                }
                            }
                        };
                    }
                }
            }
            catch (WebException ex)
            {
                Mnsj = ex.Message;
            }

            return lsUrlV3PaymentResponse;
        }

        #region Metodos Integraciones
        public void ApiObtenerCredenciales(Guid UidTipoPagoIntegracion, int IdNegocio)
        {
            if (UidTipoPagoIntegracion == Guid.Parse("3F792D20-B3B6-41D3-AF88-1BCB20D99BBE")) //SANDBOX
            {
                parametrosPragaRepository.ObtenerParametrosPragaSandbox(IdNegocio);
            }
            else if (UidTipoPagoIntegracion == Guid.Parse("D87454C9-12EF-4459-9CED-36E8401E4033")) //PRODUCCION
            {
                parametrosPragaRepository.ObtenerParametrosPragaProduccion(IdNegocio);
            }

            BusinessId = parametrosPragaRepository.parametrosPraga.BusinessId;
            if (string.IsNullOrEmpty(parametrosPragaRepository.parametrosPraga.VchUrl))
            {
                Url = $"" + "https://www.praga.io/praga-ws/url/generateUrlV3";
            }
            else
            {
                Url = $"" + parametrosPragaRepository.parametrosPraga.VchUrl;
            }
            UserCode = parametrosPragaRepository.parametrosPraga.UserCode;
            WSEncryptionKey = parametrosPragaRepository.parametrosPraga.WSEncryptionKey;
            APIKey = parametrosPragaRepository.parametrosPraga.APIKey;
            Currency = parametrosPragaRepository.parametrosPraga.Currency;
        }
        public UrlV3PaymentResponse ApiGenerarLiga(GenerarLigaPagoIntegraciones generarLigaPagoIntegraciones)
        {
            ApiObtenerCredenciales(Guid.Parse(generarLigaPagoIntegraciones.UidTipoPagoIntegracion), int.Parse(generarLigaPagoIntegraciones.businessId));

            UrlV3PaymentRequest urlV3 = new UrlV3PaymentRequest();
            AESCryptoPraga aesCryptoPraga = new AESCryptoPraga();

            UrlV3PaymentResponse urlV3PaymentResponse = new UrlV3PaymentResponse();

            //Asignacion de parametros necesarios para generar la liga
            urlV3.ammount = generarLigaPagoIntegraciones.ammount;
            urlV3.businessId = generarLigaPagoIntegraciones.businessId;
            urlV3.currency = Currency;
            urlV3.effectiveDate = generarLigaPagoIntegraciones.effectiveDate;
            urlV3.id = generarLigaPagoIntegraciones.id;
            urlV3.paymentTypes = generarLigaPagoIntegraciones.paymentTypes;
            urlV3.reference = generarLigaPagoIntegraciones.reference;
            urlV3.station = generarLigaPagoIntegraciones.integrationID;
            urlV3.userCode = UserCode;
            urlV3.valuePairs = new ValuePair[1]{new ValuePair {label = "Concepto:", value = generarLigaPagoIntegraciones.description } };

            string Mnsj = string.Empty;

            var url = Url;
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = JsonConvert.SerializeObject(urlV3);

            string encryptedString = aesCryptoPraga.encrypt(json, WSEncryptionKey);

            string json2 = encryptedString;

            request.Headers.Add("Authorization", APIKey);
            request.Method = "post";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json2);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader != null)
                        {
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();

                                if (responseBody != string.Empty)
                                {
                                    urlV3PaymentResponse = JsonConvert.DeserializeObject<UrlV3PaymentResponse>(responseBody);
                                    urlV3PaymentResponse.reference = generarLigaPagoIntegraciones.reference;
                                    urlV3PaymentResponse.referenceEmisor = generarLigaPagoIntegraciones.referenceEmisor;
                                }
                            }
                        };
                    }
                }
            }
            catch (WebException ex)
            {
                Mnsj = ex.Message;

                if (ex.Message.Contains("400"))
                {
                    urlV3PaymentResponse.code = "400";
                    urlV3PaymentResponse.message = "Petición incorrecta";
                }
            }

            return urlV3PaymentResponse;
        }

        public void EnviarRespuesta(UrlV3PaymentResponse urlV3PaymentResponse, string UrlEntrega)
        {
            try
            {
                var client = new RestClient(UrlEntrega);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject(urlV3PaymentResponse);
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
        
        #endregion
    }
}
