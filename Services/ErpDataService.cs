using System;
using System.Collections.Generic;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using EcoinverGMAO_api.Models.Dto;

public class ErpDataService
{
    private readonly string _erpConnectionString;
    private readonly string _netagroConnectionString;

    public ErpDataService(IConfiguration config)
    {
        // Lee la conexión "ErpConnection" para cultivos
        _erpConnectionString = config.GetConnectionString("ErpConnection");
        // Lee la conexión "NetagroConnection" para clientes
        _netagroConnectionString = config.GetConnectionString("NetagroConnection");
    }

    /// <summary>
    /// Ejecuta la consulta a la base de datos del ERP para devolver cultivos activos,
    /// filtrados por CUL_FechaLog > '2024-09-01' y con varios LEFT JOINs.
    /// </summary>
    /// <returns>Lista de cultivos listos para sincronizar (CultiveSyncDto).</returns>
    public List<CultiveSyncDto> GetCultivosSincronizados()
    {
        var cultivos = new List<CultiveSyncDto>();

        using var conn = new MySqlConnection(_erpConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT 
                cultivos.CUL_IdCultivo,
                cultivos.CUL_IdAgriCultivo,
                agricultores.AGR_Nombre,
                cultivos.CUL_IdFinca,
                fincas.FIN_Nombre,
                cultivos.CUL_IdNave,
                naves.NAV_Nombre,
                cultivos.CUL_IdGenero,
                generos.GEN_Nombre,
                variedades.VAR_Nombre,
                cultivos.CUL_Superficie,
                cultivos.CUL_ProduccionEstimada,
                cultivos.CUL_FechaSiembraProgra,
                cultivos.CUL_FechaFinalizaProgra,
                fincas.FIN_Latitud,
                fincas.FIN_Longitud,
                tecnicos.TEC_Nombre,
                fincas.FIN_Provincia
            FROM cultivos
                LEFT JOIN fincas      ON cultivos.CUL_IdFinca    = fincas.FIN_IdFinca
                LEFT JOIN agricultores ON cultivos.CUL_IdAgriCultivo = agricultores.AGR_Idagricultor
                LEFT JOIN naves       ON cultivos.CUL_IdNave     = naves.NAV_IdNave 
                LEFT JOIN generos     ON cultivos.CUL_IdGenero   = generos.GEN_IdGenero
                LEFT JOIN variedades  ON cultivos.CUL_IdVariedad = variedades.VAR_IdVariedad
                LEFT JOIN tecnicos    ON cultivos.CUL_IdTecnico  = tecnicos.TEC_IdTecnico
            WHERE cultivos.CUL_Activo       = 'S'
                  AND cultivos.CUL_FechaLog     > '2024-09-01'
                  AND fincas.FIN_Provincia        IS NOT NULL
                  AND CHAR_LENGTH(TRIM(fincas.FIN_Provincia)) > 0
                  AND tecnicos.TEC_Nombre         IS NOT NULL
                  AND CHAR_LENGTH(TRIM(tecnicos.TEC_Nombre)) > 0
                  AND naves.NAV_Nombre            IS NOT NULL
                  AND CHAR_LENGTH(TRIM(naves.NAV_Nombre))   > 0;

    ";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var cultivo = new CultiveSyncDto
            {
                IdCultivo = reader.GetInt32(0),
                IdAgricultor = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                NombreAgricultor = reader.IsDBNull(2) ? null : reader.GetString(2),
                IdFinca = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                NombreFinca = reader.IsDBNull(4) ? null : reader.GetString(4),
                IdNave = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                NombreNave = reader.IsDBNull(6) ? null : reader.GetString(6),
                IdGenero = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                NombreGenero = reader.IsDBNull(8) ? null : reader.GetString(8),
                NombreVariedad = reader.IsDBNull(9) ? null : reader.GetString(9),
                Superficie = reader.IsDBNull(10) ? null : reader.GetDouble(10),
                //ProduccionEstimada = reader.IsDBNull(11) ? null : reader.GetDouble(11),
                FechaSiembra = reader.IsDBNull(12) ? null : reader.GetDateTime(12),
                FechaFin = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                Latitud = reader.IsDBNull(14) ? null : reader.GetString(14),
                Longitud = reader.IsDBNull(15) ? null : reader.GetString(15),
                Tecnico = reader.IsDBNull(15) ? null : reader.GetString(16),
                Provincia = reader.IsDBNull(15) ? null : reader.GetString(17)


            };
            cultivos.Add(cultivo);
        }

        return cultivos;
    }


