using System.Collections.Generic;
using System.Threading.Tasks;
using apishop.Data;
using apishop.Models;
using Microsoft.AspNetCore.Mvc;

namespace apishop.Controllers
{
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        //Endpoint
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> getAllCategories([FromServices] DataContext context)
        {
            List<Category> list = context.Categories.FindAsync();
            return list;
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<ActionResult<Category>> getOneCategoryById(int Id)
        {
            return new Category();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> postOneCategory(
            [FromBody] Category model,
            [FromServices] DataContext context
            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                // TODO
                return BadRequest(new { message = "Não foi posivel criar um nova categora" });
            }

        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<ActionResult<Category>> putOneCategory(int Id,
        [FromBody] Category model,
        [FromServices] DataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Id != Id)
                return NotFound(new { message = "Categoria não encontrada" });
            try
            {
                context.Entry<Category>(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {

                BadRequest(new { message = "Não foi possivel alterar essac categoria" });
            }



            return Ok(model);
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<ActionResult<Category>> deleteOneCategory(int Id)
        {
            return Ok();
        }

    }
}