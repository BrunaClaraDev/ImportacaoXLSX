using Importacao.Models;
using System.Collections.Generic;

namespace Importacao.Servicos
{
    public interface ISalvarPessoas
    {
        public void Salvar(List<Pessoa> pessoas);
        public void Atualizar(Pessoa pessoa);
        public bool ExistePessoa(string cpf);
    }
}
