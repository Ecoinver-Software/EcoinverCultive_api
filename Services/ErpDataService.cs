using System;
using System.Collections.Generic;
using System.Data;
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
            SELECT cultivos.CUL_IdCultivo,
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
                   cultivos.CUL_FechaFinalizaProgra
            FROM cultivos
            LEFT JOIN fincas ON cultivos.CUL_IdFinca = fincas.FIN_IdFinca
            LEFT JOIN agricultores ON cultivos.CUL_IdAgriCultivo = agricultores.AGR_Idagricultor
            LEFT JOIN naves ON cultivos.CUL_IdNave = naves.NAV_IdNave 
            LEFT JOIN generos ON cultivos.CUL_IdGenero = generos.GEN_IdGenero
            LEFT JOIN variedades ON cultivos.CUL_IdVariedad = variedades.VAR_IdVariedad
            WHERE cultivos.CUL_Activo = 'S'
              AND cultivos.CUL_FechaLog > '2024-09-01'
              AND cultivos.CUL_IdNave IS NOT NULL
              AND naves.NAV_Nombre IS NOT NULL;
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
                ProduccionEstimada = reader.IsDBNull(11) ? null : reader.GetDouble(11),
                FechaSiembra = reader.IsDBNull(12) ? null : reader.GetDateTime(12),
                FechaFin = reader.IsDBNull(13) ? null : reader.GetDateTime(13)
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
}
