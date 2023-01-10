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
        [HttpPost("Importa-Pessoas-XLSX")]
        public ActionResult ImportaPessoas(IFormFile arquivo)
        {
            var arquivoStream = Converter.LerStream(arquivo);
            var pessoas = LerExcel.LerPessoas(arquivoStream);
            SavePessoas.Salvar(pessoas);
            
            return Ok();
        }

        [Consumes("multipart/form-data")]
        [HttpPost("Importa-Animais-XLSX")]
        public ActionResult ImportaAnimais(IFormFile arquivo)
        {
            var arquivoStream = Converter.LerStream(arquivo);
            var animais = LerExcel.LerAnimais(arquivoStream);
            SaveAnimais.Salvar(animais);

            return Ok();
        }
    }
}
