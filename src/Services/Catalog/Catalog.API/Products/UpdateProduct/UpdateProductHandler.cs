
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand
        (Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    internal class UpdateProductCommandHandler
        (IDocumentSession session,ILogger<UpdateProductCommandHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"UpdateProductCommandHandler.Handle called with {request}");
            var productFromDb = await session.LoadAsync<Product>(request.Id, cancellationToken);

            if(productFromDb is null)
            {
                throw new ProductNotFoundException();
            }

            productFromDb.Name = request.Name;
            productFromDb.Category = request.Category;
            productFromDb.Description = request.Description;
            productFromDb.ImageFile = request.ImageFile;
            productFromDb.Price = request.Price;

            session.Update(productFromDb);

            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
    }
}
