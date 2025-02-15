
using SQLite;
using System.Diagnostics;

namespace ProyectoFinalJorgePiero
{
    public partial class MainPage : ContentPage
    {
        private readonly SQLiteAsyncConnection database;

        public MainPage()
        {
            try
            {
                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "clientes.db3");
                database = new SQLiteAsyncConnection(dbPath);
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing MainPage: {ex.Message}");
            }
        }

        private async void btnEntrarCliente_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ComprobarCliente(database));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to ComprobarCliente: {ex.Message}");
            }
        }

        private async void btnEntrarAdministrativo_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ComprobarAdministrativo(database));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to ComprobarAdministrativo: {ex.Message}");
            }
        }

        private async void btnEntrarMedico_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ComprobarMedico(database));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to ComprobarMedico: {ex.Message}");
            }
        }
       
    }

}
