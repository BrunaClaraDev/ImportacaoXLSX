using Dapper;
using Dapper.Contrib.Extensions;
using Importacao.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Importacao.Actions
{
    public class SavePessoas
    {
        public static bool GetNome(Pessoa pessoa)
        {
            var pessoaExiste = new Pessoa();
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                pessoaExiste = connection.QueryFirstOrDefault<Pessoa>(" SELECT P.Nome ,P.DataCriacao,P.Id,P.Email FROM TestesImportacao.dbo.Pessoas P where UPPER (P.Nome) = '" + pessoa.Nome + "'");
                if (pessoaExiste != null)
                    return true;
                return false;
            }
        }

        public static void Atualizar(Pessoa pessoa)
        {
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                if (pessoa.Nome != null)
                {
                    var email = pessoa.Email.ToString().Replace(',', '.');
                    var pessoaAtualizada = connection.QueryFirstOrDefault<Pessoa>(" UPDATE Pessoas SET DataCriacao = '" + pessoa.DataCriacao
                        + "', Email = " + email + " WHERE Nome = '" + pessoa.Nome + "'");
                }
            }
        }

        public static void Salvar(List<Pessoa> pessoas)
        {
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                foreach (Pessoa pessoa in pessoas)
                {
                    var existe = GetNome(pessoa);
                    if (existe == false)
                        connection.Insert(pessoa);
                    else
                        Atualizar(pessoa);
                }
            }
        }
    }
}
