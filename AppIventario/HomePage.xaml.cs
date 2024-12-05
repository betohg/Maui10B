namespace AppIventario;
using Microsoft.Maui.Controls;

public partial class HomePage : ContentPage
{

    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnButton1Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CategoriaPage());



    }

    private async void OnButton2Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SupplierPage());
    }

    private async void OnButton3Clicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new ProductPage());

        //Application.Current.MainPage = new NavigationPage(new ProductPage());
    }

    private async void OnButton4Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TypeMovementPage());
    }


    private async void OnButton5Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MovementPage());
    }


    private async void OnButton6Clicked(object sender, EventArgs e)
    {
        // Mostrar cuadro de confirmación
        bool confirmLogout = await DisplayAlert("Confirmación", "¿Estás seguro de que deseas cerrar sesión?", "Sí", "No");

        if (confirmLogout)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:8000"); 
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

               
                var response = await client.PostAsync("/api/logout", null);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Has cerrado sesión correctamente.", "OK");
                    Preferences.Clear(); 

                    Application.Current.MainPage = new LoginPage(); 
                }
                else
                {
                    await DisplayAlert("Error", $"No se pudo cerrar sesión: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al cerrar sesión: {ex.Message}");
                await DisplayAlert("Error", "Ocurrió un problema al cerrar sesión. Inténtalo más tarde.", "OK");
            }
        }
    
}



}