using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.DAO
{
    public class PessoaDAO : DAOConnection
    {

        public Pessoa Read(string login, string senha)
        {   
            Pessoa pessoa = null;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select p.id as Id, p.login as Login, p.senha as Senha, 
                                p.nome as Nome, p.status as Status from pessoas p 
                                where p.login = @login and p.senha = @senha and status = @status"
            };
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@senha", senha);
            cmd.Parameters.AddWithValue("@status", Status.ATIVO.Codigo);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                pessoa = new Pessoa
                {
                    Id = (int)reader["Id"],
                    Nome = (string)reader["Nome"],
                    Login = (string)reader["Login"],
                    Senha = (string)reader["Senha"],
                    Status = Status.getInstance((int)reader["Status"])                    
                };
            }
            reader.Close();

            return pessoa;
        }



        public bool IsAdministrador(int id)
        {
            bool isAdministrador = false;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select pessoa_id as Id from administradores
                                where pessoa_id = @id"
            };
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int administradoId = (int) reader["Id"];

                isAdministrador = administradoId == id;
            }
            reader.Close();

            return isAdministrador;
        }


        public bool IsMotorista(int id)
        {
            bool isMotorista = false;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select pessoa_id as Id from motoristas 
                                where pessoa_id = @id"
            };
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int motoristaId = (int)reader["Id"];

                isMotorista = motoristaId == id;
            }
            reader.Close();

            return isMotorista;
        }


        public bool IsPosto(int id)
        {
            bool isPosto = false;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select pessoa_id as Id from postos 
                                where pessoa_id = @id"
            };
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int postoId = (int)reader["Id"];

                isPosto = postoId == id;
            }
            reader.Close();

            return isPosto;
        }
        
        
        public bool IsLoginExistente(string login)
        {
            bool existe = false;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select login as Login from pessoas
                                where login = @login"
            };
            cmd.Parameters.AddWithValue("@login", login);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string existente = (string) reader["Login"];

                existe = existente.Trim().Equals(login);
            }
            reader.Close();

            return existe;
        }

    }
}
