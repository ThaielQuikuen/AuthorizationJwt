using ATDapi.Responses;
using ATDapi.Models;
using ATDapi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace ATDapi.Controllers;

[ApiController]

public class UsersController : ControllerBase{
    private Repository repository = new Repository();
    private UsersModel usersmodel = new UsersModel();
    [HttpPost]
    [Route("UsersController/Post")]
    public async Task<BaseResponse> Post()
    {
        string query = usersmodel.insert();
        try
        {
            var rsp = await repository.InsertByQuery(query);

            return new DataResponse<dynamic> (true,(int)HttpStatusCode.Created,"User correctly created",data : rsp);
        }
        catch (System.Exception Ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError,Ex.Message);
        }
    }

}