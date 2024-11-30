using ATDapi.Models;
using ATDapi.Repositories;
using ATDapi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]

public class DificultadesController : ControllerBase
{
    private readonly Repository repository = new Repository();

    [HttpGet]
    [Route("Get")]
    public async Task<BaseResponse> Get()
    {
        try
        {
            var rsp = await repository.GetListFromProcedure<dynamic>("ObtenerDificultades");
            return new DataResponse<dynamic>(true, (int)HttpStatusCode.OK, "Lista de Recetas", data: rsp);
        }
        catch (Exception ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }


}

