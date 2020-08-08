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

        public string EstatusWhatsApp(string Telefono)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pw.VchDescripcion from TelefonosUsuarios tu, PermisosWhatsapp pw where tu.UidPermisoWhatsapp = pw.UidPermisoWhatsapp and  tu.VchTelefono = '" + Telefono + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["VchDescripcion"].ToString();
            };

            return result;
        }
        public string ObtenerNombreCliente(Guid UidCliente)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchIdWAySMS from Clientes where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["VchIdWAySMS"].ToString();
            };

            return result;
        }
        public string ObtenerNombreClienteCompleto(Guid UidCliente)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchNombreComercial from Clientes where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["VchNombreComercial"].ToString();
            };

            return result;
        }

        public string ObtenerDatosUsuario(Guid UidUsuario, Guid UidCliente)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.*, cl.IdCliente from Usuarios us, Clientes cl, ClientesUsuarios cu where cu.UidUsuario = us.UidUsuario and cu.UidCliente = cl.UidCliente and us.UidUsuario = '" + UidUsuario + "' and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["IdCliente"].ToString() + "," + item["idUsuario"].ToString();
            };

            return result;
        }

        public bool ExisteMatriculaAlumno(string Matricula, Guid UidCliente)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchMatricula from Alumnos where VchMatricula = '" + Matricula + "' and UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }

        public string ObtenerUidEstatus(string VchDescripcion)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select UidEstatus from Estatus where VchDescripcion = '" + VchDescripcion + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["UidEstatus"].ToString();
            };

            return result;
        }

        public Tuple<bool, string> ExisteAlumno(string Matricula, Guid UidCliente)
        {
            bool result = false;
            string UidAlumno = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Alumnos where VchMatricula = '" + Matricula + "' and UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                foreach (DataRow item in dt.Rows)
                {
                    UidAlumno = item["UidAlumno"].ToString();
                }
                result = true;
            }

            return Tuple.Create(result, UidAlumno);
        }
        public bool ExisteAlumnoAsociado(Guid UidAlumno)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select UidUsuarioAlumno from UsuariosAlumnos where UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
    }
}
