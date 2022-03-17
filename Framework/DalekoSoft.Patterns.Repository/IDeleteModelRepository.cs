namespace DalekoSoft.Patterns.Repository;

public interface IDeleteModelRepository<TModel, TData>
{
    ValueTask<TData> DeleteAsync(TModel model);

    ValueTask<TData> DeleteAsync(TModel model, CancellationToken cancellationToken);
}
