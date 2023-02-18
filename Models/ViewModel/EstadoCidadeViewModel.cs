using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UaiFuel.Models.ViewModel
{
    public class EstadoCidadeViewModel
    {
        public List<SelectListItem> NomesEstado { get; set; }
        public List<SelectListItem> NomesCidade { get; set; }
        public string EstadoUf { get; set; }
        public int    CidadeId { get; set; }

        public EstadoCidadeViewModel()  
        {
            NomesEstado = new List<SelectListItem>();
            NomesCidade = new List<SelectListItem>();
        }
    }

}


