namespace DalekoSoft.Patterns.Repository;

public interface ICreateModelRepository<TModel, TData>
{
    ValueTask<TData> CreateAsync(TModel model);

    ValueTask<TData> CreateAsync(TModel model, CancellationToken cancellationToken);
}
