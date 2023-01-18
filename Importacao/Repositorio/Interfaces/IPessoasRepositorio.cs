using Importacao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Importacao.Repositorio
{
    public interface IPessoasRepositorio
    {
        public Task SalvarAsync(List<Pessoa> pessoas);
    }
}
