using BlogAs.Data;
using BlogAs.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddValidatorsFromAssemblyContaining<EditorCategoryViewModelValidator>();

var app = builder.Build();

app.MapControllers();
app.Run();