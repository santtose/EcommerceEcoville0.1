using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository;

namespace EcommerceEcoville.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioDAO _usuarioDAO;

        public UsuarioController(UsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        public IActionResult Index()
        {
            ViewBag.DataHora = DateTime.Now;
            return View(_usuarioDAO.ListarTodos());
        }

        public IActionResult Cadastrar()
        {
            Usuario u = new Usuario();
            if (TempData["Endereco"] != null)
            {
                string resultado = TempData["Endereco"].ToString();
                Endereco endereco = JsonConvert.DeserializeObject<Endereco>(resultado);
                u.Endereco = endereco;
            }
            return View(u);
        }

        [HttpPost]
        public IActionResult BuscarCep(Usuario u)
        {
            string url = "https://viacep.com.br/ws/" + u.Endereco.Cep + "/json/";
            WebClient client = new WebClient();         
            TempData["Endereco"] = client.DownloadString(url);

            return RedirectToAction("Cadastrar");
        }

        [HttpPost]
        public IActionResult Cadastrar(Usuario u)
        {
            if (ModelState.IsValid)
            {
                if (_usuarioDAO.Cadastrar(u))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Email já está sendo ultilizado");
                return View(u);
            }
            return View(u);
        }
    }
}