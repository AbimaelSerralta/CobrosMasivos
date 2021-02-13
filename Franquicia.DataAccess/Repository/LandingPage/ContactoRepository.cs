using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.LandingPage
{
    public class ContactoRepository: SqlDataRepository
    {
        public bool Registrar(Guid UidPosibleCliente, string VchInstituto,
            string VchContactoPropietario, string VchCorreoElectronico,
            string VchNoTelefono, int IdFranquicia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ContactoRegistrar";

                comando.Parameters.Add("@UidPosibleCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPosibleCliente"].Value = UidPosibleCliente;

                comando.Parameters.Add("@VchInstituto", SqlDbType.VarChar, 150);
                comando.Parameters["@VchInstituto"].Value = VchInstituto;

                comando.Parameters.Add("@VchContactoPropietario", SqlDbType.VarChar, 50);
                comando.Parameters["@VchContactoPropietario"].Value = VchContactoPropietario;

                comando.Parameters.Add("@VchCorreoElectronico", SqlDbType.VarChar, 100);
                comando.Parameters["@VchCorreoElectronico"].Value = VchCorreoElectronico;

                comando.Parameters.Add("@VchNoTelefono", SqlDbType.VarChar, 50);
                comando.Parameters["@VchNoTelefono"].Value = VchNoTelefono;

                comando.Parameters.Add("@IdFranquicia", SqlDbType.Int);
                comando.Parameters["@IdFranquicia"].Value = IdFranquicia;
                
                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public bool RegistrarCita(Guid UidPosibleCliente, string VchFecha,
          string VchHora)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ContactoCitaRegistrar";

                comando.Parameters.Add("@UidPosibleCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPosibleCliente"].Value = UidPosibleCliente;

                comando.Parameters.Add("@VchFecha", SqlDbType.VarChar, 150);
                comando.Parameters["@VchFecha"].Value = VchFecha;

                comando.Parameters.Add("@VchHora", SqlDbType.VarChar, 50);
                comando.Parameters["@VchHora"].Value = VchHora;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public string ObtenerCorreoFranquiciatario(int IdFranquicia)
        {
            string Resultado = "";
            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ObtenerCorreoFranquiciatario"; 
                
                comando.Parameters.Add("@IdFranquicia", SqlDbType.Int);
                comando.Parameters["@IdFranquicia"].Value = IdFranquicia;


                DataTable dt = this.Busquedas(comando);
                foreach (DataRow item in dt.Rows)
                {
                    Resultado = item["VchCorreo"].ToString();
                }
             }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public string ValidarFranquiciatario(int IdFranquicia)
        {
            string Resultado = "";
            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ValidarFranquiciatario"; 
                
                comando.Parameters.Add("@IdFranquicia", SqlDbType.Int);
                comando.Parameters["@IdFranquicia"].Value = IdFranquicia;


                DataTable dt = this.Busquedas(comando);
                foreach (DataRow item in dt.Rows)
                {
                    Resultado = item["IsBTech"].ToString();
                }
             }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

    }

}
