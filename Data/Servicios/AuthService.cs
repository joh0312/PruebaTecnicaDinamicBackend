using System.Data;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entidades;

namespace Data.Servicios
{
    public class AuthService
    {
        private readonly AplicationDbContext _context;
        private readonly ITokenServicio _tokenServicio;


        public AuthService(AplicationDbContext context, ITokenServicio tokenServicio )
        {
            _context = context;
            _tokenServicio = tokenServicio;
        }

        public async Task<UsuarioDto> LoginAsync(string documento, DateTime fechaNacimiento)
        {
            Paciente paciente = null;

            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_LoginPaciente";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Documento", SqlDbType.NVarChar, 20) { Value = documento });
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento", SqlDbType.Date) { Value = fechaNacimiento });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            paciente = new Paciente
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Documento = reader.GetString(reader.GetOrdinal("documento")),
                                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fechanacimiento")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Apellidos = reader.GetString(reader.GetOrdinal("apellidos")),
                                Telefono = reader.GetString(reader.GetOrdinal("telefono"))
                            };
                        }
                    }
                }
            }
            finally
            {
                await connection.CloseAsync();
            }

            if (paciente == null)
            {
                return null; // o lanzar una excepción personalizada
            }

            var usuario = new Usuario
            {
                Documento = paciente.Documento
            };

            var token = _tokenServicio.CrearToken(usuario);

            return new UsuarioDto
            {
                Paciente = paciente,

                Token = token
            };
        }


    }
}