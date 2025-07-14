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
    public async Task<IActionResult> GetAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

    [HttpGet("v1/posts/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var post = await _context.Posts.Include(post => post.Category)
            .Include(post => post.Author).FirstOrDefaultAsync(x => x.Id == id);
        if (post == null)
            return NotFound(new ResultViewModel<string>("05X16 - Post not found"));

        var response = new PostResponseViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            CategoryName = post.Category.Name,
            AuthorName = post.Author.Name,
        };
        
        return Ok(new ResultViewModel<PostResponseViewModel>(response));
    }

    [HttpPost("v1/posts")]
    public async Task<IActionResult> CreatedAsync([FromBody] EditorPostViewModel editorPost)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == editorPost.CategoryId);
        if (category == null)
            return NotFound(new ResultViewModel<string>("05X17 - Category not found"));
        var author = await _context.Users.FirstOrDefaultAsync(x => x.Id == editorPost.AuthorId);
        if (author == null)
            return NotFound(new ResultViewModel<string>("05X18 - Author not found"));
        var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == editorPost.TagId);
        if (tag == null)
            return NotFound(new ResultViewModel<string>("05X19 - Tag not found"));
        var postModel = new Post
        {
            Title = editorPost.Title, 
            Category = category, 
            Author = author,
            CreateDate = DateTime.Now,
            LastUpdateDate = DateTime.Now, 
            Tags = [tag],
            Body = editorPost.Body,
            Summary = editorPost.Summary,
            Slug = editorPost.Title.Replace(" ", "-").ToLower()
        };
        await _context.Posts.AddAsync(postModel);
        await _context.SaveChangesAsync();

        var response = new PostResponseViewModel
        {
            Id = postModel.Id,
            Title = postModel.Title,
            Slug = postModel.Slug,
            CategoryName = postModel.Category.Name,
            AuthorName = postModel.Author.Name,
        };

        return Ok(new ResultViewModel<PostResponseViewModel>(response, "Post Created"));
    }

    [HttpPut("v1/posts/{id:int}")]

    public async Task<IActionResult> EditorAsync([FromRoute] int id, [FromBody] EditorPostViewModel editorPost)
    {
        var postToUpdate = await _context.Posts.Include(post => post.Category).Include(author => author.Author).FirstOrDefaultAsync(x => x.Id == id);
        if (postToUpdate == null)
            return NotFound(new ResultViewModel<string>("05X20 - Post not found"));
        
        postToUpdate.Title = editorPost.Title;
        postToUpdate.Body = editorPost.Body;
        postToUpdate.Summary = editorPost.Summary;
        postToUpdate.LastUpdateDate = DateTime.Now;
        postToUpdate.Slug = editorPost.Title.Replace(" ", "-").ToLower();
        _context.Posts.Update(postToUpdate);
        await _context.SaveChangesAsync();

        var response = new PostResponseViewModel
        {
            Id = postToUpdate.Id,
            Title = postToUpdate.Title,
            Slug = postToUpdate.Slug,
            CategoryName = postToUpdate.Category.Name,
            AuthorName = postToUpdate.Author.Name,
        };
        
        return Ok(new ResultViewModel<PostResponseViewModel>(response, "Post Updated"));
    }
    [HttpDelete("v1/posts/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var postToDelete = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
        if (postToDelete == null)
            return NotFound(new ResultViewModel<string>("05X21 - Post not found"));
        
        _context.Posts.Remove(postToDelete);
        await _context.SaveChangesAsync();
        
        return Ok(new ResultViewModel<dynamic>(postToDelete.Id, "Post Deleted"));
    }
}

