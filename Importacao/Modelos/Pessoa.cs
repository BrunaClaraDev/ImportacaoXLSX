using Dapper.Contrib.Extensions;
using System;

namespace Importacao.Models
{
    [Table("Pessoas")]
    public class Pessoa
    {
        [ExplicitKey]
        public string Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string CEP { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
