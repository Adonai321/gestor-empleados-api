using MiWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWebAPI.Data
{
    public class ProveedorData
    {
        private readonly string conexion;

        public ProveedorData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaProveedor")!;
        }

        public async Task<List<PracticaProveedor>> Lista()
        {
            List<PracticaProveedor> lista = new List<PracticaProveedor>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PROVEEDOR_SELECCIONA", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new PracticaProveedor
                        {
                            Id = Convert.ToInt64(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            ApellidoPaterno = reader["ApellidoPaterno"].ToString(),
                            ApellidoMaterno = reader["ApellidoMaterno"].ToString(),
                            Rfc = reader["Rfc"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Credito = Convert.ToDecimal(reader["Credito"]),
                            NombreBanco = reader["NombreBanco"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<PracticaProveedor> Obtener(long id)
        {
            PracticaProveedor objeto = new PracticaProveedor();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PROVEEDOR_SELECCIONA_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new PracticaProveedor
                        {
                            Id = Convert.ToInt64(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            ApellidoPaterno = reader["ApellidoPaterno"].ToString(),
                            ApellidoMaterno = reader["ApellidoMaterno"].ToString(),
                            Rfc = reader["Rfc"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Credito = Convert.ToDecimal(reader["Credito"]),
                            NombreBanco = reader["NombreBanco"].ToString()
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(PracticaProveedor objeto)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    using (var cmd = new SqlCommand("dbo.PROVEEDOR_AGREGA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                        cmd.Parameters.AddWithValue("@apellido_paterno", objeto.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@apellido_materno", objeto.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@rfc", objeto.Rfc);
                        cmd.Parameters.AddWithValue("@correo", objeto.Correo);
                        cmd.Parameters.AddWithValue("@credito", objeto.Credito);
                        cmd.Parameters.AddWithValue("@nombre_banco", objeto.NombreBanco);

                        await con.OpenAsync();
                        int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                        // Si SET NOCOUNT ON está activo, devuelve -1 pero fue exitoso
                        return filasAfectadas > 0 || filasAfectadas == -1;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public async Task<bool> Editar(PracticaProveedor objeto)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    using (var cmd = new SqlCommand("dbo.ACTUALIZAR_PROVEEDOR", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Id", objeto.Id);
                        cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                        cmd.Parameters.AddWithValue("@apellido_paterno", objeto.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@apellido_materno", objeto.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@rfc", objeto.Rfc);
                        cmd.Parameters.AddWithValue("@correo", objeto.Correo);
                        cmd.Parameters.AddWithValue("@credito", objeto.Credito);
                        cmd.Parameters.AddWithValue("@nombre_banco", objeto.NombreBanco);

                        await con.OpenAsync();
                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public async Task<bool> Eliminar(long id)
        {
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    using (var cmd = new SqlCommand("dbo.PROVEEDOR_BORRAR", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);

                        await con.OpenAsync();
                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}