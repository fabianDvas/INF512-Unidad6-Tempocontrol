using TempoControl.Business;
using TempoControl.Data;
using TempoControl.Domain;

namespace TempoControl.UI;

public class Menu
{
    private readonly FichajeService _service;
    private readonly IEmpleadoRepository _empleados;

    public Menu(FichajeService service, IEmpleadoRepository empleados)
    {
        _service = service;
        _empleados = empleados;
    }

    public void Ejecutar()
    {
        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("      TEMPOCONTROL           ");
            Console.WriteLine("  1. Gestión de Empleados     ");
            Console.WriteLine("  2. Registrar Entrada        ");
            Console.WriteLine("  3. Registrar Salida         ");
            Console.WriteLine("  4. Reporte Mensual          ");
            Console.WriteLine("  0. Salir                    ");
            Console.Write("\nOpción: ");

            switch (Console.ReadLine())
            {
                case "1": MenuEmpleados(); break;
                case "2": PoncharEntrada(); break;
                case "3": PoncharSalida(); break;
                case "4": Reporte(); break;
                case "0": salir = true; break;
                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void MenuEmpleados()
    {
        Console.Clear();
        Console.WriteLine("=== GESTIÓN DE EMPLEADOS ===");
        Console.WriteLine("1. Crear");
        Console.WriteLine("2. Listar todos");
        Console.WriteLine("3. Actualizar");
        Console.WriteLine("4. Desactivar");
        Console.Write("\nOpción: ");

        switch (Console.ReadLine())
        {
            case "1":
                var e = new Empleado();
                Console.Write("Nombre completo: ");
                e.NombreCompleto = Console.ReadLine()!;
                Console.Write("Departamento: ");
                e.Departamento = Console.ReadLine()!;
                Console.Write("Posición: ");
                e.Posicion = Console.ReadLine()!;
                _empleados.Crear(e);
                Console.WriteLine("\n✓ Empleado creado correctamente.");
                break;

            case "2":
                Console.WriteLine("\n--- LISTA DE EMPLEADOS ---");
                var lista = _empleados.ObtenerTodos();
                if (lista.Count == 0)
                {
                    Console.WriteLine("No hay empleados registrados.");
                }
                else
                {
                    foreach (var emp in lista)
                        Console.WriteLine($"  [{emp.Id}] {emp.NombreCompleto} | {emp.Departamento} | {emp.Posicion} | {(emp.Activo ? "Activo" : "Inactivo")}");
                }
                break;

            case "3":
                Console.Write("ID del empleado a actualizar: ");
                if (!int.TryParse(Console.ReadLine(), out int idA))
                {
                    Console.WriteLine("ID inválido."); break;
                }
                var ea = _empleados.ObtenerPorId(idA);
                if (ea == null) { Console.WriteLine("Empleado no encontrado."); break; }

                Console.Write($"Nuevo nombre [{ea.NombreCompleto}]: ");
                string n = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(n)) ea.NombreCompleto = n;

                Console.Write($"Nuevo departamento [{ea.Departamento}]: ");
                string d = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(d)) ea.Departamento = d;

                Console.Write($"Nueva posición [{ea.Posicion}]: ");
                string p = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(p)) ea.Posicion = p;

                _empleados.Actualizar(ea);
                Console.WriteLine("\n✓ Empleado actualizado.");
                break;

            case "4":
                Console.Write("ID del empleado a desactivar: ");
                if (!int.TryParse(Console.ReadLine(), out int idD))
                {
                    Console.WriteLine("ID inválido."); break;
                }
                _empleados.Desactivar(idD);
                Console.WriteLine("\n✓ Empleado marcado como inactivo.");
                break;

            default:
                Console.WriteLine("Opción inválida.");
                break;
        }

        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    private void PoncharEntrada()
    {
        Console.Clear();
        Console.WriteLine("=== REGISTRAR ENTRADA ===");
        Console.Write("ID del empleado: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido."); Console.ReadKey(); return;
        }
        try
        {
            _service.PoncharEntrada(id);
            Console.WriteLine($"\n✓ Entrada registrada: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error: {ex.Message}");
        }
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    private void PoncharSalida()
    {
        Console.Clear();
        Console.WriteLine("=== REGISTRAR SALIDA ===");
        Console.Write("ID del empleado: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido."); Console.ReadKey(); return;
        }
        try
        {
            _service.PoncharSalida(id);
            Console.WriteLine($"\n✓ Salida registrada: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error: {ex.Message}");
        }
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    private void Reporte()
    {
        Console.Clear();
        Console.WriteLine("=== REPORTE MENSUAL ===");
        Console.Write("Mes (1-12): ");
        if (!int.TryParse(Console.ReadLine(), out int mes) || mes < 1 || mes > 12)
        {
            Console.WriteLine("Mes inválido."); Console.ReadKey(); return;
        }
        Console.Write("Año (ej. 2025): ");
        if (!int.TryParse(Console.ReadLine(), out int anio))
        {
            Console.WriteLine("Año inválido."); Console.ReadKey(); return;
        }
        _service.GenerarReporteMensual(mes, anio);
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }
}
