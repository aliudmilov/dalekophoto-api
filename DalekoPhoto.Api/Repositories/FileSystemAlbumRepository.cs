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

    public ValueTask<IReadOnlyList<Album>> ReadAsync(CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(null as IReadOnlyList<Album>);
    }

    public ValueTask<IReadOnlyList<Album>> ReadAsync(Expression<Func<Album, bool>> expression)
    {
        return ValueTask.FromResult(null as IReadOnlyList<Album>);
    }

    public ValueTask<IReadOnlyList<Album>> ReadAsync(Expression<Func<Album, bool>> expression, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(null as IReadOnlyList<Album>);
    }

    public ValueTask<Album> ReadPortfolioAsync()
    {
        return ReadPortfolioAsync(CancellationToken.None);
    }

    public ValueTask<Album> ReadPortfolioAsync(CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(null as Album);
    }
}
