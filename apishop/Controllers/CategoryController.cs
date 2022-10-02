using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apishop.Data;
using apishop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            List<Category> list = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(list);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<ActionResult<Category>> getOneCategoryById(int Id, [FromServices] DataContext context)
        {
            Category category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
            return Ok(category);
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
            catch (DbUpdateConcurrencyException)
            {
                BadRequest(new { message = "Esse registro ja foi atualizado" });
            }
            catch (Exception)
            {

                BadRequest(new { message = "Não foi possivel alterar essac categoria" });
            }



            return Ok(model);
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<ActionResult<Category>> deleteOneCategory([FromServices] DataContext context, int Id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            if (category == null)
                NotFound(new { message = "Categoria não encontrada" });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida!" });
            }
            catch (Exception)
            {
                // TODO
                return BadRequest(new { message = "Não foi possivel remover essa categoria" });
            }
        }

    }
}