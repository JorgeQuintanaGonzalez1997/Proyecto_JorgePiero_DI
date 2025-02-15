using SQLite;
using System.Diagnostics;

namespace ProyectoFinalJorgePiero;

public partial class MedicoMainPage : ContentPage
{
    SQLiteAsyncConnection database;
    int id;
    bool conCliente = false;
    public MedicoMainPage(SQLiteAsyncConnection database, int id)
    {
        conCliente = false;
        InitializeComponent();
        this.database = database;
        CargarClientes();
        this.id = id;
    }
    private async void CargarClientes()
    {
        try
        {
            var clientes = await ObtenerClientesAsync();
            ClientesListView.ItemsSource = clientes;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al cargar clientes: {ex.Message}");
        }
    }

    private async Task<List<Cliente>> ObtenerClientesAsync()
    {
        try
        {
            return await database.Table<Cliente>().ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al acceder a SQLite: {ex.Message}");
            return new List<Cliente>();
        }
    }
    private async void OnEnviarRecetaClicked(object sender, EventArgs e)
    {
        var id = ClienteIdEntry.Text;
        var nuevaCita = CitaEntry.Text;

        if (string.IsNullOrEmpty(id))
        {
            ErrorLabel.Text = "Por favor ingrese el id de un cliente";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (string.IsNullOrEmpty(nuevaCita))
        {
            ErrorLabel.Text = "Por favor ingrese una nueva receta";
            ErrorLabel.IsVisible = true;
            return;
        }

        var cliente = await BuscarClienteAsync(id);
        if (cliente != null)
        {
            cliente.receta = nuevaCita;
            await SaveUsuarioAsync(cliente);
            ResultadoLabel.Text = "Receta actualizada correctamente";
            Debug.WriteLine("Receta actualizada.");
            CargarClientes(); // Recargar la lista de clientes
        }
        else
        {
            ErrorLabel.Text = "Cliente no encontrado";
            ErrorLabel.IsVisible = true;
        }
    }
    private async Task<int> SaveUsuarioAsync(Cliente cliente)
    {
        try
        {
            if (cliente.id != 0)
            {
                return await database.UpdateAsync(cliente);
            }
            else
            {
                return await database.InsertAsync(cliente);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al guardar en SQLite: {ex.Message}");
            return 0;
        }
    }


    private async void OnBuscarYActualizarClicked(object sender, EventArgs e)
    {
        var id = ClienteIdEntry.Text;


        if (string.IsNullOrEmpty(id))
        {
            ErrorLabel.Text = "Por favor ingrese el id de un cliente";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Autenticar usuario en la base de datos SQLite
        var user = await BuscarClienteAsync(id);

        Debug.WriteLine("dato usuario: " + id);

        Cliente cliente = new Cliente();
        cliente.usuario = id;


        if (user != null)
        {
            ResultadoLabel.Text = "El nombre del cliente seleccionado es: " + user.usuario+" con "+user.id;
            conCliente = true;
            Debug.WriteLine("Cliente encontrado.");
            // Redirige a la siguiente página si la autenticación fue exitosa

        }
        else
        {
            // Muestra un error si las credenciales no coinciden
            ErrorLabel.Text = "Cliente no encontrado";
            ErrorLabel.IsVisible = true;
        }

    }
    private async Task<Cliente> BuscarClienteAsync(string id)
    {
        try
        {
            int num = int.Parse(id);
            // Buscar usuario en la base de datos SQLite
            var user = await database.Table<Cliente>()
                .Where(u => u.id == num)
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