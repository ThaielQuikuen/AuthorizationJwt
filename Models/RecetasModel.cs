
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ATDapi.Models
{
    public class RecetasModel : Queries
    {
        public int? id { get; set; }
        public string? nombre { get; set; }
        public string? receta { get; set; }

        public string? ingredientes { get; set; }
        public TimeOnly? tiempo { get; set; }
        public int? porciones { get; set; }
        public int? fk_dificultad {  get; set; }
        public int? fk_usuario { get; set; }

        public override string insert()
        {
            return string.Format($"INSERT INTO Recetas(nombre, receta, ingredientes, tiempo, porciones, fk_dificultad, fk_usuario) VALUES('{nombre}', '{receta}', '{ingredientes}', {tiempo}, '{porciones}', {fk_dificultad}, {fk_usuario})");
        }
    }
}
