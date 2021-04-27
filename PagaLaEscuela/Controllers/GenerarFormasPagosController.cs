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

            string TipoCredenciales = string.Empty;
            string IdReferencia = string.Empty;

            //Validar si la integracion esta activa
            if (validacionesServices.ValidarEstatusIntegracion(int.Parse(generarFormaPago.IntegrationID)) == Guid.Parse("65E46BC9-1864-4145-AD1A-70F5B5F69739"))
            {
                string UidTipoPagoIntegracion = string.Empty;

                //Validar el usuario y contraseña
                if (!string.IsNullOrEmpty(generarFormaPago.User) && !string.IsNullOrEmpty(generarFormaPago.Password))
                {
                    if (!validacionesServices.ValidarUsuarioContraseniaSandbox(generarFormaPago.User, generarFormaPago.Password))
                    {
                        if (!validacionesServices.ValidarUsuarioContraseniaProduccion(generarFormaPago.User, generarFormaPago.Password))
                        {
                            codigoError.Message = "El usuario y/o contraseña son invalidos.";
                            codigoError.Codigo = "1";

                            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                        }
                        else
                        {
                            UidTipoPagoIntegracion = "D87454C9-12EF-4459-9CED-36E8401E4033";
                            TipoCredenciales = "PRODUCCION";
                        }
                    }
                    else
                    {
                        UidTipoPagoIntegracion = "3F792D20-B3B6-41D3-AF88-1BCB20D99BBE";
                        TipoCredenciales = "SANDBOX";
                    }
                }
                else
                {
                    codigoError.Message = "El usuario y la contraseña son obligatorios.";
                    codigoError.Codigo = "2";

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
                            codigoError.Codigo = "3";

                            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                        }
                    }
                    catch (Exception)
                    {
                        codigoError.Message = "El formato del ID de la integracion es incorrecto.";
                        codigoError.Codigo = "4";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                else
                {
                    codigoError.Message = "El ID de la integracion es obligatorio.";
                    codigoError.Codigo = "5";

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
                            codigoError.Codigo = "6";

                            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                        }
                        else
                        {
                            if (generarFormaPago.SchoolID.Length == 6)
                            {
                                
                            }
                            else
                            {
                                codigoError.Message = "El formato del ID de la escuela es incorrecto.";
                                codigoError.Codigo = "7";

                                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        codigoError.Message = "El formato del ID de la escuela es incorrecto.";
                        codigoError.Codigo = "7";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                else
                {
                    codigoError.Message = "El ID de la escuela es obligatorio.";
                    codigoError.Codigo = "8";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }

                //Validar el ID del comercio
                //if (!string.IsNullOrEmpty(generarFormaPago.BusinessId))
                //{
                //    try
                //    {
                //        if (!validacionesServices.ExisteNegocioSandbox(int.Parse(generarFormaPago.BusinessId)))
                //        {
                //            if (!validacionesServices.ExisteNegocioProduccion(int.Parse(generarFormaPago.BusinessId)))
                //            {
                //                codigoError.Message = "El ID del negocio no existe.";
                //                codigoError.Codigo = "9";

                //                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                //            }
                //        }
                //    }
                //    catch (Exception)
                //    {
                //        codigoError.Message = "El formato del ID del negocio es incorrecto.";
                //        codigoError.Codigo = "10";

                //        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                //    }
                //}
                //else
                //{
                //    codigoError.Message = "El ID del negocio es obligatorio.";
                //    codigoError.Codigo = "11";

                //    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                //}

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
                                codigoError.Codigo = "12";

                                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        codigoError.Message = "El formato del ID de la forma de pago es incorrecto.";
                        codigoError.Codigo = "13";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                else
                {
                    codigoError.Message = "El ID de la forma de pago es obligatorio.";
                    codigoError.Codigo = "14";

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
                        codigoError.Codigo = "15";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                else
                {
                    codigoError.Message = "El ID es obligatorio.";
                    codigoError.Codigo = "16";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }

                //Validar la Descripcion
                if (!string.IsNullOrEmpty(generarFormaPago.Description))
                {

                }
                else
                {
                    codigoError.Message = "La Descripcion es obligatorio.";
                    codigoError.Codigo = "17";

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
                            codigoError.Codigo = "18";

                            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                        }
                    }
                    catch (Exception)
                    {
                        codigoError.Message = "El formato del importe es incorrecto.";
                        codigoError.Codigo = "19";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                else
                {
                    codigoError.Message = "El importe es obligatorio.";
                    codigoError.Codigo = "20";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }

                //Validar referencia
                if (!string.IsNullOrEmpty(generarFormaPago.Reference))
                {
                    try
                    {
                        int Reference = int.Parse(generarFormaPago.Reference);

                        if (generarFormaPago.Reference.Length == 13)
                        {
                            IdReferencia = generarFormaPago.SchoolID + generarFormaPago.Reference + "001";

                            if (validacionesServices.ValidarReferencia(IdReferencia))
                            {
                                codigoError.Message = "La referencia es única e irrepetible.";
                                codigoError.Codigo = "23";

                                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                            }
                        }
                        else
                        {
                            codigoError.Message = "El formato de la referencia es incorrecto.";
                            codigoError.Codigo = "22";

                            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                        }
                    }
                    catch (Exception)
                    {
                        codigoError.Message = "El formato de la referencia es incorrecto.";
                        codigoError.Codigo = "22";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }
                else
                {
                    codigoError.Message = "La referencia es obligatorio.";
                    codigoError.Codigo = "21";

                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                }

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
                            codigoError.Codigo = "24";

                            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                        }
                    }
                    catch (Exception)
                    {
                        codigoError.Message = "El formato de la fecha vencimiento es incorrecto.";
                        codigoError.Codigo = "25";

                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, codigoError);
                    }
                }

                bool PermisoSolicitud = false;

                //Validar si tiene permiso para enviar peticion al servicio praga
                if (validacionesServices.ValidarPermisoSolicitud(Guid.Parse("1981690D-B9FA-43BA-9B97-BF624EBEEC2E"), int.Parse(generarFormaPago.IntegrationID)))
                {
                    //Enviar peticion al servicio praga
                    GenerarLigaPagoIntegraciones generarLigaPagoIntegraciones = new GenerarLigaPagoIntegraciones();

                    string BusinessId = string.Empty;


                    switch (TipoCredenciales)
                    {
                        case "SANDBOX":
                            BusinessId = validacionesServices.ObtenerBusinessIdSandbox();
                            break;
                        case "PRODUCCION":
                            BusinessId = validacionesServices.ObtenerBusinessIdProduccion(int.Parse(generarFormaPago.SchoolID));
                            break;
                    }

                    generarLigaPagoIntegraciones.user = generarFormaPago.User;
                    generarLigaPagoIntegraciones.password = generarFormaPago.Password;
                    generarLigaPagoIntegraciones.integrationID = generarFormaPago.IntegrationID;
                    generarLigaPagoIntegraciones.schoolID = generarFormaPago.SchoolID;
                    generarLigaPagoIntegraciones.description = generarFormaPago.Description;

                    generarLigaPagoIntegraciones.ammount = generarFormaPago.Amount / 100;
                    generarLigaPagoIntegraciones.businessId = BusinessId;
                    generarLigaPagoIntegraciones.effectiveDate = generarFormaPago.ExpirationDate.ToString("dd/MM/yyyy");
                    generarLigaPagoIntegraciones.id = generarFormaPago.Id;
                    generarLigaPagoIntegraciones.paymentTypes = generarFormaPago.PaymentTypes;
                    generarLigaPagoIntegraciones.reference = IdReferencia;
                    generarLigaPagoIntegraciones.referenceEmisor = generarFormaPago.Reference;

                    generarLigaPagoIntegraciones.station = generarFormaPago.IntegrationID;
                    generarLigaPagoIntegraciones.UidTipoPagoIntegracion = UidTipoPagoIntegracion;
                    enviarIntegracionesServices.EnviarPeticionPraga(generarLigaPagoIntegraciones);

                    PermisoSolicitud = true;
                }

                //Validar si tiene permiso para enviar peticion al servicio clubpago
                if (validacionesServices.ValidarPermisoSolicitud(Guid.Parse("D0E8AE95-4EDC-4A8A-A412-87B63B678FB3"), int.Parse(generarFormaPago.IntegrationID)))
                {
                    //Enviar peticion al servicio clubpago
                    GenerarRefereciaPagoIntegraciones generarRefereciaPagoIntegraciones = new GenerarRefereciaPagoIntegraciones();

                    generarRefereciaPagoIntegraciones.User = generarFormaPago.User;
                    generarRefereciaPagoIntegraciones.Password = generarFormaPago.Password;
                    generarRefereciaPagoIntegraciones.IntegrationID = generarFormaPago.IntegrationID;
                    generarRefereciaPagoIntegraciones.SchoolID = generarFormaPago.SchoolID;

                    generarRefereciaPagoIntegraciones.Description = generarFormaPago.Description;
                    generarRefereciaPagoIntegraciones.Amount = generarFormaPago.Amount.ToString();
                    generarRefereciaPagoIntegraciones.Account = IdReferencia;
                    generarRefereciaPagoIntegraciones.ReferenceEmisor = generarFormaPago.Reference;

                    generarRefereciaPagoIntegraciones.CustomerEmail = generarFormaPago.CustomerEmail;
                    generarRefereciaPagoIntegraciones.CustomerName = generarFormaPago.CustomerName;
                    generarRefereciaPagoIntegraciones.ExpirationDate = generarFormaPago.ExpirationDate.ToString("yyyy-MM-dd");
                    generarRefereciaPagoIntegraciones.UidTipoPagoIntegracion = UidTipoPagoIntegracion;
                    enviarIntegracionesServices.EnviarPeticionClubPago(generarRefereciaPagoIntegraciones);

                    PermisoSolicitud = true;
                }

                //Validar que almenos haya un permiso activo
                if (PermisoSolicitud)
                {
                    codigoError.Message = "Success";
                    codigoError.Codigo = "200";

                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, codigoError);
                }
                else
                {
                    codigoError.Message = "No tiene permiso para generar las formas de pago, por favor comuníquese con los administradores.";
                    codigoError.Codigo = "404";

                    return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, codigoError);
                }
            }
            else
            {
                codigoError.Message = "Su cuenta no tiene acceso, por favor comuníquese con los administradores.";
                codigoError.Codigo = "401";

                return Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, codigoError);
            }

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