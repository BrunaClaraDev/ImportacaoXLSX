using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Importacao.Models
{
    [Table("Animais")]
    public class Animais
    {
        [ExplicitKey]
        public string Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Nome { get; set; }
        public string Especie { get; set; }
        public decimal Peso { get; set; }
        public string ChipRastreador { get; set; }
        public string NomeDono { get; set; }

    }
}
