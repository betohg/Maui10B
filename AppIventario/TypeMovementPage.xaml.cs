using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppIventario.Models;

namespace AppIventario
{
    public partial class TypeMovementPage : ContentPage
    {
        public TypeMovementPage()
        {
            InitializeComponent();
            LoadTypeMovements();
        }

        private async void LoadTypeMovements()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetStringAsync("http://127.0.0.1:8000/api/typesmovements");
                var typeMovements = JsonConvert.DeserializeObject<List<TypeMovement>>(response);
                TypeMovementsListView.ItemsSource = typeMovements;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar los tipos de movimiento: {ex.Message}", "OK");
            }
        }

        private async void OnAddTypeMovementClicked(object sender, EventArgs e)
        {
            var typeMovementFormPage = new TypeMovementFormPage();
            typeMovementFormPage.Disappearing += async (s, args) =>
            {
                LoadTypeMovements();
            };
            await Navigation.PushModalAsync(typeMovementFormPage);
        }

        private void OnTypeMovementTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedTypeMovement = e.Item as TypeMovement;
            DisplayAlert(
                "Tipo de Movimiento Seleccionado",
                $"Nombre: {selectedTypeMovement?.Name}\nDescripción: {selectedTypeMovement?.Description}",
                "OK"
            );
            ((ListView)sender).SelectedItem = null;
        }
    }
}
