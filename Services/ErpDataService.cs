using System.Data;
using MySqlConnector;
using Microsoft.Extensions.Configuration;

public class ErpDataService
{
    private readonly string _connectionString;

    public ErpDataService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("ErpConnection");
    }

    public List<CultivoDTO> GetCultivos()
    {
        var cultivos = new List<CultivoDTO>();

        using (var conn = new MySqlConnection(_connectionString)) 
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT  cultivos.CUL_IdCultivo FROM cultivos ";
                // Ajustar nombres de columnas/tablas según tu DB
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cultivo = new CultivoDTO
                        {
                            Id = reader.GetInt32(0),
                        };
                        cultivos.Add(cultivo);
                    }
                }
            }
        }

        return cultivos;
    }
}

// Un DTO (Data Transfer Object) para mapear lo que sale de la DB
public class CultivoDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
}
