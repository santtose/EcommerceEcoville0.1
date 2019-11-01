﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class UsuarioDAO : IRepository<Usuario>
    {
        private readonly Context _context;
        public Usuario BuscarPorId(int? id)
        {
            return _context.Usuarios.Find(id);
        }

        public bool Cadastrar(Usuario objeto)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> ListarTodos()
        {
            return _context.Usuarios.ToList();
        }
    }
}