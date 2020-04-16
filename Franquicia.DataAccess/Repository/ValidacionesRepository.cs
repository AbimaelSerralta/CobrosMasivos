using Franquicia.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class ValidacionesRepository : SqlDataRepository
    {
        public bool ExisteCorreo(string Correo)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchCorreo from Usuarios where VchCorreo = '" + Correo + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }

        public bool ExisteUsuario(string Usuario)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchUsuario from SegUsuarios where VchUsuario = '" + Usuario + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteUsuarioFranquicia(Guid UidFranquicia, Guid Usuario)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from FranquiciasUsuarios where UidFranquicia = '" + UidFranquicia + "' and  UidUsuario = '" + Usuario + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteUsuarioCliente(Guid UidCliente, Guid Usuario)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ClientesUsuarios where UidCliente = '" + UidCliente + "' and  UidUsuario = '" + Usuario + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteDireccionUsuario(Guid Usuario)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from DireccionesUsuarios where UidUsuario = '" + Usuario + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool LigaAsociadoPagado(Guid UidLigaAsociado)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select lu.* from LigasUrls lu, PagosTarjeta pt where lu.IdReferencia = pt.IdReferencia and VchEstatus = 'approved' and UidLigaAsociado = '" + UidLigaAsociado + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ValidarPagoCliente(string IdReferencia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from AuxiliarPago where IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ValidarPagoClientePayCard(string IdReferencia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from AuxiliarPago where IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteCuentaDineroCliente(Guid UidCliente)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ClienteCuenta where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
    }
}
