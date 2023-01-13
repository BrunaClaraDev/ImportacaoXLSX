using Importacao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Importacao.Repositorio
{
    public interface IPessoasRepositorio
    {
        public Task SalvarAsync(List<Pessoa> pessoas);
        public Task AtualizarAsync(Pessoa pessoa);
        public Task<bool> ExistePessoaAsync(string cpf);
    }
}
