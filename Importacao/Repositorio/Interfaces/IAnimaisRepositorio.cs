using Importacao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Importacao.Repositorio
{
    public interface IAnimaisRepositorio
    {
        public Task<string> PegaIdPessoaAsync(string cpf);
        public Task <bool> ExisteAnimalAsync(string chip);
        public Task SalvarAsync(List<Animais> animais);
        public Task AtualizarAsync(Animais animal);
    }
}
