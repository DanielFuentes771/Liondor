using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Productos.Context;
using Productos.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Productos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly dbContext _context;
        private readonly IConfiguration _configuracion;

        public ProductosController(dbContext context, IConfiguration configuracion)
        {
            _context = context;
            _configuracion = configuracion;
        }

       
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Se utiliza para generar y simular el ingreso e inicio de session y la creacion de un token")]

        public IActionResult Login([FromForm] LoginModel login)
        {
            
            if (login.Username == "admin" && login.Password == "1234")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuracion["JWT_Configuracion:LLaveSecreta"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, login.Username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        [HttpGet(Name = "GetProductos")]
        [Authorize]
        [SwaggerOperation(Summary = "Se utiliza para listar todos los Productos")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetAll()
            => await _context.Productos.ToListAsync();

        [HttpGet("{ProductoId}", Name = "GetProductoSearch")]
        [Authorize]
        [SwaggerOperation(Summary = "Se utiliza para buscar un Producto")]
        public async Task<ActionResult<object>> Show(int ProductoId)
        {
            var producto = await _context.Productos.FindAsync(ProductoId);
            if (producto == null) return NotFound(new { message = "Producto no encontrado" });

            return Ok(new
            {
                producto.Id,
                producto.Activo,
                producto.Nombre,
                producto.Precio,
            });
        }

        [HttpPost(Name = "PostProducto")]
        [Authorize]
        [SwaggerOperation(Summary = "Se utiliza para agregar un Producto")]
        public async Task<ActionResult<Producto>> PostAll([FromForm] InsertarProducto nuevoProducto)
        {
            if (nuevoProducto == null) return BadRequest(new { message = "Datos inválidos" });

          
            var producto = new Producto
            {
                Nombre = nuevoProducto.Nombre,
                Activo = nuevoProducto.Activo,
                Precio = nuevoProducto.Precio,
                Stock = nuevoProducto.Stock,
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpGet("detalle/{id}", Name = "GetsProducto")]
        [SwaggerIgnore]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }


        [HttpPut("{ProductoId}", Name = "PutProducto")]
        [Authorize]
        [SwaggerOperation(Summary = "Se utiliza para actualizar un Producto")]
        public async Task<ActionResult<object>> UpdateProduct(int ProductoId, [FromForm] InsertarProducto productoActualizado)
        {
            var producto = await _context.Productos.FindAsync(ProductoId);
            if (producto == null) return NotFound(new { message = "Producto no encontrado" });

            producto.Nombre = productoActualizado.Nombre ?? producto.Nombre;
            producto.Precio = productoActualizado.Precio > 0 ? productoActualizado.Precio : producto.Precio;
            producto.Activo = productoActualizado.Activo;

            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Producto actualizado exitosamente" });
        }

        [HttpDelete("{ProductoId}", Name = "DeleteProducto")]
        [Authorize]
        [SwaggerOperation(Summary = "Se utiliza para eliminar un Producto")]
        public async Task<IActionResult> DeleteProduct(int ProductoId)
        {
            var producto = await _context.Productos.FindAsync(ProductoId);
            if (producto == null) return NotFound(new { message = "Producto no encontrado" });

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Producto eliminado exitosamente" });
        }
    }
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
