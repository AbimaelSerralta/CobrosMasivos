using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class DireccionesClientesRepository : SqlDataRepository
    {
        private DireccionesClientes _direccionesClientes = new DireccionesClientes();
        public DireccionesClientes direccionesClientes
        {
            get { return _direccionesClientes; }
            set { _direccionesClientes = value; }
        }

        public DireccionesClientes ObtenerDireccionCliente(Guid UidCliente)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from DireccionesClientes where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                direccionesClientes = new DireccionesClientes()
                {
                    UidDireccionCliente = new Guid(item["UidDireccionCliente"].ToString()),
                    Identificador = item["Identificador"].ToString(),
                    UidPais = new Guid(item["UidPais"].ToString()),
                    UidEstado = new Guid(item["UidEstado"].ToString()),
                    UidMunicipio = new Guid(item["UidMunicipio"].ToString()),
                    UidCiudad = new Guid(item["UidCiudad"].ToString()),
                    UidColonia = new Guid(item["UidColonia"].ToString()),
                    Calle = item["Calle"].ToString(),
                    EntreCalle = item["EntreCalle"].ToString(),
                    YCalle = item["YCalle"].ToString(),
                    NumeroExterior = item["NumeroExterior"].ToString(),
                    NumeroInterior = item["NumeroInterior"].ToString(),
                    CodigoPostal = item["CodigoPostal"].ToString(),
                    Referencia = item["Referencia"].ToString()
                };
            }

            return direccionesClientes;
        }
    }
}
