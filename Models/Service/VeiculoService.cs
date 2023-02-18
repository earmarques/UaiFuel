using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;
using UaiFuel.Models.ExceptionCore;

namespace UaiFuel.Models.Service
{
    public class VeiculoService
    {
        
        public IList<Veiculo> Read()
        {
            IList<Veiculo> lista = null;

            using (VeiculoDAO dao = new VeiculoDAO() )
            {
                lista = dao.Read();                
            }
            return lista;
        }


        public Veiculo Read(object pk)
        {   
            using (VeiculoDAO dao = new VeiculoDAO())
            {
                return dao.Read(pk);
            }
        }


        public void Delete(string placa)
        {
            try
            { 
                using (VeiculoDAO dao = new VeiculoDAO())
                {
                    Veiculo veiculo = new Veiculo();
                    veiculo.Placa = placa;
                    dao.Delete(veiculo);
                }
            }
            catch (SqlException exp)
            {
                if (exp.Number == 547)
                {
                    string msg = "Conflito de chave estrangeira.\n veiculoPlaca: " + placa;
                    string userMsg = "O veículo não pode ser apagado, " +
                                     "porque foi utilizado em abastecimentos.";
                    throw new FKConflictException()
                    {
                        Cause = exp,
                        Alert = AlertType.WARNING,
                        ErrorDescription = msg,
                        UserErrorDescription = userMsg
                    };
                }
                else
                    throw new AppException()
                    {
                        Cause = exp,
                        ErrorDescription = exp.Message
                    };
            }
            catch (Exception exp)
            {
                throw new AppException(exp.Message, exp);
            }
        }


        public void Update(Veiculo veiculo)
        {
            veiculo.Placa = veiculo.Placa.Trim().ToUpper();
            /** 
             * Placa é UNIQUE, logo, se houve alteração da placa,
             * a nova placa atualizada não pode ser de nenhum outro
             * veículo que já esteja cadastrado
             */
            // Buscando o veículo que está no banco
            Veiculo veiculoExistente = this.Read(veiculo.Placa);
            // Checando se ouve alteração
            if ( ! veiculo.Placa.Trim().Equals(veiculoExistente.Placa) )
            {
                // Verificar se já não existe algum veículo
                // com a mesma placa que se quer atualizar
                if ( IsPlacaExistente(veiculo.Placa) )
                {
                    string msg = "Placa do veículo é chave UNIQUE no banco.";
                    string userMsg = "Já existe um veículo com esta Placa!";
                    throw new UniquePlacaException()
                    {
                        Alert = AlertType.WARNING,
                        ErrorDescription = msg,
                        UserErrorDescription = userMsg
                    };
                }
            }
            
            veiculo.Cor = veiculo.Cor.Trim().ToUpper();
            veiculo.Modelo = veiculo.Modelo.Trim().ToUpper();


            using (VeiculoDAO dao = new VeiculoDAO())
            {
                dao.Update(veiculo);
            }
        }


        public void Create(Veiculo veiculo)
        {
            veiculo.Placa = veiculo.Placa.Trim().ToUpper();
            if ( IsPlacaExistente(veiculo.Placa) )
            {
                string msg = "Placa do veículo é chave UNIQUE no banco.";
                string userMsg = "Já existe um veículo com esta Placa!";
                throw new UniquePlacaException()
                {
                    Alert = AlertType.WARNING,
                    ErrorDescription = msg,
                    UserErrorDescription = userMsg
                };
            }
            veiculo.Cor = veiculo.Cor.Trim().ToUpper();
            veiculo.Modelo = veiculo.Modelo.Trim().ToUpper();

            using (VeiculoDAO dao = new VeiculoDAO())
            {
                dao.Create(veiculo);
            }                
        }

        
        public bool IsPlacaExistente(string placa)
        {            
            Veiculo veiculo = this.Read(placa);            
            return veiculo != null;
        }


    }
}



