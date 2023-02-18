using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;

namespace UaiFuel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();


            
            /*
            int id = 13;
            CombustivelDAO dao = new CombustivelDAO();
            Combustivel combustivel = new Combustivel();
            combustivel.Id = id;            
            dao.Delete(combustivel);
            Combustivel updated = dao.Read(id);
            */


            /*
            CombustivelDAO dao = new CombustivelDAO();
            Combustivel combustivel = new Combustivel();
            combustivel.Status = StatusCombustivel.FORA_ESTOQUE;
            combustivel.Descricao = "S505";
            dao.Update(combustivel);
            Combustivel updated = dao.Read(5);
            */
            /*
            CombustivelDAO dao = new CombustivelDAO();
            Combustivel combustivel = dao.Read(5);            
            combustivel.Descricao = "S508";
            dao.Update(combustivel);
            Combustivel updated = dao.Read(5);
            
             */



            /*
            IList<Combustivel> lista = new List<Combustivel>();
            CombustivelDAO dao = new CombustivelDAO();
            lista = dao.Read();
            Console.WriteLine("\n\n\n---------------------------------");
            foreach (Combustivel c in lista)
            {
                Console.WriteLine(c.Descricao);
            }
            */

        }




        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
