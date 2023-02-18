using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UaiFuel.Models.ExceptionCore
{

    public class AppException 
        : Exception
    {
        public Exception Cause { get; set; }
        public AlertType Alert { get; set; }

        /** 
         * Contém o texto que será informado ao usuário comum. 
         * Deve ser usada para devolver uma mensagem amigável para o usuário, 
         * escondendo os detalhes da exceção.
         */
        public String UserErrorDescription { get; set; }

        /** 
         * Contém uma descrição de erro com todos os detalhes necessários aos desenvolvedores
         * da aplicação para que possam entender o quê aconteceu. Vai para o log.
         */
        public String ErrorDescription { get; set; }

        /** 
         * Sugere o que deve ser feito a fim de corrigir o erro, se for possível saber. 
         */
        public String ErrorCorrection { get; set; }


        public AppException()
        {
            UserErrorDescription = "Falha inesperada no sistema";
            Alert = AlertType.DANGER;
        }

        public AppException(string message)
        {
            ErrorDescription = message;
        }

        public AppException(string message, Exception innerException)
            : this(message)
        {
            this.Cause = innerException;
        }
    }
}
