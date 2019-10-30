﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    //Annotations ASP.NET Core
    [Table("Produtos")]
    public class Produto
    {
        public Produto() { CriadoEm = DateTime.Now; }
        [Key]
        public int ProdutoId { get; set; }

        [Display(Name = "Nome do produto:")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Nome { get; set; }

        [Display(Name = "Descrição do produto:")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "No mínimo 5 caracteres")]
        [MaxLength(100, ErrorMessage = "No máximo 100 caracteres")]
        public string Descricao { get; set; }

        [Display ( Name = "Preço do produto:")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public double? Preco { get; set; }

        [Display (Name = "Quantidade do produto:")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [Range(1, 1000, ErrorMessage = "Os valores devem estar entre 1 e 1000")]
        public int? Quantidade { get; set; }

        public Categoria Categoria { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
