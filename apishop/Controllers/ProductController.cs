using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apishop.Data;
using apishop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apishop.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> getAllProducts([FromServices] DataContext context)
        {
            List<Product> products = await context
            .Products
            .Include(x => x.Category)
            .AsNoTracking().ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<ActionResult<Product>> getProductById(int Id, [FromServices] DataContext context)
        {
            var product = await context
            .Products
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == Id);

            return Ok(product);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Product>>> postOneProduct([FromBody] Product model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi posivel criar um novo Produto" });
            }

        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<ActionResult<Product>> putOneProduct(int Id,
        [FromBody] Product model,
        [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            if (model.Id != Id)
                return NotFound(new { message = "Produto não encontrada" });

            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                BadRequest(new { message = "Esse registro ja foi atualizado" });
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{Id:int}")]

        public async Task<ActionResult<Product>> deleteProductById(int Id, [FromServices] DataContext context)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);
            if (product == null)
                NotFound(new { message = "Produto não encontrado" });

            try
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return Ok(new { message = "Produto Excluido" });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel remover esse produto" });
            }
        }
    }
}