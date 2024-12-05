using Newtonsoft.Json;
using System.Text;
using Microsoft.Maui.Controls;

namespace AppIventario;

public partial class TypeMovementFormPage : ContentPage
{
    public TypeMovementFormPage()
    {
        InitializeComponent();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text;
        var description = DescriptionEditor.Text;

        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "El nombre del movimiento es obligatorio.", "OK");
            return;
        }

        var newTypeMovement = new
        {
            name = name,
            description = description
        };

        var client = new HttpClient();
        var json = JsonConvert.SerializeObject(newTypeMovement);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://127.0.0.1:8000/api/typesmovements", content);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Éxito", "Tipo de movimiento agregado correctamente.", "OK");
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "Hubo un error al agregar el tipo de movimiento.", "OK");
        }
    }
}
