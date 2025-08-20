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
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

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
            desabilitar_botones();
            login.Visibility = Visibility.Visible;

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

        // Eventos para la barra de título personalizada
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //funcion logueo
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string nombreUsuario = UsernameTextBox.Text;
            string contrasena = PasswordBox.Password;

            using (var context = new Rawg2Context())
            {
                var empleado = context.Empleados
                    .FirstOrDefault(e => e.Nombreusuario == nombreUsuario);

                if (empleado != null)
                {
                    if (empleado.Contraseña == contrasena)
                    {
                        MessageBox.Show("Inicio de sesión exitoso", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        login.Visibility = Visibility.Collapsed;
                        habilitar_botones();
                        EmpleadoNombre.Text = empleado.Nombre + " " + empleado.Apellidos;
                        inicio.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show("Contraseña incorrecta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Nombre de usuario no encontrado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // funcion logout
        private void logout(object sender, RoutedEventArgs e)
        {
            login.Visibility = Visibility.Visible;
            desabilitar_botones();
            EmpleadoNombre.Text = "";
            ocultar_todo();
        }

        // Funciones de interfaz de usuario
        private void ocultar_todo()

        {   // Ocultar todos los grid
            inicio.Visibility = Visibility.Hidden;
            proximos_juegos.Visibility = Visibility.Hidden;
            infoJuego.Visibility = Visibility.Hidden;
            distribuidoras.Visibility = Visibility.Hidden;
            infoJuego2.Visibility = Visibility.Hidden;
            inventario.Visibility = Visibility.Hidden;
            form_inventario.Visibility = Visibility.Hidden;
            blur1.Visibility = Visibility.Hidden;
            blur2.Visibility = Visibility.Hidden;
            mi_catalogo.Visibility = Visibility.Hidden;
            ventas.Visibility = Visibility.Hidden;

        }

        //Funcion desabilitar botones al no estar logueado
        private void desabilitar_botones()
        {
            //desabilitar botones
            bt_inicio.IsEnabled = false;
            bt_proximos_lanzamientos.IsEnabled = false;
            bt_distribuidoras.IsEnabled = false;
            bt_inventario.IsEnabled = false;
            bt_catalogo.IsEnabled = false;
            bt_ventas.IsEnabled = false;
        }

        //habilitar botones
        private void habilitar_botones()
        {
            bt_inicio.IsEnabled = true;
            bt_proximos_lanzamientos.IsEnabled = true;
            bt_distribuidoras.IsEnabled = true;
            bt_inventario.IsEnabled = true;
            bt_catalogo.IsEnabled = true;
            bt_ventas.IsEnabled = true;
        }

        //Mostrar inicio
        private void mostrar_inicio(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            resetButtonStyles();
            bt_inicio.Style = (Style)FindResource("ActiveSidebarButton");
            inicio.Visibility = Visibility.Visible;
        }

        //Mostrar proximos lanzamientos
        private async void mostrar_catalogo(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            resetButtonStyles();
            bt_proximos_lanzamientos.Style = (Style)FindResource("ActiveSidebarButton");
            await cargar_catalogo();
            proximos_juegos.Visibility = Visibility.Visible;
        }

        //Mostrar distribuidoras
        private async void mostrar_distribuidoras(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            resetButtonStyles();
            bt_distribuidoras.Style = (Style)FindResource("ActiveSidebarButton");
            await cargar_distribuidoras();
            distribuidoras.Visibility = Visibility.Visible;
        }

        // Resetear estilos de botones del sidebar
        private void resetButtonStyles()
        {
            bt_inicio.Style = (Style)FindResource("SidebarButton");
            bt_proximos_lanzamientos.Style = (Style)FindResource("SidebarButton");
            bt_distribuidoras.Style = (Style)FindResource("SidebarButton");
            bt_inventario.Style = (Style)FindResource("SidebarButton");
            bt_catalogo.Style = (Style)FindResource("SidebarButton");
            bt_ventas.Style = (Style)FindResource("SidebarButton");
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

        //añadir al inventario real
        private void add_juego_a_inventario2(object sender, EventArgs e)
        {
            blur1.Visibility = Visibility.Visible;
            blur2.Visibility = Visibility.Visible;
            form_inventario.Visibility = Visibility.Visible;

            if (gamesList2.SelectedItem is GameDetails selectedGame)
            {
                var juego2 = new Juego
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
                    // Comprobar si el juego ya existe en la base de datos
                    var juegoExistente = context.Juegos.FirstOrDefault(j => j.Nombre == selectedGame.Name);

                    if (juegoExistente != null)
                    {
                        MessageBox.Show("El juego ya existe en el inventario", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        context.Juegos.Add(juego2);
                        context.SaveChanges();
                        MessageBox.Show("Juego añadido al inventario", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
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

        // Guardar juego en inventario
        private void guardar_inventario(object sender, EventArgs e)
        {
            using (Rawg2Context conexion = new Rawg2Context())
            {
                // consulta para sacar la id del juego seleccionado
                var juego = conexion.Juegos.Where(j => j.Nombre == gameName2.Text).FirstOrDefault();

                if (juego != null)
                {
                    if (string.IsNullOrEmpty(stock.Text))
                    {
                        MessageBox.Show("Por favor, indique el numero de copias que desea", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(precioVenta.Text))
                    {
                        precioVenta.Text = "79.99";
                    }
                    // guardar en la tabla de inventario
                    Inventario inventario = new Inventario
                    {
                        IdJuego = juego.Id,
                        PrecioVenta = Convert.ToDouble(precioVenta.Text),
                        Stock = Convert.ToInt32(stock.Text)
                    };
                    conexion.Inventarios.Add(inventario);
                    conexion.SaveChanges();
                    MessageBox.Show("Juego guardado en el inventario", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se ha encontrado el juego seleccionado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                form_inventario.Visibility = Visibility.Hidden;
                blur1.Visibility = Visibility.Hidden;
                blur2.Visibility = Visibility.Hidden;
            }
        }

        // Cancelar guardar juego en inventario
        private void cancelar_guardar_inventario(object sender, EventArgs e)
        {
            infoJuego2.Visibility = Visibility.Hidden;
            distribuidoras.Visibility = Visibility.Visible;
            blur2.Visibility = Visibility.Hidden;
            blur1.Visibility = Visibility.Hidden;
        }

        //Inventario
        private async void mostrar_inventario(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            resetButtonStyles();
            bt_inventario.Style = (Style)FindResource("ActiveSidebarButton");
            await cargar_inventario();
            inventario.Visibility = Visibility.Visible;
        }

        //cargar inventario
        private async Task cargar_inventario()
        {
            try
            {
                using (var context = new Rawg2Context())
                {
                    var juegos = await context.Juegos
                        .Select(j => new
                        {
                            j.Id,
                            j.Nombre,
                            j.Precio,
                            j.Desarrollador,
                            j.Genero,
                            j.Puntuacion,
                            j.Lanzamiento,
                            j.Descripcion,
                            j.Capturas,
                            j.Portada,
                            Stock = context.Inventarios.Where(i => i.IdJuego == j.Id).Sum(i => i.Stock)
                        })
                        .Where(j => j.Stock > 0) // Filtrar juegos con stock mayor que cero
                        .ToListAsync();

                    inventoryList.ItemsSource = juegos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el inventario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //mostrar mi catalogo
        private void mostrar_micatalogo(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            resetButtonStyles();
            bt_catalogo.Style = (Style)FindResource("ActiveSidebarButton");
            mi_catalogo.Visibility = Visibility.Visible;
            cargar_micatalogo(sender, e);
        }

        //cargar mi catalogo
        private async void cargar_micatalogo(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new Rawg2Context())
                {
                    var juegos = await context.Juegos
                .OrderBy(j => j.Nombre)
                .Select(j => new
                {
                    j.Id,
                    j.Nombre,
                    j.Precio,
                    j.Desarrollador,
                    j.Genero,
                    j.Puntuacion,
                    j.Lanzamiento,
                    j.Descripcion,
                    j.Capturas,
                    j.Portada,
                    Stock = context.Inventarios.Where(i => i.IdJuego == j.Id).Sum(i => i.Stock)
                })
                .ToListAsync(); micatalogoList.ItemsSource = juegos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el catálogo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // fin de ayer

        //mostrar ventas
        private async void mostrar_ventas(object sender, RoutedEventArgs e)
        {
            ocultar_todo();
            resetButtonStyles();
            bt_ventas.Style = (Style)FindResource("ActiveSidebarButton");
            await cargar_juegos_en_inventario();
            ventas.Visibility = Visibility.Visible;
        }

        private async Task cargar_juegos_en_inventario()
        {
            try
            {
                using (var context = new Rawg2Context())
                {
                    var juegoIds = await context.Inventarios
                        .Select(i => i.IdJuego)
                        .Distinct()
                        .ToListAsync();
                    var juegos = await context.Juegos
                        .Where(j => juegoIds.Contains(j.Id))
                        .Select(j => new { j.Id, j.Nombre })
                        .ToListAsync();
                    cb_juegos_ventas.ItemsSource = juegos;
                    cb_juegos_ventas.DisplayMemberPath = "Nombre";
                    cb_juegos_ventas.SelectedValuePath = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los juegos en el inventario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // juego elegido combobox ventas
        private async void juego_elegido(object sender, SelectionChangedEventArgs e)
        {
            if (cb_juegos_ventas.SelectedItem is { } selectedJuego)
            {
                int juegoId = (int)cb_juegos_ventas.SelectedValue;
                using (var context = new Rawg2Context())
                {
                    var juego = await context.Juegos
                        .Where(j => j.Id == juegoId)
                        .Select(j => new
                        {
                            j.Nombre,
                            j.Precio,
                            j.Desarrollador,
                            j.Genero,
                            j.Puntuacion,
                            j.Lanzamiento,
                            j.Descripcion,
                            j.Capturas,
                            j.Portada,
                            Stock = context.Inventarios.Where(i => i.IdJuego == j.Id).Sum(i => i.Stock)
                        })
                        .FirstOrDefaultAsync();

                    if (juego != null)
                    {
                        tb_generoJuego.Text = juego.Genero;
                        tb_precioJuego.Text = juego.Precio.ToString();
                        tb_stockJuego.Text = juego.Stock.ToString();
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron datos para el juego seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún juego.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //funcion calcular total
        private void calcular_importe_total(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tb_cantidad.Text, out int cantidad) && cantidad > 0)
            {
                if (double.TryParse(tb_precioJuego.Text, out double precio))
                {
                    double total = precio * cantidad;
                    tb_total.Text = $"${total:F2}";
                }
                else
                {
                    MessageBox.Show("El precio del juego no es válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese una cantidad válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //funcion realizar la venta
        private void vender(object sender, RoutedEventArgs e)
        {
            



        }

        // falla la funcion calcular importe total y vender hay que pensar como hacerlo

    }
}
