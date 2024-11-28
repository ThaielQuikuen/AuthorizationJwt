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

public class DificultadesController : ControllerBase
{
    private DificultadesModel dificultadesModel = new DificultadesModel();
    private Repository repository = new Repository();
    string tabla = "Dificultades";

    [HttpGet]
    [Route("Get")]
    public async Task<BaseResponse> Get()
    {
        string query = dificultadesModel.select(tabla); 
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

    
}

