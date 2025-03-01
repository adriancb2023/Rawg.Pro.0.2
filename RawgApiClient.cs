using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

public static class RawgApiClient
{
    private static readonly string apiKey = "c685ed11971f488498064e1dc4da473f";
    private static readonly HttpClient client = new HttpClient();

    // 1. Obtener los mejores juegos de un año específico
    public static async Task<List<GameDetails>> ObtenerMejoresJuegosDelAñoAsync(string año)
    {
        string url = $"https://api.rawg.io/api/games?key={apiKey}&dates={año}-01-01,{año}-12-31&ordering=-rating";
        return await ObtenerJuegosAsync(url);
    }

    // 2. Buscar un juego por nombre
    public static async Task<List<GameDetails>> BuscarJuegoPorNombreAsync(string nombre)
    {
        string url = $"https://api.rawg.io/api/games?key={apiKey}&search={Uri.EscapeDataString(nombre)}";
        return await ObtenerJuegosAsync(url);
    }

    // 3. Filtrar juegos por año
    public static async Task<List<GameDetails>> ObtenerJuegosPorAñoAsync(string año)
    {
        string url = $"https://api.rawg.io/api/games?key={apiKey}&dates={año}-01-01,{año}-12-31&ordering=-released";
        return await ObtenerJuegosAsync(url);
    }

    // 4. Filtrar juegos por desarrolladora
    public static async Task<List<GameDetails>> ObtenerJuegosPorDesarrolladoraAsync(string desarrolladora)
    {
        string url = $"https://api.rawg.io/api/games?key={apiKey}&developers={Uri.EscapeDataString(desarrolladora)}";
        return await ObtenerJuegosAsync(url);
    }

    // 5. Obtener próximos lanzamientos
    public static async Task<List<GameDetails>> ObtenerProximosLanzamientosAsync()
    {
        string fechaActual = DateTime.UtcNow.ToString("yyyy-MM-dd");
        string url = $"https://api.rawg.io/api/games?key={apiKey}&dates={fechaActual},2025-12-31&ordering=released";
        return await ObtenerJuegosAsync(url);
    }

    // Método genérico para obtener y procesar los juegos
    private static async Task<List<GameDetails>> ObtenerJuegosAsync(string url)
    {
        HttpResponseMessage respuesta = await client.GetAsync(url);
        if (!respuesta.IsSuccessStatusCode)
        {
            string contenidoError = await respuesta.Content.ReadAsStringAsync();
            try
            {
                throw new Exception($"Error al obtener los datos de la API. Código de estado: {respuesta.StatusCode}, Contenido: {contenidoError}");
            }
            catch
            {
                MessageBox.Show("error");
            }
        }
        string respuestaJson = await respuesta.Content.ReadAsStringAsync();
        var respuestaJuego = JsonConvert.DeserializeObject<GameResponse>(respuestaJson);
        return respuestaJuego.Results;
    }

}

// Clases para deserializar la respuesta
public class GameResponse
{
    public List<GameDetails> Results { get; set; }
}

public class GameDetails
{
    public string Name { get; set; }
    public string Released { get; set; }
    public double Rating { get; set; }
    public string Background_Image { get; set; }
    public List<Screenshot> Short_Screenshots { get; set; }

    [JsonProperty("genres")]
    public List<Genre> Genres { get; set; }

    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("developers")]
    public List<Developer> Developers { get; set; }
}

public class Screenshot
{
    public string Image { get; set; }
}

public class Genre
{
    public string Name { get; set; }
}

public class Developer
{
    public string Name { get; set; }
}
