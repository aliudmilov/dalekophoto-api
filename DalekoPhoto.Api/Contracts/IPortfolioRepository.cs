namespace DalekoPhoto.Api;

public interface IPortfolioRepository
{
    ValueTask<Album> ReadPortfolioAsync();

    ValueTask<Album> ReadPortfolioAsync(CancellationToken cancellationToken);
}
