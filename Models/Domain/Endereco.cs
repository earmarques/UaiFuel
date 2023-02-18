using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UaiFuel.Models.Domain
{
    public class Endereco
    {
        public int Id { get; set; }
        public int PostoId { get; set; }
        public string Localidade { get; set; }
        public string CEP { get; set; }
        public Cidade Cidade { get; set; }


        public Endereco() { }


        public override string ToString()
        {
            return $"PostoId:{PostoId} - Id{Id}";
        }
    }
}
