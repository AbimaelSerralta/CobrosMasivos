using Franquicia.Bussiness;
using Franquicia.Bussiness.IntegracionesClubPago;
using Franquicia.Bussiness.IntegracionesPraga;
using Franquicia.Domain.Models.ClubPago;
using Franquicia.Domain.ViewModels.ClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers.IntegracionesClubPago
{
    public class GenerarReferenciaIndiController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GenerarRefe([FromBody]GenerarFormaPago generarFormaPago)
        {
            PagosIntegracionServices pagosIntegracionServices = new PagosIntegracionServices();
            RefClubPagoServices refClubPagoServices = new RefClubPagoServices();
            GenerarRefereciaClubPago generarRefereciaClubPago = new GenerarRefereciaClubPago();
            EndPointClubPagoServices endPointClubPagoServices = new EndPointClubPagoServices();

            ValidacionesServices validacionesServices = new ValidacionesServices();
            CodigoError codigoError = new CodigoError();

            string TipoCredenciales = string.Empty;
            string IdReferencia = string.Empty;

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

            //Validar si la integracion esta activa
            if (validacionesServices.ValidarEstatusIntegracion(int.Parse(generarFormaPago.IntegrationID)) == Guid.Parse("65E46BC9-1864-4145-AD1A-70F5B5F69739"))
            {
                string UidTipoPagoIntegracion = string.Empty;

                #region Validaciones antes de pedir referencia pago

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

                            if (validacionesServices.ValidarReferenciaClubPago(IdReferencia))
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

                #endregion

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

                //Validar si tiene permiso para enviar peticion al servicio clubpago
                if (validacionesServices.ValidarPermisoSolicitud(Guid.Parse("D0E8AE95-4EDC-4A8A-A412-87B63B678FB3"), int.Parse(generarFormaPago.IntegrationID)))
                {
                    ObtenerRefereciaPago obtenerRefereciaPago = generarRefereciaClubPago.ApiGenerarReferencia(generarRefereciaPagoIntegraciones);

                    if (obtenerRefereciaPago.Message == "Success" || obtenerRefereciaPago.Message == "Exitosa")
                    {
                        DateTime HoraDelServidor = DateTime.Now;
                        DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                        Guid UidPagoIntegracion = Guid.NewGuid();

                        if (pagosIntegracionServices.RegistrarPagosIntegracion(UidPagoIntegracion, int.Parse(generarRefereciaPagoIntegraciones.SchoolID), decimal.Parse(generarRefereciaPagoIntegraciones.Amount) / 100, 0, decimal.Parse(generarRefereciaPagoIntegraciones.Amount) / 100, Guid.Parse("6BE13FFE-E567-4D4D-9CBC-37DA30EC23A5"), Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604"), Guid.Parse(generarRefereciaPagoIntegraciones.UidTipoPagoIntegracion)))
                        {
                            if (refClubPagoServices.RegistrarReferencia(obtenerRefereciaPago.Folio, int.Parse(generarRefereciaPagoIntegraciones.IntegrationID), int.Parse(generarRefereciaPagoIntegraciones.SchoolID), obtenerRefereciaPago.PayFormat, obtenerRefereciaPago.BarCode, generarRefereciaPagoIntegraciones.Description, obtenerRefereciaPago.Reference, generarRefereciaPagoIntegraciones.Account, hoy, DateTime.Parse(generarRefereciaPagoIntegraciones.ExpirationDate), decimal.Parse(generarRefereciaPagoIntegraciones.Amount) / 100, generarRefereciaPagoIntegraciones.CustomerEmail, generarRefereciaPagoIntegraciones.CustomerName, UidPagoIntegracion))
                            {
                                return Request.CreateResponse(System.Net.HttpStatusCode.OK, obtenerRefereciaPago);
                            }
                            else
                            {
                                codigoError.Message = "Error interno del servidor.";
                                codigoError.Codigo = "500";

                                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, codigoError);
                            }
                        }
                        else
                        {
                            codigoError.Message = "Error interno del servidor.";
                            codigoError.Codigo = "500";

                            return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, codigoError);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, obtenerRefereciaPago);
                    }
                }
                else
                {
                    codigoError.Message = "No tiene permiso para generar referencias, por favor comuníquese con los administradores.";
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
    }

    public class GenerarFormaPago
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string IntegrationID { get; set; }
        public string SchoolID { get; set; }
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