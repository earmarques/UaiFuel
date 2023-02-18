using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.DAO
{
    class CombustivelDAO : DAOConnection, IDAO<Combustivel>
    {

        public Combustivel Create(Combustivel domainObject) 
        {
            Combustivel combustivel = domainObject;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();

            // Preparando o texto de inserção
            cmd.CommandText = @"insert into combustiveis values (@postoId, @nome, @status); 
                                select @@IDENTITY";
            cmd.Parameters.AddWithValue("@postoId", combustivel.Posto.Id);
            cmd.Parameters.AddWithValue("@nome", combustivel.Nome);
            cmd.Parameters.AddWithValue("@status", combustivel.Status.Codigo);

            // Insere e retorna o último valor de identidade inserido
            int id = Convert.ToInt32(cmd.ExecuteScalar());

            combustivel.Id = id;

            return combustivel;
        }


        public void Update(Combustivel domainObject) 
        {
            Combustivel combustivel = domainObject;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"update combustiveis set nome = @nome, status = @status 
                                where id = @id";
            cmd.Parameters.AddWithValue("@id", combustivel.Id);            
            cmd.Parameters.AddWithValue("@nome", combustivel.Nome);
            cmd.Parameters.AddWithValue("@status", combustivel.Status.Codigo);
            cmd.ExecuteNonQuery();
        }


        public void Delete(Combustivel domainObject) 
        {
            Combustivel combustivel = domainObject;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"delete from combustiveis where id = @id";
            cmd.Parameters.AddWithValue("@id", combustivel.Id);
            
            cmd.ExecuteNonQuery();
        }


        public IList<Combustivel> Read() 
        {
            IList<Combustivel> lista = new List<Combustivel>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = "select c.posto_id as postoId, id as Id, nome as Nome, status as Status " +
                              "from combustiveis c order by c.posto_id, c.nome";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Combustivel combustivel = new Combustivel();
                combustivel.Posto = new Posto()
                {
                    Id = (int)reader["postoId"]
                };
                combustivel.Id = (int) reader["Id"];                
                combustivel.Nome = (string) reader["Nome"];
                int codigo = (int) reader["Status"];
                combustivel.Status = StatusCombustivel.getInstance(codigo);

                lista.Add(combustivel);
            }
            reader.Close();

            foreach (Combustivel c in lista)
            { 
                using (PostoDAO dao = new PostoDAO())
                {
                     c.Posto = dao.Read(c.Posto.Id);
                }
            }
            return lista;            
        }


        public Combustivel Read(object pk) 
        {
            int id = (int) pk;
            Combustivel combustivel = null;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"select c.posto_id as postoId, id as Id, nome as Nome, status as Status 
                                from combustiveis c where id = @id order by c.posto_id, c.nome";            
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                combustivel = new Combustivel
                {
                    Posto = new Posto()
                    {
                        Id = (int)reader["postoId"]
                    },
                    Id = (int) reader["Id"],                    
                    Nome = (string) reader["Nome"],
                    Status = StatusCombustivel.getInstance( (int) reader["Status"] ) 
                };
            }
            reader.Close();
            using (PostoDAO dao = new PostoDAO())
            {
                combustivel.Posto = dao.Read(combustivel.Posto.Id);
            }

            return combustivel;    
        }


        public IList<Combustivel> ReadByPosto(int postoId)
        {
            
            IList<Combustivel> lista = new List<Combustivel>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"select c.posto_id as postoId, id as Id, c.nome as Nome, c.status as Status 
                                from combustiveis c where c.posto_id = @postoId and c.status = @status 
                                order by c.nome";
            cmd.Parameters.AddWithValue("@postoId", postoId);
            cmd.Parameters.AddWithValue("@status", StatusCombustivel.EM_ESTOQUE.Codigo);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Combustivel combustivel = new Combustivel();
                combustivel.Posto = new Posto()
                {
                    Id = (int)reader["postoId"]
                };
                combustivel.Id = (int)reader["Id"];
                combustivel.Nome = (string)reader["Nome"];
                int codigo = (int)reader["Status"];
                combustivel.Status = StatusCombustivel.getInstance(codigo);

                lista.Add(combustivel);
            }
            reader.Close();

            foreach (Combustivel c in lista)
            {
                using (PostoDAO dao = new PostoDAO())
                {
                    c.Posto = dao.Read(c.Posto.Id);
                }
            }
            return lista;            
        }
    }
}


