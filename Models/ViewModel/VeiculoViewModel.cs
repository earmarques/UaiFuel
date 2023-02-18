using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class VeiculoViewModel
    {
        [Display(Name = "Placa")]
        [Required(ErrorMessage = "Placa do Veículo é campo obrigatório.")]
        public string Placa { get; set; }   // PK


        [Display(Name = "Cor")]
        [Required(ErrorMessage = "Cor é campo obrigatório.")]
        public string Cor { get; set; }


        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "Modelo é campo obrigatório.")]
        public string Modelo { get; set; }


        [Display(Name = "Ano Fabricação")]
        [Required(ErrorMessage = "Ano de Fabricação é campo obrigatório.")]
        public string AnoFabricacao { get; set; }


        [Display(Name = "Ano Modelo")]
        [Required(ErrorMessage = "Ano Modelo é campo obrigatório.")]
        public string AnoModelo { get; set; }


        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status é campo obrigatório.")]
        public string CodigoStatus { get; set; }

        public List<SelectListItem> Statuses { get; set; }
        
        public VeiculoViewModel() 
        {
            Statuses = new List<SelectListItem>();
            foreach (var status in StatusVeiculo.lista)
            {
                Statuses.Add(
                    new SelectListItem
                    {
                        Value = status.Codigo.ToString(),
                        Text = status.Descricao
                    });
            }            
        }


        public VeiculoViewModel(Veiculo veiculo) :
            this()
        {
            Placa = veiculo.Placa;
            Cor = veiculo.Cor;
            Modelo = veiculo.Modelo;
            AnoFabricacao = veiculo.AnoFabricacao.ToString();
            AnoModelo = veiculo.AnoModelo.ToString();
            CodigoStatus = veiculo.Status.Codigo.ToString();
        }

    }

}
