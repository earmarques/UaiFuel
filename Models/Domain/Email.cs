using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UaiFuel.Models.Domain
{
    public class Email
    {
        public int PessoaId { get; set; }
        public string Endereco { get; set; }


        public Email() { }
        
        public Email(int pessoaId, string endereco) 
        {
            PessoaId = pessoaId;
            Endereco = endereco;
        }


        public override string ToString()
        {
            return $"{PessoaId} - {Endereco}";
        }
    }
}
