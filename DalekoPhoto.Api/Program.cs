using DalekoPhoto.Api;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAlbumRepository, FileSystemAlbumRepository>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
var dalekoPhotoPolicy = "DalekoPhotoPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(dalekoPhotoPolicy,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(
        builder.Environment.ContentRootPath,
        Environment.GetEnvironmentVariable(Constants.EnvKeyPhotoRootPath) ?? string.Empty)),
    RequestPath = Constants.PhotoRequestPath,
    OnPrepareResponse = ctx =>
    {
        // Add one week max-age of all images
        ctx.Context.Response.Headers.Append(
             "Cache-Control", $"public, max-age={(60 * 60 * 24 * 7).ToString()}");
    }
});
app.UseCors(dalekoPhotoPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();
