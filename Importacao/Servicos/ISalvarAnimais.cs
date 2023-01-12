using Importacao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Importacao.Servicos
{
    public interface ISalvarAnimais
    {
        public string PegaIdPessoa(string cpf);
        public bool ExisteAnimal(Animais animal);
        public void Salvar(List<Animais> animais);
        public void Atualizar(Animais animal);
    }
}
