using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;

namespace Data.Servicios
{
    public class MedicoService
    {
        private readonly AplicationDbContext _context;

        public MedicoService(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Medico>> ObtenerMedicosPorEspecializacionAsync(string especialidad)
        {
            var medicos = new List<Medico>();

            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "MedicosPorEspecializacion";
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetro
                    command.Parameters.Add(new SqlParameter("@Especialidad", SqlDbType.NVarChar, 100) { Value = especialidad });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            medicos.Add(new Medico
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Especialidad = reader.GetString(reader.GetOrdinal("especialidad"))
                            });
                        }
                    }
                }
            }
            finally
            {
                await connection.CloseAsync();
            }

            return medicos;
        }
    }
}