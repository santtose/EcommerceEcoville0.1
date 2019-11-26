using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace API.Controllers
{
    [Route("api/Produto")]
    [ApiController]
    public class ProdutoAPIController : ControllerBase
    {
        private readonly ProdutoDAO _produtoDAO;

        public ProdutoAPIController(ProdutoDAO produtoDAO)
        {
            _produtoDAO = produtoDAO;
        }

        // /api/Produto/ListarTodos
        [HttpGet]
        [Route("ListarTodos")]
        public IActionResult ListarTodos()
        {
            return Ok(_produtoDAO.ListarTodos());
        }

        // /api/Produto/BuscarPorId/1
        [HttpGet]
        [Route("BuscarPorId/{id}")]
        public IActionResult BuscarPorId(int id)
        {
            Produto p = _produtoDAO.BuscarPorId(id);
            if(p != null)
            {
                return Ok(p);
            }
            return NotFound(new { msg = "Produto não encontrado!" });
        }

        //GET: /api/Produto/BuscarPorCategoria/2
        [HttpGet]
        [Route("BuscarPorCategoria/{id}")]
        public IActionResult BuscarPorCategoria([FromRoute]int id)
        {
            List<Produto> produtos = _produtoDAO.ListarPorCategoria(id);
            if(produtos.Count > 0)
            {
                return Ok(produtos);
            }
            return NotFound(new { msg = "Sem resultado!" });
        }

        [HttpPost]
        [Route("Cadastrar")]
        public IActionResult Cadastrar([FromBody]Produto p)
        {
            if (ModelState.IsValid)
            {
                if (_produtoDAO.Cadastrar(p))
                {
                    return Created("", p);
                }
                return Conflict(new { msg = "Produto já existe!" });
            }
            return BadRequest(ModelState);
        }
    }
}