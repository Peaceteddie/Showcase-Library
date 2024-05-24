using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using ShowcaseLibrary.Components;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddServerSideBlazor()
.AddInteractiveServerComponents();
builder.Services.AddRazorComponents();
builder.Services.AddRazorPages();
AddBlazorise(builder.Services);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(4200, listenOptions =>
    {
        listenOptions.UseHttps("certificate.pfx");
    });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapRazorComponents<App>()
.AddInteractiveServerRenderMode();
app.MapRazorPages();
app.Run();
static void AddBlazorise(IServiceCollection services)
{
    services
        .AddBlazorise();
    services
        .AddBootstrapProviders()
        .AddFontAwesomeIcons();
}
