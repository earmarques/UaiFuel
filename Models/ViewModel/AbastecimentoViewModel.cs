using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class AbastecimentoViewModel
    {

        public int Id { get; set; }
        public string CupomFiscal { get; set; }
        
        [DataType(DataType.Currency)]
        public string ValorTotal { get; set; }


        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status é campo obrigatório.")]
        public string CodigoStatus { get; set; }
        public List<SelectListItem> Statuses { get; private set; }


        [Display(Name = "Posto")]
        [Required(ErrorMessage = "Selecione em qual Posto está ocorrendo o abastecimento.")]
        public string PostoIdHidden { get; set; }
        public string PostoId { get; set; }
        public string NomePosto { get; set; }
        public List<SelectListItem> Postos { get; set; }


        [Display(Name = "Motorista")]
        [Required(ErrorMessage = "Selecione qual Motorista está realizando o abastecimento.")]
        public string MotoristaId { get; set; }
        public List<SelectListItem> Motoristas { get; set; }


        [Display(Name = "Veículo")]
        [Required(ErrorMessage = "Selecione o Veículo que está sendo abastecido.")]
        public string VeiculoPlaca { get; set; }
        public List<SelectListItem> Veiculos { get; set; }


        public int CombustivelId { get; set; }
        public List<SelectListItem> Combustiveis { get; set; }


        public string NomeCombustivel { get; set; }
        public string Litros { get; set; }
        public string Valor { get; set; }
        public bool IsBotaoAdicionarCombustivel { get; set; }

        public List<ItemViewModel> Items { get; set; }

        public AbastecimentoViewModel() 
        {
            IsBotaoAdicionarCombustivel = false;
            Postos = new List<SelectListItem>();
            Motoristas = new List<SelectListItem>();
            Veiculos = new List<SelectListItem>();
            Combustiveis = new List<SelectListItem>();

            CodigoStatus = StatusAbastecimento.PENDENTE_PAGTO.Codigo.ToString();
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
