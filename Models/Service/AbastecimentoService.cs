using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;
using UaiFuel.Models.ExceptionCore;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Models.Service
{
    public class AbastecimentoService
    {
        public IList<Abastecimento> Pesquisar(PesquisaAbastecimentoViewModel vm) 
        {
            IList<Abastecimento> lista = null;
            // Se nenhum veículo vem selecionado, a placa vem com o value "0"
            if ( vm.VeiculoPlaca.Equals("0") )
            {
                vm.VeiculoPlaca = null;
            }

            using (AbastecimentoDAO dao = new AbastecimentoDAO())
            {
                lista = dao.Pesquisar(vm);
            }
            return lista;
        }


        public IList<Abastecimento> Read()
        {
            IList<Abastecimento> lista = null;

            using ( AbastecimentoDAO dao = new AbastecimentoDAO() )
            {
                lista = dao.Read();                
            }
            return lista;
        }


        public Abastecimento Read(int id)
        {   
            using (AbastecimentoDAO dao = new AbastecimentoDAO())
            {
                return dao.Read(id);
            }
        }


        
        public void Update(Abastecimento abastecimento)
        {
            if (abastecimento.CupomFiscal != null)
            {
                abastecimento.CupomFiscal = abastecimento.CupomFiscal.Trim();
            }
            using (AbastecimentoDAO dao = new AbastecimentoDAO())
            {
                dao.Update(abastecimento);
            }
        }


        public Abastecimento Create(Abastecimento abastecimento)
        {
            if (abastecimento.CupomFiscal != null)
            {
                abastecimento.CupomFiscal = abastecimento.CupomFiscal.Trim();
            }

            using (AbastecimentoDAO dao = new AbastecimentoDAO())
            {
                return dao.Create(abastecimento);
            }                
        }

        internal void Delete(int id)
        {
            Abastecimento abastecimento = this.Read(id);
            try
            {
                using (AbastecimentoDAO dao = new AbastecimentoDAO())
                {
                    dao.Delete(abastecimento);
                }
            }
            catch (SqlException exp)
            {
                string msg = "Falha com o banco ao tentar apagar um abastecimento. Id: " + id;
                msg += "\n\n" + exp.Message;
                string userMsg = "O combustível não pode ser apagado, " +
                                 "porque foi utilizado em um abastecimento.";
                throw new AppException()
                {
                    Cause = exp,
                    Alert = AlertType.DANGER,
                    ErrorDescription = msg,
                    UserErrorDescription = userMsg
                };
            }
            catch (Exception exp)
            {
                throw new AppException(exp.Message, exp);
            }
        }
    }
}



