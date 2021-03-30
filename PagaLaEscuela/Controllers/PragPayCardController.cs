using Franquicia.Bussiness;
using Franquicia.Domain.Models;
using Newtonsoft.Json;
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
            PagosTarjetaPragaServices pagosTarjetaPragaServices = new PagosTarjetaPragaServices();
            ColegiaturasServices colegiaturasServices = new ColegiaturasServices();
            PagosColegiaturasServices pagosColegiaturasServices = new PagosColegiaturasServices();
            CorreosServices correosServices = new CorreosServices();

            strResponse.StrResponse = HttpUtility.HtmlEncode(strResponse.StrResponse);
            var respuesta = new ResponseHelpers();
            string finalString = strResponse.StrResponse.Replace("%25", "%").Replace("%20", " ").Replace("%2B", "+").Replace("%3D", "=").Replace("%2F", "/").Replace("%0D%0A", "\r\n");
            // key con produccion
            string cadena = finalString;

            DateTime HoraDelServidor = DateTime.Now;
            DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            try
            {
                correosServices.CorreoCadena(thisDay + " finalString " + cadena, "serralta2008@gmail.com");
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }

            //string key = "E166173C2B870BDC3F62A67A77442FE1"; //Credenciales sanbox 
            //string key = "4451B4A2EBA9E3D49E7981FD2464C361";
            string key = "EFD881A116694CA63F9D33CD2F5B8FDB";
            AESCryptoPraga aesCryptoPraga = new AESCryptoPraga();
            string decryptedString = aesCryptoPraga.decrypt(key, cadena);

            if (!string.IsNullOrEmpty(decryptedString))
            {
                Respuesta respues = JsonConvert.DeserializeObject<Respuesta>(decryptedString);

                ////Simular datos (datos manuales)
                //respues.reference = "0000040000000010002001";

                if (pagosTarjetaPragaServices.AgregarInformacionTarjeta(RellenarDatos(respues, thisDay)))
                {
                    respuesta.Data = true;

                    if (respues.response == "approved")
                    {
                        // ==> ENVIO DE CORREO DE PAGO COLEGIATURA
                        var para = pagosTarjetaPragaServices.ConsultarPagoColegiatura(respues.reference);

                        if (!string.IsNullOrEmpty(para.Item1))
                        {
                            var list = pagosTarjetaPragaServices.ObtenerPagoColegiatura(Guid.Parse(para.Item1));

                            pagosTarjetaPragaServices.ActualizarPagoColegiatura(Guid.Parse(para.Item1));

                            //correosEscuelaServices.CorreoEnvioPagoColegiatura(list.Item1, list.Item2, "Comprobante de pago de colegiatura", reference, fechaRegistro, "************" + cc_number, foliocpagos, para.Item2, "APROBADO", Guid.Parse(para.Item3));

                            var data = pagosTarjetaPragaServices.ConsultarDatosValidarPago(Guid.Parse(para.Item1));

                            Guid UidClienteLocal = Guid.Parse(data.Item1);
                            Guid UidUsuario = Guid.Parse(data.Item2);
                            Guid UidFechaColegiatura = Guid.Parse(data.Item3);
                            Guid UidAlumno = Guid.Parse(data.Item4);

                            //Necesito saber el importe de la colegiatura
                            decimal ImporteCole = colegiaturasServices.ObtenerDatosFechaColegiatura(UidClienteLocal, UidUsuario, UidFechaColegiatura, UidAlumno);

                            //Necesito saber el importe de todos los pagos
                            decimal ImportePagado = pagosColegiaturasServices.ObtenerPagosPadresRLE(UidFechaColegiatura, UidAlumno);
                            decimal ImportePendiente = pagosColegiaturasServices.ObtenerPendientesPadresRLE(UidFechaColegiatura, UidAlumno);

                            // ==>Validar con importe<==
                            if (ImporteCole == ImportePagado) //el importeColegiatura es igual al importe de todos los pagos con estatus aprobado
                            {
                                //Se cambia el estatus de la colegiatura a pagado.
                                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse("605A7881-54E0-47DF-8398-EDE080F4E0AA"), Guid.Parse("80EAC55B-8363-4FA3-86A5-430971F0E0E6"), true);
                            }
                            else if (ImporteCole == (ImportePagado + ImportePendiente)) //el importe de los pagos aprobado y pendiente es igual al importe la colegiatura
                            {
                                // La colegiatura mantiene el estatus en proceso
                                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse("5554CE57-1288-46D5-B36A-8AC69CB94B9A"), Guid.Parse("80EAC55B-8363-4FA3-86A5-430971F0E0E6"), true);
                            }
                            else
                            {
                                Guid UidEstatusColeAlumnos = Guid.Parse("CEB03962-B62D-42F4-A299-582EB59E0D75");

                                if ((ImportePagado + ImportePendiente) != 0)
                                {
                                    UidEstatusColeAlumnos = Guid.Parse("2545BF35-F4D4-4D18-8234-8AB9D8A4ECB8");
                                }

                                // La colegiatura regresa al ultimo estatus
                                DateTime HoraServidor = DateTime.Now;
                                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
                                string UidEstatus = colegiaturasServices.ObtenerEstatusColegiaturasRLE(hoy, UidFechaColegiatura, UidAlumno);
                                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse(UidEstatus.ToString()), UidEstatusColeAlumnos, false);
                            }

                        }
                    }
                }
                else
                {
                    respuesta.Data = "Lo sentimos, ha ocurrido un error inesperado. " + cadena;
                }
            }
            else
            {
                respuesta.Data = "Lo sentimos, no hemos podido desifrar la cadena. " + cadena;
            }


            return respuesta;
        }

        private PagosTarjetaPraga RellenarDatos(Respuesta respuesta, DateTime thisDay)
        {
            PagosTarjetaPraga pagosTarjetaPraga = new PagosTarjetaPraga();

            pagosTarjetaPraga.IdReferencia = respuesta.reference;
            pagosTarjetaPraga.VchEstatus = respuesta.response;
            pagosTarjetaPraga.foliocpagos = respuesta.foliocpagos;
            pagosTarjetaPraga.auth = respuesta.auth;
            pagosTarjetaPraga.cd_response = respuesta.cd_response;
            pagosTarjetaPraga.cd_error = respuesta.cd_error;
            pagosTarjetaPraga.nb_error = respuesta.nb_error;
            pagosTarjetaPraga.DtmFechaDeRegistro = thisDay;
            if (string.IsNullOrEmpty(respuesta.date) && string.IsNullOrEmpty(respuesta.time))
            {
                respuesta.date = thisDay.ToString("dd/MM/yyyy");
                respuesta.time = thisDay.ToString("HH:mm:ss");
            }
            pagosTarjetaPraga.DtFechaOperacion = DateTime.Parse(DateTime.Parse(respuesta.date + " " + respuesta.time).ToString("dd/MM/yyyy HH:mm:ss"));
            pagosTarjetaPraga.nb_company = respuesta.nb_company;
            pagosTarjetaPraga.nb_merchant = respuesta.nb_merchant;
            pagosTarjetaPraga.cc_type = respuesta.cc_type;
            pagosTarjetaPraga.tp_operation = respuesta.tp_operation;
            pagosTarjetaPraga.cc_name = respuesta.cc_name;
            pagosTarjetaPraga.cc_number = respuesta.cc_number;
            pagosTarjetaPraga.cc_expmonth = respuesta.cc_expmonth;
            pagosTarjetaPraga.cc_expyear = respuesta.cc_expyear;
            if (string.IsNullOrEmpty(respuesta.amount))
            {
                respuesta.amount = "0";
            }
            pagosTarjetaPraga.amount = decimal.Parse(respuesta.amount);
            pagosTarjetaPraga.emv_key_date = respuesta.emv_key_date;
            pagosTarjetaPraga.id_url = respuesta.id_url;
            pagosTarjetaPraga.email = respuesta.email;
            pagosTarjetaPraga.payment_type = respuesta.payment_type;

            return pagosTarjetaPraga;
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

        public class Respuesta
        {
            public string reference { get; set; }
            public string response { get; set; }
            public string foliocpagos { get; set; }
            public string auth { get; set; }
            public string cd_response { get; set; }
            public string cd_error { get; set; }
            public string nb_error { get; set; }
            public string time { get; set; }
            public string date { get; set; }
            public string nb_company { get; set; }
            public string nb_merchant { get; set; }
            public string cc_type { get; set; }
            public string tp_operation { get; set; }
            public string cc_name { get; set; }
            public string cc_number { get; set; }
            public string cc_expmonth { get; set; }
            public string cc_expyear { get; set; }
            public string amount { get; set; }
            public string emv_key_date { get; set; }
            public string id_url { get; set; }
            public string email { get; set; }
            public string payment_type { get; set; }
        }
    }
}