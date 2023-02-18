using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.DAO
{
    class MotoristaDAO : DAOConnection, IDAO<Motorista>
    {

        public Motorista Create(Motorista domainObject)
        {
            Motorista motorista = domainObject;
            SqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                // Primeiro nasce a pessoa
                SqlCommand pessoaCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"insert into pessoas (nome, login, senha, status)
                                    values (@nome, @login, @senha, @status); Select @@IDENTITY"
                };
                pessoaCommand.Parameters.AddWithValue("@nome", motorista.Nome);
                pessoaCommand.Parameters.AddWithValue("@login", motorista.Login);
                pessoaCommand.Parameters.AddWithValue("@senha", motorista.Senha);
                pessoaCommand.Parameters.AddWithValue("@status", motorista.Status.Codigo);
                // Insere e retorna o último valor de identidade inserido
                int id = Convert.ToInt32(pessoaCommand.ExecuteScalar());
                motorista.Id = id;

                // Depois a pessoa toma a decisão na vida de ser um motorista
                SqlCommand motoristaCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"insert into motoristas (pessoa_id, cpf, credito)
	                                values (@pessoaId, @cpf, @credito)"
                };
                motoristaCommand.Parameters.AddWithValue("@pessoaId", motorista.Id);
                motoristaCommand.Parameters.AddWithValue("@cpf", motorista.CPF);
                motoristaCommand.Parameters.AddWithValue("@credito", motorista.Credito);
                motoristaCommand.ExecuteNonQuery();

                // Adicionar Email 
                SqlCommand emailCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"insert into emails (pessoa_id, endereco)
	                                values (@pessoaId, @endereco)"
                };
                emailCommand.Parameters.AddWithValue("@pessoaId", motorista.Id);
                emailCommand.Parameters.AddWithValue("@endereco", motorista.Emails.ToArray()[0].Endereco);
                emailCommand.ExecuteNonQuery();


                // Tudo certo
                transaction.Commit();

                return motorista;
            }
            catch (Exception exp)
            {
                // Algo deu ruim, desfazer tudo
                transaction.Rollback();

                // relançar exceção para ser tratada pela camada de serviço
                throw exp;
            }
        }


        public void Update(Motorista domainObject)
        {
            Motorista motorista = domainObject;
            SqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                // Primeiro nasce a pessoa
                SqlCommand pessoaCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"update pessoas set nome = @nome, status = @status
                                    where id = @id"
                };
                pessoaCommand.Parameters.AddWithValue("@id", motorista.Id);
                pessoaCommand.Parameters.AddWithValue("@nome", motorista.Nome);
                pessoaCommand.Parameters.AddWithValue("@status", motorista.Status.Codigo);
                pessoaCommand.ExecuteNonQuery();

                // Depois a pessoa toma a decisão na vida de ser um motorista
                SqlCommand motoristaCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"update motoristas set cpf = @cpf, credito = @credito
                                    where pessoa_id = @pessoaId"
                };
                motoristaCommand.Parameters.AddWithValue("@pessoaId", motorista.Id);
                motoristaCommand.Parameters.AddWithValue("@cpf", motorista.CPF);
                motoristaCommand.Parameters.AddWithValue("@credito", motorista.Credito);
                motoristaCommand.ExecuteNonQuery();

                // Tudo certo
                transaction.Commit();
            }
            catch (Exception exp)
            {
                // Algo deu ruim, desfazer tudo
                transaction.Rollback();

                // relançar exceção para ser tratada pela camada de serviço
                throw exp;
            }
        }


        public void Delete(Motorista domainObject)
        {
            Motorista motorista = domainObject;

            // TUDO ou NADA - Operação Atômica
            // Para evitar gerar inconsistências no banco de dados, as operações
            // que envolvem várias tabelas, abre-se uma transação. Como os dados dessas 
            // tabelas estão relacionados, não se pode ter sucesso na alteração de uma tabela
            // e insucesso em outra. Ou se obtêm êxito em todas as alterações que se deseja ou
            // ou desfaz tudo o que foi feito (rollback) e restaura o estado consistente do banco.
            SqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                // Primeiramente, apagando os e-mails
                SqlCommand emailCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"delete from emails where pessoa_id = @pessoaId"
                };
                emailCommand.Parameters.AddWithValue("@pessoaId", motorista.Id);
                emailCommand.ExecuteNonQuery();


                // Apagando o motorista
                SqlCommand motoristaCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"delete from motoristas where pessoa_id = @pessoaId"
                };
                motoristaCommand.Parameters.AddWithValue("@pessoaId", motorista.Id);
                motoristaCommand.ExecuteNonQuery();

                // Apagando a pessoa
                SqlCommand pessoaCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"delete from pessoas where id = @pessoaId"
                };
                pessoaCommand.Parameters.AddWithValue("@pessoaId", motorista.Id);
                pessoaCommand.ExecuteNonQuery();

                // Tudo certo
                transaction.Commit();
            }
            catch (Exception exp)
            {
                // Algo deu ruim, desfazer tudo
                transaction.Rollback();

                // relançar exceção para ser tratada pela camada de serviço
                throw exp;
            }
        }

        
        public IList<Motorista> Read()
        {
            IList<Motorista> lista = new List<Motorista>();
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = "select m.pessoa_id as Id, p.nome as Nome, p.senha as Senha, " +
                                    "p.status as Status, m.cpf as CPF, m.credito as Credito " +
                                    "from pessoas p, motoristas m where p.id = m.pessoa_id"
            };
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Motorista motorista = new Motorista
                {
                    Id = (int)reader["Id"],
                    Nome = (string)reader["Nome"],
                    Senha = (string)reader["Senha"],
                    CPF = (string)reader["CPF"],
                    Credito = (decimal)  reader["Credito"]
                };
                int codigo = (int) reader["Status"];
                motorista.Status = Status.getInstance(codigo);

                lista.Add(motorista);
            }
            reader.Close();

            return lista;
        }

        public Motorista Read(object pk)
        {
            // Buscando o Motorista
            int id = (int)pk;
            Motorista motorista = null;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select m.pessoa_id as Id, p.nome as Nome, p.login as Login, p.senha as Senha, 
                                        p.status as Status, m.cpf as CPF, m.credito as Credito 
                                from pessoas p, motoristas m where p.id = m.pessoa_id and Id = @id"
            };
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                motorista = new Motorista
                {
                    Id = (int)reader["Id"],
                    Nome = (string)reader["Nome"],
                    Login = (string)reader["Login"],
                    Senha = (string)reader["Senha"],
                    Status = Status.getInstance((int)reader["Status"]),
                    CPF = (string)reader["CPF"],
                    Credito = (decimal)reader["Credito"]
                };
            }
            reader.Close();

            // Buscando os e-mails deste motorista
            SqlCommand emailCommand = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select pessoa_id as PessoaId, endereco as Endereco 
                                from emails where pessoa_id = @id"
            };
            emailCommand.Parameters.AddWithValue("@id", id);

            SqlDataReader emailReader = emailCommand.ExecuteReader();
            while (emailReader.Read())
            {
                Email email = new Email
                {
                    PessoaId = (int)emailReader["PessoaId"],
                    Endereco = (string)emailReader["Endereco"]
                };
                motorista.Emails.Add(email);
            }
            emailReader.Close();

            return motorista;
        }

        public void AdicionarEmail(Email email)
        {
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"insert into emails values (@pessoaId, @endereco)"
            };
            cmd.Parameters.AddWithValue("@pessoaId", email.PessoaId);
            cmd.Parameters.AddWithValue("@endereco", email.Endereco);

            cmd.ExecuteNonQuery();
        }



        public void RemoverEmail(Email email)
        {
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"delete from emails where pessoa_id = @pessoaId and endereco = @endereco"
            };
            cmd.Parameters.AddWithValue("@pessoaId", email.PessoaId);
            cmd.Parameters.AddWithValue("@endereco", email.Endereco);

            cmd.ExecuteNonQuery();
        }


        public bool IsCpfExistente(string cpf)
        {
            bool existe = false;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select cpf as CPF from motoristas
                                where cpf = @cpf"
            };
            cmd.Parameters.AddWithValue("@cpf", cpf);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string existente = (string)reader["CPF"];
                existe = existente.Trim().Equals(cpf);
            }
            reader.Close();

            return existe;
        }
    }
}

