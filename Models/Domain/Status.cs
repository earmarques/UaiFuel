using System;
using System.Collections.Generic;
using System.Text;

namespace UaiFuel.Models.Domain
{
    /** 
     * Singleton Pattern 
     */
    sealed public class Status
    {
        public int Codigo { get; }
        public string Descricao { get; }

        static public IList<Status> lista = new List<Status>();

        static public Status ATIVO = new Status(1, "Ativo");
        static public Status INATIVO = new Status(2, "Inativo");

        private Status(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
            lista.Add(this);
        }


        static public Status getInstance(int codigo)
        {
            foreach (Status s in lista)
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
