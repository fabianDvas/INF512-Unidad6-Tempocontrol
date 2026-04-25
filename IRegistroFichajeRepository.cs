using TempoControl.Domain;

namespace TempoControl.Data;

public interface IRegistroFichajeRepository
{
    void RegistrarEntrada(int empleadoId);
    void RegistrarSalida(int empleadoId);
    List<RegistroFichaje> ObtenerPorMes(int mes, int anio);
    RegistroFichaje? ObtenerSinCerrar(int empleadoId);
}
