using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using System.Diagnostics;


namespace AppIventario;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var username = usernameEntryField.Text;
        var password = passwordEntryField.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
            return;
        }

        var client = new HttpClient();
        var loginData = new { email = username, password = password };
        var json = JsonConvert.SerializeObject(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://127.0.0.1:8000/api/login", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        Debug.WriteLine("Respuesta: " + responseContent);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                if (jsonResponse.message == "Login exitoso")
                {
                    var userId = (int)jsonResponse.user.id; 
                    Preferences.Set("UserId", userId);

                    Application.Current.MainPage = new NavigationPage(new HomePage());
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("Error al parsear JSON: " + ex.Message);
                await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
            }
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Hubo un problema con la solicitud, intente nuevamente.", "OK");
        }
    }
}