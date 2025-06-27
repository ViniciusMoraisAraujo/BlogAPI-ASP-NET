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
    private readonly IValidator<RegisterViewModel> _validator;
    
    public AccountController(TokenService tokenService, IValidator<RegisterViewModel> validator)
    {
        _tokenService = tokenService;
        _validator = validator;
    }

    [HttpPost("v1/accounts")]
    public async Task<IActionResult> CreateAsync([FromBody] RegisterViewModel model, [FromServices] BlogDataContext context)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new ResultViewModel<string>(errors, "05X16"));
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-"),
            };
        }
        catch (Exception)
        {
            return StatusCode(500, "05X15 - Internal server error");       
        }
        return Ok();
    }
    
    [HttpPost("v1/accounts/login")]
    public IActionResult Login()
    {
        var token = _tokenService.GenerateToken(null);
        return Ok(token);
    }
    
}