namespace DalekoSoft.Patterns.Repository;

public interface IUpdateModelRepository<TModel, TData>
{
    ValueTask<TData> UpdateAsync(TModel model);

    ValueTask<TData> UpdateAsync(TModel model, CancellationToken cancellationToken);
}
