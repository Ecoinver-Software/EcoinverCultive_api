using System.ComponentModel.DataAnnotations;

namespace EcoinverGMAO_api.Models
{
    public class Cultivo
    {
        public int Id { get; set; }       // Ajusta el tipo/nombre si tu tabla usa otra columna como PK
        public string Nombre { get; set; }
        // Aquí pones todas las propiedades que correspondan a las columnas de la tabla Cultivos
        // ...
    }   
}
