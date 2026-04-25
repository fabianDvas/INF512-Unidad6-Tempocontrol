using Microsoft.Data.Sqlite;
using TempoControl.Domain;

namespace TempoControl.Data;

public class RegistroFichajeRepository : IRegistroFichajeRepository
{
    private readonly DatabaseContext _context;

    public RegistroFichajeRepository(DatabaseContext context)
    {
        _context = context;
    }

    public void RegistrarEntrada(int empleadoId)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO RegistrosFichaje (EmpleadoId, HoraEntrada) VALUES ($eid, $he)";
        cmd.Parameters.AddWithValue("$eid", empleadoId);
        cmd.Parameters.AddWithValue("$he", DateTime.Now.ToString("o"));
        cmd.ExecuteNonQuery();
    }

    public void RegistrarSalida(int empleadoId)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            UPDATE RegistrosFichaje SET HoraSalida=$hs
            WHERE Id = (
                SELECT Id FROM RegistrosFichaje
                WHERE EmpleadoId=$eid AND HoraSalida IS NULL
                ORDER BY HoraEntrada DESC LIMIT 1
            )
        """;
        cmd.Parameters.AddWithValue("$hs", DateTime.Now.ToString("o"));
        cmd.Parameters.AddWithValue("$eid", empleadoId);
        cmd.ExecuteNonQuery();
    }

    public List<RegistroFichaje> ObtenerPorMes(int mes, int anio)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        string prefijo = $"{anio:D4}-{mes:D2}";
        cmd.CommandText = "SELECT * FROM RegistrosFichaje WHERE HoraEntrada LIKE $p";
        cmd.Parameters.AddWithValue("$p", prefijo + "%");
        using var reader = cmd.ExecuteReader();
        var lista = new List<RegistroFichaje>();
        while (reader.Read())
        {
            lista.Add(new RegistroFichaje
            {
                Id = reader.GetInt32(0),
                EmpleadoId = reader.GetInt32(1),
                HoraEntrada = DateTime.Parse(reader.GetString(2)),
                HoraSalida = reader.IsDBNull(3) ? null : DateTime.Parse(reader.GetString(3))
            });
        }
        return lista;
    }

    public RegistroFichaje? ObtenerSinCerrar(int empleadoId)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM RegistrosFichaje WHERE EmpleadoId=$eid AND HoraSalida IS NULL LIMIT 1";
        cmd.Parameters.AddWithValue("$eid", empleadoId);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return new RegistroFichaje
        {
            Id = reader.GetInt32(0),
            EmpleadoId = reader.GetInt32(1),
            HoraEntrada = DateTime.Parse(reader.GetString(2))
        };
    }
}
