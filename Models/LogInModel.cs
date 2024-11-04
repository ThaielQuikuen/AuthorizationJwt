
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ATDapi.Models;


public class LoginModel : Queries
{
    public int? id { get; set; }
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    public string Usuario { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public IFormFile? Photo {get; set;}

    public int role {get; set;}
    public string HashearPassword(string password)
    {
        var passwordHasher = new PasswordHasher<object>();
        return passwordHasher.HashPassword(null, password);
    }

    public override string insert()
    {
        string hash = this.HashearPassword(this.Password);
        return string.Format($"INSERT INTO Usuarios(usuario , password,fk_role) VALUES('{Usuario}','{hash}',{1})");
    }

}