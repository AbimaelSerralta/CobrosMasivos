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
        int BusinessId = 1802;
        string Url = $"https://qaag.mitec.com.mx/praga-ws/url/generateUrlV3";
        string UserCode = "1607022879421";
        string WSEncryptionKey = "996196AFE9828EE0BB0397E1405CBA9A";
        string APIKey = "MTc4NGQwN2ItM2E2ZS00MWZiLWE5MTYtMDQ4YTVhMmJhZmFl";

        string User = "Pagalaescuela433";
        string Pswd = "Pagalaescuela433$";

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

        public void AuthClub()
        {
            var client = new RestClient("https://qa.clubpago.site/auth/api/auth?=");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json",
"{\n\t\"user\":\"Pagalaescuela433\",\n\t\"pswd\":\"Pagalaescuela433$\"\n}",
ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
        }

        public List<ObtenerRefereciaPago> GenerarReferencia(string Description, string Amount, string Account, string CustomerEmail, string CustomerName, string ExpirationDate)
        {
            GenerarRefereciaPago generarRefereciaPago = new GenerarRefereciaPago();
            List<ObtenerRefereciaPago> lsObtenerRefereciaPago = new List<ObtenerRefereciaPago>();

            generarRefereciaPago.Description = Description;
            generarRefereciaPago.Amount = Amount;
            generarRefereciaPago.Account = Account;
            generarRefereciaPago.CustomerEmail = CustomerEmail;
            generarRefereciaPago.CustomerName = CustomerName;
            generarRefereciaPago.ExpirationDate = ExpirationDate;

            var client = new
            RestClient("https://qa.clubpago.site/referencegenerator/svc/generator/payformat");
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject(generarRefereciaPago);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlBhZ2FsYWVzY3VlbGE0MzMiLCJFbWlzb3JJZCI6IjQzMyIsImp0aSI6IjU5YmY5MzQyLWJhMWYtNDZiMy05NDdiLTI2NjhiOWVhYzNlOCIsIm5iZiI6MTYwOTc5NjEyMSwiZXhwIjoxNjA5ODM5MzIxLCJpc3MiOiJjbHVicGFnby5teCIsImF1ZCI6ImNsdWJwYWdvLm14In0.qwu6tDnDir_Us3EtSogw7jRepMAMxwHbc2AOkFmrGcA");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            ObtenerRefereciaPago obtenerRefereciaPago = JsonConvert.DeserializeObject<ObtenerRefereciaPago>(content.ToString());

            lsObtenerRefereciaPago.Add(obtenerRefereciaPago);

            return lsObtenerRefereciaPago;
        }
    }
}
