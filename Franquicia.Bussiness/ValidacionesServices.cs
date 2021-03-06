﻿using Franquicia.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Franquicia.Bussiness
{
    public class ValidacionesServices
    {
        private ValidacionesRepository _validacionesServices = new ValidacionesRepository();
        public ValidacionesRepository validacionesRepository
        {
            get { return _validacionesServices; }
            set { _validacionesServices = value; }
        }


        #region Metodos Genericos
        public bool TieneArroba(string arroba)
        {
            bool resultado = false;

            if (arroba.Contains("@"))
            {
                resultado = true;
            }
            return resultado;
        }

        public bool isEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public bool isUrl(string inputEmail)
        {
            string strRegex = @"^https?:\/\/[\w\-]+(\.[\w\-]+)+[/#?]?.*$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public bool VerificarPalabra(string CadenaTexto, string VerificarDato)
        {
            bool resultado = false;

            if (CadenaTexto.Contains(VerificarDato))
            {
                resultado = true;
            }
            return resultado;
        }

        public bool IsNumeric(string str)
        {
            double _val;
            bool valor = double.TryParse(str, out _val);
            return valor;
        }

        public bool IsLetter(KeyPressEventArgs e)
        {
            return (e.KeyChar >= 65 && e.KeyChar <= 90) ||
                    (e.KeyChar >= 97 && e.KeyChar <= 122) ||
                    e.KeyChar == 8 || e.KeyChar == 'Ñ'
                    || e.KeyChar == 'ñ' || e.KeyChar == 32;
        }

        public bool ExisteCorreo(string Correo)
        {
            return validacionesRepository.ExisteCorreo(Correo);
        }

        public bool ExisteUsuario(string Usuario)
        {
            return validacionesRepository.ExisteUsuario(Usuario);
        }

        public bool ExisteUsuarioFranquicia(Guid UidFranquicia, Guid UidUsuario)
        {
            return validacionesRepository.ExisteUsuarioFranquicia(UidFranquicia, UidUsuario);
        }

        public bool ExisteUsuarioCliente(Guid UidCliente, Guid UidUsuario)
        {
            return validacionesRepository.ExisteUsuarioCliente(UidCliente, UidUsuario);
        }

        public bool ExisteDireccionUsuario(Guid UidUsuario)
        {
            return validacionesRepository.ExisteDireccionUsuario(UidUsuario);
        }

        public bool LigaAsociadoPagado(Guid UidLigaAsociado)
        {
            return validacionesRepository.LigaAsociadoPagado(UidLigaAsociado);
        }
        public bool ValidarPagoCliente(string IdReferencia)
        {
            return validacionesRepository.ValidarPagoCliente(IdReferencia);
        }
        public bool ValidarPagoClientePayCard(string IdReferencia)
        {
            return validacionesRepository.ValidarPagoClientePayCard(IdReferencia);
        }
        public bool ExisteCuentaDineroCliente(Guid UidCliente)
        {
            return validacionesRepository.ExisteCuentaDineroCliente(UidCliente);
        }

        public string EstatusWhatsApp(string Telefono)
        {
            return validacionesRepository.EstatusWhatsApp(Telefono);
        }
        public string ObtenerNombreCliente(Guid UidCliente)
        {
            return validacionesRepository.ObtenerNombreCliente(UidCliente);
        }
        public string ObtenerNombreClienteCompleto(Guid UidCliente)
        {
            return validacionesRepository.ObtenerNombreClienteCompleto(UidCliente);
        }
        public string ObtenerDatosUsuario(Guid UidUsuario, Guid UidCliente)
        {
            return validacionesRepository.ObtenerDatosUsuario(UidUsuario, UidCliente);
        }

        public string EstatusCuentaPadre(Guid UidUsuario)
        {
            return validacionesRepository.EstatusCuentaPadre(UidUsuario);
        }

        public Tuple<string, string, string> Creden(Guid UidUsuario, Guid UidCliente)
        {
            return validacionesRepository.Creden(UidUsuario, UidCliente);
        }

        public bool ExisteMatricula(string Matricula)
        {
            return validacionesRepository.ExisteMatricula(Matricula);
        }

        public string ObtenerCorreoUsuario(Guid UidUsuario)
        {
            return validacionesRepository.ObtenerCorreoUsuario(UidUsuario);
        }

        public Tuple<bool, DateTime> UsarFechaPagoCole(Guid UidPagoColegiatura, string VchMatricula)
        {
            return validacionesRepository.UsarFechaPagoCole(UidPagoColegiatura, VchMatricula);
        }

        public bool TienePagosTarjeta(string IdReferencia)
        {
            return validacionesRepository.TienePagosTarjeta(IdReferencia);
        }
        public bool TienePagosTarjetaPraga(string IdReferencia)
        {
            return validacionesRepository.TienePagosTarjetaPraga(IdReferencia);
        }
        #endregion

        #region Metodos Integraciones
        #region Validaciones Integraciones
        public Guid ValidarEstatusIntegracion(int IdIntegracion)
        {
            return validacionesRepository.ValidarEstatusIntegracion(IdIntegracion);
        }
        public Guid ValidarEstatusCredencialesSandbox(string Usuario, string Contrasenia)
        {
            return validacionesRepository.ValidarEstatusCredencialesSandbox(Usuario, Contrasenia);
        }
        public Guid ValidarEstatusCredencialesProduccion(string Usuario, string Contrasenia)
        {
            return validacionesRepository.ValidarEstatusCredencialesProduccion(Usuario, Contrasenia);
        }
        public bool ValidarUsuarioContraseniaSandbox(string Usuario, string Contrasenia)
        {
            return validacionesRepository.ValidarUsuarioContraseniaSandbox(Usuario, Contrasenia);
        }
        public bool ValidarUsuarioContraseniaProduccion(string Usuario, string Contrasenia)
        {
            return validacionesRepository.ValidarUsuarioContraseniaProduccion(Usuario, Contrasenia);
        }
        public bool ExisteIntegracion(int IdIntegracion)
        {
            return validacionesRepository.ExisteIntegracion(IdIntegracion);
        }
        public bool ExisteEscuela(int IdEscuela)
        {
            return validacionesRepository.ExisteEscuela(IdEscuela);
        }
        public bool ExisteNegocioSandbox(int IdNegocio)
        {
            return validacionesRepository.ExisteNegocioSandbox(IdNegocio);
        }
        public bool ExisteNegocioProduccion(int IdNegocio)
        {
            return validacionesRepository.ExisteNegocioProduccion(IdNegocio);
        }
        public bool ExisteIdPromocionSandbox(int IdPromocion)
        {
            return validacionesRepository.ExisteIdPromocionSandbox(IdPromocion);
        }
        public bool ExisteIdPromocionProduccion(int IdPromocion)
        {
            return validacionesRepository.ExisteIdPromocionProduccion(IdPromocion);
        }
        public Tuple<bool, Guid> ExisteReferenciaIntegracion(string IdReferencia)
        {
            return validacionesRepository.ExisteReferenciaIntegracion(IdReferencia);
        }
        public string ObtenerBusinessIdSandbox()
        {
            return validacionesRepository.ObtenerBusinessIdSandbox();
        }
        public string ObtenerBusinessIdProduccion(int IdCliente)
        {
            return validacionesRepository.ObtenerBusinessIdProduccion(IdCliente);
        }
        public bool ValidarPermisoSolicitud(Guid UidSegModulo, int IdIntegracion)
        {
            return validacionesRepository.ValidarPermisoSolicitud(UidSegModulo, IdIntegracion);
        }
        public bool ValidarReferencia(string IdReferencia)
        {
            return validacionesRepository.ValidarReferencia(IdReferencia);
        }

        public bool ValidarReferenciaClubPago(string IdReferencia)
        {
            return validacionesRepository.ValidarReferenciaClubPago(IdReferencia);
        }
        public bool ValidarReferenciaPraga(string IdReferencia)
        {
            return validacionesRepository.ValidarReferenciaPraga(IdReferencia);
        }
        #endregion

        #region CheckRefence
        public Tuple<bool, Guid> ExisteReferenciaIntegracionCF(string IdReferencia, Guid UidIntegracion)
        {
            return validacionesRepository.ExisteReferenciaIntegracionCF(IdReferencia, UidIntegracion);
        }
        #endregion

        #region EndPoint
        public bool ValidarPermisoMenu(Guid UidSegModulo, Guid UidIntegracion)
        {
            return validacionesRepository.ValidarPermisoMenu(UidSegModulo, UidIntegracion);
        }
        #endregion
        #endregion
    }
}
