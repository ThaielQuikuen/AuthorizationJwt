using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ATDapi.Responses;
using ATDapi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ATDapi.Repositories;
using Microsoft.AspNetCore.Identity;

[ApiController]
public class AuthController : ControllerBase
{
    private IConfiguration _configuration;
    private LoginModel loginModel = new LoginModel();
    private Repository repository = new Repository();

    public AuthController(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    string tabla = "Usuarios";

    [HttpGet]
    [Route("AuthController/Get")]
    public async Task<BaseResponse> Get()
    {
        var pathBase = HttpContext.User.Claims;
        string query = loginModel.select(tabla);
        try
        {
            var rsp = await repository.GetListBy<dynamic>(query);
            return new DataResponse<dynamic>(true, (int)HttpStatusCode.OK, "Lista de entidades", data: rsp);
        }
        catch (Exception ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var rsp = await repository.GetByQuery<dynamic>($"SELECT usuario_id, password FROM Usuarios WHERE Usuarios.usuario  = '{model.Usuario}'");
        var passwordHasher = new PasswordHasher<object>();
        var result = passwordHasher.VerifyHashedPassword(null, rsp.password, model.Password);
        if (result == PasswordVerificationResult.Success)
        {
            int userId = rsp.usuario_id;
            var token = GenerateAccessToken(model.Usuario, userId);

            return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        return Unauthorized("Invalid credentials");
    }

    private JwtSecurityToken GenerateAccessToken(string userName, int id)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Name, userName),
        };
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60), // Token expiration time
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                SecurityAlgorithms.HmacSha256)
        );
        return token;
    }

    [HttpPost]
    [Route("AuthController/post")]
    public async Task<BaseResponse> Post([FromBody] LoginModel model)
    {
        string query = model.insert();
        try
        {
            var rsp1 = await repository.GetByQuery<dynamic>($"SELECT usuario  FROM Usuarios WHERE Usuarios.usuario  = '{model.Usuario}'");
            if (rsp1 == null){
                var rsp = await repository.InsertByQuery(query);
                return new DataResponse<dynamic>(true, (int)HttpStatusCode.Created, "User correctly created", data: rsp);
            }else{
                return new DataResponse<dynamic>(true, (int)HttpStatusCode.Created, "El numbre de usuario ya existe. Elija uno distinto", data: rsp1);
            }     
        }
        catch (Exception Ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, Ex.Message);
        }
    }

    /*[HttpPatch]
    [Route("AuthController/Patch")]*/
    
        
    


}