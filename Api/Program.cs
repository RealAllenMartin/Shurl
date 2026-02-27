using System.Security.Cryptography;
using System.Text;
using Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UrlDb>(opt => opt.UseInMemoryDatabase("UrlList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddOpenApi();
var app = builder.Build();

// TODO: Add authentication so only site/project admins can run "/AllUrls"
app.MapGet("/AllUrls", async (UrlDb db) =>
    await db.Urls.ToListAsync());

// System gets shortUrl and redirects to longUrl
// TODO: Update visits by one when called
app.MapGet("/{shortUrl}", async (string shortUrl, UrlDb db) =>
{
    var url = await db.Urls.FirstOrDefaultAsync(u => u.shortUrl == shortUrl);

    return url is not null
        ? Results.Ok(url)
        : Results.NotFound();
});

// Hashs longUrl and outputs shortUrl
// TODO: Require urlLong to resolve possible null input
app.MapPost("/CreateShortUrl", async (Url url, UrlDb db) =>
{
    byte[] urlByte = Encoding.UTF8.GetBytes(url.longUrl);
    byte[] hashedUrl = SHA256.HashData(urlByte);
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < 6; i++)
        sb.Append(hashedUrl[i].ToString("X2"));
    string shortUrl = sb.ToString();
    bool exists = await db.Urls.AnyAsync(e => e.shortUrl == shortUrl);

    //if (!exists)
    //{
        url.shortUrl = shortUrl;
        db.Urls.Add(url);
        await db.SaveChangesAsync();

        return Results.Created($"/{url.shortUrl}", url);
    //}
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Swagger"));

app.Run();