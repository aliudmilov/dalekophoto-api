using System.Linq.Expressions;

namespace DalekoSoft.Patterns.Repository;

public interface IReadModelRepository<TModel>
{
    ValueTask<IReadOnlyList<TModel>> ReadAsync();

    ValueTask<IReadOnlyList<TModel>> ReadAsync(CancellationToken cancellationToken);

    ValueTask<IReadOnlyList<TModel>> ReadAsync(Expression<Func<TModel, bool>> expression);

    ValueTask<IReadOnlyList<TModel>> ReadAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken);
}
