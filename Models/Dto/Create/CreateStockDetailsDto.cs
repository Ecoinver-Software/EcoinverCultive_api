namespace EcoinverGMAO_api.Models.Dto.Create
{
    public class CreateStockDetailsDto
    {
        public int NumBultos { get; set; }
        public int CodigoPartida { get; set; }
        public int IdGenero { get; set; }
        public string Categoria { get; set; }
        public int idControl { get; set; }
        public DateTime FechaCreacion { get; set; } 
    }
}
