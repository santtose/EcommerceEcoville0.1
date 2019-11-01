using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class EnderecoDAO : IRepository<Endereco>
    {
        private readonly Context _context;

        public EnderecoDAO(Context context)
        {
            _context = context;
        }

        public Endereco BuscarPorId(int? id)
        {
            return _context.Enderecos.Find(id);
        }

        public bool Cadastrar(Endereco objeto)
        {
            if (BuscarPorNome(objeto) == null)
            {
                _context.Enderecos.Add(objeto);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Endereco> ListarTodos()
        {
            return _context.Enderecos.ToList();
        }

        public Endereco BuscarPorNome(Endereco obj)
        {
            return _context.Enderecos.FirstOrDefault(x => x.Cep.Equals(obj.Cep));
        }
    }
}
