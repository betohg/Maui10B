using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.ObjectModel;
using AppIventario.Models;
namespace AppIventario;

public partial class MovementPage : ContentPage
{
    public ObservableCollection<Movement> Movements { get; set; }

    public MovementPage()
    {
        InitializeComponent();
        Movements = new ObservableCollection<Movement>();
        BindingContext = this;

        LoadMovements();
    }

    private async void LoadMovements()
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://127.0.0.1:8000/api/movements");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var movements = JsonConvert.DeserializeObject<List<Movement>>(json);

                foreach (var movement in movements)
                {
                    if (movement.MovementType == null)
                    {
                        movement.MovementType = new TypeMovement { Name = "Desconocido" };  // Asignar un valor por defecto
                    }
                    Movements.Add(movement);
                }
            }
            else
            {
                // Mostrar más detalles del error
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"HTTP Error: {response.StatusCode}, Details: {errorContent}");
                await DisplayAlert("Error", $"No se pudieron cargar los movimientos.\nCódigo: {response.StatusCode}\nDetalles: {errorContent}", "OK");
            }
        }
        catch (Exception ex)
        {
            // Manejar errores inesperados
            System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}