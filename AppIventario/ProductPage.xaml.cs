using Microsoft.Maui.Controls;
using System.Collections.Generic;
using AppIventario.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AppIventario
{
    public partial class ProductPage : ContentPage
    {
        public ProductPage()
        {
            InitializeComponent();
            LoadProducts();
        }


        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            var productFormPage = new ProductFormPage();
            productFormPage.Disappearing += async (s, args) =>
            {
                LoadProducts(); 
            };
            await Navigation.PushModalAsync(productFormPage); 
        }



        private async void LoadProducts()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://127.0.0.1:8000/api/products");
            var products = JsonConvert.DeserializeObject<List<Product>>(response);
            foreach (var product in products)
            {
                Debug.WriteLine($"URL de la imagen: {product.Image_Path}");  

                
                Debug.WriteLine($"URL de la imagen: {product.ImageUrl}");  
            }
            ProductsListView.ItemsSource = products;
        }

        private async void OnInventoryButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button?.CommandParameter as Product; // Asegúrate de tener un modelo de datos llamado Product

            if (product != null)
            {
                var inventoryFormPage = new InventoryFormPage(product);

                inventoryFormPage.MovementRegistered += (s, args) =>
                {
                    LoadProducts(); // Actualizar la lista de productos
                };
                await Navigation.PushAsync(inventoryFormPage);
            }
        }


        private async void OnProductsTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedProduct = e.Item as Product;
            if (selectedProduct != null)
            {
                await DisplayAlert("Producto Seleccionado",
                    $"Nombre: {selectedProduct.Name}\n" +
                    $"Precio: {selectedProduct.Price:C}\n" +
                    $"Cantidad: {selectedProduct.Quantity:C}\n" +
                    $"Descripción: {selectedProduct.Description}", "OK");
               

            }

            ((ListView)sender).SelectedItem = null;
        }
    }

   
}