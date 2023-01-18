using Dapper;
using Importacao.Dados;
using Importacao.Models;
using Importacao.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Importacao.Actions
{
    public class AnimaisRepositorio : IAnimaisRepositorio
    {
        private readonly DbSessao _db;
        private readonly int ErroPrimaryKey = 2601;
        private readonly int ErroForeignKey = 547;
        private readonly ILogger <AnimaisRepositorio> _logger;
        public AnimaisRepositorio(DbSessao dbSession, ILogger<AnimaisRepositorio> logger)
        {
            _db = dbSession;
            _logger = logger;
        }

        public async Task SalvarAsync(List<Animais> animais)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var quantidadeDeErroFK = 0;
            var quantidadeDeErroPK = 0;

            foreach (Animais animal in animais)
            {

                try
                {
                    var insertAnimais = @"BEGIN
                                                           DECLARE @IdPessoa varchar(34)
                                                           SET @IdPessoa = (select top 1 Id from Pessoas p where CPF = @cpf)

                                                           IF NOT EXISTS (SELECT A.ChipRastreador FROM TestesImportacao.dbo.Animais A where A.ChipRastreador = @chip)
                                                           BEGIN
                                                              INSERT INTO TestesImportacao.dbo.Animais (IdAnimal, IdPessoa, Nome, DataCriacao, Especie, ChipRastreador, Peso)
                                                              VALUES (@idAnimal, @IdPessoa , @nome, @data, @especie, @chip,  @peso);
                                                           END
                                                           ELSE
                                                           BEGIN
                                                                UPDATE Animais 
                                                                SET DataCriacao = @data, Nome = @nome, Especie = @especie, Peso = @peso, IdPessoa = @IdPessoa WHERE ChipRastreador = @chip
                                                           END 
                                                        END";

                    await _db.Connection.ExecuteAsync(insertAnimais,
                             new
                             {
                                 idAnimal = animal.IdAnimal,
                                 nome = animal.Nome,
                                 data = animal.DataCriacao,
                                 especie = animal.Especie,
                                 chip = animal.ChipRastreador,
                                 peso = animal.Peso,
                                 cpf = animal.IdPessoa
                             });

                }
                catch (SqlException ex)
                {
                    //FK erro ou PK erro
                    if (ex.Number == ErroForeignKey)
                        quantidadeDeErroFK++;
                    if (ex.Number == ErroPrimaryKey)
                        quantidadeDeErroPK++;

                    continue;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro: {ex.Message}");
                    throw;
                }
            }
            _logger.LogInformation($"ErrosFK:{ErroForeignKey} - ErrosPK: {ErroPrimaryKey}");
            TimeSpan ts = stopwatch.Elapsed;
            _logger.LogInformation("Tempo de duracao {0:00}:{1:00}:{2:00} em Animais ", ts.Hours, ts.Minutes, ts.Seconds);
        }
    }
}
