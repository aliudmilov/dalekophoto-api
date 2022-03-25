namespace DalekoPhoto.Api;

using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Linq.Expressions;

public class FileSystemAlbumRepository : IAlbumRepository, IPortfolioRepository
{
    private static readonly string PortfolioAlbumName = "Portfolio";
    private static readonly string SmallPhotosDirectoryName = "size-1920";
    private static readonly string LargePhotosDirectoryName = "size-2560";
    private static readonly char[] DirectoryNameLocationIdentifier = new char[] { '-' };
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMemoryCache _memoryCache;

    public FileSystemAlbumRepository(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public ValueTask<IReadOnlyList<Album>> ReadAsync()
    {
        return ReadAsync(CancellationToken.None);
    }

    public ValueTask<IReadOnlyList<Album>> ReadAsync(CancellationToken cancellationToken)
    {
        return ReadAsync(null, cancellationToken);
    }

    public ValueTask<IReadOnlyList<Album>> ReadAsync(Expression<Func<Album, bool>> expression)
    {
        return ReadAsync(expression, CancellationToken.None);
    }

    public async ValueTask<IReadOnlyList<Album>> ReadAsync(Expression<Func<Album, bool>> expression, CancellationToken cancellationToken)
    {
        List<Album> albums = await _memoryCache.GetOrCreateAsync(
            nameof(Album),
            cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
                cacheEntry.SlidingExpiration = TimeSpan.FromHours(12);

                return LoadAlbumsAsync(cancellationToken);
            });
        
        if (expression != null)
            return albums.Where(expression.Compile()).ToArray();

        return albums.ToArray();
    }

    public ValueTask<Album> ReadPortfolioAsync()
    {
        return ReadPortfolioAsync(CancellationToken.None);
    }

    public async ValueTask<Album> ReadPortfolioAsync(CancellationToken cancellationToken)
    {
        var porfolio = (await ReadAsync(x => 
            string.Equals(x.Title, PortfolioAlbumName, StringComparison.OrdinalIgnoreCase), cancellationToken))?.SingleOrDefault();
        porfolio.IsPorfolio = true;

        return porfolio;
    }

    private Task<List<Album>> LoadAlbumsAsync(CancellationToken cancellationToken)
    {
        string imagesRootPath = Environment.GetEnvironmentVariable(Constants.EnvKeyPhotoRootPath);
        if (!Directory.Exists(imagesRootPath))
        {
            throw new InvalidOperationException("Can't find the photo repositrory source");
        }
        
        cancellationToken.ThrowIfCancellationRequested();

        var albums = new List<Album>();
        foreach (var directoryPath in Directory.GetDirectories(imagesRootPath))
        {
            cancellationToken.ThrowIfCancellationRequested();

            string directoryName = new DirectoryInfo(directoryPath).Name;

            string smallImageDirectoryPath = Path.Combine(directoryPath, SmallPhotosDirectoryName);
            if (!Directory.Exists(smallImageDirectoryPath))
                continue;

            string largeImageDirectoryPath = Path.Combine(directoryPath, LargePhotosDirectoryName);

            var photos = new List<Photo>();
            foreach (var smallFilePath in Directory.GetFiles(smallImageDirectoryPath))
            {
                cancellationToken.ThrowIfCancellationRequested();

                string fileName = Path.GetFileName(smallFilePath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(smallFilePath);

                Photo photo = new Photo
                {
                    Id = GetPhotoId(directoryName, fileNameWithoutExtension),
                    SmallImageUrl = GetImageUrl(imagesRootPath, smallFilePath),
                };

                string largeFilePath = Path.Combine(largeImageDirectoryPath, fileName);
                if (File.Exists(largeFilePath))
                {
                    photo.LargeImageUrl = GetImageUrl(imagesRootPath, largeFilePath);
                }

                cancellationToken.ThrowIfCancellationRequested();

                photos.Add(photo);
            }

            albums.Add(new Album
            {
                Id = GetAlbumId(directoryName),
                Title = directoryName,
                Location = GetLocation(directoryName),
                Photos = photos.ToArray(),
            });
        }

        return Task.FromResult(albums);
    }

    private string GetAlbumId(string directoryName)
    {
        return directoryName.ToLowerInvariant();
    }

    private string GetLocation(string directoryName)
    {
        string[] parts = directoryName?.Split(DirectoryNameLocationIdentifier, StringSplitOptions.RemoveEmptyEntries);
        if (!(parts?.Length == 2))
            return null;

        if (parts[0] == parts[1])
            return parts[0];

        return $"{parts[1]}, {parts[0]}";
    }

    private string GetPhotoId(string directoryName, string fileNameWithoutExtension)
    {
        return $"{directoryName.ToLowerInvariant()}/{fileNameWithoutExtension.ToLowerInvariant()}";
    }

    private string GetImageUrl(string imagesRootPath, string imagePath)
    {
        string imageRelativePath = Path.GetRelativePath(imagesRootPath, imagePath).Replace('\\', '/');

        HttpRequest request = _httpContextAccessor.HttpContext.Request;

        return $"{request.Scheme}://{request.Host.Value}{Constants.PhotoRequestPath}/{imageRelativePath}";
    }
}
