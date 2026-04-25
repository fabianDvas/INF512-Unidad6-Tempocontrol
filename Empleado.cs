namespace TempoControl.Domain;

public class Empleado
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string Posicion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}
