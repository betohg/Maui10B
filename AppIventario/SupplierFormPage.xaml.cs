using Newtonsoft.Json;
using System.Text;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System;

namespace AppIventario
{
    public partial class SupplierFormPage : ContentPage
    {
        public SupplierFormPage()
        {
            InitializeComponent();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var description = DescriptionEditor.Text;

            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Error", "El nombre del proveedor es obligatorio.", "OK");
                return;
            }

            var newSupplier = new
            {
                name = name,
                description = description
            };

            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(newSupplier);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://127.0.0.1:8000/api/suppliers", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Proveedor agregado correctamente.", "OK");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Hubo un error al agregar el proveedor.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
