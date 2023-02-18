using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;
using UaiFuel.Models.ExceptionCore;

namespace UaiFuel.Models.Service
{
    public class CombustivelService
    {
        
        public IList<Combustivel> Read()
        {
            IList<Combustivel> lista = null;

            using ( CombustivelDAO dao = new CombustivelDAO() )
            {
                lista = dao.Read();                
            }
            return lista;
        }


        public Combustivel Read(int id)
        {   
            using (CombustivelDAO dao = new CombustivelDAO())
            {
                return dao.Read(id);
            }
        }


        public IList<Combustivel> ReadByPosto(int postoId)
        {
            using (CombustivelDAO dao = new CombustivelDAO())
            {
                return dao.ReadByPosto(postoId);
            }
        }


        public void Delete(int id)
        {
            try
            {
                using (CombustivelDAO dao = new CombustivelDAO())
                {
                    Combustivel combustivel = new Combustivel();
                    combustivel.Id = id;
                    dao.Delete(combustivel);
                }
            }
            catch (SqlException exp)
            {
                if (exp.Number == 547)
                {
                    string msg = "Conflito de chave estrangeira.\n combustivelId: " + id;
                    string userMsg = "O combustível não pode ser apagado, " +
                                     "porque foi utilizado em um abastecimento.";
                    throw new FKConflictException()
                    {
                        Cause = exp,
                        Alert = AlertType.WARNING,
                        ErrorDescription = msg,
                        UserErrorDescription = userMsg
                    };
                }
                else
                    throw new AppException()
                    {
                        Cause = exp,
                        ErrorDescription = exp.Message
                    };
            }
            catch (Exception exp)
            {
                throw new AppException(exp.Message, exp);
            }
        }


        public void Update(Combustivel combustivel)
        {
            combustivel.Nome = combustivel.Nome.Trim().ToUpper();

            using (CombustivelDAO dao = new CombustivelDAO())
            {
                dao.Update(combustivel);
            }
        }


        public Combustivel Create(Combustivel combustivel)
        {
            combustivel.Nome = combustivel.Nome.Trim().ToUpper();

            using (CombustivelDAO dao = new CombustivelDAO())
            {
                return dao.Create(combustivel);
            }                
        }
    }
}



