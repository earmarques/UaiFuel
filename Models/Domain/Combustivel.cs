using System;
using System.Collections.Generic;
using System.Text;
using UaiFuel.Models.DAO;

namespace UaiFuel.Models.Domain
{

    /** 
     * POCO Design Pattern
     */

    public class Combustivel : IDomainObject
    {

        // Properties   -------------------------------------------------------------------------------------

        public int Id { get; set; }
        public Posto Posto { get; set; }
        public string Nome { get; set; }        
        public StatusCombustivel Status { get; set; }


        // Constructors  -------------------------------------------------------------------------------------

        public Combustivel() { }


        // Override's Methods     ---------------------------------------------------------------------------

        public override string ToString()
        {
            return $"{Id} - {Nome}";
        }

        public override bool Equals(object obj)
        {
            return obj is Combustivel combustivel &&
                    Posto == combustivel.Posto &&
                    Id == combustivel.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
