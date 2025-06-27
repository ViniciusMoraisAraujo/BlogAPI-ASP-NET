using BlogAs.Data;
using BlogAs.Models;
using BlogAs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAs.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    private readonly BlogDataContext _context;

    public CategoryController(BlogDataContext context)
    {
        _context = context;
    }

    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(new ResultViewModel<List<Category>>(categories));
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound(new ResultViewModel<Category>("05X16 - Category not found"));

        return Ok(new ResultViewModel<Category>(category));
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> CreateAsync([FromBody] EditorCategoryViewModel category)
    {

        var categoryModel = new Category { Id = 0, Posts = null, Name = category.Name, Slug = category.Slug.ToLower() };
        await _context.Categories.AddAsync(categoryModel);
        await _context.SaveChangesAsync();

        return Created($"v1/categories{categoryModel.Id}", new ResultViewModel<Category>(categoryModel));
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> EditorAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel editorCategory)
    {
        var categoryToUpdate = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (categoryToUpdate == null)
            return NotFound(new ResultViewModel<string>("05X18 - Category not found"));

        categoryToUpdate.Name = editorCategory.Name;
        categoryToUpdate.Slug = editorCategory.Slug;
        _context.Categories.Update(categoryToUpdate);
        await _context.SaveChangesAsync();
        return Ok(categoryToUpdate);
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (categoryToDelete == null)
            return NotFound(new ResultViewModel<string>("05X20 - Category not found"));

        _context.Categories.Remove(categoryToDelete);
        await _context.SaveChangesAsync();
        return Ok(new ResultViewModel<Category>(categoryToDelete, "05X21 - Category deleted"));
    }
}