using System;
using System.Collections.Generic;
using System.Text;

namespace UaiFuel.Models.Domain
{
    /** 
     * Singleton Pattern 
     */

    sealed public class StatusVeiculo
    {
        public int Codigo { get; }
        public string Descricao { get; }

        static public IList<StatusVeiculo> lista = new List<StatusVeiculo>();

        static public StatusVeiculo ATIVO       = new StatusVeiculo(1, "Ativo");
        static public StatusVeiculo MANUTENCAO  = new StatusVeiculo(2, "Em Manutenção");
        static public StatusVeiculo GARAGEM     = new StatusVeiculo(3, "Garagem");
        static public StatusVeiculo VENDIDO     = new StatusVeiculo(4, "Vendido");

        private StatusVeiculo(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
            lista.Add(this);
        }


        static public StatusVeiculo getInstance(int codigo)
        {
            foreach (StatusVeiculo s in lista)
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
