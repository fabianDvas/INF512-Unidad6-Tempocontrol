using Microsoft.Data.Sqlite;
using TempoControl.Domain;

namespace TempoControl.Data;

public class EmpleadoRepository : IEmpleadoRepository
{
    private readonly DatabaseContext _context;

    public EmpleadoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public void Crear(Empleado e)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Empleados (NombreCompleto, Departamento, Posicion, Activo) VALUES ($n, $d, $p, 1)";
        cmd.Parameters.AddWithValue("$n", e.NombreCompleto);
        cmd.Parameters.AddWithValue("$d", e.Departamento);
        cmd.Parameters.AddWithValue("$p", e.Posicion);
        cmd.ExecuteNonQuery();
    }

    public Empleado? ObtenerPorId(int id)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Empleados WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read()) return MapearEmpleado(reader);
        return null;
    }

    public List<Empleado> ObtenerTodos()
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Empleados";
        using var reader = cmd.ExecuteReader();
        var lista = new List<Empleado>();
        while (reader.Read()) lista.Add(MapearEmpleado(reader));
        return lista;
    }

    public void Actualizar(Empleado e)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Empleados SET NombreCompleto=$n, Departamento=$d, Posicion=$p WHERE Id=$id";
        cmd.Parameters.AddWithValue("$n", e.NombreCompleto);
        cmd.Parameters.AddWithValue("$d", e.Departamento);
        cmd.Parameters.AddWithValue("$p", e.Posicion);
        cmd.Parameters.AddWithValue("$id", e.Id);
        cmd.ExecuteNonQuery();
    }

    public void Desactivar(int id)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Empleados SET Activo=0 WHERE Id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    private static Empleado MapearEmpleado(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        NombreCompleto = r.GetString(1),
        Departamento = r.GetString(2),
        Posicion = r.GetString(3),
        Activo = r.GetInt32(4) == 1
    };
}
