using Dapper;
using Importacao.Dados;
using Importacao.Models;
using Importacao.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Importacao.Actions
{
    public class AnimaisRepositorio : IAnimaisRepositorio
    {
        private readonly DbSessao _db;
        public AnimaisRepositorio(DbSessao dbSession)
        {
            _db = dbSession;
        }

        public async Task<string> PegaIdPessoaAsync(string cpf)
        {
            var pessoaExiste = _db.Connection.QueryFirstOrDefault<Pessoa>(" SELECT P.CPF, P.Id FROM TestesImportacao.dbo.Pessoas P where P.CPF = @cpf", new { cpf = cpf });
            if (pessoaExiste != null)
                return pessoaExiste.Id;
            return null;
        }

        public async Task<bool> ExisteAnimalAsync(string chip)
        {
            var animalExiste = _db.Connection.QueryFirstOrDefault<Animais>(" SELECT A.ChipRastreador FROM TestesImportacao.dbo.Animais A where A.ChipRastreador = @chip", new { chip = chip });
            if (animalExiste != null)
                return true;
            return false;
        }

        public async Task AtualizarAsync(Animais animal)
        {
            if (animal.Nome != null)
            {
                var peso = animal.Peso.ToString().Replace(',', '.');
                var pessoaAtualizada = _db.Connection.QueryFirstOrDefault<Animais>("UPDATE Animais SET DataCriacao = @data, Nome = @nome, Especie = @especie, Peso = @peso, IdPessoa = @id WHERE ChipRastreador = @chip",
                    new
                    {
                        data = animal.DataCriacao,
                        nome = animal.Nome,
                        especie = animal.Especie,
                        peso = animal.Peso,
                        chip = animal.ChipRastreador,
                        id = animal.IdPessoa
                    });
            }
        }

        public async Task SalvarAsync(List<Animais> animais)
        {
            foreach (Animais animal in animais)
            {
                animal.IdPessoa = await PegaIdPessoaAsync(animal.IdPessoa);
                var existe = await ExisteAnimalAsync(animal.ChipRastreador);
                if (existe == false)
                {
                    var animalSalvo =  _db.Connection.QueryFirstOrDefault<Animais>(" INSERT INTO TestesImportacao.dbo.Animais (IdAnimal, IdPessoa, Nome, DataCriacao, Especie, ChipRastreador, Peso) VALUES (@idAnimal, @idPessoa, @nome, @data, @especie, @chip,  @peso);",
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
                     await AtualizarAsync(animal);
            }
        }
    }
}
