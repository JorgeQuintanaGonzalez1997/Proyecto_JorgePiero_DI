using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalJorgePiero
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            try
            {
                _database = new SQLiteAsyncConnection(dbPath);
                CrearTablasAsync().Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        private async Task CrearTablasAsync()
        {
            try
            {
                await _database.CreateTableAsync<Cliente>();
                await _database.CreateTableAsync<Medico>();
                await _database.CreateTableAsync<Administrativo>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating tables: {ex.Message}");
            }
        }

        public Task<Cliente> GetClienteAsync(string nombre, string password)
        {
            return _database.Table<Cliente>()
                            .Where(c => c.usuario == nombre && c.password == password)
                            .FirstOrDefaultAsync();
        }

        public Task<Medico> GetMedicoAsync(string nombre, string password)
        {
            return _database.Table<Medico>()
                            .Where(m => m.usuario == nombre && m.password == password)
                            .FirstOrDefaultAsync();
        }

        public Task<Administrativo> GetAdministrativoAsync(string nombre, string password)
        {
            return _database.Table<Administrativo>()
                            .Where(a => a.usuario == nombre && a.password == password)
                            .FirstOrDefaultAsync();
        }

        public Task<Cliente> GetClienteByIdAsync(int id)
        {
            return _database.Table<Cliente>()
                            .Where(c => c.id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveUsuarioAsync<T>(T usuario) where T : Usuario
        {
            if (usuario.id != 0)
            {
                return _database.UpdateAsync(usuario);
            }
            else
            {
                return _database.InsertAsync(usuario);
            }
        }
    }

}
