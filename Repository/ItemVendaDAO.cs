using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class ItemVendaDAO : IRepository<ItemVenda>
    {
        private readonly Context _context;

        public ItemVendaDAO(Context context)
        {
            _context = context;
        }

        public ItemVenda BuscarPorId(int? id)
        {
            return _context.ItensVendas.Find(id);
        }

        public bool Cadastrar(ItemVenda i)
        {
            _context.ItensVendas.Add(i);
            _context.SaveChanges();
            return true;
        }

        public List<ItemVenda> ListarTodos()
        {
            return _context.ItensVendas.ToList();
        }

        public List<ItemVenda> ListarItensPorCarrinhoId(string carrinhoId)
        {
            return _context.ItensVendas.Include(x => x.Produto.Categoria).Where(x => x.CarrinhoId.Equals(carrinhoId)).ToList();
        }
    }
}
