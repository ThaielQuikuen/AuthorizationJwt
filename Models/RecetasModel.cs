
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
        public int? porciones { get; set; }
        public int? fk_dificultad {  get; set; }
        public int? fk_usuario { get; set; }

        public override string insert()
        {
            return string.Format($"INSERT INTO Recetas(nombre, receta, ingredientes, porciones, fk_dificultad, fk_usuario) VALUES('{nombre}', '{receta}', '{ingredientes}', '{porciones}', {fk_dificultad}, {fk_usuario})");
        }
        public string update(int recetaId)
        {
            return string.Format($"UPDATE Recetas SET nombre = '{nombre}', receta = '{receta}', ingredientes = '{ingredientes}', porciones = {porciones}, fk_dificultad = {fk_dificultad} WHERE id = {recetaId} AND fk_usuario = {fk_usuario}");
        }
        public string delete(int recetaId)
        {
            return string.Format($"DELETE FROM Recetas WHERE id = {recetaId} AND fk_usuario = {fk_usuario}");
        }

    }
}
