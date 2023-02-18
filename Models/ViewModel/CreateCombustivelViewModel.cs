using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class CreateCombustivelViewModel : CombustivelViewModel
    {

        [Display(Name = "Posto")]
        [Required(ErrorMessage = "Campo Posto é obrigatório.")]
        public string PostoId { get; set; }


        public CreateCombustivelViewModel() 
            : base()
        {
            Postos = new List<SelectListItem>();                        
        }


        public CreateCombustivelViewModel(Combustivel combustivel) 
            : base(combustivel)
        {
            PostoId = combustivel.Posto.Id.ToString();
        }
    }
}
