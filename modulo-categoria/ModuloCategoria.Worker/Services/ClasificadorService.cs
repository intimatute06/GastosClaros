namespace ModuloCategoria.Worker.Services
{
    public class ClasificadorService : IClasificadorService
    {
        private static readonly Dictionary<string, string[]> Categorias = new()
        {
            ["Comida"] = new[] { "cena", "almuerzo", "restaurante", "comida", "pizza", "cafe" },
            ["Transporte"] = new[] { "uber", "taxi", "bus", "gasolina", "transporte" },
            ["Alojamiento"] = new[] { "hotel", "hostal", "airbnb", "alojamiento" },
            ["Entretenimiento"] = new[] { "cine", "entrada", "boleto", "concierto", "tour" }
        };

        public string Clasificar(string descripcion)
        {
            var texto = descripcion.ToLowerInvariant();

            foreach (var (categoria, palabrasClave) in Categorias)
            {
                if (palabrasClave.Any(palabra => texto.Contains(palabra)))
                {
                    return categoria;
                }
            }

            return "Otros";
        }
    }
}
