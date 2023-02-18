using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;
using UaiFuel.Models.Service;
using UaiFuel.Models.Utils;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Controllers
{
    public class CombustivelController : Controller       
    {
        private readonly CombustivelService service;
        private readonly PostoService postoService;

        public CombustivelController()
        {
            service = new CombustivelService();
            postoService = new PostoService();
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
            CreateCombustivelViewModel vm = new CreateCombustivelViewModel();
            CarregarPostos(vm);
            return View(vm);
        }

        
        [HttpPost]
        public IActionResult Create(CreateCombustivelViewModel vm)
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
                CarregarPostos(vm);

                return View(vm);
            }
            
            Combustivel combustivel = new Combustivel
            {
                Posto = new Posto { Id = Convert.ToInt32(vm.PostoId)},
                Nome = vm.Nome,
                Status = StatusCombustivel.getInstance(Convert.ToInt32(vm.CodigoStatus))
            };
            service.Create(combustivel);

            return RedirectToAction("List");
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            Combustivel combustivel = service.Read(id);  
            var vm = new UpdateCombustivelViewModel(combustivel);

            return View(vm);
        }
        

        [HttpPost]
        public IActionResult Update(int id, UpdateCombustivelViewModel vm)
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

                return View(vm);
            }

            Combustivel combustivel = new Combustivel
            {
                Id = id,
                Nome = vm.Nome,
                Status = StatusCombustivel.getInstance(Convert.ToInt32(vm.CodigoStatus))
            };
            service.Update(combustivel);
            
            return RedirectToAction("List");
        }


        private void CarregarPostos(CreateCombustivelViewModel vm)
        {
            IList<Posto> postos = postoService.Read();
            foreach (Posto posto in postos)
            {
                vm.Postos.Add(
                    new SelectListItem
                    {
                        Value = posto.Id.ToString(),
                        Text = posto.Nome
                    });
            }
        }
    }
}
