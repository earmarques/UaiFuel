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
    public class Posto : Pessoa
    {
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }


        public Posto() { }


        public override string ToString()
        {
            return "Posto: " + base.ToString();
        }
    }
}
