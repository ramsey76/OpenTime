using Microsoft.EntityFrameworkCore;
using TimeManagement.Api.Extensions;
using TimeManagement.Api.Services.Departments;
using TimeManagement.Api.Services.Employees;
using TimeManagement.Api.Services.Roles;
using TimeManagement.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapEndpointsFromAssembly();

app.Run();
