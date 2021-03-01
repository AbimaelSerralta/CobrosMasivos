using Franquicia.Bussiness;
using PagaLaEscuela.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace PagaLaEscuela.Controllers
{
    public class PragPayCardController : ApiController
    {
        [HttpPost]
        public ResponseHelpers PostPagosTarjeta([FromBody] RespuestaPago strResponse)
        {
            strResponse.StrResponse = HttpUtility.HtmlEncode(strResponse.StrResponse);
            var respuesta = new ResponseHelpers();
            string finalString = strResponse.StrResponse.Replace("%25", "%").Replace("%20", " ").Replace("%2B", "+").Replace("%3D", "=").Replace("%2F", "/").Replace("%0D%0A", "\r\n");
            // key con produccion
            string cadena = finalString;

            DateTime HoraDelServidor = DateTime.Now;
            DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            try
            {
                //correosServices.CorreoCadena(thisDay + " finalString " + cadena, "serralta2008@gmail.com");
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }

            string key = "E166173C2B870BDC3F62A67A77442FE1"; //Credenciales sanbox 
            //string key = "";
            AESCryptoPraga aesCryptoPraga = new AESCryptoPraga();
            string decryptedString = aesCryptoPraga.decrypt(key, cadena);

            if (!string.IsNullOrEmpty(decryptedString))
            {
                respuesta.Data = true;
            }
            else
            {
                respuesta.Data = "Lo sentimos, no hemos podido desifrar la cadena. " + cadena;
            }


            return respuesta;
        }

        public class RespuestaPago
        {
            private string _strResponse;
            public string StrResponse
            {
                get { return _strResponse; }
                set { _strResponse = value; }
            }
        }
    }
}