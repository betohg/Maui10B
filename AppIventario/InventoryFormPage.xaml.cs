using Newtonsoft.Json;
using System.Text;
using AppIventario.Models;
using Microsoft.Maui.Storage;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AppIventario
{
    public partial class InventoryFormPage : ContentPage
    {
        private Product _product;

        public event EventHandler MovementRegistered;


        public InventoryFormPage(Product product)
        {
            InitializeComponent();
            _product = product;

            ProductNameLabel.Text = product.Name;

            var movementTypes = new List<string> { "Entrada", "Salida" }; 
            MovementTypePicker.ItemsSource = movementTypes; //
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var movementType = MovementTypePicker.SelectedItem?.ToString();
            if (int.TryParse(QuantityEntry.Text, out int quantity) && !string.IsNullOrEmpty(movementType))
            {

                var userId = Preferences.Get("UserId", 0); 

                if (userId == 0)
                {
                    await DisplayAlert("Error", "No se pudo obtener el ID del usuario. Por favor, inicie sesión nuevamente.", "OK");
                    return;
                }


                var movement = new
                {
                    product_id = _product.Id,  
                    movement_type_id = movementType == "Salida" ? 1 : 2, 
                    quantity = quantity,
                    movement_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                    user_id = userId 

                };

                Debug.WriteLine($"Sending movement: {JsonConvert.SerializeObject(movement)}"); 

                var jsonContent = new StringContent(JsonConvert.SerializeObject(movement), Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    try
                    {
                        var apiUrl = "http://127.0.0.1:8000/api/movements";  

                        var response = await client.PostAsync(apiUrl, jsonContent);

                        if (response.IsSuccessStatusCode)
                        {

                            await DisplayAlert("Éxito", "Movimiento registrado correctamente", "OK");
                            MovementRegistered?.Invoke(this, EventArgs.Empty);

                            await Navigation.PopAsync(); 
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine(errorContent);
                            await DisplayAlert("Error", $"Hubo un problema al registrar el movimiento. Código de estado: {response.StatusCode}\nDetalles: {errorContent}", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Error de conexión: {ex.Message}", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
            }
        }
    }
}
