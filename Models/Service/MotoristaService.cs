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
    public class MotoristaService
    {
        private readonly LoginService loginService;

        public MotoristaService()
        {
            loginService = new LoginService();
        }

        public IList<Motorista> Read()
        {
            IList<Motorista> lista = null;

            using (MotoristaDAO dao = new MotoristaDAO() )
            {
                lista = dao.Read();                
            }
            return lista;
        }


        public Motorista Read(object pk)
        {   
            using (MotoristaDAO dao = new MotoristaDAO())
            {
                return dao.Read(pk);
            }
        }


        public void Delete(int id)
        {
            Motorista motorista = this.Read(id);
            try
            { 
                using (MotoristaDAO dao = new MotoristaDAO())
                {
                    dao.Delete(motorista);
                }
            }            
            catch (SqlException exp)
            {
                if (exp.Number == 547)
                {
                    string msg = "Conflito de chave estrangeira.\n motoristaId: " + id;
                    string userMsg = "O combustível não pode ser apagado, " +
                                     "porque foi utilizado em um abastecimento.";
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


        public void Create(Motorista motorista)
        {
            motorista.Nome = motorista.Nome.Trim().ToUpper();
            motorista.Login = motorista.Login.Trim();
            motorista.Senha = motorista.Senha.Trim();
            motorista.CPF = motorista.CPF.Trim();

            bool existeLogin = loginService.IsLoginExistente(motorista.Login);
            if (existeLogin)
            {
                string msg = "login está como UNIQUE no banco.";
                string userMsg = "Já existe um usuário com este login!";
                throw new UniqueLoginException()
                {
                    Alert = AlertType.WARNING,
                    ErrorDescription = msg,
                    UserErrorDescription = userMsg
                };
            }

            bool existeCpf = this.IsCpfExistente(motorista.CPF);
            if (existeCpf)
            {
                string msg = "cpf está como UNIQUE no banco.";
                string userMsg = "Já existe um motorista com este CPF!";
                throw new UniqueLoginException()
                {
                    Alert = AlertType.WARNING,
                    ErrorDescription = msg,
                    UserErrorDescription = userMsg
                };
            }

            using (MotoristaDAO dao = new MotoristaDAO())
            {
                dao.Create(motorista);
            }                
        }


        public void Update(Motorista motorista)
        {
            motorista.Nome = motorista.Nome.Trim().ToUpper();
            motorista.CPF = motorista.CPF.Trim();
            /** 
             * CPF é UNIQUE, logo, se houve alteração de cpf,
             * o novo cpf atualizado não pode ser de nenhum outro
             * motorista que já esteja cadastrado
             */
            // Buscando o motorista que está no banco
            Motorista motoristaExistente = Read(motorista.Id);
            // Checando se ouve alteração
            if ( ! motorista.CPF.Trim().Equals(motoristaExistente.CPF) )
            {
                // Verificar se já não existe algum motorista
                // com o mesmo cpf que se quer atualizar
                if( IsCpfExistente(motorista.CPF) )
                {
                    string msg = "cpf está como UNIQUE no banco.";
                    string userMsg = "Já existe um motorista com este CPF!";
                    throw new UniqueLoginException()
                    {
                        Alert = AlertType.WARNING,
                        ErrorDescription = msg,
                        UserErrorDescription = userMsg
                    };
                }
            }
            using (MotoristaDAO dao = new MotoristaDAO())
            {
                dao.Update(motorista);
            }
        }


        public void AdicionarEmail(Email email)
        {
            using (MotoristaDAO dao = new MotoristaDAO())
            {
                dao.AdicionarEmail(email);
            }
        }


        public void RemoverEmail(Email email)
        {
            using (MotoristaDAO dao = new MotoristaDAO())
            {
                dao.RemoverEmail(email);
            }
        }

        public bool IsCpfExistente(string cpf)
        {
            bool existe = false;
            using (MotoristaDAO dao = new MotoristaDAO())
            {
                existe = dao.IsCpfExistente(cpf);
            }
            return existe;
        }

        public IList<PessoaViewModel> GetPessoasViewModel()
        {
            IList<PessoaViewModel> lista = new List<PessoaViewModel>();
            IList<Motorista> motoristas = this.Read();
            foreach (Motorista m in motoristas)
            {
                lista.Add( new PessoaViewModel()
                { 
                    CodigoStatus = m.Status.Codigo.ToString(),
                    Id = m.Id,
                    Nome = m.Nome
                });
            }
            return lista;
        }
    }
}



