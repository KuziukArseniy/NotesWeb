using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using System.Reflection;
using Notes.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotesDbContext>();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll"); // ƒобавл€ем middleware дл€ обработки запросов

// »спользуем scope дл€ инициализации базы данных
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<NotesDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        // ќбработка ошибок инициализации базы данных
    }
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();