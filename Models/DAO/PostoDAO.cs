using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.DAO
{
    class PostoDAO : DAOConnection, IDAO<Posto>
    {

        public Posto Create(Posto domainObject)
        {
            Posto posto = domainObject;
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
                pessoaCommand.Parameters.AddWithValue("@nome", posto.Nome);
                pessoaCommand.Parameters.AddWithValue("@login", posto.Login);
                pessoaCommand.Parameters.AddWithValue("@senha", posto.Senha);
                pessoaCommand.Parameters.AddWithValue("@status", posto.Status.Codigo);
                // Insere e retorna o último valor de identidade inserido
                int id = Convert.ToInt32(pessoaCommand.ExecuteScalar());
                posto.Id = id;


                // Depois se torna uma pessoa jurídica Posto de Gasolina
                SqlCommand postoCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"insert into postos (pessoa_id, cnpj, telefone)
	                                values (@pessoaId, @cnpj, @telefone)"
                };
                postoCommand.Parameters.AddWithValue("@pessoaId", posto.Id);
                postoCommand.Parameters.AddWithValue("@cnpj", posto.CNPJ);
                postoCommand.Parameters.AddWithValue("@telefone", posto.Telefone);
                postoCommand.ExecuteNonQuery();

                // Salvando o Endereço do Posto
                SqlCommand enderecoCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"insert into enderecos (posto_id, cep, localidade, cidade_id)
	                                values (@postoId, @cep, @localidade, @cidadeId)"
                };
                enderecoCommand.Parameters.AddWithValue("@postoId", posto.Id);
                enderecoCommand.Parameters.AddWithValue("@cep", posto.Endereco.CEP);
                enderecoCommand.Parameters.AddWithValue("@localidade", posto.Endereco.Localidade);
                enderecoCommand.Parameters.AddWithValue("@cidadeId", posto.Endereco.Cidade.Id);
                enderecoCommand.ExecuteNonQuery();

                // Adicionar Email 
                SqlCommand emailCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"insert into emails (pessoa_id, endereco)
	                                values (@pessoaId, @endereco)"
                };
                emailCommand.Parameters.AddWithValue("@pessoaId", posto.Id);
                emailCommand.Parameters.AddWithValue("@endereco", posto.Emails.ToArray()[0].Endereco);
                emailCommand.ExecuteNonQuery();


                // Tudo certo
                transaction.Commit();

                return posto;
            }
            catch (Exception exp)
            {
                // Algo deu ruim, desfazer tudo
                transaction.Rollback();

                // relançar exceção para ser tratada pela camada de serviço
                throw exp;
            }
        }


        public void Update(Posto domainObject)
        {
            Posto posto = domainObject;
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
                pessoaCommand.Parameters.AddWithValue("@id", posto.Id);
                pessoaCommand.Parameters.AddWithValue("@nome", posto.Nome);
                pessoaCommand.Parameters.AddWithValue("@status", posto.Status.Codigo);
                pessoaCommand.ExecuteNonQuery();

                // Salva a pessoa jurídica Posto de gasolina
                SqlCommand postoCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"update postos set cnpj = @cnpj, telefone = @telefone
                                    where pessoa_id = @pessoaId"
                };
                postoCommand.Parameters.AddWithValue("@pessoaId", posto.Id);
                postoCommand.Parameters.AddWithValue("@cnpj", posto.CNPJ);
                postoCommand.Parameters.AddWithValue("@telefone", posto.Telefone);
                postoCommand.ExecuteNonQuery();

                // Salvando o Endereço do Posto
                SqlCommand enderecoCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"update enderecos set cep = @cep, localidade = @localidade, cidade_id = @cidadeId 
                                    where posto_id = @postoId and id = @id"
                };
                enderecoCommand.Parameters.AddWithValue("@postoId", posto.Id);
                enderecoCommand.Parameters.AddWithValue("@id", posto.Endereco.Id);
                enderecoCommand.Parameters.AddWithValue("@cep", posto.Endereco.CEP);
                enderecoCommand.Parameters.AddWithValue("@localidade", posto.Endereco.Localidade);
                enderecoCommand.Parameters.AddWithValue("@cidadeId", posto.Endereco.Cidade.Id);
                enderecoCommand.ExecuteNonQuery();

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


        public void Delete(Posto domainObject)
        {
            Posto posto = domainObject;

            // TUDO ou NADA - Operação Atômica
            // Para evitar gerar inconsistências no banco de dados, as operações
            // que envolvem várias tabelas, abre-se uma transação. Como os dados dessas 
            // tabelas estão relacionados, não se pode ter sucesso na alteração de uma tabela
            // e insucesso em outra. Ou se obtêm êxito em todas as alterações que se deseja ou
            // ou desfaz tudo o que foi feito (rollback) e restaura o estado consistente do banco.
            SqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                // Primeiramente, apagando as entidades fracas

                // E-mails
                SqlCommand emailCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"delete from emails where pessoa_id = @pessoaId"
                };
                emailCommand.Parameters.AddWithValue("@pessoaId", posto.Id);
                emailCommand.ExecuteNonQuery();

                // Endereço
                SqlCommand enderecoCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"delete from enderecos where posto_id = @postoId"
                };
                enderecoCommand.Parameters.AddWithValue("@postoId", posto.Id);
                enderecoCommand.ExecuteNonQuery();

                // Apagando o posto
                SqlCommand postoCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"delete from postos where pessoa_id = @pessoaId"
                };
                postoCommand.Parameters.AddWithValue("@pessoaId", posto.Id);
                postoCommand.ExecuteNonQuery();

                // Apagando a pessoa
                SqlCommand pessoaCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"delete from pessoas where id = @pessoaId"
                };
                pessoaCommand.Parameters.AddWithValue("@pessoaId", posto.Id);
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

        
        public IList<Posto> Read() 
        {
            IList<Posto> lista = new List<Posto>();
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = "select po.pessoa_id as Id, pe.nome as Nome, pe.senha as Senha, " +
                              "pe.status as Status, po.cnpj as CNPJ, po.telefone as Telefone " +
                              "from pessoas pe, postos po where pe.id = po.pessoa_id"
            };
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Posto posto = new Posto
                {
                    Id = (int)reader["Id"],
                    Nome = (string)reader["Nome"],
                    Senha = (string)reader["Senha"],
                    CNPJ = (string)reader["CNPJ"],
                    Telefone = (string)reader["Telefone"]
                };
                int codigo = (int)reader["Status"];
                posto.Status = Status.getInstance(codigo);

                lista.Add(posto);
            }
            reader.Close();

            return lista;
        }

        public Posto Read(object pk) 
        {
            // Buscando o Posto
            int id = (int)pk;
            Posto posto = null;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select po.pessoa_id as Id, p.nome as Nome, p.login as Login, p.senha as Senha, 
                                       p.status as Status, po.cnpj as CNPJ, po.telefone as Telefone 
                                from pessoas p, postos po where p.id = po.pessoa_id and Id = @id"
            };
            cmd.Parameters.AddWithValue("@id", id);
           
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                posto = new Posto
                {
                    Id = (int)reader["Id"],
                    Nome = (string)reader["Nome"],
                    Login = (string)reader["Login"],                    
                    Senha = (string)reader["Senha"],
                    Status = Status.getInstance((int)reader["Status"]),
                    CNPJ = (string)reader["CNPJ"],
                    Telefone = (string)reader["Telefone"]
                };
            }
            reader.Close();


            // Buscando o Endereço 
            SqlCommand enderecoCommand = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select ed.id as Id, ed.cep as CEP, ed.localidade as Localidade, 
                                       c.id as CidadeId, c.nome as Cidade, et.uf as UF, et.nome as EstadoNome
                                from enderecos ed, cidades c, estados et
                                where ed.cidade_id = c.id and c.estado_uf = et.uf and ed.posto_id = @postoId"
            };
            enderecoCommand.Parameters.AddWithValue("@postoId", id);

            SqlDataReader enderecoReader = enderecoCommand.ExecuteReader();
            if (enderecoReader.Read())
            {
                Estado estado = new Estado
                {
                    UF = (string)enderecoReader["UF"],
                    Nome = (string)enderecoReader["EstadoNome"]
                };

                Cidade cidade = new Cidade 
                {
                    Estado = estado,
                    Id = (int)enderecoReader["CidadeId"],
                    Nome = (string)enderecoReader["Cidade"]
                };

                Endereco endereco = new Endereco
                { 
                    Cidade = cidade,
                    Id = (int)enderecoReader["Id"],
                    CEP = (string)enderecoReader["CEP"],
                    Localidade = (string)enderecoReader["Localidade"]
                };

                posto.Endereco = endereco;
            }
            enderecoReader.Close();


            // Buscando os e-mails deste posto de gasolina
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
                posto.Emails.Add(email);
            }
            emailReader.Close();

            return posto;
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


        public bool IsCnpjExistente(string cnpj)
        {
            bool existe = false;
            SqlCommand cmd = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select cnpj as CNPJ from postos
                                where cnpj = @cnpj"
            };
            cmd.Parameters.AddWithValue("@cnpj", cnpj);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string existente = (string)reader["CNPJ"];
                existe = existente.Trim().Equals(cnpj);
            }
            reader.Close();

            return existe;
        }
    }
}
