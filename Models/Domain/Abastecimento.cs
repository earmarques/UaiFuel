using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UaiFuel.Models.DAO;


namespace UaiFuel.Models.Domain
{
 
    public class Abastecimento : IDomainObject
    {
        // Properties   -------------------------------------------------------------------------------------

        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string CupomFiscal { get; set; }
        public decimal ValorTotal { get; set; }
        public StatusAbastecimento Status { get; set; }
        public IList<AbastecimentoCombustivel> Combustiveis { get; set; }
        // Composições
        public Posto Posto { get; set; }
        public Motorista Motorista { get; set; }
        public Veiculo Veiculo { get; set; }


        // Constructors     ---------------------------------------------------------------------------------

        public Abastecimento()
        {
            DataCriacao = DateTime.Now;
            Status = StatusAbastecimento.PENDENTE_PAGTO;
            Combustiveis = new List<AbastecimentoCombustivel>();
        }


        // Methods      -------------------------------------------------------------------------------------

        
        public void RemoverCombustivel(Combustivel combustivel)
        {
            IEnumerator<AbastecimentoCombustivel> enumerator = Combustiveis.GetEnumerator();
            while( enumerator.MoveNext() )
            {
                AbastecimentoCombustivel ac = enumerator.Current;

                if ( ac.Abastecimento.Equals(this) && ac.Combustivel.Equals(combustivel) )
                {
                    // Remove da coleção de combustíveis
                    enumerator.Dispose();
                    // Garbage Collector - recolher memória
                    ac.Abastecimento = null;
                    ac.Combustivel = null;
                }
            }
        }


        public override string ToString()
        {
            return $"abast_id: {Id}";
        }


        public override bool Equals(object obj)
        {
            return obj is Abastecimento abastecimento &&
                   Id == abastecimento.Id;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
