using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class CreateMotoristaViewModel : MotoristaViewModel
    {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Campo E-mail é obrigatório.")]
        public string NovoEmail { get; set; }


        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public CreateMotoristaViewModel()
            : base()
        {
        }
        public CreateMotoristaViewModel(Motorista motorista)
            : base(motorista)
        {
        }
    }
}


