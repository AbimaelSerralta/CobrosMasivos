using Franquicia.Bussiness;
using Franquicia.Domain.Models.ClubPago;
using Franquicia.Domain.ViewModels.ClubPago;
using Franquicia.Domain.ViewModels.Praga;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers
{
    public class GenerarFormasPagosController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage FormasPagos([FromBody]GenerarFormaPago generarFormaPago)
        {
            ValidacionesServices validacionesServices = new ValidacionesServices();
            CodigoError codigoError = new CodigoError();
            EnviarIntegracionesServices enviarIntegracionesServices = new EnviarIntegracionesServices();

            string UidTipoPagoIntegracion = string.Empty;

            //Validar el usuario y contraseña
            if (!string.IsNullOrEmpty(generarFormaPago.User) && !string.IsNullOrEmpty(generarFormaPago.Password))
            {
                if (!validacionesServices.ValidarUsuarioContraseniaSandbox(generarFormaPago.User, generarFormaPago.Password))
                {
                    if (!validacionesServices.ValidarUsuarioContraseniaProduccion(generarFormaPago.User, generarFormaPago.Password))
                    {
                        codigoError.Message = "El usuario y/o contraseña son invalidos.";
                        codigoError.Codigo = "";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                    else
                    {
                        UidTipoPagoIntegracion = "D87454C9-12EF-4459-9CED-36E8401E4033";
                    }
                }
                else
                {
                    UidTipoPagoIntegracion = "3F792D20-B3B6-41D3-AF88-1BCB20D99BBE";
                }
            }
            else
            {
                codigoError.Message = "El usuario y la contraseña son obligatorios.";
                codigoError.Codigo = "";

                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
            }


            //Validar si existe el ID de la integracion
            if (!string.IsNullOrEmpty(generarFormaPago.IntegrationID))
            {
                try
                {
                    if (!validacionesServices.ExisteIntegracion(int.Parse(generarFormaPago.IntegrationID)))
                    {
                        codigoError.Message = "El ID de la integracion no existe.";
                        codigoError.Codigo = "";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                catch (Exception)
                {
                    codigoError.Message = "El formato del ID de la integracion es incorrecto.";
                    codigoError.Codigo = "";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }
            }
            else
            {
                codigoError.Message = "El ID de la integracion es obligatorio.";
                codigoError.Codigo = "";

                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
            }

            //Validar si existe el ID de la escuela
            if (!string.IsNullOrEmpty(generarFormaPago.SchoolID))
            {
                try
                {
                    if (!validacionesServices.ExisteEscuela(int.Parse(generarFormaPago.SchoolID)))
                    {
                        codigoError.Message = "El ID de la escuela no existe.";
                        codigoError.Codigo = "";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                catch (Exception)
                {
                    codigoError.Message = "El formato del ID de la escuela es incorrecto.";
                    codigoError.Codigo = "";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }
            }
            else
            {
                codigoError.Message = "El ID de la escuela es obligatorio.";
                codigoError.Codigo = "";

                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
            }

            //Validar el ID del comercio
            if (!string.IsNullOrEmpty(generarFormaPago.BusinessId))
            {
                try
                {
                    if (!validacionesServices.ExisteNegocioSandbox(int.Parse(generarFormaPago.BusinessId)))
                    {
                        if (!validacionesServices.ExisteNegocioProduccion(int.Parse(generarFormaPago.BusinessId)))
                        {
                            codigoError.Message = "El ID del negocio no existe.";
                            codigoError.Codigo = "";

                            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                        }
                    }
                }
                catch (Exception)
                {
                    codigoError.Message = "El formato del ID del negocio es incorrecto.";
                    codigoError.Codigo = "";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }
            }
            else
            {
                codigoError.Message = "El ID del negocio es obligatorio.";
                codigoError.Codigo = "";

                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
            }

            //Validar el ID de las Formas de pago (promocion)
            if (!string.IsNullOrEmpty(generarFormaPago.PaymentTypes))
            {
                try
                {
                    if (!validacionesServices.ExisteIdPromocionSandbox(int.Parse(generarFormaPago.PaymentTypes)))
                    {
                        if (!validacionesServices.ExisteIdPromocionProduccion(int.Parse(generarFormaPago.PaymentTypes)))
                        {
                            codigoError.Message = "El ID de la forma de pago no existe.";
                            codigoError.Codigo = "";

                            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                        }
                    }
                }
                catch (Exception)
                {
                    codigoError.Message = "El formato del ID de la forma de pago es incorrecto.";
                    codigoError.Codigo = "";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }
            }
            else
            {
                codigoError.Message = "El ID de la forma de pago es obligatorio.";
                codigoError.Codigo = "";

                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
            }

            //Validar el ID
            if (!string.IsNullOrEmpty(generarFormaPago.Id.ToString()))
            {
                try
                {
                    int ID = int.Parse(generarFormaPago.Id.ToString());
                }
                catch (Exception)
                {
                    codigoError.Message = "El formato del ID es incorrecto.";
                    codigoError.Codigo = "";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }
            }
            else
            {
                codigoError.Message = "El ID es obligatorio.";
                codigoError.Codigo = "";

                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
            }

            //Validar la Descripcion
            if (!string.IsNullOrEmpty(generarFormaPago.Description))
            {

            }
            else
            {
                codigoError.Message = "La Descripcion es obligatorio.";
                codigoError.Codigo = "";

                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
            }

            //Validar el importe min 50.00 Max 15,000.00
            if (!string.IsNullOrEmpty(generarFormaPago.Amount.ToString()))
            {
                decimal Min = 50;
                decimal Max = 15000;

                decimal Amount = generarFormaPago.Amount / 100;

                try
                {
                    if (Amount >= Min && Amount <= Max)
                    {
                    }
                    else
                    {
                        codigoError.Message = "El importe ingresado debe ser minimo de $50.00 y máximo $15,000.00.";
                        codigoError.Codigo = "";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                catch (Exception)
                {
                    codigoError.Message = "El formato del importe es incorrecto.";
                    codigoError.Codigo = "";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }
            }
            else
            {
                codigoError.Message = "El importe es obligatorio.";
                codigoError.Codigo = "";

                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
            }

            //Validar referencia si existe

            //Validar fecha de vencimiento
            if (!string.IsNullOrEmpty(generarFormaPago.ExpirationDate.ToString("dd/MM/yyyy")))
            {
                DateTime HoraServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                try
                {
                    if (DateTime.Parse(generarFormaPago.ExpirationDate.ToString("dd/MM/yyyy")) >= DateTime.Parse(hoy.ToString("dd/MM/yyyy")))
                    {

                    }
                    else
                    {
                        codigoError.Message = "La fecha de vencimiento tiene que ser mayor o igual al día de hoy.";
                        codigoError.Codigo = "";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                catch (Exception)
                {
                    codigoError.Message = "El formato de la fecha vencimiento es incorrecto.";
                    codigoError.Codigo = "";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }
            }

            //Enviar peticion al servicio praga
            GenerarLigaPagoIntegraciones generarLigaPagoIntegraciones = new GenerarLigaPagoIntegraciones();

            generarLigaPagoIntegraciones.user = generarFormaPago.User;
            generarLigaPagoIntegraciones.password = generarFormaPago.Password;
            generarLigaPagoIntegraciones.integrationID = generarFormaPago.IntegrationID;
            generarLigaPagoIntegraciones.schoolID = generarFormaPago.SchoolID;
            generarLigaPagoIntegraciones.description = generarFormaPago.Description;

            generarLigaPagoIntegraciones.ammount = generarFormaPago.Amount / 100;
            generarLigaPagoIntegraciones.businessId = generarFormaPago.BusinessId;
            generarLigaPagoIntegraciones.effectiveDate = generarFormaPago.ExpirationDate.ToString("dd/MM/yyyy");
            generarLigaPagoIntegraciones.id = generarFormaPago.Id;
            generarLigaPagoIntegraciones.paymentTypes = generarFormaPago.PaymentTypes;
            generarLigaPagoIntegraciones.reference = generarFormaPago.Reference;
            generarLigaPagoIntegraciones.station = generarFormaPago.IntegrationID;
            generarLigaPagoIntegraciones.UidTipoPagoIntegracion = UidTipoPagoIntegracion;
            enviarIntegracionesServices.EnviarPeticionPraga(generarLigaPagoIntegraciones);

            //Enviar peticion al servicio clubpago
            GenerarRefereciaPagoIntegraciones generarRefereciaPagoIntegraciones = new GenerarRefereciaPagoIntegraciones();

            generarRefereciaPagoIntegraciones.User = generarFormaPago.User;
            generarRefereciaPagoIntegraciones.Password = generarFormaPago.Password;
            generarRefereciaPagoIntegraciones.IntegrationID = generarFormaPago.IntegrationID;
            generarRefereciaPagoIntegraciones.SchoolID = generarFormaPago.SchoolID;

            generarRefereciaPagoIntegraciones.Description = generarFormaPago.Description;
            generarRefereciaPagoIntegraciones.Amount = generarFormaPago.Amount.ToString();
            generarRefereciaPagoIntegraciones.Account = generarFormaPago.Reference;
            generarRefereciaPagoIntegraciones.CustomerEmail = generarFormaPago.CustomerEmail;
            generarRefereciaPagoIntegraciones.CustomerName = generarFormaPago.CustomerName;
            generarRefereciaPagoIntegraciones.ExpirationDate = generarFormaPago.ExpirationDate.ToString("yyyy-MM-dd");
            generarRefereciaPagoIntegraciones.UidTipoPagoIntegracion = UidTipoPagoIntegracion;
            enviarIntegracionesServices.EnviarPeticionClubPago(generarRefereciaPagoIntegraciones);

            codigoError.Message = "Success";
            codigoError.Codigo = "";

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, codigoError);

        }

        public class GenerarFormaPago
        {
            public string User { get; set; }
            public string Password { get; set; }
            public string IntegrationID { get; set; }
            public string SchoolID { get; set; }
            public string BusinessId { get; set; }
            public string PaymentTypes { get; set; }
            public string Id { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public string Reference { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerName { get; set; }
            public DateTime ExpirationDate { get; set; }
        }

        public class CodigoError
        {
            public string Codigo { get; set; }
            public string Message { get; set; }
        }
    }
}