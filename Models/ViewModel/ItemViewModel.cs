using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class ItemViewModel
    {
        public int CombustivelId { get; set; }
        public string NomeCombustivel { get; set; }
        public int PostoId { get; set; }
        public string NomePosto { get; set; }
        public string Litros { get; set; }
        public string Valor { get; set; }
    }


}
