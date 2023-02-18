using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class EmailViewModel
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string NovoEmail { get; set; }
    }
}



