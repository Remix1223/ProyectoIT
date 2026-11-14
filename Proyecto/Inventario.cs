using System;
using System.Collections.Generic;
using System.Linq;

public class Inventario
{
    private List<Producto> productos = new List<Producto>();
    private int siguienteId = 1;

    public int Agregar(string nombre, string categoria, decimal precio)
    {
        Producto nuevo = new Producto(siguienteId, nombre, categoria, precio);
        productos.Add(nuevo);
        siguienteId++;
        return nuevo.Id;
    }

    public Producto? BuscarPorId(int id)
    {
        return productos.FirstOrDefault(p => p.Id == id);
    }

    public List<Producto> BuscarPorNombre(string texto)
    {
        return productos.Where(p => p.Nombre.ToLower().Contains(texto.ToLower())).ToList();
    }

    public List<Producto> BuscarPorCategoria(string categoria)
    {
        return productos.Where(p => p.Categoria.ToLower() == categoria.ToLower()).ToList();
    }

    public List<Producto> BuscarPorPrecio(decimal minimo, decimal maximo)
    {
        return productos.Where(p => p.Precio >= minimo && p.Precio <= maximo).ToList();
    }

    public bool Modificar(int id, string nombre, string categoria, decimal? precio)
    {
        Producto? producto = BuscarPorId(id);
        if (producto == null) return false;

        if (nombre != "") producto.Nombre = nombre;
        if (categoria != "") producto.Categoria = categoria;
        if (precio != null) producto.Precio = precio.Value;
        return true;
    }

    public bool Eliminar(int id)
    {
        Producto? producto = BuscarPorId(id);
        if (producto == null) return false;

        productos.Remove(producto);
        return true;
    }

    public List<Producto> ObtenerTodos()
    {
        return new List<Producto>(productos);
    }
}