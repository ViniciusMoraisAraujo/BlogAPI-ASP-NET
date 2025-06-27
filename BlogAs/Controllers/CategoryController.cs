using BlogAs.Data;
using BlogAs.Models;
using BlogAs.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAs.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IValidator<EditorCategoryViewModel> _validator;
    private readonly BlogDataContext _context;

    public CategoryController(IValidator<EditorCategoryViewModel> validator, BlogDataContext context)
    {
        _validator = validator;
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
    public async Task<IActionResult> CreateAsync([FromServices] BlogDataContext context,
        [FromBody] EditorCategoryViewModel category)
    {
        var validationResult = await _validator.ValidateAsync(category);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new ResultViewModel<string>(errors, "0X50"));
        }

        var categoryModel = new Category { Id = 0, Posts = null, Name = category.Name, Slug = category.Slug.ToLower() };
        await context.Categories.AddAsync(categoryModel);
        await context.SaveChangesAsync();

        return Created($"v1/categories{categoryModel.Id}", new ResultViewModel<Category>(categoryModel));
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> EditorAsync([FromRoute] int id, [FromServices] BlogDataContext context,
        [FromBody] EditorCategoryViewModel editorCategory)
    {
        var validationResult = await _validator.ValidateAsync(editorCategory);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new ResultViewModel<string>(errors, "05X17"));
        }

        var categoryToUpdate = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (categoryToUpdate == null)
            return NotFound(new ResultViewModel<string>("05X18 - Category not found"));

        categoryToUpdate.Name = editorCategory.Name;
        categoryToUpdate.Slug = editorCategory.Slug;
        context.Categories.Update(categoryToUpdate);
        await context.SaveChangesAsync();
        return Ok(categoryToUpdate);
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        var categoryToDelete = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (categoryToDelete == null)
            return NotFound(new ResultViewModel<string>("05X20 - Category not found"));

        context.Categories.Remove(categoryToDelete);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Category>(categoryToDelete, "05X21 - Category deleted"));
    }
}