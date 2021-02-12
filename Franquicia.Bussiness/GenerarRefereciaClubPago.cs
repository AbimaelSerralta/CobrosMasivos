using Franquicia.DataAccess.Repository;
using Franquicia.Domain;
using Franquicia.Domain.Models.ClubPago;
using Franquicia.Domain.Models.Praga;
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
    public class GenerarRefereciaClubPago
    {
        private ParametrosClubPagoRepository _parametrosClubPagoRepository = new ParametrosClubPagoRepository();
        public ParametrosClubPagoRepository parametrosClubPagoRepository
        {
            get { return _parametrosClubPagoRepository; }
            set { _parametrosClubPagoRepository = value; }
        }

        int BusinessId = 1802;
        string Url = $"https://qaag.mitec.com.mx/praga-ws/url/generateUrlV3";
        string UserCode = "1607022879421";
        string WSEncryptionKey = "996196AFE9828EE0BB0397E1405CBA9A";
        string APIKey = "MTc4NGQwN2ItM2E2ZS00MWZiLWE5MTYtMDQ4YTVhMmJhZmFl";

        string VchUrlAuth = "";
        string VchUrlGenerarRef = "";
        string User = "";
        string Pswd = "";

        public GenerarRefereciaClubPago()
        {
            parametrosClubPagoRepository.ObtenerParametrosClubPago();

            VchUrlAuth = parametrosClubPagoRepository.parametrosClubPago.VchUrlAuth;
            VchUrlGenerarRef = parametrosClubPagoRepository.parametrosClubPago.VchUrlGenerarRef;
            User = parametrosClubPagoRepository.parametrosClubPago.VchUser;
            Pswd = parametrosClubPagoRepository.parametrosClubPago.VchPass;
        }

        public void ApiGenerarURL(decimal Ammount, string Currency, string EffectiveDate, string Id, string PaymentTypes, string Reference, string Station)
        {
            UrlV3PaymentRequest urlV3 = new UrlV3PaymentRequest();
            AESCryptoPraga aesCryptoPraga = new AESCryptoPraga();

            urlV3.ammount = Ammount;
            urlV3.businessId = BusinessId;
            urlV3.currency = Currency;
            urlV3.effectiveDate = EffectiveDate;
            urlV3.id = Id;
            urlV3.paymentTypes = PaymentTypes;
            urlV3.reference = Reference;
            urlV3.station = Station;
            urlV3.userCode = UserCode;

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
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Mnsj = responseBody;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Mnsj = ex.Message;
            }
        }

        public List<AuthClub> AuthClub()
        {
            List<AuthClub> lsAuthClub = new List<AuthClub>();

            var client = new RestClient(VchUrlAuth);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\n\t\"user\":\"" + User + "\",\n\t\"pswd\":\"" + Pswd + "\"\n}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            if (content != string.Empty)
            {
                AuthClub authClub = JsonConvert.DeserializeObject<AuthClub>(content.ToString());

                lsAuthClub.Add(authClub);
            }

            return lsAuthClub;
        }

        public List<ObtenerRefereciaPago> GenerarReferencia(string Description, string Amount, string Account, string CustomerEmail, string CustomerName, string ExpirationDate)
        {
            string token = string.Empty;
            foreach (var item in AuthClub())
            {
                token = item.Token;
            }

            GenerarRefereciaPago generarRefereciaPago = new GenerarRefereciaPago();
            List<ObtenerRefereciaPago> lsObtenerRefereciaPago = new List<ObtenerRefereciaPago>();

            try
            {
                generarRefereciaPago.Description = Description;
                generarRefereciaPago.Amount = Amount;
                generarRefereciaPago.Account = Account;
                generarRefereciaPago.CustomerEmail = CustomerEmail;
                generarRefereciaPago.CustomerName = CustomerName;
                generarRefereciaPago.ExpirationDate = ExpirationDate;

                var client = new RestClient(VchUrlGenerarRef);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject(generarRefereciaPago);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Bearer " + token);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                if (content != string.Empty)
                {
                    ObtenerRefereciaPago obtenerRefereciaPago = JsonConvert.DeserializeObject<ObtenerRefereciaPago>(content.ToString());

                    lsObtenerRefereciaPago.Add(obtenerRefereciaPago);
                }
                
                return lsObtenerRefereciaPago;
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;

                return lsObtenerRefereciaPago;
            }
        }
    }
}
