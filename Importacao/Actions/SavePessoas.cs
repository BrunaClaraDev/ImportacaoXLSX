using Dapper;
using Dapper.Contrib.Extensions;
using Importacao.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Importacao.Actions
{
    public class SavePessoas
    {
        public static bool ExistePessoa(Pessoa pessoa)
        {
            var pessoaExiste = new Pessoa();
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                pessoaExiste = connection.QueryFirstOrDefault<Pessoa>(" SELECT P.CPF FROM TestesImportacao.dbo.Pessoas P where P.CPF = '" + pessoa.CPF + "'");
                if (pessoaExiste != null)
                    return true;
                return false;
            }
        }

        public static string GetId(string cpf)
        {
            var pessoaExiste = new Pessoa();
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                pessoaExiste = connection.QueryFirstOrDefault<Pessoa>(" SELECT P.CPF, P.Id FROM TestesImportacao.dbo.Pessoas P where P.CPF = '" + cpf + "'");
                if (pessoaExiste != null)
                    return pessoaExiste.Id;
                return null;
            }
        }

        public static void Atualizar(Pessoa pessoa)
        {
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                if (pessoa.Nome != null)
                {
                    var pessoaAtualizada = connection.QueryFirstOrDefault<Pessoa>(" UPDATE Pessoas SET DataCriacao = '" + pessoa.DataCriacao
                        + "', CEP = '" + pessoa.CEP + "', Telefone = '"+ pessoa.Telefone +"', Nome = '"+ pessoa.Nome +"' WHERE CPF = '" + pessoa.CPF + "'");
                }
            }
        }

        public static void Salvar(List<Pessoa> pessoas)
        {
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                foreach (Pessoa pessoa in pessoas)
                {
                    var existe = ExistePessoa(pessoa);
                    if (existe == false)
                        connection.Insert(pessoa);
                    else
                        Atualizar(pessoa);
                }
            }
        }
    }
}
