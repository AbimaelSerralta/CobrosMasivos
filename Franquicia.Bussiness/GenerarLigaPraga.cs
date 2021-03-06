﻿using Franquicia.DataAccess.Repository;
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

        public List<UrlV3PaymentResponse> ApiGenerarURL(decimal Ammount, string EffectiveDate, string Id, string PaymentTypes, string Reference, string Station)
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
    }
}
