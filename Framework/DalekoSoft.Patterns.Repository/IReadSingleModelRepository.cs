using System.Linq.Expressions;

namespace DalekoSoft.Patterns.Repository;

public interface IReadSingleModelRepository<TModel>
{
    ValueTask<TModel> ReadSingleAsync();

    ValueTask<TModel> ReadSingleAsync(CancellationToken cancellationToken);

    ValueTask<TModel> ReadSingleAsync(Expression<Func<TModel, bool>> expression);

    ValueTask<TModel> ReadSingleAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken);
}
