using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class UserViewModel
    {
        
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Campo Nome é obrigatório.")]
        [MinLength(3,ErrorMessage = "O login requer no mínimo três caracteres.")]
        public string Login { get; set; }


        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Campo Senha é obrigatório.")]
        [MinLength(3, ErrorMessage = "A senha requer no mínimo três caracteres.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public int CodigoStatus { get; set; }
        public string Nome { get; set; }

        public string Tipo { get; set; }
        public int PessoaId { get; set; }

    } 
}
