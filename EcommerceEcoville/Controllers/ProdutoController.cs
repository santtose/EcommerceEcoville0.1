using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using EcommerceEcoville.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository;

namespace EcommerceEcoville.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoDAO _produtoDAO;
        private readonly CategoriaDAO _categoriaDAO;
        private readonly ItemVendaDAO _itemVendaDAO;
        private readonly UtilsSession _utilSession;
        private readonly IHostingEnvironment _hosting;

        public ProdutoController(ProdutoDAO produtoDAO, CategoriaDAO categoriaDAO, ItemVendaDAO itemVendaDAO, UtilsSession utilsSession, IHostingEnvironment hosting)
        {
            _produtoDAO = produtoDAO;
            _categoriaDAO = categoriaDAO;
            _itemVendaDAO = itemVendaDAO;
            _utilSession = utilsSession;
            _hosting = hosting;
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
            ViewBag.Categorias = new SelectList(_categoriaDAO.ListarTodos(), "CategoriaId", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto p, int drpCategorias, IFormFile fupImagem)
        {
            ViewBag.Categorias = new SelectList(_categoriaDAO.ListarTodos(), "CategoriaId", "Nome");

            

            if (ModelState.IsValid)
            {
                if (fupImagem != null)
                {
                    string arquivo = Guid.NewGuid().ToString() + Path.GetExtension(fupImagem.FileName);
                    string caminho = Path.Combine(_hosting.WebRootPath, "ecommerceimagens", arquivo);
                    fupImagem.CopyTo(new FileStream(caminho, FileMode.Create));
                    p.Imagem = arquivo;
                }
                else
                {
                    p.Imagem = "semimagem.png";
                }

                p.Categoria = _categoriaDAO.BuscarPorId(drpCategorias);
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
            if (id == null)
            {
                return NotFound();
            }
            var obj = _produtoDAO.BuscarPorId(id.Value);
            if(id == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Remover(int id)
        {
            _produtoDAO.Remover(id);
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

        public IActionResult AdicionarAoCarrinho(int id)
        {
            Produto p = _produtoDAO.BuscarPorId(id);

            ItemVenda i = new ItemVenda
            {
                Produto = p,
                Quantidade = 1,
                Preco = p.Preco.Value,
                CarrinhoId = _utilSession.RetornarCarrinhoId()
            };
            _itemVendaDAO.Cadastrar(i);
            return RedirectToAction("CarrinhoCompras");
        }

        public IActionResult CarrinhoCompras()
        {
            return View(_itemVendaDAO.ListarItensPorCarrinhoId(_utilSession.RetornarCarrinhoId()));
        }
    }
}