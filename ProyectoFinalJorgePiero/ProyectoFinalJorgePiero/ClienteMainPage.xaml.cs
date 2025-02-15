using SQLite;
using System.Diagnostics;

namespace ProyectoFinalJorgePiero;

public partial class ClienteMainPage : ContentPage
{
    SQLiteAsyncConnection database;
    int id;

    bool conCliente = false;
    public ClienteMainPage(SQLiteAsyncConnection database,int id)
    {
        this.id = id;
        InitializeComponent();
        this.database = database;
        CargarCliente();


    }
    private async void CargarCliente()
    {
        try
        {
            Debug.WriteLine("Id antes de buscar: "+id);
            var cliente = await BuscarClienteAsync(id);
            if (cliente != null)
            {
                ClienteInfoLabel.Text = $"ID: {cliente.id}\nUsuario: {cliente.usuario}\nCita: {cliente.cita}\nReceta: {cliente.receta}";
            }
            else
            {
                ClienteInfoLabel.Text = "Cliente no encontrado";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al cargar cliente: {ex.Message}");
            ClienteInfoLabel.Text = "Error al cargar cliente";
        }
    }

    private async Task<Cliente> BuscarClienteAsync(int id)
    {
        try
        {
            return await database.Table<Cliente>()
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();
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