using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleAPSViewer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();
builder.Services.Configure<ApsSettings>(builder.Configuration.GetSection("APS"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/api/token", async ([FromServices] IHttpClientFactory httpClientFactory, [FromServices] IOptions<ApsSettings> apsSettings) =>
{
    var client = httpClientFactory.CreateClient();
    var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = apsSettings.Value.TokenServerUrl,
        ClientId = apsSettings.Value.ClientId,
        ClientSecret = apsSettings.Value.ClientSecret,
        Scope = "viewables:read"
    });

    return new
    {
        response.AccessToken,
        response.ExpiresIn
    };
});

app.MapRazorPages();

app.Run();
