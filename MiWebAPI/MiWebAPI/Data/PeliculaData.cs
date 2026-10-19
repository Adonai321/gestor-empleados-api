using MiWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWebAPI.Data
{
    public class PeliculaData
    {
        private readonly string conexion;

        public PeliculaData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaProveedor")!;
        }

        public async Task<List<Pelicula>> Lista()
        {
            List<Pelicula> lista = new List<Pelicula>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PELICULA_CONSULTARTODOS", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Pelicula
                        {
                            IdPelicula = Convert.ToInt64(reader["IdPelicula"]),
                            Codigo = reader["Codigo"].ToString(),
                            Titulo = reader["Titulo"].ToString(),
                            Director = reader["Director"].ToString(),
                            Genero = reader["Genero"].ToString(),
                            AñoEstreno = Convert.ToInt32(reader["AñoEstreno"]),
                            DuracionMinutos = Convert.ToDecimal(reader["DuracionMinutos"]),
                            Activo = Convert.ToBoolean(reader["Activo"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Pelicula> Obtener(long Id)
        {
            Pelicula objeto = new Pelicula();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("PELICULA_CONSULTARPORID", con);
                cmd.Parameters.AddWithValue("@IdPelicula", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (HasColumn(reader, "IdPelicula"))
                        {
                            objeto = new Pelicula
                            {
                                IdPelicula = Convert.ToInt64(reader["IdPelicula"]),
                                Codigo = reader["Codigo"].ToString(),
                                Titulo = reader["Titulo"].ToString(),
                                Director = reader["Director"].ToString(),
                                Genero = reader["Genero"].ToString(),
                                AñoEstreno = Convert.ToInt32(reader["AñoEstreno"]),
                                DuracionMinutos = Convert.ToDecimal(reader["DuracionMinutos"]),
                                Activo = Convert.ToBoolean(reader["Activo"])
                            };
                        }
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Pelicula objeto)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("PELICULA_INSERTAR", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Codigo", objeto.Codigo);
                cmd.Parameters.AddWithValue("@Titulo", objeto.Titulo);
                cmd.Parameters.AddWithValue("@Director", objeto.Director);
                cmd.Parameters.AddWithValue("@Genero", objeto.Genero);
                cmd.Parameters.AddWithValue("@AñoEstreno", objeto.AñoEstreno);
                cmd.Parameters.AddWithValue("@DuracionMinutos", objeto.DuracionMinutos);

                try
                {
                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int numeroError = Convert.ToInt32(reader["NumeroError"]);
                            respuesta = (numeroError == 1);
                        }
                    }
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Pelicula objeto)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("PELICULA_ACTUALIZAR", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPelicula", objeto.IdPelicula);
                cmd.Parameters.AddWithValue("@Codigo", objeto.Codigo);
                cmd.Parameters.AddWithValue("@Titulo", objeto.Titulo);
                cmd.Parameters.AddWithValue("@Director", objeto.Director);
                cmd.Parameters.AddWithValue("@Genero", objeto.Genero);
                cmd.Parameters.AddWithValue("@AñoEstreno", objeto.AñoEstreno);
                cmd.Parameters.AddWithValue("@DuracionMinutos", objeto.DuracionMinutos);

                try
                {
                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int numeroError = Convert.ToInt32(reader["NumeroError"]);
                            respuesta = (numeroError == 1);
                        }
                    }
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(long idPelicula)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("PELICULA_ELIMINAR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pelicula", idPelicula);

                try
                {
                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int numeroError = Convert.ToInt32(reader["NumeroError"]);
                            respuesta = (numeroError == 1);
                        }
                    }
                }
                catch
                {
                    respuesta = false;
                }
            }

            return respuesta;
        }

        private bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}