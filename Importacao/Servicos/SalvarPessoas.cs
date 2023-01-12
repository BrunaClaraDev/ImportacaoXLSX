using Dapper;
using Dapper.Contrib.Extensions;
using Importacao.Dados;
using Importacao.Models;
using Importacao.Servicos;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Importacao.Actions
{
    public class SalvarPessoas : ISalvarPessoas
    {
        private DbSession _db;
        public SalvarPessoas(DbSession dbSession)
        {
            _db = dbSession;
        }

        public bool ExistePessoa(Pessoa pessoa)
        {
            using (var con = _db.Connection)
            {
                var pessoaExiste = con.QueryFirstOrDefault<Pessoa>(" SELECT P.CPF FROM TestesImportacao.dbo.Pessoas P where P.CPF = @cpf", new { cpf = pessoa.CPF});
                if (pessoaExiste != null)
                    return true;
                return false;
            }
        }

        public void Atualizar(Pessoa pessoa)
        {
            using (var con = _db.Connection)
            {
                if (pessoa.Nome != null)
                {
                    var pessoaAtualizada = con.QueryFirstOrDefault<Pessoa>(" UPDATE Pessoas SET DataCriacao = @data, CEP = @cep, Telefone = @telefone, Nome = @nome WHERE CPF = @cpf",
                        new {data = pessoa.DataCriacao,
                             cep = pessoa.CEP,
                             telefone = pessoa.Telefone,
                             nome = pessoa.Nome,
                             cpf = pessoa.CPF});
                }
            }
        }

        public void Salvar(List<Pessoa> pessoas)
        {
            using (var con = _db.Connection)
            {
                foreach (Pessoa pessoa in pessoas)
                {
                    var existe = ExistePessoa(pessoa);
                    if (existe == false)
                        con.Insert(pessoa);
                    else
                        Atualizar(pessoa);
                }
            }
        }
    }
}
