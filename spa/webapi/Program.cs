using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAd", options);
            options.Events = new JwtBearerEvents();

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


builder.Services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder
        .WithOrigins("https://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
