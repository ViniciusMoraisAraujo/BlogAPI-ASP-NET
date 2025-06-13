using BlogAs.Data;
using BlogAs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAs.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
    {
        var categories = await context.Categories.ToListAsync();
        return Ok(categories);
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> CreateAsync([FromServices] BlogDataContext context, [FromBody] Category category)
    {
        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories{category.Id}", category);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "05XE9 - Unable to create category");
        }
        catch (Exception)
        {
            return StatusCode(500, "05X10 - Internal server error");
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromServices] BlogDataContext context,
        [FromBody] Category category)
    {
        var categoryToUpdate = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (categoryToUpdate == null)
            return NotFound();
        
        categoryToUpdate.Name = category.Name;
        categoryToUpdate.Slug = category.Slug;
        context.Categories.Update(categoryToUpdate);
        await context.SaveChangesAsync();
        return Ok(categoryToUpdate);
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        var categoryToDelete = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (categoryToDelete == null)
            return NotFound();
        
        context.Categories.Remove(categoryToDelete);
        await context.SaveChangesAsync();
        return Ok(categoryToDelete);
    }
}