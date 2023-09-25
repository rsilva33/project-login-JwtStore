namespace JwtStore.API.Extensions;

public static class DatabaseManagementService
{
  public static void MigrationInitialization(this IApplicationBuilder app)
  {
    using var serviceScope = app.ApplicationServices.CreateScope();

    var serviceDb = serviceScope.ServiceProvider.GetService<AppDbContext>();

    serviceDb.Database.Migrate();
  }
}