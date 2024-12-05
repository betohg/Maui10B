using Newtonsoft.Json;
using System.Text;

using AppIventario.Models;
using Microsoft.Maui.Storage;

namespace AppIventario
{
    public partial class ProductFormPage : ContentPage
    {
        private string _selectedImagePath;

        public ProductFormPage()
        {
            InitializeComponent();
            LoadCategoriesAndSuppliers();
        }

        // Cargar las categorías desde la API
        private async void LoadCategoriesAndSuppliers()
        {
            // Cargar categorías
            var client = new HttpClient();
            var categoryResponse = await client.GetStringAsync("http://127.0.0.1:8000/api/categories");
            var categories = JsonConvert.DeserializeObject<List<Category>>(categoryResponse);

            // Asignar las categorías al Picker de categorías
            CategoryPicker.ItemsSource = categories;
            CategoryPicker.ItemDisplayBinding = new Binding("name");  // Asumiendo que la propiedad de categoría es "Name"

            // Cargar proveedores
            var supplierResponse = await client.GetStringAsync("http://127.0.0.1:8000/api/suppliers");
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(supplierResponse);

            // Asignar los proveedores al Picker de proveedores
            SupplierPicker.ItemsSource = suppliers;
            SupplierPicker.ItemDisplayBinding = new Binding("Name");  // Asumiendo que la propiedad de proveedor es "Name"
        }

        // Método para seleccionar la imagen
        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una imagen"
            });

            if (result != null)
            {
                _selectedImagePath = result.FullPath;
                ProductImage.Source = ImageSource.FromFile(_selectedImagePath); // Muestra la imagen seleccionada
            }
        }

        // Método para guardar el producto
        private async void OnSaveProductClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ProductNameEntry.Text) || string.IsNullOrEmpty(ProductPriceEntry.Text) || _selectedImagePath == null)
            {
                await DisplayAlert("Error", "Todos los campos son requeridos.", "OK");
                return;
            }

            // Obtener la categoría y proveedor seleccionados
            var selectedCategory = (CategoryPicker.SelectedItem as Category);
            var selectedSupplier = (SupplierPicker.SelectedItem as Supplier);

            if (selectedCategory == null || selectedSupplier == null)
            {
                await DisplayAlert("Error", "Debes seleccionar una categoría y un proveedor.", "OK");
                return;
            }

            var product = new Product
            {
                Name = ProductNameEntry.Text,
                Description = ProductDescriptionEditor.Text,
                Price = decimal.Parse(ProductPriceEntry.Text),
                Quantity = int.Parse(ProductQuantityEntry.Text),
                CategoryId = selectedCategory.id, // Usamos la categoría seleccionada
                SupplierId = selectedSupplier.Supplier_Id, // Usamos el proveedor seleccionado
                Image_Path = _selectedImagePath // Ruta de la imagen seleccionada
            };

            var client = new HttpClient();
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(product.Name), "name");
            formData.Add(new StringContent(product.Description), "description");
            formData.Add(new StringContent(product.Price.ToString()), "price");
            formData.Add(new StringContent(product.Quantity.ToString()), "quantity");

            formData.Add(new StringContent(product.CategoryId.ToString()), "category_id");
            formData.Add(new StringContent(product.SupplierId.ToString()), "supplier_id"); // Agregar el SupplierId

            if (_selectedImagePath != null)
            {
                var imageContent = new ByteArrayContent(System.IO.File.ReadAllBytes(_selectedImagePath));
                imageContent.Headers.Add("Content-Type", "image/jpeg"); // Tipo MIME adecuado
                formData.Add(imageContent, "image", System.IO.Path.GetFileName(_selectedImagePath));
            }

            var response = await client.PostAsync("http://127.0.0.1:8000/api/products", formData);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Producto guardado correctamente.", "OK");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Hubo un error al guardar el producto.", "OK");
            }
        }
    }
}