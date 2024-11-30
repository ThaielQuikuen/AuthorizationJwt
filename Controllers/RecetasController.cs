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

        try
        {
            var rsp = await repository.GetListFromProcedure<dynamic>("VerRecetaPorUsuario",recetasModel.select(userId));
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
            var rsp = await repository.ExecuteProcedure("CargarReceta",model.insert());

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
            var usuarioIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null)
            {
                return new BaseResponse(false, (int)HttpStatusCode.Unauthorized, "Usuario no autenticado");
            }
   
            
            int rsp = await repository.ExecuteProcedure("ModificarReceta", model.modificar(model.id));
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
    

    [HttpDelete]
    [Route("Delete")]
    public async Task<BaseResponse> DeleteRecipe([FromQuery]int id)
    {
        try
        {
            // Ejecutar la consulta
            int result = await repository.ExecuteProcedure("EliminarReceta",recetasModel.delete(id)); 
            if (result > 0)
            {
                return new BaseResponse(true, (int)HttpStatusCode.OK, "Receta eliminada exitosamente");
            }
            else
            {
                return new BaseResponse(false, (int)HttpStatusCode.NotFound, "No se encontró la receta o no tienes permiso para eliminarla");
            }
        }
        catch (Exception ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }



}

