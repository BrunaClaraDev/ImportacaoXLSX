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
        public static List<Pessoa> LerPessoas(MemoryStream stream)
        {
            var pessoas = new List<Pessoa>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage pacote = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = pacote.Workbook.Worksheets[0];
                int colunaCont = worksheet.Dimension.End.Column;

                int linhaCont = worksheet.Dimension.End.Row;

                for (int linha = 2; linha <= linhaCont; linha++)
                {
                    var pessoa = new Pessoa();
                    try
                    {
                        pessoa.Id = Guid.NewGuid().ToString("N");
                        pessoa.Email = Convert.ToDecimal(worksheet.Cells[linha, 2].Value);
                        pessoa.DataCriacao = DateTime.Now;
                        pessoa.Nome = worksheet.Cells[linha, 1].Value?.ToString();
                    }

                    catch(Exception ex)
                    {
                        throw;
                    }
                    pessoas.Add(pessoa);
                }
            }
            foreach(Pessoa pessoa in pessoas)
            {
                pessoa.Nome = pessoa.Nome?.Trim();
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
                    try
                    {
                        animal.Id = Guid.NewGuid().ToString("N");
                        animal.DataCriacao = DateTime.Now;
                        animal.Nome = worksheet.Cells[linha, 1].Value?.ToString();
                        animal.Especie = worksheet.Cells[linha, 2].Value?.ToString();
                        animal.Peso = Convert.ToDecimal(worksheet.Cells[linha, 3].Value);
                        animal.ChipRastreador = worksheet.Cells[linha, 4].Value?.ToString();
                        animal.NomeDono = worksheet.Cells[linha, 5].Value?.ToString();
                    }

                    catch (Exception ex)
                    {
                        throw;
                    }
                    animais.Add(animal);
                }
            }
            foreach (Animais animal in animais)
            {
                animal.Nome = animal.Nome?.Trim();
                animal.Especie = animal.Especie?.Trim();
                animal.ChipRastreador = animal.ChipRastreador?.Trim();
                animal.NomeDono = animal.Nome?.Trim();
            }
            return animais;
        }
    }
}
