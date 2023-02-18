using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class UpdateCombustivelViewModel : CombustivelViewModel
    {

        public string Posto { get; set; }

        public UpdateCombustivelViewModel() 
            : base()
        {
        }


        public UpdateCombustivelViewModel(Combustivel combustivel) 
            : base(combustivel)
        {
            Posto = combustivel.Posto.Nome;
        }
    }
}
