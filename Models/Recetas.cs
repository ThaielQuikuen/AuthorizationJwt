
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ATDapi.Models
{
    public class Recetas
    {
        public int? id { get; set; }
        public string? nombre { get; set; }
        public string? receta { get; set; }

        public string? ingredientes { get; set; }
        public string? tiempo { get; set; }
        public int? porciones { get; set; }
        public int? fk_dificultad {  get; set; }
        public int? fk_usuario { get; set; }


    }
}
