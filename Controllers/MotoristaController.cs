using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using UaiFuel.Models.Domain;
using UaiFuel.Models.ExceptionCore;
using UaiFuel.Models.Service;
using UaiFuel.Models.Utils;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Controllers
{
    public class MotoristaController : Controller       
    {
        private readonly MotoristaService service;        

        public MotoristaController()
        {
            service = new MotoristaService();
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


        public IActionResult Create()
        {
            
            return View(new CreateMotoristaViewModel());
        }


        [HttpPost]
        public IActionResult Create(CreateMotoristaViewModel vm)
        {
            // Validação
            if ( ! ModelState.IsValid )
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                IEnumerable<string> messages = allErrors.Select(x => x.ErrorMessage);

                FrontUtil.SetupErrorsViewBag(this, messages);

                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }                

                return View(vm);
            }

            Motorista motorista = new Motorista
            { 
                Id = vm.Id,
                Nome = vm.Nome,
                Login = vm.Login,
                Senha = vm.Senha,
                CPF = vm.CPF,
                Credito = Convert.ToDecimal(vm.Credito),
                Status = Status.getInstance(Convert.ToInt32(vm.CodigoStatus))
            };
            motorista.Emails.Add(new Email { Endereco = vm.NovoEmail });

            try
            {
                service.Create(motorista);
                return RedirectToAction("List");
            }
            catch (AppException exp)
            {
                FrontUtil.SetupViewBag(this, exp);
                return View(vm);
            }
            /*
            catch (Exception exp)
            {
                AppException exception = new AppException();

                HttpContext.Session.SetString("exp", JsonSerializer.Serialize<Exception>(exception));
                return View("/Error/Error");
            }
            */
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            Motorista motorista = service.Read(id);
            UpdateMotoristaViewModel vm = new UpdateMotoristaViewModel(motorista);

            return View(vm);
        }
        
        [HttpPost]
        public IActionResult Update(int id, UpdateMotoristaViewModel vm)
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

                Motorista motorista = service.Read(id);
                return View(new UpdateMotoristaViewModel(motorista));
            }

            if (vm.NovoEmail != null)
            {   
                Email email = new Email
                {
                    PessoaId = id,
                    Endereco = vm.NovoEmail
                };
                service.AdicionarEmail(email);

                return RedirectToRoute(new { controller = "Motorista", action = "Update", id });
            }
            else
            {
                Motorista motorista = new Motorista
                {
                    Id = vm.Id,
                    Nome = vm.Nome,
                    Status = Status.getInstance(Convert.ToInt32(vm.CodigoStatus)),
                    Credito = Convert.ToDecimal(vm.Credito),
                    CPF = vm.CPF
                };
                try
                {   
                    service.Update(motorista);
                    return RedirectToAction("List");
                }
                catch (AppException exp)
                {
                    FrontUtil.SetupViewBag(this, exp);
                    return View(vm);
                }
            }
        }

        

        /*
        [HttpGet]
        public IActionResult AdicionarEmail(int id)
        {
            Motorista motorista = service.Read(id);
            var vm = new EmailViewModel
            { 
                Id = motorista.Id,
                Nome = motorista.Nome
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

            return RedirectToRoute(new { controller = "Motorista", action = "Update", id = id });
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

            return RedirectToRoute(new { controller = "Motorista", action = "Update", id });
        }
    }
}

