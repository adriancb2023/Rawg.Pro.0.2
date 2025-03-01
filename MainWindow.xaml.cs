using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Rawg.Pro._0._2.Modelo;

namespace Proyecto_Final_PRO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<GameDetails> Games { get; set; }
        private bool isLoading = false;

        public MainWindow()
        {
            InitializeComponent();


            DataContext = this;
            gamesList.Items.Clear();
            Games = new ObservableCollection<GameDetails>();
            gamesList.ItemsSource = Games;
            gamesList2.ItemsSource = Games;

            if (VisualTreeHelper.GetChildrenCount(gamesList) > 0)
            {
                ((ScrollViewer)VisualTreeHelper.GetChild(gamesList, 0)).ScrollChanged += GamesList_ScrollChanged;
            }
        }

        // Funciones de interfaz de usuario
        private void ocultar_todo()

        {   // Ocultar todos los grid
            inicio.Visibility = Visibility.Hidden;
            proximos_juegos.Visibility = Visibility.Hidden;
            infoJuego.Visibility = Visibility.Hidden;
            distribuidoras.Visibility = Visibility.Hidden;
            infoJuego2.Visibility = Visibility.Hidden;



        }

        //Mostrar inicio
        private void mostrar_inicio(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            inicio.Visibility = Visibility.Visible;
        }

        //Mostrar proximos lanzamientos
        private async void mostrar_catalogo(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            await cargar_catalogo();
            proximos_juegos.Visibility = Visibility.Visible;
        }

        //Mostrar distribuidoras
        private async void mostrar_distribuidoras(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            await cargar_distribuidoras();
            distribuidoras.Visibility = Visibility.Visible;
        }

        //Mostrar cargar juegos api
        private async Task cargar_catalogo()
        {
            try
            {
                List<GameDetails> games = await RawgApiClient.ObtenerProximosLanzamientosAsync();
                Games.Clear();
                foreach (GameDetails game in games)
                {
                    Games.Add(game);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error al cargar el catálogo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Detectar cuando se llega al final de la lista y cargar más juegos
        private async void GamesList_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight && !isLoading)
            {
                isLoading = true;
                try
                {
                    List<GameDetails> moreGames = await RawgApiClient.ObtenerProximosLanzamientosAsync();
                    foreach (GameDetails game in moreGames)
                    {
                        Games.Add(game);
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Error al cargar más juegos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    isLoading = false;
                }
            }
        }

        //mostrar info del elemento seleccionado
        private void elemento_seleccionado(object sender, EventArgs e)
        {
            if (gamesList.SelectedItem is GameDetails selectedGame)
            {
                gameName.Text = selectedGame.Name;
                gameReleased.Text = selectedGame.Released;
                gameRating.Text = selectedGame.Rating.ToString();
                gameGenre.Text = string.Join(", ", selectedGame.Genres.Select(g => g.Name));
                gamePrice.Text = selectedGame.Price ?? "78.99€";
                gameDeveloper.Text = selectedGame.Developers != null ? string.Join(", ", selectedGame.Developers.Select(d => d.Name)) : "No conocido";
                gameDescription.Text = "Capturas del Juego";
                gameImage.Source = new BitmapImage(new Uri(selectedGame.Background_Image));
                gameScreenshots.ItemsSource = selectedGame.Short_Screenshots.Select(s => s.Image).ToList();

                infoJuego.Visibility = Visibility.Visible;
                proximos_juegos.Visibility = Visibility.Hidden;
            }
            proximos_juegos.Visibility = Visibility.Hidden;
            infoJuego.Visibility = Visibility.Visible;
        }

        //mostar captura en imagen
        private void abir_captura(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is string imageUrl)
            {
                imagengrande.Source = new BitmapImage(new Uri(imageUrl));
            }
        }

        // añadir juego a inventario
        private void add_juego_a_inventario(object sender, EventArgs e)
        {
            if (gamesList.SelectedItem is GameDetails selectedGame)
            {
                var juego = new Juego
                {
                    Nombre = selectedGame.Name,
                    Precio = selectedGame.Price,
                    Desarrollador = selectedGame.Developers != null ? string.Join(", ", selectedGame.Developers.Select(d => d.Name)) : "No conocido",
                    Genero = string.Join(", ", selectedGame.Genres.Select(g => g.Name)),
                    Puntuacion = selectedGame.Rating.ToString(),
                    Lanzamiento = selectedGame.Released,
                    Descripcion = "Capturas del Juego",
                    Capturas = string.Join(", ", selectedGame.Short_Screenshots.Select(s => s.Image)),
                    Portada = selectedGame.Background_Image
                };
                using (var context = new Rawg2Context())
                {
                    context.Juegos.Add(juego);
                    context.SaveChanges();
                }
                MessageBox.Show("Juego añadido al inventario", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        //cargar distribuidoras
        private async Task cargar_distribuidoras()
        {
            try
            {
                List<GameDetails> games = await RawgApiClient.ObtenerMejoresJuegosDelAñoAsync("2024");
                Games.Clear();
                foreach (GameDetails game in games)
                {
                    Games.Add(game);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error al cargar las distribuidoras: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Elemento seleccionado2
        private void elemento_seleccionado2(object sender, EventArgs e)
        {
            if (gamesList2.SelectedItem is GameDetails selectedGame)
            {
                gameName2.Text = selectedGame.Name;
                gameReleased2.Text = selectedGame.Released;
                gameRating2.Text = selectedGame.Rating.ToString();
                gameGenre2.Text = string.Join(", ", selectedGame.Genres.Select(g => g.Name));
                gamePrice2.Text = selectedGame.Price ?? "78.99€";
                gameDeveloper2.Text = selectedGame.Developers != null ? string.Join(", ", selectedGame.Developers.Select(d => d.Name)) : "No conocido";
                gameDescription2.Text = "Capturas del Juego";
                gameImage2.Source = new BitmapImage(new Uri(selectedGame.Background_Image));
                gameScreenshots2.ItemsSource = selectedGame.Short_Screenshots.Select(s => s.Image).ToList();
                infoJuego2.Visibility = Visibility.Visible;
                distribuidoras.Visibility = Visibility.Hidden;
            }
            distribuidoras.Visibility = Visibility.Hidden;
            infoJuego2.Visibility = Visibility.Visible;
        }

        private void add_juego_a_inventario2(object sender, EventArgs e)
        {
            if (gamesList2.SelectedItem is GameDetails selectedGame)
            {
                var juego = new Juego
                {
                    Nombre = selectedGame.Name,
                    Precio = selectedGame.Price,
                    Desarrollador = selectedGame.Developers != null ? string.Join(", ", selectedGame.Developers.Select(d => d.Name)) : "No conocido",
                    Genero = string.Join(", ", selectedGame.Genres.Select(g => g.Name)),
                    Puntuacion = selectedGame.Rating.ToString(),
                    Lanzamiento = selectedGame.Released,
                    Descripcion = "Capturas del Juego",
                    Capturas = string.Join(", ", selectedGame.Short_Screenshots.Select(s => s.Image)),
                    Portada = selectedGame.Background_Image
                };
                using (var context = new Rawg2Context())
                {
                    context.Juegos.Add(juego);
                    context.SaveChanges();
                }
                MessageBox.Show("Juego añadido al inventario", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // aplicar filtro
        private async void aplicar_filtro(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_titulo.Text))
            {
                // Buscar juegos por título
                try
                {
                    List<GameDetails> games = await RawgApiClient.BuscarJuegoPorNombreAsync(tb_titulo.Text);
                    Games.Clear();
                    foreach (GameDetails game in games)
                    {
                        Games.Add(game);
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Error al buscar juegos por título: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (!string.IsNullOrEmpty(tb_fecha.Text))
            {
                // Buscar juegos por fecha
                try
                {
                    List<GameDetails> games = await RawgApiClient.ObtenerJuegosPorAñoAsync(tb_fecha.Text);
                    Games.Clear();
                    foreach (GameDetails game in games)
                    {
                        Games.Add(game);
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Error al buscar juegos por fecha: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (!string.IsNullOrEmpty(tb_ano.Text))
            {
                // Buscar juegos por desarrolladora
                try
                {
                    List<GameDetails> games = await RawgApiClient.ObtenerMejoresJuegosDelAñoAsync(tb_ano.Text);
                    Games.Clear();
                    foreach (GameDetails game in games)
                    {
                        Games.Add(game);
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Error al buscar juegos por desarrolladora: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, rellene un campo para realizar la búsqueda", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //abrir imagen 2
        private void abir_captura2(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is string imageUrl)
            {
                imagengrande2.Source = new BitmapImage(new Uri(imageUrl));
            }
        }

        /*
        // Guardar juego en inventario
        private void guardar_inventario(object sender, EventArgs e)
        {
            // quiero que coja el nombre del juego seleccionado y haga una consulta para sacar su id y guardarlo en la tabla de inventario con el resto de datos que el usuario introduzca
            using (Rawg2Context conexion = new Rawg2Context()) {
                // consulta para sacar la id del juego seleccionado
                var juego = conexion.Juegos.Where(j => j.Nombre == gameName2.Text).FirstOrDefault();

                if (juego != null)
                {
                    // guardar en la tabla de inventario
                    Inventario inventario = new Inventario
                    {
                        IdJuego = juego.Id,
                        Precio = Convert.ToDecimal(tb_precio.Text),
                        Estado = tb_estado.Text,
                        Plataforma = tb_plataforma.Text,
                        Comentario = tb_comentario.Text
                    };
                    conexion.Inventarios.Add(inventario);
                    conexion.SaveChanges();
                    MessageBox.Show("Juego guardado en el inventario", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se ha encontrado el juego seleccionado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }
        }*/

        // funciona git
    }
}
