using Newtonsoft.Json;
using System.Text;
using Microsoft.Maui.Controls;
namespace AppIventario;
using AppIventario.Models;

public partial class CategoryFormPage : ContentPage
{
    public CategoryFormPage()
    {
        InitializeComponent();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text;
        var description = DescriptionEditor.Text;

        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "El nombre de la categoría es obligatorio.", "OK");
            return;
        }

        var newCategory = new
        {
            name = name,
            description = description
        };

        var client = new HttpClient();
        var json = JsonConvert.SerializeObject(newCategory);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://127.0.0.1:8000/api/categories", content);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Éxito", "Categoría agregada correctamente.", "OK");
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "Hubo un error al agregar la categoría.", "OK");
        }
    }
}