using SQLite;
using System.Diagnostics;

namespace ProyectoFinalJorgePiero;

public partial class ComprobarMedico : ContentPage
{
    private readonly SQLiteAsyncConnection database;

    public ComprobarMedico(SQLiteAsyncConnection database)
    {
        InitializeComponent();
        this.database = database;
        // Ruta de la base de datos SQLite


        // Crear la tabla Clientes si no existe
        database.CreateTableAsync<Medico>().Wait();
    }

    private async void OnBtnLoggin(object sender, EventArgs e)
    {
        var username = UserEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorLabel.Text = "Por favor ingrese usuario y contrase�a";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Autenticar usuario en la base de datos SQLite
        var user = await AuthenticateMedicoAsync(username, password);
        Debug.WriteLine("dato usuario: " + username);

        Medico medico = new Medico();
        medico.usuario = username;


        if (user != null)
        {
            Debug.WriteLine("Medico autenticado correctamente.");
            // Redirige a la siguiente p�gina si la autenticaci�n fue exitosa
            await Navigation.PushAsync(new MedicoMainPage(database, user.id));
        }
        else
        {
            // Muestra un error si las credenciales no coinciden
            ErrorLabel.Text = "Usuario o contrase�a incorrectos";
            ErrorLabel.IsVisible = true;
        }

    }
    private async void OnDeleteAllMedicosClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmar", "�Est� seguro de que desea eliminar todos los m�dicos?", "S�", "No");
        if (confirm)
        {
            try
            {
                await database.DeleteAllAsync<Administrativo>();
                Debug.WriteLine("Todos los m�dicos han sido eliminados.");
                await DisplayAlert("�xito", "Todos los m�dicos han sido eliminados.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar m�dicos: {ex.Message}");
                await DisplayAlert("Error", "Hubo un error al eliminar los m�dicos.", "OK");
            }
        }
    }


    // M�todo para agregar un usuario de prueba
    private async void OnAddTestAdministrativoAsync(object sender, EventArgs e)
    {
        Medico medico = new Medico();
        medico.usuario = UserEntry.Text;
        medico.password = PasswordEntry.Text;



        await database.InsertAsync(medico);
        Debug.WriteLine("Usuario agregado exitosamente.");
    }


    private async Task<Medico> AuthenticateMedicoAsync(string username, string password)
    {
        try
        {
            // Buscar usuario en la base de datos SQLite
            var user = await database.Table<Medico>()
                .Where(u => u.usuario == username && u.password == password)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                Debug.WriteLine($"Usuario autenticado: {user.usuario}");

                return user;

            }

            Debug.WriteLine("Usuario o contrase�a incorrectos.");
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