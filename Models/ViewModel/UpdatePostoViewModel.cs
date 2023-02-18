using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.ViewModel
{
    public class UpdatePostoViewModel : PostoViewModel
    {

        public string NovoEmail { get; set; }


        // Constructors     ---------------------------------------------------------------------------------

        public UpdatePostoViewModel()
            : base()
        {
        }
        public UpdatePostoViewModel(Posto posto)
            : base(posto)
        {
        }
    }
}



