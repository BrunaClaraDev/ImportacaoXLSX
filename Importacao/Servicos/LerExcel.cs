using Importacao.Dados;
using Importacao.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xceed.Wpf.Toolkit;

namespace Importacao.Actions
{
    public class LerExcel
    {
        private DbSession _db;
        public LerExcel(DbSession dbSession)
        {
            _db = dbSession;
        }
        public static List<Pessoa> LerPessoas(MemoryStream stream)
        {
            var pessoas = new List<Pessoa>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage pacote = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = pacote.Workbook.Worksheets[0];
                int colunaCont = worksheet.Dimension.End.Column;
                int linhaCont = worksheet.Dimension.End.Row;
                int posicao = 1;
                for (int coluna = 1; coluna <= colunaCont; coluna++)
                {
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "NomePessoa")
                        posicao = coluna;
                }

                for (int linha = 2; linha <= linhaCont; linha++)
                {
                    var pessoa = new Pessoa();

                    pessoa.Id = Guid.NewGuid().ToString("N");
                    pessoa.DataCriacao = DateTime.Now;
                    pessoa.Nome = worksheet.Cells[linha, posicao].Value?.ToString();
                    pessoa.CPF = worksheet.Cells[linha, posicao + 1].Value?.ToString();
                    pessoa.Telefone = worksheet.Cells[linha, posicao + 2].Value?.ToString();
                    pessoa.CEP = worksheet.Cells[linha, posicao + 3].Value?.ToString();

                    if(pessoa.CPF != null)
                        pessoas.Add(pessoa);
                }
            }
            foreach(Pessoa pessoa in pessoas)
            {
                pessoa.Nome = pessoa.Nome?.Trim();
                pessoa.CPF = pessoa.CPF?.Replace("-", "").Replace(".", "").Replace(@"\", "").Trim();
                pessoa.CEP = pessoa.CEP?.Replace("-", "".Trim());
                pessoa.Telefone = pessoa.Telefone?.Replace("-", "").Replace("(", "").Replace(")", "").Trim();
            }
            return pessoas;
        }

        public static List<Animais> LerAnimais(MemoryStream stream)
        {
            var animais = new List<Animais>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage pacote = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = pacote.Workbook.Worksheets[0];
                int colunaCont = worksheet.Dimension.End.Column;
                int linhaCont = worksheet.Dimension.End.Row;
                

                for (int linha = 2; linha <= linhaCont; linha++)
                {
                    var animal = new Animais();
                    for (int coluna = 1; coluna <= colunaCont; coluna++)
                    {
                        if (worksheet.Cells[1, coluna].Value?.ToString() == "CPF")
                            animal.IdPessoa = worksheet.Cells[linha, coluna].Value?.ToString();
                    }
                    
                     animal.IdAnimal = Guid.NewGuid().ToString("N");
                     animal.DataCriacao = DateTime.Now;
                     animal.Nome = worksheet.Cells[linha, 1].Value?.ToString();
                     animal.Especie = worksheet.Cells[linha, 2].Value?.ToString();
                     animal.Peso = Convert.ToDecimal(worksheet.Cells[linha, 3].Value);
                     animal.ChipRastreador = worksheet.Cells[linha, 4].Value?.ToString();

                    if (animal.ChipRastreador != null)
                        animais.Add(animal);
                }
            }
            foreach (Animais animal in animais)
            {
                animal.Nome = animal.Nome?.Trim();
                animal.Especie = animal.Especie?.Trim();
                animal.ChipRastreador = animal.ChipRastreador?.Trim();
                string cpf = animal.IdPessoa?.Replace("-", "").Replace(".", "").Replace(@"\", "");
                //var salvarPessoas = new SalvarPessoas(_db);
                //animal.IdPessoa = salvarPessoas.PegaId(cpf);
            }
            return animais;
        }
    }
}
