using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using AppIventario.Models;
using System.Diagnostics;

namespace AppIventario
{
    public partial class CategoriaPage : ContentPage
    {
        public CategoriaPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetStringAsync("http://127.0.0.1:8000/api/categories");
                var categories = JsonConvert.DeserializeObject<List<Category>>(response);
                CategoriesListView.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar las categor�as: {ex.Message}", "OK");
            }
        }

        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            string newName = await DisplayPromptAsync("Nueva Categor�a", "Nombre:");
            string newDescription = await DisplayPromptAsync("Nueva Categor�a", "Descripci�n:");

            if (!string.IsNullOrWhiteSpace(newName))
            {
                var newCategory = new Category
                {
                    name = newName,
                    description = newDescription
                };

                try
                {
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(newCategory);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://127.0.0.1:8000/api/categories", content);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("�xito", "Categor�a agregada correctamente.", "OK");
                        LoadCategories();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo agregar la categor�a.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo agregar la categor�a: {ex.Message}", "OK");
                }
            }
        }


        private void OnCategoryTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedCategory = e.Item as Category;
            DisplayAlert("Categoria seleccionada", selectedCategory?.name, "OK");
        }
    

    private async void OnEditClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var category = button?.BindingContext as Category;

            if (category != null)
            {
                string newName = await DisplayPromptAsync("Editar Categor�a", "Nuevo nombre:", initialValue: category.name);
                string newDescription = await DisplayPromptAsync("Editar Categor�a", "Nueva descripci�n:", initialValue: category.description);

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    category.name = newName;
                    category.description = newDescription;

                    try
                    {
                        var client = new HttpClient();
                        var json = JsonConvert.SerializeObject(category);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PutAsync($"http://127.0.0.1:8000/api/categories/{category.id}", content);

                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("�xito", "Categor�a actualizada correctamente.", "OK");
                            LoadCategories();
                        }
                        else
                        {
                            await DisplayAlert("Error", "No se pudo actualizar la categor�a.", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"No se pudo actualizar la categor�a: {ex.Message}", "OK");
                    }
                }
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var category = button?.BindingContext as Category;

            if (category != null)
            {
                bool confirm = await DisplayAlert("Confirmar Eliminaci�n", $"�Eliminar categor�a '{category.name}'?", "S�", "No");

                if (confirm)
                {
                    Debug.WriteLine(category.id);
                    try
                    {
                        var client = new HttpClient();
                        var response = await client.DeleteAsync($"http://127.0.0.1:8000/api/categories/{category.id}");
                        Debug.WriteLine(category.id);

                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("�xito", "Categor�a eliminada correctamente.", "OK");
                            LoadCategories();
                        }
                        else
                        {
                            await DisplayAlert("Error", "No se pudo eliminar la categor�a.", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"No se pudo eliminar la categor�a: {ex.Message}", "OK");
                    }
                }
            }
        }
    }
}
