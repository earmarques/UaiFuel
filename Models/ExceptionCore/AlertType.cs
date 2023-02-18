using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UaiFuel.Models.ExceptionCore
{
    sealed public class AlertType
    {

        public int Codigo { get; }
        public string Descricao { get; }

        static public AlertType PRIMARY   = new AlertType(1, "PRIMARY");
        static public AlertType SECONDARY = new AlertType(2, "SECONDARY");
        static public AlertType SUCCESS   = new AlertType(3, "SUCCESS");
        static public AlertType DANGER    = new AlertType(3, "DANGER");
        static public AlertType WARNING   = new AlertType(4, "WARNING");
        static public AlertType INFO      = new AlertType(5, "INFO");


        private AlertType(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
        }


        public override string ToString()
        {
            return $"{Codigo} - {Descricao}";
        }
	}

}

