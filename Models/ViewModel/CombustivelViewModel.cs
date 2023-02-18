using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class CombustivelViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Campo Status é obrigatório.")]
        public string CodigoStatus { get; set; }

        public List<SelectListItem> Statuses { get; private set; }
        public List<SelectListItem> Postos { get; set; }

        public CombustivelViewModel() 
        {
            CodigoStatus = StatusCombustivel.EM_ESTOQUE.Codigo.ToString();
            Statuses = new List<SelectListItem>();
            foreach (var status in StatusCombustivel.lista)
            {
                Statuses.Add(
                    new SelectListItem
                    {
                        Value = status.Codigo.ToString(),
                        Text = status.Descricao
                    });
            }
        }


        public CombustivelViewModel(Combustivel combustivel) :
            this()
        {
            Id = combustivel.Id;
            Nome = combustivel.Nome;
            CodigoStatus = combustivel.Status.Codigo.ToString();                        
        }
    }
}
