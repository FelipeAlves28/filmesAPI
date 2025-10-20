using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um filme
        /// </summary>
        /// <param name="filmeDto">Objeto para criação do filme no banco de dados</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Status para quando houver criação dos dados com sucesso</response>
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);
            _context.Filmes.Add(filme);
            _context.SaveChanges();
            CreatedAtActionResult CreatedAtAction = new CreatedAtActionResult(nameof(RecuperaFilmePorId), "filme", new { id = filme.id }, filme);

            return CreatedAtAction;
        }

        /// <summary>
        /// Retorna uma lista de filmes
        /// </summary>
        /// <param name="skip">Utilizado para retornar por seções de acordo com o número definido</param>
        /// <param name="take">Utilizado para retornar a quantidade definida de resultados</param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Status para quando for recuperado os filmes com sucesso</response>
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
        }

        /// <summary>
        /// Retorna um filme por id
        /// </summary>
        /// <param name="id">Identificador para recuperar o filme no banco de dados</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Status para quando for recuperado o filme com sucesso</response>
        [HttpGet("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public IActionResult RecuperaFilmePorId(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.id == id);

            if (filme is null)
            {
                return new NotFoundResult();
            }

            ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);
            return new OkObjectResult(filmeDto);
        }

        /// <summary>
        /// Atualiza todos os dados de um filme
        /// </summary>
        /// <param name="id">Identificador para atualizar o filme no banco de dados</param>
        /// <param name="filmeDto">Objeto alterado que vai ser utilizado para atualizar no banco de dados</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Status para quando for atualizado o filme com sucesso</response>
        [HttpPut("{id}")]
        [ProducesResponseType(statusCode:StatusCodes.Status204NoContent)]
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.id == id);
            if (filme is null)
            {
                return new NotFoundResult();
            }

            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();

            return new NoContentResult();
        }


        /// <summary>
        /// Atualiza apenas os campos necessários de um filme
        /// </summary>
        /// <param name="id">Identificador para atualizar o filme no banco de dados</param>
        /// <param name="patch">Objeto trazendo apenas o campo necessário para atualização</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Status para quando for atualizado o filme com sucesso</response>
        //Patch utilizando o NewtonSoft.Json
        [HttpPatch("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.id == id);
            if (filme is null)
            {
                return new NotFoundResult();
            }

            var filmeAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
            patch.ApplyTo(filmeAtualizar, ModelState);

            if (!TryValidateModel(filmeAtualizar))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(filmeAtualizar, filme);
            _context.SaveChanges();

            return new NoContentResult();
        }
        /// <summary>
        /// Deleta um filme
        /// </summary>
        /// <param name="id">Identificador para deletar o filme no banco de dados</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Status para quando for atualizado o filme com sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public IActionResult DeletaFilme(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.id == id);
            if (filme is null)
            {
                return new NotFoundResult();
            }

            _context.Remove(filme);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}
