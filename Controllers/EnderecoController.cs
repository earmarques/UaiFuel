using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UaiFuel.Models.DAO;
using UaiFuel.Models.Domain;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Controllers
{
    public class EnderecoController : Controller
    {
        
        public ActionResult List()
        {
            List<SelectListItem> nomesEstados = new List<SelectListItem>();
            EstadoCidadeViewModel vm = new EstadoCidadeViewModel();

            IList<Estado> estados = null;
            using (EstadoCidadeDAO dao = new EstadoCidadeDAO())
            {
                estados = dao.GetEstados();
            }
            foreach (var estado in estados)
            {
                nomesEstados.Add(
                    new SelectListItem { Text = estado.Nome, Value = estado.UF });
            }
            vm.NomesEstado = nomesEstados;
            return View(vm);
        }


        [HttpPost]
        public JsonResult GetCidade(string uf)
        {
            List<List<string>> nomesCidade = new List<List<string>>();

            if (!string.IsNullOrEmpty(uf))
            {
                IList<Cidade> cidades = null;
                using (EstadoCidadeDAO dao = new EstadoCidadeDAO())
                {
                    cidades = dao.GetCidades(uf);
                }
                
                foreach (var cidade in cidades)
                {
                    List<string> dict = new List<string>
                    {
                        cidade.Id.ToString(),
                        cidade.Nome
                    };
                    nomesCidade.Add(dict);
                }
            }
            return new JsonResult(nomesCidade);
        }
    }
}

