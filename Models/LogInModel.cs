
using System.ComponentModel.DataAnnotations;

namespace ATDapi.Models;


public class LoginModel : Queries
{
    public int? id { get; set; }
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public int role {get; set;}
    public override string insert()
    {
        return string.Format($"INSERT INTO Users(username, password,fk_role) VALUES('{Username}','{Password}',{1})");
    }

}