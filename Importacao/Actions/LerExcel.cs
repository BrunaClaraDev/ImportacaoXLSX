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
        public static List<Pessoa> LerXLSX(MemoryStream stream)
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
    }
}
