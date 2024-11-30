
using Dapper;

namespace ATDapi.Models
{
    public class RecetasModel
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? receta { get; set; }
        public string? ingredientes { get; set; }
        public int? porciones { get; set; }
        public int? fk_dificultad {  get; set; }
        public int? fk_usuario { get; set; }

        public DynamicParameters insert()
        {
            var dp = new DynamicParameters();
            dp.Add("nombre", nombre);
            dp.Add("receta", receta);
            dp.Add("ingredientes", ingredientes);
            dp.Add("porciones", porciones);
            dp.Add("fk_dificultad", fk_dificultad);
            dp.Add("fk_usuario", fk_usuario);
            return dp;
        }

        public DynamicParameters select(int id)
        {
            var dp = new DynamicParameters();
            dp.Add("usuario_id",id);
            return dp;
        }

        public DynamicParameters modificar(int receta_id)
        {
            var dp = new DynamicParameters();
            dp.Add("receta_id", receta_id);
            dp.Add("nombre", nombre);
            dp.Add("receta", receta);
            dp.Add("ingredientes", ingredientes);
            dp.Add("porciones", porciones);
            dp.Add("fk_dificultad", fk_dificultad);
            return dp;
        }

        public DynamicParameters delete(int receta_id)
        {
            var dp = new DynamicParameters();
            dp.Add("receta_id",receta_id);
            return dp;
        }
    }
}
