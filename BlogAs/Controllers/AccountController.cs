using BlogAs.Data;
using BlogAs.Models;
using BlogAs.Services;
using BlogAs.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BlogAs.Controllers;


[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly BlogDataContext _context;
    
    public AccountController(TokenService tokenService, BlogDataContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    [HttpPost("v1/accounts")]
    public async Task<IActionResult> CreateAsync([FromBody] RegisterViewModel model, [FromServices] EmailService emailService)
    {
        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-"),
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);
        
        bool verification =  _context.Users.Any(x => x.Email == user.Email);
        if (verification)
            return BadRequest(new ResultViewModel<string>("05X17 - Email already registered"));

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        emailService.SendEmail(user.Email, user.Email, "Welcome", $"Your password is: {password}");

        return Ok(new ResultViewModel<dynamic>(
            new
            {
                user = user.Email
            }));
    }
    
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        var user = await _context.Users.AsNoTracking().Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);
        
        if (user == null)
            return NotFound(new ResultViewModel<string>("05X19 - User not found"));
        
        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return Unauthorized(new ResultViewModel<string>("05X22 - Invalid password"));
        
        var token = _tokenService.GenerateToken(user); 
        return Ok(new ResultViewModel<dynamic>(token));
    }
}