using Dapper;
using Dapper.Contrib.Extensions;
using Importacao.Dados;
using Importacao.Models;
using Importacao.Servicos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Importacao.Actions
{
    public class SalvarAnimais : ISalvarAnimais
    {
        private DbSession _db;
        public SalvarAnimais(DbSession dbSession)
        {
            _db = dbSession;
        }

        public bool ExisteAnimal(Animais animal)
        {
            var animalExiste = new Animais();
            using (var con = _db.Connection)
            {
                animalExiste = con.QueryFirstOrDefault<Animais>(" SELECT A.ChipRastreador FROM TestesImportacao.dbo.Animais A where A.ChipRastreador = @chip", new { chip = animal.ChipRastreador});
                if (animalExiste != null)
                    return true;
                return false;
            }
        }

        public void Atualizar(Animais animal)
        {
            using (var con = _db.Connection)
            {
                if (animal.Nome != null)
                {
                    var idPessoa = "";
                    if (animal.IdPessoa != null)
                        idPessoa = animal.IdPessoa;
                    else
                        idPessoa = null;
                    var peso = animal.Peso.ToString().Replace(',', '.');
                    var pessoaAtualizada = con.QueryFirstOrDefault<Pessoa>("UPDATE Animais SET DataCriacao = @data, Nome = @nome, Especie = @especie, Peso = @peso, IdPessoa = @id WHERE ChipRastreador = @chip", 
                        new {data =animal.DataCriacao,
                             nome = animal.Nome,
                             especie = animal.Especie,
                             peso = animal.Peso,
                             chip = animal.ChipRastreador,
                             id = idPessoa});
                }
            }
        }

        public void Salvar(List<Animais> animais)
        {
            using (var con = _db.Connection)
            {
                foreach (Animais animal in animais)
                {
                    var existe = ExisteAnimal(animal);
                    if (existe == false)
                        con.Insert(animal);
                    else
                        Atualizar(animal);
                }
            }
        }
    }
}
