using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class UpdateAbastecimentoViewModel
    {

        public int Id { get; set; }
        public string CupomFiscal { get; set; }

        [DataType(DataType.Currency)]
        public string ValorTotal { get; set; }


        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status é campo obrigatório.")]
        public string CodigoStatus { get; set; }
        public List<SelectListItem> Statuses { get; private set; }


        public string PostoId { get; set; }
        public string NomePosto { get; set; }


        public string MotoristaId { get; set; }
        public string NomeMotorista { get; set; }


        public string VeiculoPlaca { get; set; }

        public List<ItemViewModel> Items { get; set; }

        public UpdateAbastecimentoViewModel()
        {
            Items = new List<ItemViewModel>();

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

        public UpdateAbastecimentoViewModel(Abastecimento abastecimento)
            : this()
        {
            Id = abastecimento.Id;
            CupomFiscal = abastecimento.CupomFiscal;
            ValorTotal = decimal.Round(abastecimento.ValorTotal, 2, MidpointRounding.AwayFromZero).ToString();
            
            CodigoStatus = abastecimento.Status.Codigo.ToString();

            PostoId = abastecimento.Posto.Id.ToString();
            NomePosto = abastecimento.Posto.Nome;
            MotoristaId = abastecimento.Motorista.Id.ToString();
            NomeMotorista = abastecimento.Motorista.Nome;
            VeiculoPlaca = abastecimento.Veiculo.Placa;

            if (abastecimento.Combustiveis != null)
            {
                foreach (AbastecimentoCombustivel ac in abastecimento.Combustiveis)
                { 
                    this.Items.Add(
                        new ItemViewModel()
                        {
                            CombustivelId = ac.Combustivel.Id,
                            NomeCombustivel = ac.Combustivel.Nome,
                            PostoId = ac.Combustivel.Posto.Id,
                            NomePosto = ac.Combustivel.Posto.Nome,
                            Litros = ac.Litros.ToString(),
                            Valor = decimal.Round(ac.Valor, 2, MidpointRounding.AwayFromZero).ToString()
                        });
                }
            }
        }
    }
}
