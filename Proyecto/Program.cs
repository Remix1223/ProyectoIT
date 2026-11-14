using System;
using System.Collections.Generic;
using System.Linq;

List<Dictionary<string, object>> registros = new List<Dictionary<string, object>>();
int siguienteId = 1;

string PedirTexto(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string valor = (Console.ReadLine() ?? "").Trim();
        if (valor != "") return valor;
        Console.WriteLine("No puede estar vacío.");
    }
}

int PedirEntero(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        if (int.TryParse(Console.ReadLine(), out int n)) return n;
        Console.WriteLine("Debe ser un número entero.");
    }
}

decimal PedirDecimal(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        if (decimal.TryParse(Console.ReadLine(), out decimal d) && d >= 0) return d;
        Console.WriteLine("Debe ser un número válido (0 o mayor).");
    }
}

Dictionary<string, object>? BuscarPorId(int id)
{
    return registros.FirstOrDefault(r => (int)r["id"] == id);
}

void Agregar()
{
    string nombre = PedirTexto("Nombre: ");
    string categoria = PedirTexto("Categoría: ");
    decimal precio = PedirDecimal("Precio: ");

    registros.Add(new Dictionary<string, object>
    {
        { "id", siguienteId },
        { "nombre", nombre },
        { "categoria", categoria },
        { "precio", precio }
    });
    Console.WriteLine($"✔ Agregado con ID {siguienteId}");
    siguienteId++;
}

void Listar(List<Dictionary<string, object>> lista)
{
    if (lista.Count == 0)
    {
        Console.WriteLine("No hay registros.");
        return;
    }
    foreach (var r in lista)
        Console.WriteLine($"[{r["id"]}] {r["nombre"]} | {r["categoria"]} | ${(decimal)r["precio"]:F2}");
}

void Buscar()
{
    Console.WriteLine("1 Por ID  2 Por nombre  3  Por categoría  4  Por rango de precio");
    Console.Write("Criterio: ");
    string op = (Console.ReadLine() ?? "").Trim();

    switch (op)
    {
        case "1":
            var r = BuscarPorId(PedirEntero("ID: "));
            Listar(r != null ? new List<Dictionary<string, object>> { r } : new List<Dictionary<string, object>>());
            break;
        case "2":
            string t = PedirTexto("Nombre contiene: ").ToLower();
            Listar(registros.Where(x => ((string)x["nombre"]).ToLower().Contains(t)).ToList());
            break;
        case "3":
            string c = PedirTexto("Categoría: ").ToLower();
            Listar(registros.Where(x => ((string)x["categoria"]).ToLower() == c).ToList());
            break;
        case "4":
            decimal min = PedirDecimal("Precio mínimo: ");
            decimal max = PedirDecimal("Precio máximo: ");
            Listar(registros.Where(x => (decimal)x["precio"] >= min && (decimal)x["precio"] <= max).ToList());
            break;
        default:
            Console.WriteLine(" Opción inválida.");
            break;
    }
}

void Modificar()
{
    var r = BuscarPorId(PedirEntero("ID a modificar: "));
    if (r == null)
    {
        Console.WriteLine(" No existe ese ID.");
        return;
    }

    Console.WriteLine("Deja vacío para mantener el valor actual.");
    Console.Write($"Nombre [{r["nombre"]}]: ");
    string nombre = (Console.ReadLine() ?? "").Trim();
    Console.Write($"Categoría [{r["categoria"]}]: ");
    string categoria = (Console.ReadLine() ?? "").Trim();
    Console.Write($"Precio [{r["precio"]}]: ");
    string precio = (Console.ReadLine() ?? "").Trim();

    if (nombre != "") r["nombre"] = nombre;
    if (categoria != "") r["categoria"] = categoria;
    if (precio != "")
    {
        if (decimal.TryParse(precio, out decimal p) && p >= 0)
            r["precio"] = p;
        else
            Console.WriteLine("Precio inválido, se mantuvo el anterior.");
    }
    Console.WriteLine(" Modificado.");
}

void Eliminar()
{
    var r = BuscarPorId(PedirEntero("ID a eliminar: "));
    if (r == null)
    {
        Console.WriteLine(" No existe ese ID.");
        return;
    }

    Console.Write($"¿Eliminar '{r["nombre"]}'? (s/n): ");
    if ((Console.ReadLine() ?? "").Trim().ToLower() == "s")
    {
        registros.Remove(r);
        Console.WriteLine("Eliminado.");
    }
}

Console.OutputEncoding = System.Text.Encoding.UTF8;

bool salir = false;
while (!salir)
{
    Console.WriteLine("\n MENÚ");
    Console.WriteLine("1. Agregar\n2. Buscar\n3. Modificar\n4. Eliminar\n5. Listar\n6. Salir");
    Console.Write("Opción: ");
    string op = (Console.ReadLine() ?? "").Trim();

    try
    {
        switch (op)
        {
            case "1": Agregar(); break;
            case "2": Buscar(); break;
            case "3": Modificar(); break;
            case "4": Eliminar(); break;
            case "5": Listar(registros); break;
            case "6":
                Console.Write("¿Seguro que desea salir? (s/n): ");
                if ((Console.ReadLine() ?? "").Trim().ToLower() == "s")
                {
                    Console.WriteLine("Hasta luego.");
                    salir = true;
                }
                break;
            default:
                Console.WriteLine(" Opción inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Error inesperado: {ex.Message}");
    }
}