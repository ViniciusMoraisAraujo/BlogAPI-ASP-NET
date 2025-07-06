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
    public async Task<IActionResult> CreateAsync([FromBody] RegisterViewModel model)
    {
        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-"),
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);
        
        bool verification = await _context.Users.AnyAsync(x => x.Email == user.Email);
        if (verification)
            return BadRequest(new ResultViewModel<string>("05X17 - Email already registered"));

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new ResultViewModel<dynamic>(
            new
            {
                user = user.Email, password
            }));
    }
    
    [HttpPost("v1/accounts/login")]
    public IActionResult Login()
    {
        var token = _tokenService.GenerateToken(null);
        return Ok(token);
    }
    
}