    /// <summary>
    /// Ejecuta una consulta en la base de datos NetagroComer para obtener los clientes activos.
    /// </summary>
    /// <returns>Lista de clientes listos para sincronizar (ClientSyncDto).</returns>
    public List<ClientSyncDto> GetClientsSincronizados()
    {
        var clients = new List<ClientSyncDto>();

        using var conn = new MySqlConnection(_netagroConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
           SELECT clientes.CLI_Idcliente, clientes.CLI_Nombre FROM clientes
           WHERE clientes.CLI_bloqueo = 'N'
        ";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var client = new ClientSyncDto
            {
                ClientId = reader.GetInt32("CLI_Idcliente"),
                Name = reader.IsDBNull(reader.GetOrdinal("CLI_Nombre")) ? null : reader.GetString("CLI_Nombre"),
            };
            clients.Add(client);
        }

        return clients;
    }
    public List<CultiveDataRealDto> GetCultivosProduccionReal()
    {
        var lista = new List<CultiveDataRealDto>();

        using var conn = new MySqlConnection(_erpConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT
            c.CUL_IdCultivo,
            c.CUL_IdFinca,
            c.CUL_IdAgriCultivo,
            a.AGR_Nombre,
            c.CUL_IdGenero,
            c.CUL_Superficie,
            SUM(l.AEL_kilosnetos)                                   AS KilosNetos,
            (SUM(l.AEL_kilosnetos) / NULLIF(c.CUL_Superficie,0))   AS KilosM2
        FROM cultivos AS c
        LEFT JOIN fincas     AS f ON c.CUL_IdFinca    = f.FIN_IdFinca
        LEFT JOIN agricultores AS a ON c.CUL_IdAgriCultivo = a.AGR_Idagricultor
        LEFT JOIN naves      AS n ON c.CUL_IdNave     = n.NAV_IdNave
        LEFT JOIN generos    AS g ON c.CUL_IdGenero   = g.GEN_IdGenero
        LEFT JOIN variedades AS v ON c.CUL_IdVariedad = v.VAR_IdVariedad
        LEFT JOIN tecnicos   AS t ON c.CUL_IdTecnico  = t.TEC_IdTecnico
        LEFT JOIN netagrocomer.albentrada_lineas AS l 
            ON c.CUL_IdCultivo = l.AEL_idcultivo
        WHERE
            c.CUL_Activo    = 'S'
            AND c.CUL_FechaLog > '2024-09-01'
            AND f.FIN_Provincia    IS NOT NULL
            AND CHAR_LENGTH(TRIM(f.FIN_Provincia)) > 0
            AND t.TEC_Nombre       IS NOT NULL
            AND CHAR_LENGTH(TRIM(t.TEC_Nombre)) > 0
            AND n.NAV_Nombre       IS NOT NULL
            AND CHAR_LENGTH(TRIM(n.NAV_Nombre))   > 0
        GROUP BY
            c.CUL_IdCultivo,
            c.CUL_IdFinca,
            c.CUL_IdAgriCultivo,
            a.AGR_Nombre,
            c.CUL_IdGenero,
            c.CUL_Superficie
        ORDER BY
            KilosM2 DESC;
    ";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new CultiveDataRealDto
            {
                // Id lo igualamos al IdCultivo para mantener un identificador único
                Id = reader.GetInt32("CUL_IdCultivo"),
                IdCultivo = reader.GetInt32("CUL_IdCultivo"),
                IdAgricultor = reader.IsDBNull(reader.GetOrdinal("CUL_IdAgriCultivo"))
                                      ? (int?)null
                                      : reader.GetInt32("CUL_IdAgriCultivo"),
                NombreAgricultor = reader.IsDBNull(reader.GetOrdinal("AGR_Nombre"))
                                      ? null
                                      : reader.GetString("AGR_Nombre"),
                IdFinca = reader.IsDBNull(reader.GetOrdinal("CUL_IdFinca"))
                                      ? (int?)null
                                      : reader.GetInt32("CUL_IdFinca"),
                IdGenero = reader.IsDBNull(reader.GetOrdinal("CUL_IdGenero"))
                                      ? (int?)null
                                      : reader.GetInt32("CUL_IdGenero"),
                Superficie = reader.IsDBNull(reader.GetOrdinal("CUL_Superficie"))
                                      ? (double?)null
                                      : reader.GetDouble("CUL_Superficie"),
                KilosNetos = reader.IsDBNull(reader.GetOrdinal("KilosNetos"))
                                      ? (double?)null
                                      : reader.GetDouble("KilosNetos"),
                KilosM2 = reader.IsDBNull(reader.GetOrdinal("KilosM2"))
                                      ? (double?)null
                                      : reader.GetDouble("KilosM2")
            });
        }

