using Dapper;
using Importacao.Dados;
using Importacao.Models;
using Importacao.Repositorio;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Importacao.Actions
{
    public class PessoasRepositorio : IPessoasRepositorio
    {
        private readonly DbSessao _db;
        private readonly int ErroPrimaryKey = 2601;
        private readonly int ErroForeignKey = 547;
        private readonly ILogger<PessoasRepositorio> _logger;
        public PessoasRepositorio(DbSessao dbSession, ILogger<PessoasRepositorio> logger)
        {
            _db = dbSession;
            _logger = logger;
        }

        public async Task SalvarAsync(List<Pessoa> pessoas)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var quantidadeDeErroFK = 0;
            var quantidadeDeErroPK = 0;

            foreach (Pessoa pessoa in pessoas)
            {
                try
                {
                    var insertPessoa = @"BEGIN
                                   IF NOT EXISTS (SELECT P.CPF FROM TestesImportacao.dbo.Pessoas P where P.CPF = @cpf)
                                   BEGIN
                                       INSERT INTO TestesImportacao.dbo.Pessoas (Id, Nome, DataCriacao, CPF, CEP, Telefone) 
                                        VALUES (@id, @nome, @data, @cpf, @cep,  @telefone);
                                   END
                                   ELSE
                                   BEGIN
                                        UPDATE Pessoas 
                                        SET DataCriacao = @data, CEP = @cep, Telefone = @telefone, Nome = @nome WHERE CPF = @cpf
                                   END 
                                END";

                    await _db.Connection.ExecuteAsync(insertPessoa,
                       new
                       {
                           id = pessoa.Id,
                           nome = pessoa.Nome,
                           data = pessoa.DataCriacao,
                           cpf = pessoa.CPF,
                           cep = pessoa.CEP,
                           telefone = pessoa.Telefone
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

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            _logger.LogInformation("Tempo de duracao {0:00}:{1:00}:{2:00} em Pessoas ", ts.Hours, ts.Minutes, ts.Seconds);
        }



    }
}
