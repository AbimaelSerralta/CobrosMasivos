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
    public class FranquiciatariosRepository : SqlDataRepository
    {

        private FranquiciasGridViewModel _franquiciasGridViewModel = new FranquiciasGridViewModel();
        public FranquiciasGridViewModel franquiciasGridViewModel
        {
            get { return _franquiciasGridViewModel; }
            set { _franquiciasGridViewModel = value; }
        }

        public List<FranquiciasGridViewModel> CargarFranquiciatarios()
        {
            List<FranquiciasGridViewModel> lsFranquiciasGridViewModel = new List<FranquiciasGridViewModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select fr.*, es.VchDescripcion, es.VchIcono from Franquiciatarios fr, Estatus es where fr.UidEstatus = es.UidEstatus";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsFranquiciasGridViewModel.Add(new FranquiciasGridViewModel()
                {
                    UidFranquiciatarios = new Guid(item["UidFranquiciatarios"].ToString()),
                    VchRFC = item["VchRFC"].ToString(),
                    VchRazonSocial = item["VchRazonSocial"].ToString(),
                    VchNombreComercial = item["VchNombreComercial"].ToString(),
                    DtFechaAlta = (DateTime) item["DtFechaAlta"],
                    VchCorreoElectronico = item["VchCorreoElectronico"].ToString(),
                    UidEstatus = new Guid(item["UidEstatus"].ToString()),
                    VchEstatus = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString()
                });
            }

            return lsFranquiciasGridViewModel;
        }

        public bool RegistrarFranquiciatarios(Franquiciatarios franquiciatarios, DireccionesFranquiciatarios direccionesFranquiciatarios, TelefonosFranquicias telefonosFranquicias)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_FranquiciatariosRegistrar";

                comando.Parameters.Add("@UidFranquiciatarios", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquiciatarios"].Value = franquiciatarios.UidFranquiciatarios;

                comando.Parameters.Add("@VchRFC", SqlDbType.VarChar, 50);
                comando.Parameters["@VchRFC"].Value = franquiciatarios.VchRFC;

                comando.Parameters.Add("@VchRazonSocial", SqlDbType.VarChar, 50);
                comando.Parameters["@VchRazonSocial"].Value = franquiciatarios.VchRazonSocial;

                comando.Parameters.Add("@VchNombreComercial", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombreComercial"].Value = franquiciatarios.VchNombreComercial;

                comando.Parameters.Add("@DtFechaAlta", SqlDbType.DateTime);
                comando.Parameters["@DtFechaAlta"].Value = franquiciatarios.DtFechaAlta;

                comando.Parameters.Add("@VchCorreoElectronico", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCorreoElectronico"].Value = franquiciatarios.VchCorreoElectronico;

                //===========================DIRECCION==================================================

                comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                comando.Parameters["@Identificador"].Value = direccionesFranquiciatarios.Identificador;

                comando.Parameters.Add("@UidPais", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPais"].Value = direccionesFranquiciatarios.UidPais;

                comando.Parameters.Add("@UidEstado", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstado"].Value = direccionesFranquiciatarios.UidEstado;

                comando.Parameters.Add("@UidMunicipio", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidMunicipio"].Value = direccionesFranquiciatarios.UidMunicipio;

                comando.Parameters.Add("@UidCiudad", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCiudad"].Value = direccionesFranquiciatarios.UidCiudad;
                
                comando.Parameters.Add("@UidColonia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColonia"].Value = direccionesFranquiciatarios.UidColonia;

                comando.Parameters.Add("@Calle", SqlDbType.VarChar, 50);
                comando.Parameters["@Calle"].Value = direccionesFranquiciatarios.Calle;
                
                comando.Parameters.Add("@EntreCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@EntreCalle"].Value = direccionesFranquiciatarios.EntreCalle;

                comando.Parameters.Add("@YCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@YCalle"].Value = direccionesFranquiciatarios.YCalle;

                comando.Parameters.Add("@NumeroExterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroExterior"].Value = direccionesFranquiciatarios.NumeroExterior;

                comando.Parameters.Add("@NumeroInterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroInterior"].Value = direccionesFranquiciatarios.NumeroInterior;

                comando.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 30);
                comando.Parameters["@CodigoPostal"].Value = direccionesFranquiciatarios.CodigoPostal;

                comando.Parameters.Add("@Referencia", SqlDbType.VarChar, 100);
                comando.Parameters["@Referencia"].Value = direccionesFranquiciatarios.Referencia;
                
                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosFranquicias.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosFranquicias.UidTipoTelefono;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        
        public bool ActualizarFranquiciatarios(Franquiciatarios franquiciatarios, DireccionesFranquiciatarios direccionesFranquiciatarios, TelefonosFranquicias telefonosFranquicias)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_FranquiciatariosActualizar";

                comando.Parameters.Add("@UidFranquiciatarios", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidFranquiciatarios"].Value = franquiciatarios.UidFranquiciatarios;

                comando.Parameters.Add("@VchRFC", SqlDbType.VarChar, 50);
                comando.Parameters["@VchRFC"].Value = franquiciatarios.VchRFC;

                comando.Parameters.Add("@VchRazonSocial", SqlDbType.VarChar, 50);
                comando.Parameters["@VchRazonSocial"].Value = franquiciatarios.VchRazonSocial;

                comando.Parameters.Add("@VchNombreComercial", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNombreComercial"].Value = franquiciatarios.VchNombreComercial;

                comando.Parameters.Add("@VchCorreoElectronico", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCorreoElectronico"].Value = franquiciatarios.VchCorreoElectronico;

                comando.Parameters.Add("@UidEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatus"].Value = franquiciatarios.UidEstatus;

                //===========================DIRECCION==================================================

                comando.Parameters.Add("@Identificador", SqlDbType.VarChar, 50);
                comando.Parameters["@Identificador"].Value = direccionesFranquiciatarios.Identificador;

                comando.Parameters.Add("@UidPais", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPais"].Value = direccionesFranquiciatarios.UidPais;

                comando.Parameters.Add("@UidEstado", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstado"].Value = direccionesFranquiciatarios.UidEstado;

                comando.Parameters.Add("@UidMunicipio", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidMunicipio"].Value = direccionesFranquiciatarios.UidMunicipio;

                comando.Parameters.Add("@UidCiudad", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCiudad"].Value = direccionesFranquiciatarios.UidCiudad;
                
                comando.Parameters.Add("@UidColonia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidColonia"].Value = direccionesFranquiciatarios.UidColonia;

                comando.Parameters.Add("@Calle", SqlDbType.VarChar, 50);
                comando.Parameters["@Calle"].Value = direccionesFranquiciatarios.Calle;
                
                comando.Parameters.Add("@EntreCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@EntreCalle"].Value = direccionesFranquiciatarios.EntreCalle;

                comando.Parameters.Add("@YCalle", SqlDbType.VarChar, 50);
                comando.Parameters["@YCalle"].Value = direccionesFranquiciatarios.YCalle;

                comando.Parameters.Add("@NumeroExterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroExterior"].Value = direccionesFranquiciatarios.NumeroExterior;

                comando.Parameters.Add("@NumeroInterior", SqlDbType.VarChar, 50);
                comando.Parameters["@NumeroInterior"].Value = direccionesFranquiciatarios.NumeroInterior;

                comando.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 30);
                comando.Parameters["@CodigoPostal"].Value = direccionesFranquiciatarios.CodigoPostal;

                comando.Parameters.Add("@Referencia", SqlDbType.VarChar, 100);
                comando.Parameters["@Referencia"].Value = direccionesFranquiciatarios.Referencia;
                
                //===========================TELEFONO==================================================

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchTelefono"].Value = telefonosFranquicias.VchTelefono;

                comando.Parameters.Add("@UidTipoTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTelefono"].Value = telefonosFranquicias.UidTipoTelefono;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        #region AdminFranquicias
        public void ObtenerFranquicia(Guid UidAdministrador)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select fr.*, es.VchDescripcion, es.VchIcono from Franquiciatarios fr, Estatus es, FranquiciasUsuarios fu, Usuarios us where fu.UidUsuario = us.UidUsuario and fr.UidFranquiciatarios = fu.UidFranquicia and fr.UidEstatus = es.UidEstatus and us.UidUsuario = '"+ UidAdministrador +"'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                franquiciasGridViewModel = new FranquiciasGridViewModel()
                {
                    UidFranquiciatarios = new Guid(item["UidFranquiciatarios"].ToString()),
                    VchRFC = item["VchRFC"].ToString(),
                    VchRazonSocial = item["VchRazonSocial"].ToString(),
                    VchNombreComercial = item["VchNombreComercial"].ToString()
                };
            }
        }
        #endregion
    }
}
