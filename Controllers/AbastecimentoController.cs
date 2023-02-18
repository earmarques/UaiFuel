using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;
using UaiFuel.Models.Service;
using UaiFuel.Models.Utils;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Controllers
{
    public class AbastecimentoController : Controller       
    {
        private readonly AbastecimentoService service;
        private readonly CombustivelService combustivelService;
        private readonly PostoService postoService;
        private readonly MotoristaService motoristaService;
        private readonly VeiculoService veiculoService;

        public AbastecimentoController()
        {
            service = new AbastecimentoService();
            combustivelService = new CombustivelService();
            postoService = new PostoService();
            motoristaService = new MotoristaService();
            veiculoService = new VeiculoService();
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


        public IActionResult Pesquisar()
        {
            PesquisaAbastecimentoViewModel vm = new PesquisaAbastecimentoViewModel();
            CarregarListaCombosPesquisa(vm);
            return View(vm);
        }

        
        [HttpPost]
        public IActionResult Pesquisar(PesquisaAbastecimentoViewModel vm)
        {
            vm.DataInicial = Convert.ToDateTime(vm.StrDataInicial);
            vm.DataFinal   = Convert.ToDateTime(vm.StrDataFinal);
            if (vm.DataFinal != default(DateTime))
            {
                vm.DataFinal = vm.DataFinal.AddDays(1);
            }
            vm.Abastecimentos = service.Pesquisar(vm);
            vm.RealizouPesquisa = true;
            CarregarListaCombosPesquisa(vm);
            return View(vm);
        }


        public IActionResult Create()
        {
            // Caso tenha itens_combustível e valor total na sessão de outro abastecimento,
            // vamos limpar tudo
            FrontUtil.ExtractFromSession<List<ItemViewModel>>(this.HttpContext, "itens");
            FrontUtil.ExtractFromSession<decimal>(this.HttpContext, "total"); 
            AbastecimentoViewModel vm = new AbastecimentoViewModel();
            CarregarListaCombos(vm);
            return View(vm);
        }

        
        [HttpPost]
        public JsonResult GetCombustiveis(int postoId)
        {
            List<List<string>> combustiveisJson = new List<List<string>>();


            IList<Combustivel> combustiveis = combustivelService.ReadByPosto(postoId);
            foreach (Combustivel combustivel in combustiveis)
            {
                List<string> dict = new List<string>
                {
                    combustivel.Id.ToString(),
                    combustivel.Nome
                };
                combustiveisJson.Add(dict);
                }

            return new JsonResult(combustiveisJson);
        }


        [HttpPost]
        public IActionResult Create(AbastecimentoViewModel vm)
        {
            if (vm.IsBotaoAdicionarCombustivel)
            {
                // Limpar erros de validações - não vou criar abastecimento ainda
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                IEnumerable<string> messages = allErrors.Select(x => x.ErrorMessage);

                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }

                // Cria item da linha da tabela
                ItemViewModel item = new ItemViewModel()
                {
                    CombustivelId = vm.CombustivelId,
                    NomeCombustivel = vm.NomeCombustivel,
                    PostoId = Convert.ToInt32(vm.PostoId),
                    NomePosto = vm.NomePosto,
                    Litros = vm.Litros,
                    Valor = vm.Valor
                };

                // Resgatar da sessão os item adicionados
                List<ItemViewModel> itens = FrontUtil.GetFromSession<List<ItemViewModel>>(
                        this.HttpContext, "itens");
                if (itens == null) itens = new List<ItemViewModel>();

                // Adiciona novo item combustível
                itens.Add(item);

                // Readiciona à sessão os a lista de combustível - itens 
                FrontUtil.AddToSession<List<ItemViewModel>>(this, "itens", itens);

                // Valor total do abastecimento
                decimal total, litros, valor;
                total = litros = valor = 0;
                foreach (var it in itens)
                {
                    litros = Convert.ToDecimal(it.Litros, new CultureInfo("pt-BR"));
                    valor = Convert.ToDecimal(it.Valor, new CultureInfo("pt-BR"));
                    total += valor * litros;
                }

                CarregarListaCombos(vm);
                //vm.Litros = null;
                //vm.Valor = null;
                //vm.ValorTotal = total.ToString();

                FrontUtil.AddToSession<decimal>(this, "total", total);
                ViewBag.FixarPosto = true;

                return View(vm);
            }

            else 
            {
                // Combustíveis
                List<ItemViewModel> itens = FrontUtil.ExtractFromSession<List<ItemViewModel>>(
                        this.HttpContext, "itens");
                
                // Valor total do abastecimento
                decimal valorTotal, litros, valor;
                valorTotal = litros = valor = 0;
                foreach (var it in itens)
                {
                    litros = Convert.ToDecimal(it.Litros, new CultureInfo("pt-BR"));
                    valor = Convert.ToDecimal(it.Valor, new CultureInfo("pt-BR"));
                    valorTotal += valor * litros;
                }
                
                // Abastecimento
                Abastecimento abastecimento = new Abastecimento()
                {
                    CupomFiscal = vm.CupomFiscal,
                    DataCriacao = DateTime.Now,
                    Motorista = new Motorista() { Id = Convert.ToInt32(vm.MotoristaId) },
                    Posto = new Posto() { Id = Convert.ToInt32(vm.PostoId) },
                    Veiculo = new Veiculo() { Placa = vm.VeiculoPlaca },
                    Status = StatusAbastecimento.getInstance(Convert.ToInt32(vm.CodigoStatus)), 
                    ValorTotal = valorTotal
                };

                // Adicionando Combustíveis ao Abastecimento
                foreach (var it in itens)
                {
                    litros = Convert.ToDecimal(it.Litros, new CultureInfo("pt-BR"));
                    valor = Convert.ToDecimal(it.Valor, new CultureInfo("pt-BR"));

                    Combustivel combustivel = new Combustivel() 
                    { 
                        Id = it.CombustivelId,
                        Posto = new Posto { Id = it.PostoId }
                    };
                    abastecimento.Combustiveis.Add(
                    new AbastecimentoCombustivel(abastecimento, combustivel)
                    {
                        Litros = litros,
                        Valor = valor
                    });
                }

                // Persistir Abastecimento
                service.Create(abastecimento);
            }
            return RedirectToAction("List");
        }



        [HttpGet]
        public IActionResult Update(int id)
        {
            Abastecimento abastecimento = service.Read(id);
            UpdateAbastecimentoViewModel vm = new UpdateAbastecimentoViewModel(abastecimento);

            return View(vm);
        }


        [HttpPost]
        public IActionResult Update(int id, UpdateAbastecimentoViewModel vm)
        {
            Abastecimento abastecimento = null;
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                IEnumerable<string> messages = allErrors.Select(x => x.ErrorMessage);

                FrontUtil.SetupErrorsViewBag(this, messages);

                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }

                abastecimento = service.Read(id);
                return View(new UpdateAbastecimentoViewModel(abastecimento));
            }

            abastecimento = new Abastecimento()
            {
                Id = vm.Id,
                CupomFiscal = vm.CupomFiscal,
                Status = StatusAbastecimento.getInstance(Convert.ToInt32(vm.CodigoStatus))
            };
            service.Update(abastecimento);

            return RedirectToAction("List");
        }




        private void CarregarListaPosto(AbastecimentoViewModel vm)
        {
            IList<Posto> postos = postoService.Read();
            if (vm.Postos == null) vm.Postos = new List<SelectListItem>();
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


        private void CarregarListaMotorista(AbastecimentoViewModel vm)
        {
            IList<Motorista> motoristas = motoristaService.Read();
            if (vm.Motoristas == null) vm.Motoristas = new List<SelectListItem>();
            foreach (Motorista motorista in motoristas)
            {
                vm.Motoristas.Add(
                    new SelectListItem
                    {
                        Value = motorista.Id.ToString(),
                        Text = motorista.Nome
                    });
            }
        }


        private void CarregarListaVeiculo(AbastecimentoViewModel vm)
        {
            IList<Veiculo> veiculos = veiculoService.Read();
            if (vm.Veiculos == null) vm.Veiculos = new List<SelectListItem>();
            foreach (Veiculo veiculo in veiculos)
            {
                vm.Veiculos.Add(
                    new SelectListItem
                    {
                        Value = veiculo.Placa,
                        Text = veiculo.Placa
                    });
            }
        }


        private void CarregarListaCombustivelPosto(AbastecimentoViewModel vm)
        {
            IList<Combustivel> combustiveis = combustivelService.ReadByPosto(
                                                Convert.ToInt32(vm.PostoIdHidden));
            if (vm.Combustiveis == null) 
                vm.Combustiveis = new List<SelectListItem>();

            foreach (Combustivel combustivel in combustiveis)
            {
                vm.Combustiveis.Add(
                    new SelectListItem
                    {
                        Value = combustivel.Id.ToString(),
                        Text = combustivel.Nome
                    });
            }
        }

        private void CarregarListaCombos(AbastecimentoViewModel vm)
        {
            CarregarListaPosto(vm);
            CarregarListaMotorista(vm);
            CarregarListaVeiculo(vm);
        }


        [HttpGet]
        public IActionResult RemoverItem(int id)
        {
            List<ItemViewModel> itens = FrontUtil.GetFromSession<List<ItemViewModel>>(
                this.HttpContext, "itens");

            itens.RemoveAll(it => it.CombustivelId == id);
            
            FrontUtil.AddToSession<List<ItemViewModel>>(this, "itens", itens);

            
            return View();
        }


        private void CarregarListaCombosPesquisa(PesquisaAbastecimentoViewModel vm)
        {
            vm.Motoritas = motoristaService.GetPessoasViewModel();
            vm.Postos = postoService.GetPessoasViewModel();
            vm.Veiculos = veiculoService.Read();
        }

    }
}
