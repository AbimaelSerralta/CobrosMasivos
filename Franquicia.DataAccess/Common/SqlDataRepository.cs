using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Common
{
    public class SqlDataRepository
    {
        //private SqlConnection _sqlConnection = new SqlConnection("Data Source=.;Initial Catalog = cobrosmasivosLocal; Integrated Security = True");
        //private SqlConnection _sqlConnection = new SqlConnection("Data Source=192.168.1.73;Initial Catalog=cobrosmasivosLocal;User ID=sa;Password=Serralta");
        //private SqlConnection _sqlConnection = new SqlConnection("Data Source=.;Initial Catalog = cobrosmasivos; Integrated Security = True");
        //private SqlConnection _sqlConnection = new SqlConnection("Data Source=.;Initial Catalog = cobrosmasivosAzure; Integrated Security = True");
        //private SqlConnection _sqlConnection = new SqlConnection("Data Source=.;Initial Catalog = pagalaescuelaAzure; Integrated Security = True");

        //Servicio web
        private SqlConnection _sqlConnection = new SqlConnection("Server=tcp:cobroscontarjeta.database.windows.net,1433;Initial Catalog=cobrosmasivos;Persist Security Info=False;User ID=cobroscontarjeta;Password=NZ#U%PzpHkGa;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
        //private SqlConnection _sqlConnection = new SqlConnection("Server=tcp:cobroscontarjeta.database.windows.net,1433;Initial Catalog=Pagalaescuela;Persist Security Info=False;User ID=cobroscontarjeta;Password=NZ#U%PzpHkGa;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
        //private SqlConnection _sqlConnection = new SqlConnection("Data Source=den1.mssql7.gear.host;Initial Catalog=cobrosmasivos;User ID=cobrosmasivos;Password=Dg64_t119_RE");

        public DataTable Busquedas(SqlCommand SentenciaSQL)
        {
            DataTable Tabla = new DataTable();
            if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Closed)
                _sqlConnection.Open();
            try
            {
                SentenciaSQL.Connection = _sqlConnection;
                SqlDataAdapter Adaptador = new SqlDataAdapter(SentenciaSQL);
                Adaptador.Fill(Tabla);
            }
            catch (Exception E)
            {
                string Error = E.Message;
                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
            return Tabla;
        }

        public DataTable Consultas(string Consulta)
        {
            DataTable Tabla = new DataTable();
            if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Closed)
                _sqlConnection.Open();

            try
            {
                SqlCommand ComandoSQL = new SqlCommand(Consulta, _sqlConnection);
                SqlDataAdapter Adaptador = new SqlDataAdapter(ComandoSQL);
                Adaptador.Fill(Tabla);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }

            return Tabla;
        }

        public int EncontrarCoincidenciasSQL(String query)
        {
            int Coincidencias;
            DataTable dt = new DataTable();
            SqlCommand comando = new SqlCommand(query, _sqlConnection);
            SqlDataAdapter adap = new SqlDataAdapter(comando);
            adap.Fill(dt);
            DataRow row = dt.Rows[0];
            Coincidencias = Convert.ToInt32(row[0]);
            return Coincidencias;
        }

        public bool ExecuteTransaction(System.Collections.Generic.List<SqlCommand> LsCommands)
        {
            if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Closed)
                _sqlConnection.Open();

            SqlTransaction sTransaction = this._sqlConnection.BeginTransaction(); ;
            try
            {

                foreach (SqlCommand cCommand in LsCommands)
                {
                    cCommand.Connection = this._sqlConnection;
                    cCommand.Transaction = sTransaction;
                    cCommand.ExecuteNonQuery();
                    cCommand.Dispose();
                }

                sTransaction.Commit();
                return true;
            }
            catch (Exception)
            {
                sTransaction.Rollback();
                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public bool ManipulacionDeDatos(SqlCommand SentenciaSQL)
        {
            if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Closed)
                _sqlConnection.Open();
            try
            {
                //Se le asigna la conexion
                SentenciaSQL.Connection = _sqlConnection;
                //Se ejecula el comando
                SentenciaSQL.ExecuteNonQuery();
                //Borrar el contenido de la consulta
                SentenciaSQL.Dispose();
                //Retorna verdaro
                return true;
            }
            catch (Exception)
            {
                //Retorna falso
                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}
