using System;
using System.Collections.Generic;
using System.Text;

namespace UaiFuel.Models.Domain
{
    /** 
     * Value Object Design Pattern
     */
    public class AbastecimentoCombustivelId
    {
        // Properties   -------------------------------------------------------------------------------------

        public int AbastecimentoId { get; set; }
        public int CombustivelId   { get; set; }

        
        // Constructors  -------------------------------------------------------------------------------------

        public AbastecimentoCombustivelId() { }

        public AbastecimentoCombustivelId(int abastecimentoId, int combustivelId)
        {
            AbastecimentoId = abastecimentoId;
            CombustivelId = combustivelId;
        }


        // Override's Methods     ---------------------------------------------------------------------------
        
        public override string ToString()
        {
            return $"AbastId:{AbastecimentoId} - CombId:{CombustivelId}";
        }

        public override bool Equals(object obj)
        {
            return obj is AbastecimentoCombustivelId id &&
                   AbastecimentoId == id.AbastecimentoId &&
                   CombustivelId   == id.CombustivelId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AbastecimentoId, CombustivelId);
        }
    }
}
