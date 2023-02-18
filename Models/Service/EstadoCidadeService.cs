using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.Service
{
    public class EstadoCidadeService
    {
        
        public IList<Estado> GetEstados()
        {
            IList<Estado> lista = null;
            using (EstadoCidadeDAO dao = new EstadoCidadeDAO())
            {
                lista = dao.GetEstados();
            }
            return lista;
        }


        public IList<Cidade> GetCidades(string uf)
        {
            IList<Cidade> lista = null;
            using (EstadoCidadeDAO dao = new EstadoCidadeDAO())
            {
                lista = dao.GetCidades(uf);
            }
            return lista;
        }

        public Cidade GetCidadeById(int id)
        {
            Cidade cidade = null;
            using (EstadoCidadeDAO dao = new EstadoCidadeDAO())
            {
                cidade = dao.GetCidadeById(id);
            }
            return cidade;
        }
    }
}


