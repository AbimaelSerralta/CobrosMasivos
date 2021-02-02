using Franquicia.Domain;
using Franquicia.Domain.Models.Praga;
using Newtonsoft.Json;
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
        int BusinessId = 13131;
        string Url = $"https://qaag.mitec.com.mx/praga-ws/url/generateUrlV3";
        string UserCode = "1610137579779";
        string WSEncryptionKey = "4451B4A2EBA9E3D49E7981FD2464C361";
        string APIKey = "ZDMxYzc3MGItZjEyMS00OTRhLTkxNmQtYmE5Yjk0M2YzYzlm";

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
    }
}
