using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UaiFuel.Models.Domain
{

    /** 
     * POCO Design Pattern
     */
    public class Pessoa : IDomainObject
    {

        // Properties   -------------------------------------------------------------------------------------

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public Status Status { get; set; }
        public List<Email> Emails { get; set; }


        // Constructors  -------------------------------------------------------------------------------------

        public Pessoa() 
        {
            Status = Status.ATIVO;
            Emails = new List<Email>();
        }


        // Override's Methods     ---------------------------------------------------------------------------

        public override string ToString()
        {
            return $"{Id} - {Nome}";
        }

        public override bool Equals(object obj)
        {
            return obj is Pessoa pessoa &&
                   Id == pessoa.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
