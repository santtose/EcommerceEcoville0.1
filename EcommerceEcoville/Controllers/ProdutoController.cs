using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace EcommerceEcoville.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoDAO _produtoDAO;
        public ProdutoController(ProdutoDAO produtoDAO)
        {
            _produtoDAO = produtoDAO;
        }

        //Métodos dentro de um controller são de chamados
        //de actions
        public IActionResult Index()
        {
            ViewBag.DataHora = DateTime.Now;
            return View(_produtoDAO.ListarTodos());
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto p)
        {
            if (ModelState.IsValid)
            {
                if (_produtoDAO.Cadastrar(p))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError
                    ("", "Esse produto já existe!");
                return View(p);
            }
            return View(p);
        }

        public IActionResult Remover(int? id)
        {
            if (id != null)
            {
                _produtoDAO.Remover(id);
            }
            else
            {
                //Redirecionar para uma página de erro
            }
            return RedirectToAction("Index");
        }
        public IActionResult Alterar(int? id)
        {
            return View(_produtoDAO.BuscarPorId(id));
        }

        [HttpPost]
        public IActionResult Alterar(Produto p)
        {
            _produtoDAO.Alterar(p);
            return RedirectToAction("Index");
        }

        public IActionResult Detalhes(int? id)
        {
            var obj = _produtoDAO.BuscarPorId(id.Value);
            return View(obj);
        }
    }
}