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
    public class ClientesServices
    {
        private Clientes _clientes = new Clientes();
        public Clientes clientes
        {
            get { return _clientes; }
            set { _clientes = value; }
        }

        private ClientesRepository _clientesRepository = new ClientesRepository();
        public ClientesRepository clientesRepository
        {
            get { return _clientesRepository; }
            set { _clientesRepository = value; }
        }

        public List<Clientes> lsClientes = new List<Clientes>();
        public List<ClientesGridViewModel> lsClientesGridViewModel = new List<ClientesGridViewModel>();

        public List<ClientesGridViewEmpresasModel> lsClientesGridViewEmpresasModel = new List<ClientesGridViewEmpresasModel>();

        public List<ClientesGridViewModel> CargarClientes(Guid UidFranquiciatario)
        {
            return lsClientesGridViewModel = clientesRepository.CargarClientes(UidFranquiciatario);
        }

        public void ObtenerCliente(Guid UidCliente)
        {
            clientes = new Clientes();
            clientes = lsClientesGridViewModel.Find(x => x.UidCliente == UidCliente);
        }

        public bool RegistrarClientes(
            string Rfc, string RazonSocial, string NombreComercial, DateTime FechaAlta, string Correo, string IdWAySMS,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidFranquiciatario)
        {
            Guid UidCliente = Guid.NewGuid();

            bool result = false;
            if (clientesRepository.RegistrarClientes(
                new Clientes
                {
                    UidCliente = UidCliente,
                    VchRFC = Rfc,
                    VchRazonSocial = RazonSocial,
                    VchNombreComercial = NombreComercial,
                    DtFechaAlta = FechaAlta,
                    VchCorreoElectronico = Correo,
                    VchIdWAySMS = IdWAySMS,
                    UidFranquiciatario = UidFranquiciatario
                }, 
                new DireccionesClientes 
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
                new TelefonosClientes
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

        public bool ActualizarClientes(
            Guid UidCliente, string Rfc, string RazonSocial, string NombreComercial, string Correo, Guid UidEstatus, string IdWAySMS,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono)
        {

            bool result = false;
            if (clientesRepository.ActualizarClientes(
                new Clientes
                {
                    UidCliente = UidCliente,
                    VchRFC = Rfc,
                    VchRazonSocial = RazonSocial,
                    VchNombreComercial = NombreComercial,
                    VchCorreoElectronico = Correo,
                    UidEstatus = UidEstatus,
                    VchIdWAySMS = IdWAySMS
                },
                new DireccionesClientes
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
                new TelefonosClientes
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

        #region AdminGeneral
        public List<ClientesGridViewEmpresasModel> CargarTodosClientes()
        {
            return lsClientesGridViewEmpresasModel = clientesRepository.CargarTodosClientes();
        }
        #endregion

        #region AdminCliente
        public void ObtenerClientes(Guid UidAdministrador)
        {
            clientesRepository.ObtenerClientes(UidAdministrador);
        }
        #endregion
    }
}
