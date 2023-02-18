using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using UaiFuel.Models.Domain;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Models.DAO
{
    class AbastecimentoDAO : DAOConnection, IDAO<Abastecimento>
    {

        public IList<Abastecimento> Pesquisar(PesquisaAbastecimentoViewModel vm)
        {
            IList<Abastecimento> lista = new List<Abastecimento>();
            // Montando a Select Interno
            string selectInterno = "SELECT a.id FROM abastecimentos as a, v_motoristas as m, " +
                "v_postos as p, veiculos as v, combustiveis as c, abastecimentos__combustiveis as ac ";
            // Join's
            selectInterno += "WHERE a.id = ac.abastecimento_id AND c.id = ac.combustivel_id AND " +
                " a.motorista_id = MotoristaId AND a.posto_id = PostoId AND a.veiculo_placa = Placa ";
            // Filtros
            if ( vm.NomeCombustivel != null && vm.NomeCombustivel.Trim() != "" )
                selectInterno += " AND c.nome LIKE '%" + vm.NomeCombustivel.Trim() + "%' ";
            if ( vm.VeiculoPlaca != null && vm.VeiculoPlaca.Trim() != "" )
                selectInterno += " AND v.placa LIKE '%" + vm.VeiculoPlaca.Trim() + "%' ";
            if ( Convert.ToInt32(vm.PostoId) != default(int) )
                selectInterno += " AND p.PostoId = " + Convert.ToInt32(vm.PostoId);
            if (Convert.ToInt32(vm.MotoristaId) != default(int))
                selectInterno += " AND MotoristaId = " + Convert.ToInt32(vm.MotoristaId);
            if (Convert.ToInt32(vm.CodigoStatus) != default(int))
                selectInterno += " AND a.status = " + Convert.ToInt32(vm.CodigoStatus);
            if ( vm.DataInicial != default(DateTime))
                selectInterno += " AND a.data >= '" + vm.DataInicial + "'";
            if ( vm.DataFinal != default(DateTime))
                selectInterno += " AND a.data <= '" + vm.DataFinal + "'";

            selectInterno += " GROUP BY a.id ";

            // Montando a Select da Pesquisa
            string selectPesquisa = "SELECT a.id as Id, a.data as Data, a.cupom_fiscal as CupomFiscal, " +
                    "a.valor_total as ValorTotal, a.status as Status, a.veiculo_placa as VeiculoPlaca, " +
                    "po.PostoId as PostoId, po.PostoNome as NomePosto, " +
                    "m.MotoristaId as MotoristaId, m.MotoristaNome as NomeMotorista " +
                "FROM abastecimentos a " +                                
                "INNER JOIN v_postos po ON a.posto_id = po.PostoId " +
                "INNER JOIN v_motoristas m ON a.motorista_id = m.MotoristaId " +
                "INNER JOIN veiculos v ON a.veiculo_placa = v.placa ";

            // Enbutindo o Select Interno no Select da Pesquisa
            selectPesquisa += "WHERE a.id in ( " + selectInterno + " )";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = selectPesquisa;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Abastecimento abastecimento = new Abastecimento()
                {
                    Id = (int)reader["Id"],
                    DataCriacao = (DateTime)reader["Data"],
                    CupomFiscal = Convert.IsDBNull(reader["CupomFiscal"]) ? null : (string)reader["CupomFiscal"],
                    ValorTotal = (decimal)reader["ValorTotal"],
                    Status = StatusAbastecimento.getInstance( (int)reader["Status"] ),
                    Veiculo = new Veiculo()
                    {
                        Placa = (string)reader["VeiculoPlaca"]
                    },
                    Posto = new Posto()
                    {
                        Id = (int)reader["postoId"],
                        Nome = (string)reader["NomePosto"]
                    },
                    Motorista = new Motorista()
                    {
                        Id = (int)reader["MotoristaId"],
                        Nome = (string)reader["NomeMotorista"]
                    }
                };
                lista.Add(abastecimento);
            }
            reader.Close();

            return lista;
        }


        public Abastecimento Create(Abastecimento domainObject)
        {
            Abastecimento abastecimento = domainObject;
            SqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                // Abastecimento
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"insert into abastecimentos (data, cupom_fiscal, valor_total, status, 
                                            veiculo_placa, motorista_id, posto_id)
                                    values	(@data, @cupomFiscal, @valorTotal, @status, 
                                            @veiculoPlaca, @motoristaId, @postoId); Select @@IDENTITY"
                };
                sqlCommand.Parameters.AddWithValue("@data", abastecimento.DataCriacao);
                sqlCommand.Parameters.AddWithValue("@cupomFiscal", 
                    ( (object)abastecimento.CupomFiscal ) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@valorTotal", abastecimento.ValorTotal);
                sqlCommand.Parameters.AddWithValue("@status", abastecimento.Status.Codigo);
                sqlCommand.Parameters.AddWithValue("@veiculoPlaca", abastecimento.Veiculo.Placa);
                sqlCommand.Parameters.AddWithValue("@motoristaId", abastecimento.Motorista.Id);
                sqlCommand.Parameters.AddWithValue("@postoId", abastecimento.Posto.Id   );
                
                int id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                abastecimento.Id = id;


                // Abastecimento__Combustivel - Relacionamento N--N
                foreach (AbastecimentoCombustivel ab in abastecimento.Combustiveis)
                { 
                    SqlCommand nnCommand = new SqlCommand
                    {
                        Connection = base.GetConnection(),
                        Transaction = transaction,
                        CommandText = @"insert into abastecimentos__combustiveis (abastecimento_id, 
                                                posto_id, combustivel_id, valor, litros)
                                        values	(@abastecimentoId, @postoId, @combustivelId, @valor, @litros)"
                    };
                    nnCommand.Parameters.AddWithValue("@abastecimentoId", abastecimento.Id);
                    nnCommand.Parameters.AddWithValue("@postoId", abastecimento.Posto.Id);
                    nnCommand.Parameters.AddWithValue("@combustivelId", ab.Combustivel.Id);
                    nnCommand.Parameters.AddWithValue("@valor", ab.Valor);
                    nnCommand.Parameters.AddWithValue("@litros", ab.Litros);

                    nnCommand.ExecuteNonQuery();
                }
                
                // Tudo certo
                transaction.Commit();

                return abastecimento;
            }
            catch (Exception exp)
            {
                // Algo deu ruim, desfazer tudo
                transaction.Rollback();

                // relançar exceção para ser tratada pela camada de serviço
                throw exp;
            }
        }


        public void Update(Abastecimento domainObject)
        {
            Abastecimento abastecimento = domainObject;            
            try
            {
                // Primeiro nasce a pessoa
                SqlCommand pessoaCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    CommandText = @"update abastecimentos set cupom_fiscal = @cupomFiscal, status = @status
                                    where id = @id"
                };
                pessoaCommand.Parameters.AddWithValue("@id", abastecimento.Id); 
                pessoaCommand.Parameters.AddWithValue("@cupomFiscal", 
                    ( (object)abastecimento.CupomFiscal ) ?? DBNull.Value);
                pessoaCommand.Parameters.AddWithValue("@status", abastecimento.Status.Codigo);
                pessoaCommand.ExecuteNonQuery();
                
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        public void Delete(Abastecimento domainObject)
        {
            Abastecimento abastecimento = domainObject;
            SqlTransaction transaction = base.GetConnection().BeginTransaction();
            try
            {
                // Abastecimento__Combustivel - Relacionamento N--N
                foreach (AbastecimentoCombustivel ab in abastecimento.Combustiveis)
                {
                    SqlCommand nnCommand = new SqlCommand
                    {
                        Connection = base.GetConnection(),
                        Transaction = transaction,
                        CommandText = @"delete from abastecimentos__combustiveis
                                        where abastecimento_id = @abastecimentoId 
                                        and posto_id = @postoId"
                    };
                    nnCommand.Parameters.AddWithValue("@abastecimentoId", abastecimento.Id);
                    nnCommand.Parameters.AddWithValue("@postoId", abastecimento.Posto.Id);
                    nnCommand.ExecuteNonQuery();
                }

                // Apagando a o abastecimento
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = base.GetConnection(),
                    Transaction = transaction,
                    CommandText = @"delete from abastecimentos where id = @abastecimentoId"
                };
                sqlCommand.Parameters.AddWithValue("@abastecimentoId", abastecimento.Id);
                sqlCommand.ExecuteNonQuery();

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


        public IList<Abastecimento> Read()
        {
            IList<Abastecimento> lista = new List<Abastecimento>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = "select * from v_abastecimentos";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Abastecimento abastecimento = new Abastecimento() 
                {
                    Id = (int)reader["Id"],
                    DataCriacao = (DateTime)reader["Data"],

                    CupomFiscal = reader.IsDBNull(2) ? default(string) : (string)reader["CupomFiscal"],
                    ValorTotal = (decimal)reader["ValorTotal"],
                    Status = StatusAbastecimento.getInstance( (int)reader["Status"] ),
                    // Relacionamentos
                    Veiculo = new Veiculo() { Placa = (string)reader["VeiculoPlaca"] },
                    Posto = new Posto()
                    {
                        Id = (int)reader["PostoId"],
                        Nome = (string)reader["PostoNome"]
                    },
                    Motorista = new Motorista() 
                    {
                        Id = (int)reader["MotoristaId"],
                        Nome = (string)reader["MotoristaNome"]
                    }
                };

                lista.Add(abastecimento);
            }
            reader.Close();

            return lista;
        }

        public Abastecimento Read(object pk)
        {
            int id = (int)pk;
            Abastecimento abastecimento = null;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = base.GetConnection();
            cmd.CommandText = @"select * from v_abastecimentos where Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                abastecimento = new Abastecimento()
                {
                    Id = (int)reader["Id"],
                    DataCriacao = (DateTime)reader["Data"],

                    CupomFiscal = reader.IsDBNull(2) ? default(string) : (string)reader["CupomFiscal"],
                    ValorTotal = (decimal)reader["ValorTotal"],
                    Status = StatusAbastecimento.getInstance((int)reader["Status"]),
                    // Relacionamentos
                    Veiculo = new Veiculo() { Placa = (string)reader["VeiculoPlaca"] },
                    Posto = new Posto()
                    {
                        Id = (int)reader["PostoId"],
                        Nome = (string)reader["PostoNome"]
                    },
                    Motorista = new Motorista()
                    {
                        Id = (int)reader["MotoristaId"],
                        Nome = (string)reader["MotoristaNome"]
                    }
                };
            }
            reader.Close();

            // Buscando os abastecimentos no Relacionamento N--N
            SqlCommand nnCommand = new SqlCommand
            {
                Connection = base.GetConnection(),
                CommandText = @"select c.id as CombustivelId, c.nome as NomeCombustivel, po.PostoId, c.status as Status,
                                    po.PostoNome as NomePosto, ac.litros as Litros, ac.valor as Valor
                                from abastecimentos__combustiveis ac 
                                    inner join abastecimentos a on a.id = ac.abastecimento_id 
                                    inner join v_postos po on po.PostoId = ac.posto_id 
                                    inner join combustiveis c on c.id = ac.combustivel_id  and c.posto_id = ac.posto_id
                                    and a.id = @id"
            };
            nnCommand.Parameters.AddWithValue("@id", abastecimento.Id);

            SqlDataReader nnReader = nnCommand.ExecuteReader();
            while (nnReader.Read())
            {
                Combustivel combustivel = new Combustivel
                {
                    Id = (int)nnReader["CombustivelId"],
                    Nome = (string)nnReader["NomeCombustivel"],
                    Status = StatusCombustivel.getInstance((int)nnReader["Status"]),
                    Posto = new Posto()
                    { 
                        Id = (int)nnReader["PostoId"],
                        Nome = (string)nnReader["NomePosto"]
                    }                    
                };
                abastecimento.Combustiveis.Add(
                    new AbastecimentoCombustivel(abastecimento, combustivel)
                    { 
                        Litros = (decimal)nnReader["Litros"],
                        Valor = (decimal)nnReader["Valor"]
                    });
            }
            nnReader.Close();

            return abastecimento;
        }
    }
}
