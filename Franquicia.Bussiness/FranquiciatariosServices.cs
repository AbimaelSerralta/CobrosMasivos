using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class FranquiciatariosServices
    {
        private Franquiciatarios _franquiciatarios = new Franquiciatarios();
        public Franquiciatarios franquiciatarios
        {
            get { return _franquiciatarios; }
            set { _franquiciatarios = value; }
        }

        private FranquiciatariosRepository _franquiciatariosRepository = new FranquiciatariosRepository();
        public FranquiciatariosRepository franquiciatariosRepository
        {
            get { return _franquiciatariosRepository; }
            set { _franquiciatariosRepository = value; }
        }


        public List<Franquiciatarios> lsFranquiciatarios = new List<Franquiciatarios>();
        public List<FranquiciasGridViewModel> lsFranquiciasGridViewModel = new List<FranquiciasGridViewModel>();

        public List<FranquiciasGridViewModel> CargarFranquiciatarios()
        {
            return lsFranquiciasGridViewModel = franquiciatariosRepository.CargarFranquiciatarios();
        }

        public void ObtenerFraquiciatario(Guid UidFranquiciatario)
        {
            franquiciatarios = new Franquiciatarios();
            franquiciatarios = lsFranquiciasGridViewModel.Find(x => x.UidFranquiciatarios == UidFranquiciatario);
        }

        public bool RegistrarFranquiciatarios(
            string Rfc, string RazonSocial, string NombreComercial, DateTime FechaAlta, string Correo,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono)
        {
            Guid UidFranquiciatario = Guid.NewGuid();

            bool result = false;
            if (franquiciatariosRepository.RegistrarFranquiciatarios(
                new Franquiciatarios
                {
                    UidFranquiciatarios = UidFranquiciatario,
                    VchRFC = Rfc,
                    VchRazonSocial = RazonSocial,
                    VchNombreComercial = NombreComercial,
                    DtFechaAlta = FechaAlta,
                    VchCorreoElectronico = Correo
                }, 
                new DireccionesFranquiciatarios 
                {
                    Identificador = Identificador, 
                    UidPais = UidPais, 
                    UidEstado = UidEstado, 
                    UidMunicipio = Municipio, 
                    UidCiudad = UidCiudad, 
                    UidColonia = UidColonia, 
                    Calle = Calle, 
                    EntreCalle = EntreCalle, 
                    YCalle = YCalle, 
                    NumeroExterior = NumeroExterior, 
                    NumeroInterior = NumeroInterior, 
                    CodigoPostal = CodigoPostal, 
                    Referencia = Referencia
                }, 
                new TelefonosFranquicias 
                {
                    VchTelefono = Telefono,  
                    UidTipoTelefono = UidTipoTelefono
                }
                
                ))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarFranquiciatarios(
            Guid UidFranquiciatario, string Rfc, string RazonSocial, string NombreComercial, string Correo, Guid UidEstatus,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono)
        {

            bool result = false;
            if (franquiciatariosRepository.ActualizarFranquiciatarios(
                new Franquiciatarios
                {
                    UidFranquiciatarios = UidFranquiciatario,
                    VchRFC = Rfc,
                    VchRazonSocial = RazonSocial,
                    VchNombreComercial = NombreComercial,
                    VchCorreoElectronico = Correo,
                    UidEstatus = UidEstatus
                },
                new DireccionesFranquiciatarios
                {
                    Identificador = Identificador,
                    UidPais = UidPais,
                    UidEstado = UidEstado,
                    UidMunicipio = Municipio,
                    UidCiudad = UidCiudad,
                    UidColonia = UidColonia,
                    Calle = Calle,
                    EntreCalle = EntreCalle,
                    YCalle = YCalle,
                    NumeroExterior = NumeroExterior,
                    NumeroInterior = NumeroInterior,
                    CodigoPostal = CodigoPostal,
                    Referencia = Referencia
                },
                new TelefonosFranquicias
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                }

                ))
            {
                result = true;
            }
            return result;
        }

        public void BuscarFranquiciatarios(string RFC, string RazonSocial, string NombreComercial, Guid UidEstatus)
        {
            lsFranquiciasGridViewModel = franquiciatariosRepository.BuscarFranquiciatarios(RFC, RazonSocial, NombreComercial, UidEstatus);
        }

        #region AdminFranquicias
        public void ObtenerFranquicia(Guid UidAdministrador)
        {
            franquiciatariosRepository.ObtenerFranquicia(UidAdministrador);
        }
        #endregion
    }
}
