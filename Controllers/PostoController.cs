using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;
using UaiFuel.Models.ExceptionCore;
using UaiFuel.Models.Service;
using UaiFuel.Models.Utils;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Controllers
{
    public class PostoController : Controller       
    {
        private readonly PostoService service;
        private readonly EstadoCidadeService estadoCidadeService;
        private readonly LoginService loginService;

        public PostoController()
        {
            service = new PostoService();
            estadoCidadeService = new EstadoCidadeService();
            loginService = new LoginService();
        }


        public IActionResult List()
        {   
            return View(service.Read());
        }


        public IActionResult Delete(int id)
        {
            service.Delete(id);
            return RedirectToAction("List");
        }


        [HttpPost]
        public JsonResult GetCidade(string uf)
        {
            List<List<string>> cidadesJson = new List<List<string>>();

            if (!string.IsNullOrEmpty(uf))
            {
                IList<Cidade> cidades = estadoCidadeService.GetCidades(uf);
                foreach (Cidade cidade in cidades)
                {
                    List<string> dict = new List<string>
                    {
                        cidade.Id.ToString(),
                        cidade.Nome
                    };
                    cidadesJson.Add(dict);
                }
            }
            return new JsonResult(cidadesJson);
        }
 

        public IActionResult Create()
        {
            CreatePostoViewModel vm = new CreatePostoViewModel();
            this.CarregarEstados(vm);
            return View(vm);
        }


        [HttpPost]
        public IActionResult Create(CreatePostoViewModel vm)
        {
            // Validação
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                IEnumerable<string> messages = allErrors.Select(x => x.ErrorMessage);

                FrontUtil.SetupErrorsViewBag(this, messages);

                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }

                this.CarregarEstados(vm);
                return View(vm);
            }
            /*
            bool jaExisteLogin = loginService.IsLoginExistente(vm.Login);
            if (jaExisteLogin)
            {
                ViewBag.Message = "Já existe um usuário com este login!";
                this.CarregarEstados(vm);
                return View(vm);
            }
            */
            Endereco endereco = new Endereco
            {
                CEP = vm.CEP,
                Localidade = vm.Localidade,
                Cidade = estadoCidadeService.GetCidadeById(Convert.ToInt32(vm.CidadeId))
            };
            Posto posto = new Posto
            {
                Id = vm.Id,
                Nome = vm.Nome,
                Login = vm.Login,
                Senha = vm.Senha,
                CNPJ = vm.CNPJ,
                Telefone = vm.Telefone,
                Status = Status.getInstance(Convert.ToInt32(vm.CodigoStatus)),
                Endereco = endereco 
            };
            posto.Emails.Add( new Email { Endereco = vm.NovoEmail } );

            try
            {   
                service.Create(posto);
                return RedirectToAction("List");
            }
            catch (AppException exp)
            {
                FrontUtil.SetupViewBag(this, exp);
                this.CarregarEstados(vm);
                return View(vm);
            }
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            Posto posto = service.Read(id);
            UpdatePostoViewModel vm = new UpdatePostoViewModel(posto);
            this.CarregarEstados(vm);

            return View(vm);
        }


        [HttpPost]
        public IActionResult Update(int id, UpdatePostoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                IEnumerable<string> messages = allErrors.Select(x => x.ErrorMessage);

                FrontUtil.SetupErrorsViewBag(this, messages);

                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }

                Posto posto = service.Read(id);
                UpdatePostoViewModel viewModel = new UpdatePostoViewModel(posto);
                this.CarregarEstados(viewModel);
                this.CarregarCidades(viewModel);

                return View(viewModel);
            }

            if (vm.NovoEmail != null)
            {
                Email email = new Email
                {
                    PessoaId = id,
                    Endereco = vm.NovoEmail
                };
                service.AdicionarEmail(email);

                return RedirectToRoute(new { controller = "Posto", action = "Update", id });
            }
            else
            {
                Endereco endereco = new Endereco
                {
                    Id = vm.EnderecoId,
                    PostoId = vm.Id,
                    CEP = vm.CEP,
                    Localidade = vm.Localidade,
                    Cidade = estadoCidadeService.GetCidadeById(Convert.ToInt32(vm.CidadeId))
                };
                Posto posto = new Posto
                {
                    Id = vm.Id,
                    Nome = vm.Nome,
                    CNPJ = vm.CNPJ,
                    Telefone = vm.Telefone,
                    Status = Status.getInstance(Convert.ToInt32(vm.CodigoStatus)),
                    Emails = vm.Emails,
                    Endereco = endereco
                };
                try
                {   
                    service.Update(posto);
                    return RedirectToAction("List");
                }
                catch (AppException exp)
                {
                    FrontUtil.SetupViewBag(this, exp);
                    this.CarregarEstados(vm);
                    this.CarregarCidades(vm);

                    return View(vm);
                }
            }
        }

        /*
        [HttpGet]
        public IActionResult AdicionarEmail(int id)
        {
            Posto posto = service.Read(id);
            EmailViewModel vm = new EmailViewModel
            { 
                Id = posto.Id,
                Nome = posto.Nome
            };
            return View(vm);
        }


        [HttpPost]
        public IActionResult AdicionarEmail(int id, EmailViewModel vm)
        {
            Email email = new Email
            {
                PessoaId = id,
                Endereco = vm.NovoEmail
            };
            service.AdicionarEmail(email);

            return RedirectToRoute(new { controller = "Posto", action = "Update", id });
        }
        */

        [HttpGet] 
        public IActionResult RemoverEmail(int id, string endereco)
        {
            Email email = new Email
            {
                PessoaId = id,
                Endereco = endereco
            };
            service.RemoverEmail(email);

            return RedirectToRoute(new { controller = "Posto", action = "Update", id });
        }


        private void CarregarEstados(PostoViewModel vm)
        {
            IList<Estado> estados = estadoCidadeService.GetEstados();
            foreach (Estado estado in estados)
            {
                vm.Estados.Add(
                    new SelectListItem
                    {
                        Text = estado.UF + " - " + estado.Nome,
                        Value = estado.UF
                    });
            }
        }


        private void CarregarCidades(PostoViewModel vm)
        {
            string estadoUf = vm.EstadoUF;
            if (estadoUf != null)
            { 
                IList<Cidade> cidades = estadoCidadeService.GetCidades(vm.EstadoUF);
                foreach (Cidade cidade in cidades)
                {
                    vm.Cidades.Add(
                        new SelectListItem
                        {
                            Value = cidade.Id.ToString(),
                            Text = cidade.Nome
                        });
                }
            }
        }
    }
}

