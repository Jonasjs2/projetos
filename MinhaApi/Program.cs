var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Garante HTTPS na porta 5250
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5250, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();

app.MapPost("/login", (LoginRequest login) =>
{
    if (login.Username == "admin" && login.Password == "123456")
        return Results.Ok(new { token = "fake-jwt-token" });
    return Results.Unauthorized();
});

// Adiciona suporte explícito ao preflight
app.MapMethods("/login", new[] { "OPTIONS" }, () => Results.Ok());

app.Run();

public record LoginRequest(string Username, string Password);