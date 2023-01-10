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
        public static bool GetNome(Animais animal)
        {
            var animalExiste = new Animais();
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                animalExiste = connection.QueryFirstOrDefault<Animais>(" SELECT A.ChipRastreador FROM TestesImportacao.dbo.Animais A where A.ChipRastreador = '"+ animal.ChipRastreador +"'");
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
                    var peso = animal.Peso.ToString().Replace(',', '.');
                    var pessoaAtualizada = connection.QueryFirstOrDefault<Pessoa>("UPDATE Animais SET DataCriacao = '"+ animal.DataCriacao +"', Nome = '"+ 
                        animal.Nome +"', Especie = '"+ animal.Especie +"', Peso = "+ peso +", NomeDono = '"+ animal.NomeDono 
                        +"' WHERE ChipRastreador = '"+ animal.ChipRastreador +"'");
                }
            }
        }

        public static void Salvar(List<Animais> animais)
        {
            using (var connection = new SqlConnection("Server=.\\sqlexpress;Database=TestesImportacao;Trusted_Connection=True;"))
            {
                foreach (Animais animal in animais)
                {
                    var existe = GetNome(animal);
                    if (existe == false)
                        connection.Insert(animal);
                    else
                        Atualizar(animal);
                }
            }
        }
    }
}
