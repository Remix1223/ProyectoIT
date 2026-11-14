public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Categoria { get; set; }
    public decimal Precio { get; set; }

    public Producto(int id, string nombre, string categoria, decimal precio)
    {
        Id = id;
        Nombre = nombre;
        Categoria = categoria;
        Precio = precio;
    }

    public override string ToString()
    {
        return $"[{Id}] {Nombre} | {Categoria} | ${Precio:F2}";
    }
}