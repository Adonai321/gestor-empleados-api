using MiWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWebAPI.Data
{
    public class ProductoData
    {

        private readonly string conexion;

        public ProductoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<ProductoDetalle>> Lista()
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_LISTAR", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new ProductoDetalle
                        {

                            Id = Convert.ToInt64(reader["id"]),
                            NombreProducto = reader["NombreProducto"].ToString(),
                            Precio = Convert.ToDecimal(reader["precio"]),
                            Cantidad = Convert.ToInt32(reader["cantidad"]),
                            Total = Convert.ToDecimal(reader["total"]),
                            Descripcion = reader["Descripcion"].ToString()

                        });
                    }
                }
            }
            return lista;
        }

        public async Task<ProductoDetalle> Obtener(long Id)
        {
            ProductoDetalle objeto = new ProductoDetalle();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_OBTENER", con);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.CommandType = CommandType.StoredProcedure;
                

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new ProductoDetalle
                        {
                            Id = Convert.ToInt64(reader["Id"]),
                            NombreProducto = reader["NombreProducto"].ToString(),
                            Precio = Convert.ToDecimal(reader["precio"]),
                            Cantidad = Convert.ToInt32(reader["cantidad"]),
                            Total = Convert.ToDecimal(reader["total"]),
                            Descripcion = reader["Descripcion"].ToString()
                        };
                    }
                }
            }
            return objeto;
        }


        public async Task<bool> Crear(ProductoDetalle objeto)
        {
            using (var con = new SqlConnection(conexion))
            {
                using (var cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_AGREGA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre_PRODUCTO",
                        objeto.NombreProducto);
                    cmd.Parameters.AddWithValue("@PRECIO", objeto.Precio);
                    cmd.Parameters.AddWithValue("@CANTIDAD", objeto.Cantidad);
                    cmd.Parameters.AddWithValue("@TOTAL", objeto.Total);
                    cmd.Parameters.AddWithValue("@DESCRIPCION",
                        objeto.Descripcion ?? (object)DBNull.Value);

                    try
                    {
                        await con.OpenAsync();
                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (SqlException)
                    {
                        return false;
                    }
                }
            }
        }



        public async Task<bool> Editar(ProductoDetalle objeto)
        {
            using (var con = new SqlConnection(conexion))
            {
                using (var cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_EDITAR", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", objeto.Id);
                    cmd.Parameters.AddWithValue("@NombreProducto",
                        objeto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Precio", objeto.Precio);
                    cmd.Parameters.AddWithValue("@Cantidad", objeto.Cantidad);
                    cmd.Parameters.AddWithValue("@Descripcion",
                        objeto.Descripcion ?? (object)DBNull.Value);

                    try
                    {
                        await con.OpenAsync();

                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (SqlException)
                    {
                        return false;
                    }
                }
            }
        }



        public async Task<bool> Eliminar(long id)
        {
            using (var con = new SqlConnection(conexion))
            {
                using (var cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_ELIMINAR", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        await con.OpenAsync();

                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                    catch (SqlException)
                    {
                        return false;
                    }
                }
            }
        }

    }
}
