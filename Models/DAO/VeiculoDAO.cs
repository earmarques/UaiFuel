using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.DAO
{
    class VeiculoDAO : DAOConnection, IDAO<Veiculo>
    {

        public Veiculo Create(Veiculo domainObject)
        {
            Veiculo veiculo = domainObject;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"insert into veiculos values 
                                (@placa, @cor, @status, @modelo, @ano_fabricacao, @ano_modelo)"
            };
            cmd.Parameters.AddWithValue("@placa", veiculo.Placa);
            cmd.Parameters.AddWithValue("@cor", veiculo.Cor);
            cmd.Parameters.AddWithValue("@status", veiculo.Status.Codigo);
            cmd.Parameters.AddWithValue("@modelo", veiculo.Modelo);
            cmd.Parameters.AddWithValue("@ano_fabricacao", veiculo.AnoFabricacao);
            cmd.Parameters.AddWithValue("@ano_modelo", veiculo.AnoModelo);            
            cmd.ExecuteNonQuery();

            return veiculo;
        }


        public void Update(Veiculo domainObject)
        {
            Veiculo veiculo = domainObject;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"update veiculos set cor = @cor, status = @status, modelo = @modelo, 
                                    ano_fabricacao = @ano_fabricacao, ano_modelo = @ano_modelo
                                where placa = @placa"
            };
            cmd.Parameters.AddWithValue("@placa", veiculo.Placa);
            cmd.Parameters.AddWithValue("@cor", veiculo.Cor);
            cmd.Parameters.AddWithValue("@status", veiculo.Status.Codigo);
            cmd.Parameters.AddWithValue("@modelo", veiculo.Modelo);
            cmd.Parameters.AddWithValue("@ano_fabricacao", veiculo.AnoFabricacao);
            cmd.Parameters.AddWithValue("@ano_modelo", veiculo.AnoModelo);
            cmd.ExecuteNonQuery();
        }


        public void Delete(Veiculo domainObject)
        {
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"delete from veiculos where placa = @placa"
            };
            cmd.Parameters.AddWithValue("@placa", domainObject.Placa);
            cmd.ExecuteNonQuery();
        }


        public IList<Veiculo> Read()
        {
            IList<Veiculo> lista = new List<Veiculo>();
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = "select * from veiculos order by placa"
            };
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Veiculo veiculo = new Veiculo
                {
                    Placa = (string)reader["Placa"],
                    Cor = (string)reader["Cor"],
                    Modelo = (string)reader["Modelo"],
                    AnoFabricacao = (int)reader["Ano_Fabricacao"],
                    AnoModelo = (int)reader["Ano_Modelo"]
                };
                int codigo = (int) reader["Status"];
                veiculo.Status = StatusVeiculo.getInstance(codigo);

                lista.Add(veiculo);
            }
            reader.Close();

            return lista;
        }


        public Veiculo Read(object pk)
        {
            string placa = (string) pk;
            Veiculo veiculo = null;

            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select * from veiculos where placa = @placa"
            };
            cmd.Parameters.AddWithValue("@placa", placa);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                veiculo = new Veiculo
                {
                    Placa = (string) reader["Placa"],
                    Cor = (string) reader["Cor"],                    
                    Status = StatusVeiculo.getInstance((int)reader["Status"]),
                    Modelo = (string) reader["Modelo"],
                    AnoFabricacao = (int) reader["Ano_Fabricacao"],
                    AnoModelo = (int) reader["Ano_Modelo"]
                };
            }
            reader.Close();

            return veiculo;
        }


        public bool IsPlacaExistente(string placa)
        {
            bool existe = false;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select placa as Placa from veiculos
                                where placa = @placa"
            };
            cmd.Parameters.AddWithValue("@placa", placa);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string existente = (string)reader["Placa"];
                existe = existente.Trim().Equals(placa);
            }
            reader.Close();

            return existe;
        }
    }
}



