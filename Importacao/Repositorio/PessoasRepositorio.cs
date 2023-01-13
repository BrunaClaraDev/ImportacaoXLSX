using Dapper;
using Importacao.Dados;
using Importacao.Models;
using Importacao.Repositorio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Importacao.Actions
{
    public class PessoasRepositorio : IPessoasRepositorio
    {
        private readonly DbSessao _db;
        public PessoasRepositorio(DbSessao dbSession)
        {
            _db = dbSession;
        }

        public async Task<bool> ExistePessoaAsync(string cpf)
        {
            var pessoaExiste = _db.Connection.QueryFirstOrDefault<Pessoa>(" SELECT P.CPF FROM TestesImportacao.dbo.Pessoas P where P.CPF = @cpf", new { cpf = cpf });
            if (pessoaExiste != null)
                return true;
            return false;
        }

        public async Task AtualizarAsync(Pessoa pessoa)
        {
            if (pessoa.Nome != null)
            {
                var pessoaAtualizada = _db.Connection.QueryFirstOrDefault<Pessoa>(" UPDATE Pessoas SET DataCriacao = @data, CEP = @cep, Telefone = @telefone, Nome = @nome WHERE CPF = @cpf",
                new
                {
                    data = pessoa.DataCriacao,
                    cep = pessoa.CEP,
                    telefone = pessoa.Telefone,
                    nome = pessoa.Nome,
                    cpf = pessoa.CPF
                });
            }
        }

        public async Task SalvarAsync(List<Pessoa> pessoas)
        {
            foreach (Pessoa pessoa in pessoas)
            {
                var existe = await ExistePessoaAsync(pessoa.CPF);
                if (existe == false)
                {
                    var pessoaSalva = _db.Connection.QueryFirstOrDefault<Pessoa>(" INSERT INTO TestesImportacao.dbo.Pessoas (Id, Nome, DataCriacao, CPF, CEP, Telefone) VALUES (@id, @nome, @data, @cpf, @cep,  @telefone);",
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
                else
                    await AtualizarAsync(pessoa);
            }
        }
    }
}
