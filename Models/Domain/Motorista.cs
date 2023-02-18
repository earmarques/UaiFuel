using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UaiFuel.Models.DAO;

namespace UaiFuel.Models.Domain
{
    /** 
     * POCO Design Pattern
     */

    public class Motorista : Pessoa
    {
        // Properties   -------------------------------------------------------------------------------------

        public string CPF { get; set; }
        public decimal Credito { get; set; }



        // Constructors  -------------------------------------------------------------------------------------
        
        public Motorista() { }

        public override string ToString()
        {
            return "Motorista: " + base.ToString();
        }
    }
}
