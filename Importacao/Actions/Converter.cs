using Microsoft.AspNetCore.Http;
using System.IO;

namespace Importacao.Actions
{
    public class Converter
    {
        internal static MemoryStream LerStream(IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                formFile?.CopyTo(stream);

                var byteArray = stream.ToArray();

                return new MemoryStream(byteArray);
            }
        }
    }
}
