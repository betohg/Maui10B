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
        // Mostrar cuadro de confirmaci�n
        bool confirmLogout = await DisplayAlert("Confirmaci�n", "�Est�s seguro de que deseas cerrar sesi�n?", "S�", "No");

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
                    await DisplayAlert("�xito", "Has cerrado sesi�n correctamente.", "OK");
                    Preferences.Clear(); 

                    Application.Current.MainPage = new LoginPage(); 
                }
                else
                {
                    await DisplayAlert("Error", $"No se pudo cerrar sesi�n: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepci�n al cerrar sesi�n: {ex.Message}");
                await DisplayAlert("Error", "Ocurri� un problema al cerrar sesi�n. Int�ntalo m�s tarde.", "OK");
            }
        }
    
}



}