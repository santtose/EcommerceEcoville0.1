using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class UsuarioDAO : IRepository<Usuario>
    {
        private readonly Context _context;

        public UsuarioDAO(Context context)
        {
            _context = context;
        }

        public Usuario BuscarPorId(int? id)
        {
            return _context.Usuarios.Include(x => x.Endereco).FirstOrDefault(obj => obj.UsuarioId == id);
        }

        public bool Cadastrar(Usuario u)
        {
            if(BuscarUsuarioPorEmail(u) == null)
            {
                _context.Usuarios.Add(u);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Usuario> ListarTodos()
        {
            return _context.Usuarios.Include("Endereco").ToList();
        }

        public Usuario BuscarUsuarioPorEmail(Usuario u)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Email.Equals(u.Email));
        }
    }
}
