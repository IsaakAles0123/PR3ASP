using Microsoft.EntityFrameworkCore;
using PR1_ASP.Data;
using PR1_ASP.Models;

namespace PR1_ASP
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<SneakerShopContext>(options =>
                options.UseSqlServer(connectionString));

            var app = builder.Build();

            await using (var scope = app.Services.CreateAsyncScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SneakerShopContext>();
                await SneakerShopPricesBootstrap.ApplyDemoPriceFixesAsync(db).ConfigureAwait(false);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync().ConfigureAwait(false);
        }
    }
}