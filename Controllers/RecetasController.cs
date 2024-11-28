using ATDapi.Models;
using ATDapi.Repositories;
using ATDapi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]

public class RecetasController : ControllerBase
{
    private RecetasModel recetasModel = new RecetasModel();
    private Repository repository = new Repository();
    string tabla = "Recetas";

    /*[HttpGet]
    [Route("Get")]
    public async Task<BaseResponse> Get()
    {
        string query = recetasModel.select(tabla); 
        try
        {
            var rsp = await repository.GetListBy<dynamic>(query);
            return new DataResponse<dynamic>(true, (int)HttpStatusCode.OK, "Lista de Recetas", data: rsp);
        }
        catch (Exception ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }*/
    [HttpGet]
    [Route("Get")]
    public async Task<BaseResponse> Get()
    {
        var usuarioIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (usuarioIdClaim == null)
        {
            return new BaseResponse(false, (int)HttpStatusCode.Unauthorized, "Usuario no autenticado");
        }

        int userId = int.Parse(usuarioIdClaim.Value); 
        string query = recetasModel.select(tabla) + $" WHERE fk_usuario = {userId}"; // ver recetas solo del usuario aut

        try
        {
            var rsp = await repository.GetListBy<dynamic>(query);
            return new DataResponse<dynamic>(true, (int)HttpStatusCode.OK, "Lista de Recetas", data: rsp);
        }
        catch (Exception ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("post")]
    public async Task<BaseResponse> Post([FromBody] RecetasModel model)
    {
        try
        {
            // Obtén el `fk_usuario` del usuario autenticado
            var usuarioIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (usuarioIdClaim == null)
            {
                return new BaseResponse(false, (int)HttpStatusCode.Unauthorized, "Usuario no autenticado");
            }

            // Log para debugging
            Console.WriteLine($"Usuario autenticado con ID: {usuarioIdClaim.Value}");
            // Asigna el `fk_usuario` al modelo
            model.fk_usuario = int.Parse(usuarioIdClaim.Value);

            // Inserta el registro
            string query = model.insert();
            var rsp = await repository.InsertByQuery(query);

            return new DataResponse<dynamic>(true, (int)HttpStatusCode.Created, "Receta creada correctamente", data: rsp);
        }
        catch (Exception ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPatch]
    [Route("Patch")]
    public async Task<BaseResponse> Patch([FromBody] RecetasModel model)
    {
        try
        {
            // Verifica si la receta existe
            var usuarioIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null)
            {
                return new BaseResponse(false, (int)HttpStatusCode.Unauthorized, "Usuario no autenticado");
            }

            int userId = int.Parse(usuarioIdClaim.Value);
            if (model.fk_usuario != userId)
            {
                return new BaseResponse(false, (int)HttpStatusCode.Forbidden, "No puedes modificar recetas de otros usuarios");
            }

            // Llama al método update del modelo para generar la consulta SQL
            string query = model.update(model.id.Value);

            // Ejecuta la consulta utilizando el repositorio
            var rsp = await repository.InsertByQuery(query); // Usamos InsertByQuery porque el método genera una consulta SQL

            // Verifica si se afectó alguna fila (si rsp > 0 significa que se hizo la actualización)
            if (rsp > 0)
            {
                return new DataResponse<dynamic>(true, (int)HttpStatusCode.OK, "Receta actualizada correctamente", data: rsp);
            }
            else
            {
                return new DataResponse<dynamic>(false, (int)HttpStatusCode.NotFound, "Receta no encontrada", data: rsp);
            }
        }
        catch (Exception ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        try
        {
            var usuarioId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string query = recetasModel.delete(id);

            // Ejecutar la consulta
            int result = await repository.DeleteAsync(query); 

            if (result > 0)
            {
                return Ok(new { message = "Receta eliminada exitosamente" });
            }
            else
            {
                return NotFound(new { message = "No se encontró la receta o no tienes permiso para eliminarla" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error al eliminar la receta", error = ex.Message });
        }
    }



}

