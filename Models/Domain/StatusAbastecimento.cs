using System;
using System.Collections.Generic;
using System.Text;

namespace UaiFuel.Models.Domain
{
    /** 
     * Singleton Pattern 
     */
    sealed public class StatusAbastecimento
    {
        public int Codigo { get; }
        public string Descricao { get; }
        static public IList<StatusAbastecimento> lista = new List<StatusAbastecimento>();

        static public StatusAbastecimento PENDENTE_PAGTO = new StatusAbastecimento(1, "Pendente Pagto");
        static public StatusAbastecimento CANCELADO =      new StatusAbastecimento(2, "Cancelado");        
        static public StatusAbastecimento PAGO =           new StatusAbastecimento(3, "Pago");

        private StatusAbastecimento(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
            lista.Add(this);
        }

        static public StatusAbastecimento getInstance(int codigo)
        {
            foreach (StatusAbastecimento s in lista)
            {
                if (s.Codigo == codigo)
                {
                    return s;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return $"{Codigo} - {Descricao}";
        }
    }
}
