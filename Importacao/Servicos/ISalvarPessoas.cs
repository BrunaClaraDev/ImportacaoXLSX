using Importacao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Importacao.Servicos
{
    public interface ISalvarPessoas
    {
        public void Salvar(List<Pessoa> pessoas);
        public void Atualizar(Pessoa pessoa);
        public string PegaId(string cpf);
        public bool ExistePessoa(Pessoa pessoa);
    }
}
