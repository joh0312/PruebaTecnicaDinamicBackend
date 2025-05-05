using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data.Servicios
{
    public class CitaService
    {
        private readonly AplicationDbContext _context;

        public CitaService(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task ReservarCitaAsync(int idCita, int idPaciente)
        {
            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_ReservarCita";
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.Add(new SqlParameter("@IdCita", SqlDbType.Int) { Value = idCita });
                    command.Parameters.Add(new SqlParameter("@IdPaciente", SqlDbType.Int) { Value = idPaciente });

                    // Ejecutar el procedimiento almacenado
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex) when (ex.Number == 50000) // Captura el error lanzado por RAISERROR
            {
                throw new InvalidOperationException("La cita no está disponible.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
