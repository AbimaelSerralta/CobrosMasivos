using Franquicia.DataAccess.Repository;
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
        #endregion
    }
}
