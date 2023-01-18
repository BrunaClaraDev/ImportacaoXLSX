using Importacao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Importacao.Repositorio
{
    public interface IAnimaisRepositorio
    {
        public Task SalvarAsync(List<Animais> animais);
    }
}
