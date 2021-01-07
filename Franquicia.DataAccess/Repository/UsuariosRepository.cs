using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class UsuariosRepository : SqlDataRepository
    {
        public List<Usuarios> CargarUsuarios()
        {
            List<Usuarios> lsUsuarios = new List<Usuarios>();

            SqlCommand query = new SqlCommand();

            query.CommandText = "Select * from Usuarios";

            //Para StoredProcedure
            //query.CommandType = CommandType.StoredProcedure;
            //Para Consultas slq
            query.CommandType = CommandType.Text;


            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsUsuarios.Add(new Usuarios()
                {
                    UidUsuario = new Guid(item["UidUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString(),
                    StrCorreo = item["VchCorreo"].ToString(),
                    UidEstatus = new Guid(item["UidEstatus"].ToString())
                });
            }

            return lsUsuarios;
        }

        public bool LoginUsuario(string Usuario, string Password)
        {
            bool resultado = false;
            SqlCommand comando = new SqlCommand();

            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "sp_ManejoDeSessionLogearUsuarios";

                comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 20);
                comando.Parameters["@Usuario"].Value = Usuario;

                comando.Parameters.Add("@Password", SqlDbType.VarChar, 32);
                comando.Parameters["@Password"].Value = Password;

                DataTable dt = this.Busquedas(comando);

                foreach (DataRow item in dt.Rows)
                {
                    new Usuarios()
                    {
                        UidUsuario = new Guid(item["UidUsuario"].ToString()),
                        StrNombre = item["VchNombre"].ToString(),
                        StrApePaterno = item["VchApePaterno"].ToString(),
                        StrApeMaterno = item["VchApeMaterno"].ToString(),
                        StrCorreo = item["VchCorreo"].ToString(),
                        UidEstatus = new Guid(item["UidEstatus"].ToString())
                    };
                }
                resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;
        }

        #region PanelTutor
        #region ReporteLigasEscuela
        public List<UsuarioGridViewModel> CargarTutoresAlumnos(Guid UidAlumno)
        {
            List<UsuarioGridViewModel> lsUsuarioGridViewModel = new List<UsuarioGridViewModel>();

            SqlCommand query = new SqlCommand();

            query.CommandText = "select us.* from Usuarios us, UsuariosAlumnos ua, Alumnos al where us.UidUsuario = ua.UidUsuario and ua.UidAlumno = al.UidAlumno and al.UidAlumno = '" + UidAlumno + "'";

            query.CommandType = CommandType.Text;
            
            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsUsuarioGridViewModel.Add(new UsuarioGridViewModel()
                {
                    UidUsuario = new Guid(item["UidUsuario"].ToString()),
                    StrNombre = item["VchNombre"].ToString(),
                    StrApePaterno = item["VchApePaterno"].ToString(),
                    StrApeMaterno = item["VchApeMaterno"].ToString()
                });
            }

            return lsUsuarioGridViewModel;
        }
        #endregion
        #endregion
    }
}
