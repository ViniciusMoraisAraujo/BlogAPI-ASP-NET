using BlogAs.Data;
using BlogAs.Models;
using BlogAs.ViewModels;
using BlogAs.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAs.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    private readonly BlogDataContext _context;
   
    public PostController(BlogDataContext context)
    {
        _context = context;
    }
    
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAsync([FromQuery]int page = 1, [FromQuery]int pageSize = 10)
    {
        var posts = await _context.Posts.AsNoTracking().Include(x => x.Category)
            .Include(x => x.Author).Select(x => new ListPostsViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                LastUpdateDate = x.LastUpdateDate,
                Category = x.Category.Name,
                Author = $"{x.Author.Name} ({x.Author.Email})"
            })
            .OrderByDescending(x => x.LastUpdateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return Ok(new ResultViewModel<dynamic>(new
        {
            page,
            pageSize,
            total = await _context.Posts.CountAsync(),
            posts
        }));
    }

    [HttpPost("v1/posts")]
    public async Task<IActionResult> CreatedAsync([FromBody] EditorPostViewModel editorPost)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == editorPost.CategoryId);
        var author = await _context.Users.FirstOrDefaultAsync(x => x.Id == editorPost.AuthorId);
        var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == editorPost.TagId);
        var postModel = new Post
        {
            Id = 0, 
            Title = editorPost.Title, 
            Category = category, 
            Author = author,
            CreateDate = DateTime.Now,
            LastUpdateDate = DateTime.Now, 
            Body = editorPost.Body
        };
        return Ok();
    }
}