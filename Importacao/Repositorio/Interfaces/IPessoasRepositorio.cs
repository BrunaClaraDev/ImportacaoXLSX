using Importacao.Models;
using System.Collections.Generic;

namespace Importacao.Repositorio
{
    public interface IPessoasRepositorio
    {
        public void Salvar(List<Pessoa> pessoas);
        public void Atualizar(Pessoa pessoa);
        public bool ExistePessoa(string cpf);
    }
}
