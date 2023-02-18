using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class MotoristaViewModel: PessoaViewModel
    {
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Campo CPF é obrigatório.")]
        public string CPF { get; set; }


        [DataType(DataType.Currency)]
        [Display(Name = "Crédito")]
        [Required(ErrorMessage = "Campo Crédito é obrigatório.")]
        public string Credito { get; set; }


        public MotoristaViewModel() 
            : base()
        {               
        }


        public MotoristaViewModel(Motorista motorista) 
            : base(motorista)
        {
            Credito = Convert.ToDecimal(decimal.Round(motorista.Credito, 2, 
                                                      MidpointRounding.AwayFromZero), 
                                        new CultureInfo("pt-BR")).ToString();
            CPF = motorista.CPF;
        }
    }
}

