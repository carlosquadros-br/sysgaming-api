using System.Net.WebSockets;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SysgamingApi.Src.Application;
using SysgamingApi.Src.Application.Bets.Command;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Infrastructure;
using SysgamingApi.Src.Presentation;
using SysgamingApi.Src.Presentation.WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.
AddInfrastructureDI(builder.Configuration)
.AddApplicationDI(builder.Configuration);



builder.Services.AddControllers(options =>
{
    options.Filters.Add(new GlobalExceptionFilter());
}).AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
    options.RegisterValidatorsFromAssemblyContaining<CreateBetValidator>();

});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sysgaming Bets API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(1)
});

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
    {
        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var notificationService = context.RequestServices.GetRequiredService<INotifcationService>();
        await notificationService.CreateConnection(webSocket);
        await ReceiveMessages(webSocket, notificationService);
    }
    else
    {
        await next();
    }
});
app.MapControllers();

app.Run();

async Task ReceiveMessages(WebSocket webSocket, INotifcationService notificationService)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

    while (!result.CloseStatus.HasValue)
    {
        string message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
}
