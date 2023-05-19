using Dapper;
using ProyectoVenta.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace ProyectoVenta.Datos
{
    public class DA_Venta
    {
        public string Registrar(string Venta_xml)
        {
            var respuesta = "";
            var cn = new Conexion();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    oconexion.Open();

                    var proc = oconexion.Query<string>("sp_registrar_venta", new { Venta_xml }, null, true, null, commandType: CommandType.StoredProcedure)
                        .FirstOrDefault();

                    respuesta = proc;
                    oconexion.Close();
                }
            }
            catch (Exception ex)
            {
                respuesta = "";
            }

            return respuesta;
        }

        public Venta Detalle(string nrodocumento)
        {
            Venta? oVenta = new Venta();
            var cn = new Conexion();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    oconexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_detalle_venta", oconexion);
                    cmd.Parameters.AddWithValue("nrodocumento", nrodocumento);
                    cmd.CommandType = CommandType.StoredProcedure;

                    XmlReader dr = cmd.ExecuteXmlReader();

                    if (dr.Read())
                    {
                        XDocument doc = XDocument.Load(dr);

                        oVenta = (doc.Element("Venta") != null) ? (from v in doc.Elements("Venta")
                                                                   select new Venta()
                                                                   {
                                                                       TipoPago = v.Element("TipoPago").Value,
                                                                       NumeroDocumento = v.Element("NumeroDocumento").Value,
                                                                       DocumentoCliente = v.Element("DocumentoCliente").Value,
                                                                       NombreCliente = v.Element("NombreCliente").Value,
                                                                       TelefonoCliente = v.Element("TelefonoCliente").Value,
                                                                       DireccionCliente = v.Element("DireccionCliente").Value,
                                                                       MontoPagoCon = Convert.ToDecimal(v.Element("MontoPagoCon").Value, new CultureInfo("es-NI")),
                                                                       MontoCambio = Convert.ToDecimal(v.Element("MontoCambio").Value, new CultureInfo("es-NI")),
                                                                       MontoSubTotal = Convert.ToDecimal(v.Element("MontoSubTotal").Value, new CultureInfo("es-NI")),
                                                                       MontoIGV = Convert.ToDecimal(v.Element("MontoIGV").Value, new CultureInfo("es-NI")),
                                                                       MontoTotal = Convert.ToDecimal(v.Element("MontoTotal").Value, new CultureInfo("es-NI")),
                                                                       FechaRegistro = v.Element("FechaRegistro").Value,
                                                                       oDetalleVenta = (v.Element("DetalleVenta") != null) ? (from i in v.Element("DetalleVenta").Elements("Item")
                                                                                                                              select new Detalle_Venta()
                                                                                                                              {
                                                                                                                                  oProducto = new Producto()
                                                                                                                                  {
                                                                                                                                      Descripcion = i.Element("Descripcion").Value
                                                                                                                                  },
                                                                                                                                  Cantidad = Convert.ToInt32(i.Element("Cantidad").Value),
                                                                                                                                  PrecioVenta = Convert.ToDecimal(i.Element("PrecioVenta").Value, new CultureInfo("es-PE")),
                                                                                                                                  Total = Convert.ToDecimal(i.Element("Total").Value, new CultureInfo("es-PE")),
                                                                                                                              }).ToList() : new List<Detalle_Venta>()

                                                                   }).FirstOrDefault() : new Venta();

                    }


                }
            }
            catch (Exception ex)
            {
                oVenta = new Venta();
            }

            return oVenta;
        }

        public class SaleRespone
        {
            public string NroDocumento { get; set; }
        }

    }
}
