
namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid id):ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsDeleted);
    internal class DeleteProductCommandHandler
        (IDocumentSession session,ILogger<DeleteProductCommandHandler> logger)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productFromDb = await session.LoadAsync<Product>(request.id,cancellationToken);

            if (productFromDb is null)
                throw new ProductNotFoundException();

            session.Delete(productFromDb);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
