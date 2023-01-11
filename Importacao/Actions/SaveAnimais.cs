using Dapper;
using Dapper.Contrib.Extensions;
using Importacao.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Importacao.Actions
{
    public class SaveAnimais
    {
        public static bool ExisteAnimal(Animais animal)
        {
            var animalExiste = new Animais();
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                animalExiste = connection.QueryFirstOrDefault<Animais>(" SELECT A.ChipRastreador FROM TestesImportacao.dbo.Animais A where A.ChipRastreador = @chip", new { chip = animal.ChipRastreador});
                if (animalExiste != null)
                    return true;
                return false;
            }
        }

        public static void Atualizar(Animais animal)
        {
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                if (animal.Nome != null)
                {
                    var idPessoa = "";
                    if (animal.IdPessoa != null)
                        idPessoa = animal.IdPessoa;
                    else
                        idPessoa = null;
                    var peso = animal.Peso.ToString().Replace(',', '.');
                    var pessoaAtualizada = connection.QueryFirstOrDefault<Pessoa>("UPDATE Animais SET DataCriacao = @data, Nome = @nome, Especie = @especie, Peso = @peso, IdPessoa = @id WHERE ChipRastreador = @chip", 
                        new {data =animal.DataCriacao,
                             nome = animal.Nome,
                             especie = animal.Especie,
                             peso = animal.Peso,
                             chip = animal.ChipRastreador,
                             id = idPessoa});
                }
            }
        }

        public static void Salvar(List<Animais> animais)
        {
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                foreach (Animais animal in animais)
                {
                    var existe = ExisteAnimal(animal);
                    if (existe == false)
                        connection.Insert(animal);
                    else
                        Atualizar(animal);
                }
            }
        }
    }
}
