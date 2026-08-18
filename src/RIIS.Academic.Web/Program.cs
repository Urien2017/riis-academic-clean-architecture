
using Radzen;
using RIIS.Academic.Infrastructure;
using RIIS.Academic.Infrastructure.Persistence;
using RIIS.Academic.Web.Components;
using RIIS.Academic.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRadzenComponents();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddRiisAcademicInfrastructure(builder.Configuration);

var app = builder.Build();

//await app.Services.InitializeRiisAcademicDatabaseAsync();

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapRiisAcademicExportEndpoints();

app.Run();
