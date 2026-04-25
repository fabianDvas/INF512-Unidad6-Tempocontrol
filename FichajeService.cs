using TempoControl.Data;
using TempoControl.Domain;

namespace TempoControl.Business;

public class FichajeService
{
    private readonly IEmpleadoRepository _empleados;
    private readonly IRegistroFichajeRepository _fichajes;

    public FichajeService(IEmpleadoRepository empleados, IRegistroFichajeRepository fichajes)
    {
        _empleados = empleados;
        _fichajes = fichajes;
    }

    public void PoncharEntrada(int empleadoId)
    {
        var abierto = _fichajes.ObtenerSinCerrar(empleadoId);
        if (abierto != null)
            throw new InvalidOperationException("Este empleado ya tiene una entrada sin cerrar.");
        _fichajes.RegistrarEntrada(empleadoId);
    }

    public void PoncharSalida(int empleadoId)
    {
        var abierto = _fichajes.ObtenerSinCerrar(empleadoId);
        if (abierto == null)
            throw new InvalidOperationException("No hay entrada abierta para este empleado.");
        _fichajes.RegistrarSalida(empleadoId);
    }

    public void GenerarReporteMensual(int mes, int anio)
    {
        var registros = _fichajes.ObtenerPorMes(mes, anio);
        var empleados = _empleados.ObtenerTodos();

        Console.WriteLine($"\n{"".PadLeft(50, '=')}");
        Console.WriteLine($"  REPORTE MENSUAL — {mes:D2}/{anio}");
        Console.WriteLine($"{"".PadLeft(50, '=')}");

        var agrupado = registros
            .Where(r => r.HoraSalida.HasValue)
            .GroupBy(r => r.EmpleadoId);

        bool hayDatos = false;
        foreach (var grupo in agrupado)
        {
            hayDatos = true;
            var emp = empleados.FirstOrDefault(e => e.Id == grupo.Key);
            if (emp == null) continue;

            double totalHoras = grupo.Sum(r => (r.HoraSalida!.Value - r.HoraEntrada).TotalHours);
            int diasTrabajados = grupo.Select(r => r.HoraEntrada.Date).Distinct().Count();

            Console.WriteLine($"\n  Empleado : {emp.NombreCompleto}");
            Console.WriteLine($"  Depto    : {emp.Departamento}");
            Console.WriteLine($"  Posición : {emp.Posicion}");
            Console.WriteLine($"  Días     : {diasTrabajados}");
            Console.WriteLine($"  Horas    : {totalHoras:F2}h");
            Console.WriteLine($"  {"".PadLeft(40, '-')}");
        }

        if (!hayDatos)
            Console.WriteLine("\n  No hay registros completados para este período.");
    }
}
