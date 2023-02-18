using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class PessoaViewModel
    {
        public int Id { get; set; }


        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Nome é obrigatório.")]
        [MinLength(3)]
        public string Nome { get; set; }


        [Display(Name = "Login")]
        [Required(ErrorMessage = "Campo Login é obrigatório.")]
        [MinLength(3)]
        public string Login { get; set; }


        [Display(Name = "Status")]
        [Required(ErrorMessage = "Campo Status é obrigatório.")]
        public string CodigoStatus { get; set; }


        public List<SelectListItem> Statuses { get; set; }


        public PessoaViewModel()
        {
            CodigoStatus = Status.ATIVO.Codigo.ToString();
            Statuses = new List<SelectListItem>();
            Emails = new List<Email>();

            foreach (var status in Status.lista)
            {
                Statuses.Add(
                    new SelectListItem
                    {
                        Value = status.Codigo.ToString(),
                        Text = status.Descricao
                    });
            }
        }


        public List<Email> Emails { get; set; }


        public PessoaViewModel(Pessoa pessoa) :
            this()
        {
            Id = pessoa.Id;
            Nome = pessoa.Nome;
            Login = pessoa.Login;
            CodigoStatus = pessoa.Status.Codigo.ToString();
            Emails = pessoa.Emails;
        }
    }
}