using ProyectoVenta.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoVenta.Datos
{
    public class DA_Cliente
    {
        public List<Cliente> Listar()
        {

            var oLista = new List<Cliente>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_listar_cliente", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new Cliente()
                        {
                            IdCliente = Convert.ToInt32(dr["Id_Cliente"]),
                            NumeroIdentificacion = dr["NumeroIdentificacion"].ToString(),
                            NombresCompletos = dr["Nombres"].ToString(),
                            NumeroTelefonico = dr["telefono"].ToString(),
                            Correo = dr["correo"].ToString(),
                            Direccion = dr["Direccion"].ToString()
                        });
                    }
                }
            }

            return oLista;
        }


        public bool Guardar(Cliente obj)
        {
            bool respuesta;
            var cn = new Conexion();
            try
            {

                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    oconexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_guardar_cliente", oconexion);
                    cmd.Parameters.AddWithValue("NumeroIdentificacion", obj.NumeroIdentificacion);
                    cmd.Parameters.AddWithValue("Nombres", obj.NombresCompletos);
                    cmd.Parameters.AddWithValue("telefono", obj.NumeroTelefonico);
                    cmd.Parameters.AddWithValue("correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public bool Editar(Cliente obj)
        {
            bool respuesta;
            var cn = new Conexion();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    oconexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_editar_cliente", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", obj.IdCliente);
                    cmd.Parameters.AddWithValue("NumeroIdentificacion", obj.NumeroIdentificacion);
                    cmd.Parameters.AddWithValue("Nombres", obj.NombresCompletos);
                    cmd.Parameters.AddWithValue("Telefono", obj.NumeroTelefonico);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public bool Eliminar(int idCliente)
        {
            bool respuesta;
            var cn = new Conexion();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    oconexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_eliminar_cliente", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", idCliente);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }
    }
}
