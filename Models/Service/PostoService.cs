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
    public class PostoService
    {
        private readonly LoginService loginService;


        public PostoService()
        {
            loginService = new LoginService();
        }
        public IList<Posto> Read()
        {
            IList<Posto> lista = null;

            using (PostoDAO dao = new PostoDAO() )
            {
                lista = dao.Read();                
            }
            return lista;
        }


        public Posto Read(object pk)
        {   
            using (PostoDAO dao = new PostoDAO())
            {
                return dao.Read(pk);
            }
        }


        public void Delete(int id)
        {
            Posto posto = this.Read(id);
            try
            { 
                using (PostoDAO dao = new PostoDAO())
                {
                    dao.Delete(posto);
                }
            }
            catch (SqlException exp)
            {
                if (exp.Number == 547)
                {
                    string msg = "Conflito de chave estrangeira.\n postoId: " + id;
                    string userMsg = "O posto de combustível não pode ser apagado, " +
                                     "porque há registos de abastecimentos em que foi utilizado.";
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


        public IList<PessoaViewModel> GetPessoasViewModel()
        {
            IList<PessoaViewModel> lista = new List<PessoaViewModel>();
            IList<Posto> postos = this.Read();
            foreach (Posto p in postos)
            {
                lista.Add(new PessoaViewModel()
                {
                    CodigoStatus = p.Status.Codigo.ToString(),
                    Id = p.Id,
                    Nome = p.Nome
                });
            }
            return lista;
        }
        

        public void Create(Posto posto)
        {
            posto.Nome = posto.Nome.Trim().ToUpper();
            posto.Login = posto.Login.Trim();
            posto.Senha = posto.Senha.Trim();
            posto.CNPJ = posto.CNPJ.Trim();

            bool existeLogin = loginService.IsLoginExistente(posto.Login);
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

            bool existeCnpj = this.IsCnpjExistente(posto.CNPJ);
            if (existeCnpj)
            {
                string msg = "cnpj está como UNIQUE no banco.";
                string userMsg = "Já existe um posto de combustível com este CNPJ!";
                throw new UniqueLoginException()
                {
                    Alert = AlertType.WARNING,
                    ErrorDescription = msg,
                    UserErrorDescription = userMsg
                };
            }

            using (PostoDAO dao = new PostoDAO())
            {
                dao.Create(posto);
            }                
        }


        public void Update(Posto posto)
        {
            posto.Nome = posto.Nome.Trim().ToUpper();
            posto.CNPJ = posto.CNPJ.Trim();
            /** 
             * CNPJ é UNIQUE, logo, se houve alteração de cnpj,
             * o novo cnpj atualizado não pode ser de nenhum outro
             * posto de gasolina que já esteja cadastrado
             */
            // Buscando o cnpj que está no banco
            Posto postoExistente = Read(posto.Id);
            // Checando se ouve alteração
            if ( ! posto.CNPJ.Trim().Equals(postoExistente.CNPJ) )
            {
                // Verificar se já não existe algum motorista
                // com o mesmo cpf que se quer atualizar
                if (IsCnpjExistente(posto.CNPJ))
                {
                    string msg = "cnpj está como UNIQUE no banco.";
                    string userMsg = "Já existe um posto de combustível com este CNPJ!";
                    throw new UniqueLoginException()
                    {
                        Alert = AlertType.WARNING,
                        ErrorDescription = msg,
                        UserErrorDescription = userMsg
                    };
                }
            }
            using (PostoDAO dao = new PostoDAO())
            {
                dao.Update(posto);
            }
        }


        public void AdicionarEmail(Email email)
        {
            using (PostoDAO dao = new PostoDAO())
            {
                dao.AdicionarEmail(email);
            }
        }


        public void RemoverEmail(Email email)
        {
            using (PostoDAO dao = new PostoDAO())
            {
                dao.RemoverEmail(email);
            }
        }


        public bool IsCnpjExistente(string cnpj)
        {
            bool existe = false;
            using (PostoDAO dao = new PostoDAO())
            { 
                existe = dao.IsCnpjExistente(cnpj);
            }
            return existe;
        }
    }
}



