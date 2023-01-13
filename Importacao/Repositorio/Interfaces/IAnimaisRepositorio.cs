﻿using Importacao.Models;
using System.Collections.Generic;

namespace Importacao.Repositorio
{
    public interface IAnimaisRepositorio
    {
        public string PegaIdPessoa(string cpf);
        public bool ExisteAnimal(string chip);
        public void Salvar(List<Animais> animais);
        public void Atualizar(Animais animal);
    }
}