        return lista;
    }
    public List<GenderSyncDto> GetGenerosSincronizados()
    {
        var generos = new List<GenderSyncDto>();

        using var conn = new MySqlConnection(_netagroConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT 
            generos.GEN_IdGenero,
            generos.GEN_NombreGenero,
            familiasgeneros.FAG_idfamilia,
            familiasgeneros.FAG_nombre
        FROM generos
        LEFT JOIN familiasgeneros
          ON LEFT(generos.GEN_IdGenero, 2) = familiasgeneros.FAG_idfamilia;
     ";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var genero = new GenderSyncDto
            {
                IdGenero = reader.GetInt32(0),
                NombreGenero = reader.IsDBNull(1) ? null : reader.GetString(1),
                IdFamilia = reader.IsDBNull(2) ? null : reader.GetInt32(2).ToString(),
                NombreFamilia = reader.IsDBNull(3) ? null : reader.GetString(3)
            };
            generos.Add(genero);
        }

        return generos;
    }
    public List<CultiveDataRealDto> GetProduccionPorTiempo(
        DateTime fechaInicio,
        int idGenero)
    {
        var resultados = new List<CultiveDataRealDto>();

        using var conn = new MySqlConnection(_erpConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
       SELECT
    DATE_SUB(e.AEN_fecha, INTERVAL WEEKDAY(e.AEN_fecha) DAY) AS SemanaInicio,
    DATE_ADD(DATE_SUB(e.AEN_fecha, INTERVAL WEEKDAY(e.AEN_fecha) DAY), INTERVAL 6 DAY) AS SemanaFin,
    c.CUL_IdCultivo,
    c.CUL_IdAgriCultivo,
    a.AGR_Nombre AS NombreAgricultor,
    c.CUL_IdGenero,
    c.CUL_Superficie,
    SUM(l.AEL_kilosnetos) AS TotalKilosNetos
FROM cultivos AS c
LEFT JOIN fincas AS f
    ON c.CUL_IdFinca = f.FIN_IdFinca
LEFT JOIN agricultores AS a
    ON c.CUL_IdAgriCultivo = a.AGR_Idagricultor
LEFT JOIN naves AS n
    ON c.CUL_IdNave = n.NAV_IdNave
LEFT JOIN generos AS g
    ON c.CUL_IdGenero = g.GEN_IdGenero
LEFT JOIN variedades AS v
    ON c.CUL_IdVariedad = v.VAR_IdVariedad
LEFT JOIN tecnicos AS t
    ON c.CUL_IdTecnico = t.TEC_IdTecnico
LEFT JOIN netagrocomer.albentrada_lineas AS l
    ON c.CUL_IdCultivo = l.AEL_idcultivo
LEFT JOIN netagrocomer.albentrada AS e
    ON l.AEL_idalbaran = e.AEN_idalbaran
WHERE
    c.CUL_Activo = 'S'
    AND c.CUL_FechaLog > '2024-09-01'
    AND f.FIN_Provincia IS NOT NULL
    AND CHAR_LENGTH(TRIM(f.FIN_Provincia)) > 0
    AND t.TEC_Nombre IS NOT NULL
    AND CHAR_LENGTH(TRIM(t.TEC_Nombre)) > 0
    AND n.NAV_Nombre IS NOT NULL
    AND CHAR_LENGTH(TRIM(n.NAV_Nombre)) > 0
    AND e.AEN_fecha BETWEEN @fechaInicio AND CURDATE()
    AND l.AEL_idgenero = @idGenero
GROUP BY
    SemanaInicio,
    SemanaFin,
    c.CUL_IdCultivo,
    c.CUL_IdAgriCultivo,
    a.AGR_Nombre,
    c.CUL_IdGenero,
    c.CUL_Superficie
ORDER BY
    SemanaInicio;

    ";

        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
        cmd.Parameters.AddWithValue("@idGenero", idGenero);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            resultados.Add(new CultiveDataRealDto
            {
                FechaInicio = reader.GetDateTime("SemanaInicio"),
                FechaFin = reader.GetDateTime("SemanaFin"),
                IdCultivo = reader.GetInt32("CUL_IdCultivo"),
                IdAgricultor = reader.IsDBNull(reader.GetOrdinal("CUL_IdAgriCultivo"))
                                     ? (int?)null
                                     : reader.GetInt32("CUL_IdAgriCultivo"),
                NombreAgricultor = reader.IsDBNull(reader.GetOrdinal("NombreAgricultor"))
                                     ? null
                                     : reader.GetString("NombreAgricultor"),
                IdGenero = reader.IsDBNull(reader.GetOrdinal("CUL_IdGenero"))
                                     ? (int?)null
                                     : reader.GetInt32("CUL_IdGenero"),
                Superficie = reader.IsDBNull(reader.GetOrdinal("CUL_Superficie"))
                                     ? (double?)null
                                     : reader.GetDouble("CUL_Superficie"),
                KilosNetos = reader.IsDBNull(reader.GetOrdinal("TotalKilosNetos"))
                                     ? (double?)null
                                     : reader.GetDouble("TotalKilosNetos")
            });
        }

        return resultados;
    }



}
