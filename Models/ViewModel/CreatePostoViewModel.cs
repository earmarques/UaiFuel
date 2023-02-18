using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class CreatePostoViewModel : PostoViewModel
    {

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Campo E-mail é obrigatório.")]
        public string NovoEmail { get; set; }


        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        // Constructors     ---------------------------------------------------------------------------------

        public CreatePostoViewModel()
            : base()
        {
        }
        public CreatePostoViewModel(Posto posto) 
            : base(posto)
        {
        }
    }
}



