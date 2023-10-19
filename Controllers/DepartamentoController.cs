using Api_EmpleadosADO.Data;
using Api_EmpleadosADO.Models;
using Api_EmpleadosADO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;

namespace Api_EmpleadosADO.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        //private readonly string conexion = "";
        private readonly IDepartamento _departamentoServices;

        public DepartamentoController(IDepartamento departamentoServices /*IConfiguration configuration*/) 
        {
            //conexion = configuration.GetConnectionString("ConexionBD");
            _departamentoServices = departamentoServices;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]//200-404
        public async Task<ActionResult<Departamento>> GetAll() 
        {
            var dpto = await _departamentoServices.GetDepartamentos();
            return Ok(dpto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Departamento>> GetDptoById(int id)
        {
            var dpto = await _departamentoServices.GetById(id);

            if( dpto.IdDepartamento == 0 || dpto.Descripcion == null)
            {
                return NotFound();
            }

            return dpto;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]//200-201-400
        public async Task<ActionResult<Departamento>> CreateDpto([FromBody] Departamento dpto)
        {
            var departamento = await _departamentoServices.Create(dpto);

            return Ok(departamento);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDpto(int id, [FromBody] Departamento dpto)
        {
            if (id != dpto.IdDepartamento)
            {
                return BadRequest();
            }

            var departamento = await _departamentoServices.Update(id, dpto);

            if (departamento.IdDepartamento == 0 || departamento.Descripcion == null)
            {
                return NotFound();
            }

            return NoContent();

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]//204-400-404
        public async Task<IActionResult> DeleteDpto(int id)
        {
            var departamento = await _departamentoServices.Delete(id);

            if (!departamento)
            {
                return NotFound();
            }

            return NoContent();

        }

    }

}
