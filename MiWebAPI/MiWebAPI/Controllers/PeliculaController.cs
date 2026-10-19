using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MiWebAPI.Data;
using MiWebAPI.Models;

namespace MiWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly PeliculaData _peliculaData;
        public PeliculaController(PeliculaData peliculaData)
        {
            _peliculaData = peliculaData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Pelicula> Lista = await _peliculaData.Lista();
            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(long id)
        {
            Pelicula objeto = await _peliculaData.Obtener(id);
            return StatusCode(StatusCodes.Status200OK, objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Pelicula objeto)
        {
            bool respuesta = await _peliculaData.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Pelicula objeto)
        {
            bool respuesta = await _peliculaData.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(long id)
        {
            bool respuesta = await _peliculaData.Eliminar(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
