
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ATDapi.Models;


public class LoginModel
{
    public int id { get; set; }
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    public string Usuario { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }



    public string HashearPassword(string password)
    {
        var passwordHasher = new PasswordHasher<object>();
        return passwordHasher.HashPassword(null, password);
    }

    public DynamicParameters Login()
    {
        var dp = new DynamicParameters();
        dp.Add("usuario", this.Usuario);
        return dp;
    }

    public DynamicParameters insert()
    {
        var dp = new DynamicParameters();
        dp.Add("usuario", Usuario);
        dp.Add("password", HashearPassword(Password));
        return dp;
    }

    public DynamicParameters Existe(string Usuario)
    {
        var dp = new DynamicParameters();
        dp.Add("usuario", Usuario);
        return dp;
    }



}