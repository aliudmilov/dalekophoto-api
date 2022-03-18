namespace DalekoPhoto.Api;

using System.Linq.Expressions;

public class FileSystemAlbumRepository : IAlbumRepository, IPortfolioRepository
{
    public FileSystemAlbumRepository()
    {
    }

    public ValueTask<IReadOnlyList<Album>> ReadAsync()
    {
        return ReadAsync(CancellationToken.None);
    }

    public async ValueTask<IReadOnlyList<Album>> ReadAsync(CancellationToken cancellationToken)
    {
        return new List<Album>();
    }

    public async ValueTask<IReadOnlyList<Album>> ReadAsync(Expression<Func<Album, bool>> expression)
    {
        return new List<Album>();
    }

    public async ValueTask<IReadOnlyList<Album>> ReadAsync(Expression<Func<Album, bool>> expression, CancellationToken cancellationToken)
    {
        return new List<Album>();
    }

    public ValueTask<Album> ReadPortfolioAsync()
    {
        return ReadPortfolioAsync(CancellationToken.None);
    }

    public async ValueTask<Album> ReadPortfolioAsync(CancellationToken cancellationToken)
    {
        return new Album();
    }
}
