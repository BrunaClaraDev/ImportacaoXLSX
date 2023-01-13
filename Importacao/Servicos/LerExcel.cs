using Importacao.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

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
                int posCPF = colunaCont + 1, posNome = colunaCont + 1, posCEP = colunaCont + 1, posTelefone = colunaCont + 1;
                for (int coluna = 1; coluna <= colunaCont; coluna++)
                {
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "NomePessoa")
                        posNome = coluna;
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "CPF")
                        posCPF = coluna;
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "CEP")
                        posCEP = coluna;
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "Telefone")
                        posTelefone = coluna;
                }

                for (int linha = 2; linha <= linhaCont; linha++)
                {
                    var pessoa = new Pessoa();

                    pessoa.Id = Guid.NewGuid().ToString("N");
                    pessoa.DataCriacao = DateTime.Now;
                    pessoa.Nome = worksheet.Cells[linha, posNome].Value?.ToString();
                    pessoa.CPF = worksheet.Cells[linha, posCPF].Value?.ToString();
                    pessoa.Telefone = worksheet.Cells[linha, posTelefone].Value?.ToString();
                    pessoa.CEP = worksheet.Cells[linha, posCEP].Value?.ToString();

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
                int posCPF = colunaCont + 1, posNome = colunaCont + 1, posPeso = colunaCont + 1, posEspecie = colunaCont + 1, posChip = colunaCont + 1;
                for (int coluna = 1; coluna <= colunaCont; coluna++)
                {
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "CPF")
                        posCPF = coluna;
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "NomeAnimal")
                        posNome = coluna;
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "Especie")
                        posEspecie = coluna;
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "Peso")
                        posPeso = coluna;
                    if (worksheet.Cells[1, coluna].Value?.ToString() == "ChipRastreador")
                        posChip = coluna;
                }

                for (int linha = 2; linha <= linhaCont; linha++)
                {
                    var animal = new Animais();
                    
                     animal.IdAnimal = Guid.NewGuid().ToString("N");
                     animal.DataCriacao = DateTime.Now;
                     animal.Nome = worksheet.Cells[linha, posNome].Value?.ToString();
                     animal.Especie = worksheet.Cells[linha, posEspecie].Value?.ToString();
                     animal.Peso = Convert.ToDecimal(worksheet.Cells[linha, posPeso].Value);
                     animal.ChipRastreador = worksheet.Cells[linha, posChip].Value?.ToString();
                     animal.IdPessoa = worksheet.Cells[linha, posCPF].Value?.ToString();

                    if (animal.ChipRastreador != null)
                        animais.Add(animal);
                }
            }
            foreach (Animais animal in animais)
            {
                animal.Nome = animal.Nome?.Trim();
                animal.Especie = animal.Especie?.Trim();
                animal.ChipRastreador = animal.ChipRastreador?.Trim();
                animal.IdPessoa = animal.IdPessoa?.Replace("-", "").Replace(".", "").Replace(@"\", "");
            }
            return animais;
        }
    }
}
