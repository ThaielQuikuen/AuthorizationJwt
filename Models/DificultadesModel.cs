using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ATDapi.Models
{
    public class DificultadesModel : Queries
    {
        public int dificultad_id { get; set; }

        public string dificultad { get; set; }
        public override string insert()
        {
            return string.Format($"INSERT INTO Dificultades(dificultad) VALUES('{dificultad}')");
        }
    }
}
