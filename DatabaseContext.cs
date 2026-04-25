using Microsoft.Data.Sqlite;

namespace TempoControl.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string dbPath = "tempocontrol.db")
    {
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

    public SqliteConnection GetConnection() => new SqliteConnection(_connectionString);

    private void InitializeDatabase()
    {
        using var conn = GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Empleados (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                NombreCompleto TEXT NOT NULL,
                Departamento TEXT NOT NULL,
                Posicion TEXT NOT NULL,
                Activo INTEGER NOT NULL DEFAULT 1
            );

            CREATE TABLE IF NOT EXISTS RegistrosFichaje (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EmpleadoId INTEGER NOT NULL,
                HoraEntrada TEXT NOT NULL,
                HoraSalida TEXT,
                FOREIGN KEY (EmpleadoId) REFERENCES Empleados(Id)
            );
        """;
        cmd.ExecuteNonQuery();
    }
}
