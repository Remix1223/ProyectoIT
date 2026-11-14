using System;
using System.Collections.Generic;

public class Menu
{
    private Inventario inventario = new Inventario();

    private string PedirTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string valor = (Console.ReadLine() ?? "").Trim();
            if (valor != "") return valor;
            Console.WriteLine("No puede estar vacío.");
        }
    }

    private int PedirEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine(), out int n)) return n;
            Console.WriteLine("Debe ser un número entero.");
        }
    }

    private decimal PedirDecimal(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (decimal.TryParse(Console.ReadLine(), out decimal d) && d >= 0) return d;
            Console.WriteLine("Debe ser un número válido (0 o mayor).");
        }
    }

    private void MostrarLista(List<Producto> lista)
    {
        if (lista.Count == 0)
        {
            Console.WriteLine("No hay registros.");
            return;
        }
        foreach (Producto p in lista)
            Console.WriteLine(p);
    }

    private void Agregar()
    {
        string nombre = PedirTexto("Nombre: ");
        string categoria = PedirTexto("Categoría: ");
        decimal precio = PedirDecimal("Precio: ");
        int id = inventario.Agregar(nombre, categoria, precio);
        Console.WriteLine($"Agregado con ID {id}");
    }

    private void Buscar()
    {
        Console.WriteLine("1. Por ID  2. Por nombre  3. Por categoría  4. Por rango de precio");
        Console.Write("Criterio: ");
        string opcion = (Console.ReadLine() ?? "").Trim();

        switch (opcion)
        {
            case "1":
                Producto? p = inventario.BuscarPorId(PedirEntero("ID: "));
                MostrarLista(p != null ? new List<Producto> { p } : new List<Producto>());
                break;
            case "2":
                MostrarLista(inventario.BuscarPorNombre(PedirTexto("Nombre contiene: ")));
                break;
            case "3":
                MostrarLista(inventario.BuscarPorCategoria(PedirTexto("Categoría: ")));
                break;
            case "4":
                decimal minimo = PedirDecimal("Precio mínimo: ");
                decimal maximo = PedirDecimal("Precio máximo: ");
                MostrarLista(inventario.BuscarPorPrecio(minimo, maximo));
                break;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }

    private void Modificar()
    {
        int id = PedirEntero("ID a modificar: ");
        Producto? actual = inventario.BuscarPorId(id);
        if (actual == null)
        {
            Console.WriteLine("No existe ese ID.");
            return;
        }

        Console.WriteLine("Deja vacío para mantener el valor actual.");
        Console.Write($"Nombre [{actual.Nombre}]: ");
        string nombre = (Console.ReadLine() ?? "").Trim();
        Console.Write($"Categoría [{actual.Categoria}]: ");
        string categoria = (Console.ReadLine() ?? "").Trim();
        Console.Write($"Precio [{actual.Precio}]: ");
        string textoPrecio = (Console.ReadLine() ?? "").Trim();

        decimal? precio = null;
        if (textoPrecio != "")
        {
            if (decimal.TryParse(textoPrecio, out decimal p) && p >= 0)
                precio = p;
            else
                Console.WriteLine("Precio inválido, se mantuvo el anterior.");
        }

        inventario.Modificar(id, nombre, categoria, precio);
        Console.WriteLine("Modificado.");
    }

    private void Eliminar()
    {
        int id = PedirEntero("ID a eliminar: ");
        Producto? producto = inventario.BuscarPorId(id);
        if (producto == null)
        {
            Console.WriteLine("No existe ese ID.");
            return;
        }

        Console.Write($"¿Eliminar '{producto.Nombre}'? (s/n): ");
        if ((Console.ReadLine() ?? "").Trim().ToLower() == "s")
        {
            inventario.Eliminar(id);
            Console.WriteLine("Eliminado.");
        }
    }

    public void Ejecutar()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("\nMENÚ");
            Console.WriteLine("1. Agregar\n2. Buscar\n3. Modificar\n4. Eliminar\n5. Listar\n6. Salir");
            Console.Write("Opción: ");
            string opcion = (Console.ReadLine() ?? "").Trim();

            try
            {
                switch (opcion)
                {
                    case "1": Agregar(); break;
                    case "2": Buscar(); break;
                    case "3": Modificar(); break;
                    case "4": Eliminar(); break;
                    case "5": MostrarLista(inventario.ObtenerTodos()); break;
                    case "6":
                        Console.Write("¿Seguro que desea salir? (s/n): ");
                        if ((Console.ReadLine() ?? "").Trim().ToLower() == "s")
                        {
                            Console.WriteLine("Hasta luego.");
                            salir = true;
                        }
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }
        }
    }
}