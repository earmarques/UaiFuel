using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class PesquisaAbastecimentoViewModel
    {
        public string CombustivelId { get; set; }
        public string NomeCombustivel { get; set; }
        public string VeiculoPlaca { get; set; }
        public string PostoId { get; set; }
        public string NomePosto { get; set; }
        public string MotoristaId { get; set; }
        public string NomeMotorista { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string StrDataInicial { get; set; }
        public string StrDataFinal { get; set; }


        public string CodigoStatus { get; set; }
        public List<SelectListItem> Statuses { get; set; }
        public IList<Abastecimento> Abastecimentos { get; set; }
        public IList<PessoaViewModel> Postos { get; set; }
        public IList<PessoaViewModel> Motoritas { get; set; }
        public IList<Veiculo> Veiculos { get; set; }

        public bool RealizouPesquisa { get; set; }

        public PesquisaAbastecimentoViewModel()
        {
            RealizouPesquisa = false;

            Abastecimentos = new List<Abastecimento>();
            Postos = new List<PessoaViewModel>();
            Motoritas = new List<PessoaViewModel>();
            Veiculos = new List<Veiculo>();

            Statuses = new List<SelectListItem>();
            foreach (var status in StatusAbastecimento.lista)
            {
                Statuses.Add(
                    new SelectListItem
                    {
                        Value = status.Codigo.ToString(),
                        Text = status.Descricao
                    });
            }
        }
    }
}

