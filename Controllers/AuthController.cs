using Microsoft.AspNetCore.Mvc;
using System.Net;
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
    private readonly IConfiguration _configuration;
    private readonly LoginModel loginModel = new LoginModel();
    private readonly Repository repository = new Repository();

    public AuthController(IConfiguration configuration)
    {
        this._configuration = configuration;
    }


    [HttpGet]
    [Route("AuthController/Get")]
    public async Task<BaseResponse> Get()
    {
        try
        {
            var rsp = await repository.GetListFromProcedure<dynamic>("TodosUsuario");
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
        var rsp = await repository.GetListFromProcedure<UsuarioLogueado>("LoginUsuario", model.Login());
        var user = rsp.FirstOrDefault();
        if (user != null && !string.IsNullOrEmpty(user.password))
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, user.password, model.Password);
            if (result == PasswordVerificationResult.Success)
            {
                int userId = user.usuario_id;
                var token = GenerateAccessToken(model.Usuario, userId);
                return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }
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
        try
        {
            var rsp1 = await repository.GetListFromProcedure<dynamic>("ExisteUsuario", model.Existe(model.Usuario));
            int cont = rsp1.Count;
            if (cont == 0)
            {
                var rsp = await repository.ExecuteProcedure("CargarUsuario", model.insert());
                return new DataResponse<dynamic>(true, (int)HttpStatusCode.Created, "Usuario creado correctamente", data: rsp);
            }
            else
            {
                return new DataResponse<dynamic>(false, (int)HttpStatusCode.Created, "El nombre de usuario ya existe. Elija uno distinto", data: rsp1);
            }
        }
        catch (Exception Ex)
        {
            return new BaseResponse(false, (int)HttpStatusCode.InternalServerError, Ex.Message);
        }
    }






}