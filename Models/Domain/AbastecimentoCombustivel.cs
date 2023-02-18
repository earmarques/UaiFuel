using System;
using System.Collections.Generic;
using System.Text;

namespace UaiFuel.Models.Domain
{
    public class AbastecimentoCombustivel
    {
        
        // Properties   -------------------------------------------------------------------------------------
                
        public AbastecimentoCombustivelId Pk { get; set; }  // PK
        public Abastecimento Abastecimento { get; set; }
        public Combustivel Combustivel { get; set; }
        public decimal Valor { get; set; }
        public decimal Litros { get; set; }

        
        // Constructors  -------------------------------------------------------------------------------------

        public AbastecimentoCombustivel() { }

        public AbastecimentoCombustivel(Abastecimento abastecimento, Combustivel combustivel)
        {
            Abastecimento = abastecimento ?? throw new ArgumentNullException(nameof(abastecimento));
            Combustivel = combustivel ?? throw new ArgumentNullException(nameof(combustivel));

            this.Pk = new AbastecimentoCombustivelId(abastecimento.Id, combustivel.Id);
        }


        // Override's Methods     ---------------------------------------------------------------------------

        public override string ToString()
        {
            return this.Pk.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            if (obj == null || !(obj is AbastecimentoCombustivel))
            {
                return false;
            }

            AbastecimentoCombustivel that = (AbastecimentoCombustivel) obj;
            return this.Abastecimento.Equals(that.Abastecimento) &&
                   this.Combustivel.Equals(that.Combustivel);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Abastecimento, Combustivel);
        }
    }
}
