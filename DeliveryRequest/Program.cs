using DeliveryRequest.DBService;
using DeliveryRequest.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;

try
{

    CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

    var builder = WebApplication.CreateBuilder(args);

    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connection));
    builder.Services.AddSingleton<IDBInterface, DbService>();

    var serverIP = (string?)builder.Configuration.GetSection("WebProtocolSettings").GetValue(typeof(string), "Url");
    var serverPort = (int)(builder.Configuration.GetSection("WebProtocolSettings").GetValue(typeof(int), "Port") ?? 5000);

    if (serverIP == null)
    {
        builder.WebHost.ConfigureKestrel((context, serverOptions) =>
        {
            serverOptions.Listen(IPAddress.Loopback, serverPort);
        });
    }
    else
    {
        builder.WebHost.ConfigureKestrel((context, serverOptions) =>
        {
            serverOptions.Listen(IPAddress.Parse(serverIP), serverPort);
        });
    }

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch
{
    Console.WriteLine("Что-то совсем пошло не так");
}