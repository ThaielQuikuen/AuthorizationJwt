using ATDapi.Models;
using ATDapi.Repositories;
using ATDapi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]

public class RecetasController : ControllerBase
{
    private RecetasModel recetasModel = new RecetasModel();
    private Repository repository = new Repository();
    string tabla = "Recetas";

    [HttpGet]
    [Route("RecetasController/Get")]
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
}

