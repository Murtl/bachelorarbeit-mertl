using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Fügt Microsoft Identity Platform (AAD v2.0) Unterstützung hinzu, um diese API zu schützen
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAd", options);
            options.Events = new JwtBearerEvents();

            // Validiert, dass der Mandant der Anwendung mit dem Mandanten des ID-Tokens übereinstimmt
            options.Events.OnTokenValidated = async context =>
            {
                var tenantId = context.Principal.FindFirst("tid")?.Value;
                if (!string.IsNullOrEmpty(tenantId) && !tenantId.Equals(builder.Configuration["AzureAd:TenantId"]))
                {
                    throw new UnauthorizedAccessException("Tenant not authorized");
                }
                await Task.CompletedTask.ConfigureAwait(false);
            };  
        }, options => { builder.Configuration.Bind("AzureAd", options); });


// Fügt Cross-Origin Resource Sharing (CORS) Unterstützung hinzu, um die API von einer anderen Domäne aus zu erreichen
builder.Services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder
        .WithOrigins("https://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.AddControllers();
// Lernt mehr über die Konfiguration von Swagger/OpenAPI unter https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Konfiguriert die HTTP-Anforderungs-Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
