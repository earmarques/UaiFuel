using System;
using System.Collections.Generic;
using System.Text;

namespace UaiFuel.Models.Domain
{
    /** 
     * Singleton Pattern 
     */
    sealed public class StatusCombustivel
    {
        public int Codigo { get; }
        public string Descricao { get; }
        static public IList<StatusCombustivel> lista = new List<StatusCombustivel>();

        static public StatusCombustivel EM_ESTOQUE = new StatusCombustivel(1, "Em Estoque");
        static public StatusCombustivel FORA_ESTOQUE = new StatusCombustivel(2, "Fora de Estoque");


        private StatusCombustivel(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
            lista.Add(this);
        }


        static public StatusCombustivel getInstance(int codigo)
        {
            foreach (StatusCombustivel s in lista)
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
