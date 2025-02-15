using SQLite;
using System.Diagnostics;

namespace ProyectoFinalJorgePiero;

public partial class ComprobarCliente : ContentPage
{
    private readonly SQLiteAsyncConnection database;

    public ComprobarCliente(SQLiteAsyncConnection database)
    {
        InitializeComponent();
        this.database = database;
        // Ruta de la base de datos SQLite


        // Crear la tabla Clientes si no existe
        database.CreateTableAsync<Cliente>().Wait();
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
        var user = await AuthenticateClienteoAsync(username, password);
        Debug.WriteLine("dato usuario: " + username);

        Cliente cliente = new Cliente();
        cliente.usuario = username;


        if (user != null)
        {
            Debug.WriteLine("Cliente autenticado correctamente.");
            // Redirige a la siguiente página si la autenticación fue exitosa
            Debug.WriteLine($"id autenticado: {user.id}");
            await Navigation.PushAsync(new ClienteMainPage(database,user.id));
        }
        else
        {
            // Muestra un error si las credenciales no coinciden
            ErrorLabel.Text = "Usuario o contraseña incorrectos";
            ErrorLabel.IsVisible = true;
        }

    }


    // Método para agregar un usuario de prueba
    private async void OnAddTestClienteAsync(object sender, EventArgs e)
    {
        Cliente cliente = new Cliente();
        cliente.usuario = UserEntry.Text;
        cliente.password = PasswordEntry.Text;
        cliente.cita = " ";
        cliente.receta = " ";



        await database.InsertAsync(cliente);
        Debug.WriteLine("Usuario agregado exitosamente.");
    }


    private async Task<Cliente> AuthenticateClienteoAsync(string username, string password)
    {
        try
        {
            // Buscar usuario en la base de datos SQLite
            var user = await database.Table<Cliente>()
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
    private async void OnDeleteAllClientesClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmar", "¿Está seguro de que desea eliminar todos los clientes?", "Sí", "No");
        if (confirm)
        {
            try
            {
                await database.DeleteAllAsync<Cliente>();
                Debug.WriteLine("Todos los clientes han sido eliminados.");
                await DisplayAlert("Éxito", "Todos los clientes han sido eliminados.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar clientes: {ex.Message}");
                await DisplayAlert("Error", "Hubo un error al eliminar los clientes.", "OK");
            }
        }
    }
    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}