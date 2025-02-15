using SQLite;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoFinalJorgePiero;

public partial class ComprobarAdministrativo : ContentPage
{

    private readonly SQLiteAsyncConnection database;
   
    public ComprobarAdministrativo(SQLiteAsyncConnection database)
    {
        InitializeComponent();
        this.database = database;
        // Ruta de la base de datos SQLite
        

        // Crear la tabla Clientes si no existe
        database.CreateTableAsync<Administrativo>().Wait();
    }

    private async void OnBtnLoggin(object sender, EventArgs e)
    {
        var username = UserEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorLabel.Text = "Por favor ingrese usuario y contraseña";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Autenticar usuario en la base de datos SQLite
        var user = await AuthenticateAdministrativoAsync(username, password);
        Debug.WriteLine("dato usuario: " + username);

        Administrativo administrativo = new Administrativo();
        administrativo.usuario = username;


        if (user != null)
        {
            Debug.WriteLine("Administrativo autenticado correctamente.");
            // Redirige a la siguiente página si la autenticación fue exitosa
            await Navigation.PushAsync(new AdministrativoMainPage(database,user.id));
        }
        else
        {
            // Muestra un error si las credenciales no coinciden
            ErrorLabel.Text = "Usuario o contraseña incorrectos";
            ErrorLabel.IsVisible = true;
        }

    }
    private async void OnDeleteAllAdministrativosClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmar", "¿Está seguro de que desea eliminar todos los administrativos?", "Sí", "No");
        if (confirm)
        {
            try
            {
                await database.DeleteAllAsync<Administrativo>();
                Debug.WriteLine("Todos los administrativos han sido eliminados.");
                await DisplayAlert("Éxito", "Todos los administrativos han sido eliminados.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar administrativos: {ex.Message}");
                await DisplayAlert("Error", "Hubo un error al eliminar los administrativos.", "OK");
            }
        }
    }


    // Método para agregar un usuario de prueba
    private async void OnAddTestAdministrativoAsync(object sender, EventArgs e)
    {
        Administrativo administrativo = new Administrativo();
        administrativo.usuario = UserEntry.Text;
        administrativo.password = PasswordEntry.Text;



        await database.InsertAsync(administrativo);
        Debug.WriteLine("Usuario agregado exitosamente.");
    }


    private async Task<Administrativo> AuthenticateAdministrativoAsync(string username, string password)
    {
        try
        {
            // Buscar usuario en la base de datos SQLite
            var user = await database.Table<Administrativo>()
                .Where(u => u.usuario == username && u.password == password)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                Debug.WriteLine($"Usuario autenticado: {user.usuario}");

                return user;

            }

            Debug.WriteLine("Usuario o contraseña incorrectos.");
            return null;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al acceder a SQLite: {ex.Message}");
            return null;

        }

    }
    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }



}