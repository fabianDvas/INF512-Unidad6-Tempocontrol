using TempoControl.Domain;

namespace TempoControl.Data;

public interface IEmpleadoRepository
{
    void Crear(Empleado empleado);
    Empleado? ObtenerPorId(int id);
    List<Empleado> ObtenerTodos();
    void Actualizar(Empleado empleado);
    void Desactivar(int id);
}
