using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;
using System.Data;

namespace UaiFuel.Models.DAO
{
    public abstract class DAOConnection : IDisposable
    {   
        private string strConexao = @"Data Source = localhost;
                            Initial Catalog = uaifuel;
                            Integrated Security = false;
                            User Id = sa;
                            Password = dba";
        protected SqlConnection connectionDB;

        protected DAOConnection()
        {   
        }

        protected SqlConnection GetConnection()
        {
            if (connectionDB == null)
            {
                try
                {
                    connectionDB = new SqlConnection(strConexao);
                    connectionDB.Open();
                }
                catch (SqlException exp)
                {
                    Console.WriteLine("Erro ao abrir conexão com o banco dados.\nErro=>\n" + exp);
                }
            }
            else if (connectionDB.State == ConnectionState.Open)
            {
                return connectionDB;
            }
            else if( connectionDB.State == ConnectionState.Broken)
            {
                connectionDB.Close();
                try
                {
                    //connectionDB = new SqlConnection(strConexao);
                    connectionDB.Open();
                }
                catch (SqlException exp)
                {
                    Console.WriteLine("Erro ao instanciar a conexão.\nErro=>\n" + exp);
                }
            }
            return this.connectionDB;
            
        }

        public void Dispose()
        {
            connectionDB.Close();
        }


    }
}




