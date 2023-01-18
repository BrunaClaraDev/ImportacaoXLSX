using Importacao.Actions;
using Importacao.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Importacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportacoesController : ControllerBase
    {
        private readonly ILogger<ImportacoesController> _logger;
        private readonly IAnimaisRepositorio _animaisRepositorio;
        private readonly IPessoasRepositorio _pessoasRepositorio;

        public ImportacoesController(ILogger<ImportacoesController> logger, IAnimaisRepositorio animaisRepositorio, IPessoasRepositorio pessoasRepositorio)
        {
            _logger = logger;
            _animaisRepositorio = animaisRepositorio;
            _pessoasRepositorio = pessoasRepositorio;
        }

        [Consumes("multipart/form-data")]
        [HttpPost("Importa-Pessoas-XLSX")]
        public async Task<ActionResult> ImportaPessoas(IFormFile arquivo)
        {
            try
            {
                var arquivoStream = Converter.LerStream(arquivo);
                var pessoas = LerExcel.LerPessoas(arquivoStream);
                await _pessoasRepositorio.SalvarAsync(pessoas);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [Consumes("multipart/form-data")]
        [HttpPost("Importa-Animais-XLSX")]
        public async Task<ActionResult> ImportaAnimais(IFormFile arquivo)
        {
            try
            {
                var arquivoStream = Converter.LerStream(arquivo);
                var animais = LerExcel.LerAnimais(arquivoStream);
                await _animaisRepositorio.SalvarAsync(animais);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(500, ex.Message); ;

            }
        }

        [Consumes("multipart/form-data")]
        [HttpPost("Importa-AnimaisEPessoas-XLSX")]
        public async Task<ActionResult> ImportaAnimaisEPessoas(IFormFile arquivo)
        {
            try
            {
                var arquivoStream = Converter.LerStream(arquivo);
                var pessoas = LerExcel.LerPessoas(arquivoStream);
                var animais = LerExcel.LerAnimais(arquivoStream);
                await Task.WhenAll(
                    _pessoasRepositorio.SalvarAsync(pessoas),
                    _animaisRepositorio.SalvarAsync(animais)
                );
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
