using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiOpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer("Bearer", options =>
   {
       options.Audience = "api_scope";
       options.Authority = "https://localhost:44367";
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateIssuerSigningKey = true,
           ValidAudiences = ["api_scope"],
           ValidIssuers = ["https://localhost:44367"],
       };
   });

builder.Services.AddOpenApi(options =>
{
    //options.UseTransformer((document, context, cancellationToken) =>
    //{
    //    document.Info = new()
    //    {
    //        Title = "My API",
    //        Version = "v1",
    //        Description = "API for Damien"
    //    };
    //    return Task.CompletedTask;
    //});
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

// Open up security restrictions to allow this to work
// Not recommended in production
//var deploySwaggerUI = app.Environment.IsDevelopment();
var deploySwaggerUI = app.Configuration.GetValue<bool>("DeploySwaggerUI");

app.UseSecurityHeaders(
    SecurityHeadersDefinitions.GetHeaderPolicyCollection(deploySwaggerUI));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//app.MapOpenApi(); // /openapi/v1.json
app.MapOpenApi("/openapi/v1/openapi.json");
//app.MapOpenApi("/openapi/{documentName}/openapi.json");

if (deploySwaggerUI)
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1/openapi.json", "v1");
    });
}

app.Run();

internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // "bearer" refers to the header name here
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;
        }
        document.Info = new()
        {
            Title = "My API Bearer scheme",
            Version = "v1",
            Description = "API for Damien"
        };
    }
}