using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UaiFuel.Models.Domain
{
    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Estado Estado { get; set; }
    }
}
