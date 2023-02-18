using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class UpdateMotoristaViewModel : MotoristaViewModel
    {
        public string NovoEmail { get; set; }

        public UpdateMotoristaViewModel() 
            : base()
        {
        }
        public UpdateMotoristaViewModel(Motorista motorista)
            : base(motorista)               
        {             
        }
    }
}
