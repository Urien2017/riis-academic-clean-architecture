
using RIIS.Academic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<RiisAcademicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RIISAcademic")));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
