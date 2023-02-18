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

    public class Administrador : Pessoa
    {
        public override string ToString()
        {
            return "Administrador: " + base.ToString();
        }
    }
}
