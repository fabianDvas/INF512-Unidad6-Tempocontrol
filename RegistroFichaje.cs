namespace TempoControl.Domain;

public class RegistroFichaje
{
    public int Id { get; set; }
    public int EmpleadoId { get; set; }
    public DateTime HoraEntrada { get; set; }
    public DateTime? HoraSalida { get; set; }
}
