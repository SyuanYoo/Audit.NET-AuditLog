using AuditLog.AuditSetup;
using AuditLog.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services to the container and configure the audit global filter
builder.Services.AddControllers(mvc =>
{
    mvc.AuditSetupFilter();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HTTP context accessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<NORTHWINDContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Enable buffering for auditing HTTP request body
app.Use(async (context, next) => {
    context.Request.EnableBuffering();
    await next();
});

// Configure the audit middleware
app.AuditSetupMiddleware();


// Configure the audit output.
app.AuditSetupOutput();

app.Run();
