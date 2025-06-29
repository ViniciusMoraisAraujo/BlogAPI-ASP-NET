using BlogAs.Data;
using BlogAs.Models;
using BlogAs.Services;
using BlogAs.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogAs.Controllers;


[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    
    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
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
        
        return Ok();
    }
    
    [HttpPost("v1/accounts/login")]
    public IActionResult Login()
    {
        var token = _tokenService.GenerateToken(null);
        return Ok(token);
    }
    
}