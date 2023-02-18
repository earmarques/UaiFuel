using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class PostoViewModel : PessoaViewModel
    {

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Campo CNPJ é obrigatório.")]
        public string CNPJ { get; set; }


        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "Campo Telefone é obrigatório.")]
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }


        // Endereco     -------------------------------------------------------------------------------------

        public int EnderecoId { get; set; }


        [Display(Name = "Localidade")]
        [Required(ErrorMessage = "Campo Localidade é obrigatório.")]
        public string Localidade { get; set; }


        [Display(Name = "CEP")]
        [Required(ErrorMessage = "Campo CEP é obrigatório.")]
        public string CEP { get; set; }


        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "Campo Cidade é obrigatório.")]
        public string CidadeId { get; set; }

        public string CidadeIdHidden { get; set; }
        


        [Display(Name = "UF")]
        [Required(ErrorMessage = "Campo Estado obrigatório.")]
        public string EstadoUF { get; set; }


        public List<SelectListItem> Estados { get; set; }
        public List<SelectListItem> Cidades { get; set; }


        // Cosntructors     ---------------------------------------------------------------------------------

        public PostoViewModel() 
            : base()
        {
            Estados = new List<SelectListItem>();
            Cidades = new List<SelectListItem>();
        }


        public PostoViewModel(Posto posto) 
            : base(posto) 
        {
            Estados = new List<SelectListItem>();
            Cidades = new List<SelectListItem>();

            Telefone = posto.Telefone;
            CNPJ = posto.CNPJ;

            Endereco endereco = posto.Endereco;
            if (endereco != null)
            { 
                EnderecoId = endereco.Id;
                CEP = endereco.CEP;
                Localidade = endereco.Localidade;
                CidadeId = endereco.Cidade.Id.ToString();
                EstadoUF = endereco.Cidade.Estado.UF;            
            }
        }
    }
}



