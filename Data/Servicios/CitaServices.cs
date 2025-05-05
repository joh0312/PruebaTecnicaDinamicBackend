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


        public async Task<List<CitaDto>> ObtenerCitasDisponiblesAsync()
        {
            var citas = new List<CitaDto>();

            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "obtenerCitas";
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            citas.Add(new CitaDto
                            {
                                Id = reader.GetInt32(0),
                                IdPaciente = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                                IdMedico = reader.GetInt32(2),
                                Especialidad = reader.GetString(3),
                                FechaHora = reader.GetDateTime(4),
                                Estado = reader.GetString(5)
                            });
                        }
                    }
                }
            }
            finally
            {
                await connection.CloseAsync();
            }

            return citas;
        }

        public class CitaDto
        {
            public int Id { get; set; }
            public int? IdPaciente { get; set; }
            public int IdMedico { get; set; }
            public string Especialidad { get; set; }
            public DateTime FechaHora { get; set; }
            public string Estado { get; set; }
        }
    }
}
