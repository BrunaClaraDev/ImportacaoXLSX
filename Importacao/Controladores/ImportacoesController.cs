using Importacao.Actions;
using Importacao.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
        public ActionResult ImportaPessoas(IFormFile arquivo)
        {
            try
            {
                var arquivoStream = Converter.LerStream(arquivo);
                var pessoas = LerExcel.LerPessoas(arquivoStream);
                _pessoasRepositorio.Salvar(pessoas);

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
        public ActionResult ImportaAnimais(IFormFile arquivo)
        {
            try
            {
                var arquivoStream = Converter.LerStream(arquivo);
                var animais = LerExcel.LerAnimais(arquivoStream);
                _animaisRepositorio.Salvar(animais);

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
        public ActionResult ImportaAnimaisEPessoas(IFormFile arquivo)
        {
            try
            {
                var arquivoStream = Converter.LerStream(arquivo);
                var pessoas = LerExcel.LerPessoas(arquivoStream);
                _pessoasRepositorio.Salvar(pessoas);
                var animais = LerExcel.LerAnimais(arquivoStream);
                _animaisRepositorio.Salvar(animais);

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
