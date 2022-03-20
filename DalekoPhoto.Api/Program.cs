using DalekoPhoto.Api;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAlbumRepository, FileSystemAlbumRepository>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(
        builder.Environment.ContentRootPath,
        Environment.GetEnvironmentVariable(Constants.EnvKeyPhotoRootPath))),
    RequestPath = Constants.PhotoRequestPath,
    OnPrepareResponse = ctx =>
    {
        // Add one week max-age of all images
        ctx.Context.Response.Headers.Append(
             "Cache-Control", $"public, max-age={(60 * 60 * 24 * 7).ToString()}");
    }
});

app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
