var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi("MyOpenApiV1DocName"); 

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapOpenApi("/openapi/{documentName}/openapi.json");
// /openapi/MyOpenApiV1DocName/openapi.json

app.Run();
