using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    [Table("Enderecos")]
    public class Endereco
    {
        [Key]
        public int EnderecoId { get; set; }

        [Display(Name = "Rua:")]
        public string Logradouro { get; set; }

        [Display(Name = "CEP:")]
        public string Cep { get; set; }

        [Display(Name = "Bairro:")]
        public string Bairro { get; set; }

        [Display(Name = "Cidade:")]
        public string Localidade { get; set; }

        [Display(Name = "Estado:")]
        public string Uf { get; set; }

        [Display(Name = "Numero:")]
        public int Numero { get; set; }

        public DateTime CriadoEm { get; set; }

        public Endereco()
        {
            CriadoEm = DateTime.Now;

        }
    }
}
