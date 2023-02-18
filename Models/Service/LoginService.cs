using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;
using UaiFuel.Models.ExceptionCore;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Models.Service
{
    public class LoginService
    {
        public UserViewModel Read(string login, string senha)
        {
            UserViewModel vm = null;
            Pessoa pessoa = null;
            using (PessoaDAO dao = new PessoaDAO())
            {
                pessoa = dao.Read(login, senha);
            }
            if (pessoa == null) return vm;
            
            vm = new UserViewModel()
            {
                PessoaId = pessoa.Id,
                Login = pessoa.Login,
                Senha = pessoa.Senha,
                CodigoStatus = pessoa.Status.Codigo,
                Nome = pessoa.Nome
            };


            bool isMotorista = false;
            using (PessoaDAO dao = new PessoaDAO())
                isMotorista = dao.IsMotorista(pessoa.Id);            
            if (isMotorista)            
            {
                vm.Tipo = "MOTORISTA";
                return vm;
            }

            bool isPosto = false;
            using (PessoaDAO dao = new PessoaDAO())
                isPosto = dao.IsPosto(pessoa.Id);
            if (isPosto)
            {
                vm.Tipo = "POSTO";
                return vm;
            }

            bool isAdministrador = false;
            using (PessoaDAO dao = new PessoaDAO())
                isAdministrador = dao.IsAdministrador(pessoa.Id);
            if (isAdministrador)
            {
                vm.Tipo = "ADMINISTRADOR";
                return vm;
            }

            return vm;
        }


        public bool IsLoginExistente(string login)
        {
            bool existe = false;
            using (PessoaDAO dao = new PessoaDAO())
            { 
                existe = dao.IsLoginExistente(login);
            }
            
            return existe;
        }


        
    }
}
