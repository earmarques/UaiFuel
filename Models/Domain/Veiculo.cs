
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UaiFuel.Models.DAO;

namespace UaiFuel.Models.Domain
{

    public class Veiculo : IDomainObject
    {
        
        // Properties   -------------------------------------------------------------------------------------

        public string Placa { get; set; }   // PK
        public string Cor { get; set; }
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public int AnoModelo { get; set; }
        public StatusVeiculo Status { get; set; }


        // Constructors     ---------------------------------------------------------------------------------

        public Veiculo() 
        {
            Status = StatusVeiculo.ATIVO;
        }


        // Object Overrides Methods     ---------------------------------------------------------------------   

        public override string ToString()
        {
            return $"Veículo: placa {Placa}";
        }

        public override bool Equals(object obj)
        {
            return obj is Veiculo veiculo &&
                   Placa == veiculo.Placa;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Placa);
        }
    }
}
