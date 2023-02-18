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
    [Route("veiculo")]
    public class VeiculoController : Controller       
    {
        private readonly VeiculoService service;

        public VeiculoController()
        {
            service = new VeiculoService();
        }

        [Route("list")]
        public IActionResult List()
        {   
            return View(service.Read());
        }


        [Route("delete/{placa}")]
        public IActionResult Delete(string placa)
        {
            service.Delete(placa);
            return RedirectToAction("List");
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View(new VeiculoViewModel());
        }


        [HttpPost]
        [Route("create")]
        public IActionResult Create(VeiculoViewModel vm)
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
            Veiculo veiculo = new Veiculo
            { 
                Placa = vm.Placa,
                Cor = vm.Cor,
                Modelo = vm.Modelo,
                AnoFabricacao = Convert.ToInt32(vm.AnoFabricacao),
                AnoModelo = Convert.ToInt32(vm.AnoModelo),
                Status = StatusVeiculo.getInstance(Convert.ToInt32(vm.CodigoStatus))
            };
            try
            {
                service.Create(veiculo);
                return RedirectToAction("List");
            }
            catch (AppException exp)
            {
                FrontUtil.SetupViewBag(this, exp);
                return View(vm);
            }
        }


        [HttpGet]
        [Route("update/{placa}")]
        public IActionResult Update(string placa)
        {
            Veiculo veiculo = service.Read(placa);
            var vm = new VeiculoViewModel(veiculo);

            return View(vm);
        }
        
        [HttpPost]
        [Route("update/{placa}")]
        public IActionResult Update(string placa, VeiculoViewModel vm)
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

            Veiculo veiculo = new Veiculo
            {
                Placa = vm.Placa,
                Cor = vm.Cor,
                Modelo = vm.Modelo,
                AnoFabricacao = Convert.ToInt32(vm.AnoFabricacao),
                AnoModelo = Convert.ToInt32(vm.AnoModelo),
                Status = StatusVeiculo.getInstance(Convert.ToInt32(vm.CodigoStatus))
            };
            try
            {
                service.Update(veiculo);
                return RedirectToAction("List");
            }
            catch (AppException exp)
            {
                FrontUtil.SetupViewBag(this, exp);
                return View(vm);
            }            
        }
    }
}
