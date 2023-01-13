using Dapper;
using Importacao.Dados;
using Importacao.Models;
using Importacao.Servicos;
using System.Collections.Generic;

namespace Importacao.Actions
{
    public class SalvarAnimais : ISalvarAnimais
    {
        private readonly DbSession _db;
        public SalvarAnimais(DbSession dbSession)
        {
            _db = dbSession;
        }

        public string PegaIdPessoa(string cpf)
        {
            var pessoaExiste = _db.Connection.QueryFirstOrDefault<Pessoa>(" SELECT P.CPF, P.Id FROM TestesImportacao.dbo.Pessoas P where P.CPF = @cpf", new { cpf = cpf });
            if (pessoaExiste != null)
                return pessoaExiste.Id;
            return null;
        }

        public bool ExisteAnimal(string chip)
        {
            var animalExiste = _db.Connection.QueryFirstOrDefault<Animais>(" SELECT A.ChipRastreador FROM TestesImportacao.dbo.Animais A where A.ChipRastreador = @chip", new { chip = chip});
            if (animalExiste != null)
                return true;
            return false;
        }

        public void Atualizar(Animais animal)
        {
            if (animal.Nome != null)
            {
                var peso = animal.Peso.ToString().Replace(',', '.');
                var pessoaAtualizada = _db.Connection.QueryFirstOrDefault<Pessoa>("UPDATE Animais SET DataCriacao = @data, Nome = @nome, Especie = @especie, Peso = @peso, IdPessoa = @id WHERE ChipRastreador = @chip",
                    new{
                        data = animal.DataCriacao,
                        nome = animal.Nome,
                        especie = animal.Especie,
                        peso = animal.Peso,
                        chip = animal.ChipRastreador,
                        id = animal.IdPessoa
                    });
            }
        }

        public void Salvar(List<Animais> animais)
        {
            foreach (Animais animal in animais)
            {
                animal.IdPessoa = PegaIdPessoa(animal.IdPessoa);
                var existe = ExisteAnimal(animal.ChipRastreador);
                if (existe == false)
                {
                    var animalSalvo = _db.Connection.QueryFirstOrDefault<Pessoa>(" INSERT INTO TestesImportacao.dbo.Animais (IdAnimal, IdPessoa, Nome, DataCriacao, Especie, ChipRastreador, Peso) VALUES (@idAnimal, @idPessoa, @nome, @data, @especie, @chip,  @peso);",
                        new
                        {
                            idAnimal = animal.IdAnimal,
                            idPessoa = animal.IdPessoa,
                            nome = animal.Nome,
                            data = animal.DataCriacao,
                            especie = animal.Especie,
                            chip = animal.ChipRastreador,
                            peso = animal.Peso
                        });
                }
                else
                    Atualizar(animal);
            }
        }
    }
}
