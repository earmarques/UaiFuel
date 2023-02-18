using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.DAO
{
    public class EstadoCidadeDAO : DAOConnection
    {
        public IList<Estado> GetEstados()
        {
            List<Estado> lista = new List<Estado>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = "select uf as UF, nome as Nome from estados order by uf";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Estado estado = new Estado()
                {
                    UF = (string)reader["uf"],
                    Nome = (string)reader["Nome"]
                };
                lista.Add(estado);
            }
            reader.Close();

            return lista;
        }


        public IList<Cidade> GetCidades(string uf)
        {
            List<Cidade> lista = new List<Cidade>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"select c.id as cidadeId, c.nome as Cidade, e.uf as UF, e.nome as Estado 
                                from cidades c, estados e where c.estado_uf = e.uf and 
                                    e.uf = @uf order by Cidade";
            cmd.Parameters.AddWithValue("@uf", uf);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Estado estado = new Estado()
                { 
                    UF = (string)reader["UF"],
                    Nome = (string)reader["Estado"]
                };

                Cidade cidade = new Cidade()
                {
                    Estado = estado,
                    Id = (int)reader["cidadeId"],                   
                    Nome = (string)reader["Cidade"]
                };
                lista.Add(cidade);
            }
            reader.Close();

            return lista;
        }

        internal Cidade GetCidadeById(int id)
        {
            Cidade cidade = null;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"select c.id as cidadeId, c.nome as Cidade, e.uf as UF, e.nome as Estado 
                                from cidades c, estados e where c.estado_uf = e.uf and c.id = @id ";
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Estado estado = new Estado()
                {
                    UF = (string)reader["UF"],
                    Nome = (string)reader["Estado"]
                };

                cidade = new Cidade()
                {
                    Estado = estado,
                    Id = (int)reader["cidadeId"],
                    Nome = (string)reader["Cidade"]
                };
            }
            reader.Close();

            return cidade;
        }
    }
}
