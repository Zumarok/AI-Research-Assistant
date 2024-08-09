using BlazorFrontend.Client.Pages;
using BlazorFrontend.Components;

namespace BlazorFrontend;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        
        // Register HttpClient
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.WebHost.GetSetting("baseUrl")) });

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
       
        app.UseHttpsRedirection(); // Existing line

        app.UseStaticFiles(); // Ensure this is only called once

        app.UseRouting(); // Existing line

        app.UseAntiforgery(); // Existing line

        app.UseEndpoints(endpoints =>
        {
            // Ensure no duplicate mappings here
            endpoints.MapRazorComponents<App>().AddInteractiveServerRenderMode();
            // Remove any duplicate or conflicting endpoint mappings
        });
        app.Run();
    }
}
