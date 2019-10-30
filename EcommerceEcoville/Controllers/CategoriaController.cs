using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Repository;

namespace EcommerceEcoville.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaDAO _categoriaDAO;

        public CategoriaController(CategoriaDAO categoriaDAO)
        {
            _categoriaDAO = categoriaDAO;
        }
               
        // GET: Categoria
        public IActionResult Index()
        {
            
            return View(_categoriaDAO.ListarTodos());
        }

        // GET: Categoria/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if (_categoriaDAO.Cadastrar(categoria))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Categoria já existe!");
            }
            return View(categoria);
        }

    }
}
