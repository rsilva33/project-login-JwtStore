var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddDataBase();
builder.AddJwtAuthentication();

var app = builder.Build();
DatabaseManagementService.MigrationInitialization(app);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.Run();
