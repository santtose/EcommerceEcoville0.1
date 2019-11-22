using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository;

namespace EcommerceEcoville.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioDAO _usuarioDAO;
        private readonly UserManager<UsuarioLogado> _userManager;
        private readonly SignInManager<UsuarioLogado> _signInManager;

        public UsuarioController(UsuarioDAO usuarioDAO, UserManager<UsuarioLogado> userManager, SignInManager<UsuarioLogado> signInManager)
        {
            _usuarioDAO = usuarioDAO;
            _userManager = userManager;
            _signInManager = signInManager;
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
        public async Task<IActionResult> Cadastrar(Usuario u)
        {
            if (ModelState.IsValid)
            {
                UsuarioLogado usuarioLogado = new UsuarioLogado
                {
                    Email = u.Email,
                    UserName = u.Email
                };
                IdentityResult result = await _userManager.CreateAsync(usuarioLogado, u.Senha);

                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(usuarioLogado, isPersistent: false);

                    if (_usuarioDAO.Cadastrar(u))
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Email já está sendo ultilizado");
                    return View(u);
                }

                AdicionarErros(result);
            }
            return View(u);
        }

        public IActionResult Remover(int? id)
        {
            if (id != null)
                _usuarioDAO.Remover(id);
            else
            {
                //erro
            }
                return RedirectToAction("Index");
        }

        public IActionResult Alterar(int? id)
        {
            return View(_usuarioDAO.BuscarPorId(id));
        }

        [HttpPost]
        public IActionResult Alterar(Usuario u)
        {
            _usuarioDAO.Alterar(u);
            return RedirectToAction("Index");
        }

        private void AdicionarErros(IdentityResult result)
        {
            foreach(var erro in result.Errors)
            {
                ModelState.AddModelError("", erro.Description);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Produto");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario u)
        {
            var resultado = await _signInManager.PasswordSignInAsync(u.Email, u.Senha, true, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Produto");
            }
            ModelState.AddModelError("", "Falha no login!");
            return View();
        }
    }
}