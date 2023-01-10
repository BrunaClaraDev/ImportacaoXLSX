using Importacao.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Importacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportacoesController : ControllerBase
    {

        private readonly ILogger<ImportacoesController> _logger;

        public ImportacoesController(ILogger<ImportacoesController> logger)
        {
            _logger = logger;
        }

        [Consumes("multipart/form-data")]
        [HttpPost("Importa-arquivo-XLSX")]
        public ActionResult ImportaArquivo(IFormFile arquivo)
        {
            var arquivoStream = Converter.LerStream(arquivo);
            var pessoas = LerExcel.LerXLSX(arquivoStream);
            SavePessoas.Salvar(pessoas);
            
            return Ok();
        }
    }
}
