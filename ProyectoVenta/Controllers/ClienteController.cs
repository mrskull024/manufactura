using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoVenta.Datos;
using ProyectoVenta.Models;

namespace ProyectoVenta.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        DA_Cliente _daCliente = new DA_Cliente();
        public IActionResult Clientes()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaClientes()
        {
            List<Cliente> oLista = new();
            oLista = _daCliente.Listar();
            return Json(new { data = oLista });
        }

        [HttpPost]
        public JsonResult GuardarCliente([FromBody] Cliente obj)
        {
            string operacion = Request.Headers["operacion"];
            bool respuesta;

            if (operacion == "crear")
            {
                respuesta = _daCliente.Guardar(obj);
            }
            else
            {
                respuesta = _daCliente.Editar(obj);
            }

            return Json(new { respuesta = respuesta });
        }

        [HttpDelete]
        public JsonResult EliminarCliente(int idCliente)
        {
            bool respuesta;
            respuesta = _daCliente.Eliminar(idCliente);
            return Json(new { respuesta = respuesta });
        }
    }
}
