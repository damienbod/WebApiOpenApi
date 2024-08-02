using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.UseTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "My API",
            Version = "v1",
            Description = "API for Damien"
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapOpenApi(); // /openapi/v1.json
app.MapOpenApi("/openapi/v1/openapi.json");
//app.MapOpenApi("/openapi/{documentName}/openapi.json");

app.Run();
