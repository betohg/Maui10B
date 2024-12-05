using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppIventario.Models;
using System.Text;
using System.Diagnostics;

namespace AppIventario
{
    public partial class SupplierPage : ContentPage
    {
        public SupplierPage()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private async void LoadSuppliers()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://127.0.0.1:8000/api/suppliers");
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(response);
            SuppliersListView.ItemsSource = suppliers;
        }

        private async void OnAddSupplierClicked(object sender, EventArgs e)
        {
            var supplierFormPage = new SupplierFormPage();
            supplierFormPage.Disappearing += async (s, args) =>
            {
                LoadSuppliers();
            };
            await Navigation.PushModalAsync(supplierFormPage);
        }

        private void OnSupplierTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedSupplier = e.Item as Supplier;
            DisplayAlert("Proveedor Seleccionado", $"Nombre: {selectedSupplier?.Name}\nDescripción: {selectedSupplier?.Description}", "OK");
            ((ListView)sender).SelectedItem = null;
        }


        private async void OnEditSupplierClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var supplier = button?.BindingContext as Supplier;

            if (supplier != null)
            {
                // Lógica para editar el proveedor
                string newName = await DisplayPromptAsync("Editar Proveedor", "Nuevo nombre:", initialValue: supplier.Name);
                string newDescription = await DisplayPromptAsync("Editar Proveedor", "Nueva descripción:", initialValue: supplier.Description);

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    supplier.Name = newName;
                    supplier.Description = newDescription;

                    try
                    {
                        var client = new HttpClient();
                        var json = JsonConvert.SerializeObject(supplier);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        Debug.WriteLine(supplier.Supplier_Id);
                        Debug.WriteLine($"URL: http://127.0.0.1:8000/api/suppliers/{supplier.Supplier_Id}");

                        var response = await client.PutAsync($"http://127.0.0.1:8000/api/suppliers/{supplier.Supplier_Id}", content);

                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Éxito", "Proveedor actualizado correctamente.", "OK");
                            LoadSuppliers();
                        }
                        else
                        {
                            await DisplayAlert("Error", $"No se pudo actualizar el proveedor: {response}", "OK");

                            var errorResponse = await response.Content.ReadAsStringAsync();
                            await DisplayAlert("Error", $"No se pudo actualizar el proveedor: {errorResponse}", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error al actualizar proveedor: {ex}");
                        await DisplayAlert("Error", $"No se pudo actualizar el proveedor: {ex.Message}", "OK");
                    }
                }
            }
        }

        private async void OnDeleteSupplierClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var supplier = button?.BindingContext as Supplier;

            if (supplier != null)
            {
                bool confirm = await DisplayAlert("Confirmar Eliminación", $"¿Eliminar proveedor '{supplier.Name}'?", "Sí", "No");

                if (confirm)
                {
                    try
                    {
                        var client = new HttpClient();
                      

                        var response = await client.DeleteAsync($"http://127.0.0.1:8000/api/suppliers/{supplier.Supplier_Id}");

                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Éxito", "Proveedor eliminado correctamente.", "OK");
                            LoadSuppliers();
                        }
                        else
                        {
                           

                            await DisplayAlert("Error", "No se pudo eliminar el proveedor.", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"No se pudo eliminar el proveedor: {ex.Message}", "OK");
                    }
                }
            }
        }


    }
}


