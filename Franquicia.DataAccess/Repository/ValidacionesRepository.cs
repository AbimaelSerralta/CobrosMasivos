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
        #region Metodos Genericos       
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
        public bool EsMiAlumno(Guid UidAlumno, string VchCorreo, Guid UidCliente)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Usuarios us, UsuariosAlumnos ua, Alumnos al, Clientes cl where us.UidUsuario = ua.UidUsuario and al.UidAlumno = ua.UidAlumno and al.UidCliente = cl.UidCliente and al.UidAlumno = '" + UidAlumno + "' and us.VchCorreo = '" + VchCorreo + "' and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public Tuple<bool, string> ExisteAlumnoAsociado(Guid UidAlumno, Guid UidCliente)
        {
            bool result = false;
            string correo = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //query.CommandText = "select UidUsuarioAlumno from UsuariosAlumnos where UidAlumno = '" + UidAlumno + "'";
            query.CommandText = "select us.VchCorreo from Usuarios us, UsuariosAlumnos ua, Alumnos al, Clientes cl where us.UidUsuario = ua.UidUsuario and al.UidAlumno = ua.UidAlumno and al.UidCliente = cl.UidCliente and al.UidAlumno = '" + UidAlumno + "' and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = true;
                correo = item["VchCorreo"].ToString();
            }

            return Tuple.Create(result, correo);
        }

        public string EstatusCuentaPadre(Guid UidUsuario)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select UidEstatusCuenta from SegUsuarios where UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["UidEstatusCuenta"].ToString();
            };

            return result;
        }

        public Tuple<string, string, string> Creden(Guid UidUsuario, Guid UidCliente)
        {
            string Email = string.Empty;
            string Pass = string.Empty;
            string Comercio = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select us.VchCorreo, su.VchContrasenia, cl.VchIdWAySMS from SegUsuarios su, Usuarios us, Clientes cl, ClientesUsuarios cu where cl.UidCliente = cu.UidCliente and cu.UidUsuario = us.UidUsuario and us.UidUsuario = su.UidUsuario and us.UidUsuario = '" + UidUsuario + "' and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Email = item["VchCorreo"].ToString();
                    Pass = item["VchContrasenia"].ToString();
                    Comercio = item["VchIdWAySMS"].ToString();
                }
            }

            return Tuple.Create(Email, Pass, Comercio);
        }

        public bool ExisteMatricula(string Matricula)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchMatricula from Alumnos where VchMatricula = '" + Matricula + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }

        public string ObtenerCorreoUsuario(Guid UidUsuario)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchCorreo from Usuarios where UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["VchCorreo"].ToString();
            };

            return result;
        }

        #endregion

        #region Metodos Integraciones
        #region Validaciones Integraciones
        public Guid ValidarEstatusIntegracion(int IdIntegracion)
        {
            Guid UidEstatus = Guid.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Integraciones where IdIntegracion = '" + IdIntegracion + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                UidEstatus = Guid.Parse(item["UidEstatus"].ToString());
            }

            return UidEstatus;
        }
        public bool ValidarUsuarioContraseniaSandbox(string Usuario, string Contrasenia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from CredenSandbox where VchUsuario = '" + Usuario + "' and VchContrasenia = '" + Contrasenia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ValidarUsuarioContraseniaProduccion(string Usuario, string Contrasenia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from CredenProduccion where VchUsuario = '" + Usuario + "' and VchContrasenia = '" + Contrasenia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteIntegracion(int IdIntegracion)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Integraciones where IdIntegracion = '" + IdIntegracion + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteEscuela(int IdEscuela)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Clientes where IdCliente = '" + IdEscuela + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteNegocioSandbox(int IdNegocio)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ParametrosPragaIntegracion where BusinessId = '" + IdNegocio + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteNegocioProduccion(int IdNegocio)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ParametrosPraga where BusinessId = '" + IdNegocio + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteIdPromocionSandbox(int IdPromocion)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from CodigoPromocionesPragaSandbox where VchCodigo = '" + IdPromocion + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ExisteIdPromocionProduccion(int IdPromocion)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from CodigoPromocionesPragaProduccion where VchCodigo = '" + IdPromocion + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public Tuple<bool, Guid> ExisteReferenciaIntegracion(string IdReferencia)
        {
            bool result = false;
            Guid UidIntegracion = Guid.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select inte.UidIntegracion from Integraciones inte, RefClubPago rcp where inte.IdIntegracion = rcp.IdIntegracion and rcp.IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                foreach (DataRow item in dt.Rows)
                {
                    UidIntegracion = Guid.Parse(item["UidIntegracion"].ToString());
                }
                result = true;
            }

            return Tuple.Create(result, UidIntegracion);
        }
        public string ObtenerBusinessIdSandbox()
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select BusinessId from ParametrosPragaIntegracion ";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["BusinessId"].ToString();
            };

            return result;
        }
        public string ObtenerBusinessIdProduccion(int IdCliente)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pp.BusinessId from ParametrosPraga pp, Clientes cl where pp.UidPropietario = cl.UidCliente and cl.IdCliente = '" + IdCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["BusinessId"].ToString();
            };

            return result;
        }
        public bool ValidarPermisoSolicitud(Guid UidSegModulo, int IdIntegracion)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select ai.* from SegModulosIntegraciones smi, AccesosIntegraciones ai, Integraciones inte where smi.UidSegModulo = ai.UidSegModulo and ai.UidIntegracion = inte.UidIntegracion and smi.UidSegModulo = '" + UidSegModulo + "' and inte.IdIntegracion = '" + IdIntegracion + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ValidarReferencia(string IdReferencia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchCuenta as IdReferencia from RefClubPago where VchCuenta = '" + IdReferencia + "' union select IdReferencia from LigasUrlsPragaIntegracion where IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }

        public bool ValidarReferenciaClubPago(string IdReferencia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchCuenta as IdReferencia from RefClubPago where VchCuenta = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        public bool ValidarReferenciaPraga(string IdReferencia)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select IdReferencia from LigasUrlsPragaIntegracion where IdReferencia = '" + IdReferencia + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        #endregion

        #region CheckRefence
        public Tuple<bool, Guid> ExisteReferenciaIntegracionCF(string IdReferencia, Guid UidIntegracion)
        {
            bool result = false;
            Guid UidIntegraci = Guid.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select inte.UidIntegracion from Integraciones inte, RefClubPago rcp where inte.IdIntegracion = rcp.IdIntegracion and rcp.IdReferencia = '" + IdReferencia + "' and inte.UidIntegracion = '" + UidIntegracion + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                foreach (DataRow item in dt.Rows)
                {
                    UidIntegraci = Guid.Parse(item["UidIntegracion"].ToString());
                }
                result = true;
            }

            return Tuple.Create(result, UidIntegraci);
        }
        #endregion

        #region EndPoint
        public bool ValidarPermisoMenu(Guid UidSegModulo, Guid UidIntegracion)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select ai.* from SegModulosIntegraciones smi, AccesosIntegraciones ai, Integraciones inte where smi.UidSegModulo = ai.UidSegModulo and ai.UidIntegracion = inte.UidIntegracion and smi.UidSegModulo = '" + UidSegModulo + "' and inte.UidIntegracion = '" + UidIntegracion + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
        #endregion

        #endregion
    }
}
