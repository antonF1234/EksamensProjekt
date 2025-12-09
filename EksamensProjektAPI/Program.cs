using EksamensProjektAPI.Repositories;
using EksamensProjektAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ProjectRepo>();
builder.Services.AddScoped<TaskRepo>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<UsersSkillsService>();
builder.Services.AddScoped<UsersTasksService>();
builder.Services.AddScoped<UsersTasksRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();