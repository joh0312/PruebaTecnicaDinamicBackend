using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;
using Models.DTOs;

namespace Data.Servicios
{
    public class PacienteService
    {
        private readonly AplicationDbContext _context;

        public PacienteService(AplicationDbContext context)
        {
            _context = context;
        }

        // Método para insertar un paciente y obtener el ID insertado
        public async Task<int> InsertarPacienteAsync(string documento, DateTime fechaNacimiento, string nombre, string apellidos, string telefono)
        {
            int newId = 0;

            // Obtener la conexión desde el DbContext
            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "InsertPaciente_insert";
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.Add(new SqlParameter("@Documento", SqlDbType.NVarChar, 20) { Value = documento });
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento", SqlDbType.Date) { Value = fechaNacimiento });
                    command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100) { Value = nombre });
                    command.Parameters.Add(new SqlParameter("@Apellidos", SqlDbType.NVarChar, 100) { Value = apellidos });
                    command.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.NVarChar, 20) { Value = telefono });

                    // Ejecutar el comando y leer el resultado
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Leer el ID como Decimal y convertirlo a Int32
                            newId = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("Id")));
                        }
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 50000) // Captura el error lanzado por RAISERROR
            {
                throw new InvalidOperationException("El documento ya existe en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                throw new InvalidOperationException("Error al insertar el paciente.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }

            return newId;
        }
        // Método para obtener un paciente por documento
        public async Task<Paciente> ObtenerPacientePorDocumentoAsync(string documento)
        {
            Paciente paciente = null;

            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_GetPacienteByDocumento";
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetro
                    command.Parameters.Add(new SqlParameter("@Documento", SqlDbType.NVarChar, 20) { Value = documento });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            paciente = new Paciente
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Documento = reader.GetString(reader.GetOrdinal("documento")),
                                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fechanacimiento")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombres")),
                                Apellidos = reader.GetString(reader.GetOrdinal("apellidos")),
                                Telefono = reader.GetString(reader.GetOrdinal("telefono"))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener el paciente.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }

            return paciente;
        }

        // Método para actualizar el nombre de un paciente
        public async Task ActualizarPacienteAsync(int id, string documento, DateTime fechaNacimiento, string nombre, string apellidos, string telefono)
        {
            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_UpdatePaciente";
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
                    command.Parameters.Add(new SqlParameter("@Documento", SqlDbType.NVarChar, 20) { Value = documento });
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento", SqlDbType.Date) { Value = fechaNacimiento });
                    command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100) { Value = nombre });
                    command.Parameters.Add(new SqlParameter("@Apellidos", SqlDbType.NVarChar, 100) { Value = apellidos });
                    command.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.NVarChar, 20) { Value = telefono });

                    // Ejecutar el comando
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al actualizar el paciente.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }


        // Método para eliminar un paciente por ID
        public async Task EliminarPacienteAsync(int id)
        {
            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_DeletePaciente";
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetro
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

                    // Ejecutar el comando
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al eliminar el paciente.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
