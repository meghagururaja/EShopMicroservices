
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand
        (Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product Name is required")
                .Length(2, 150).WithMessage("Product Name must be between 2 and 150 characters");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class UpdateProductCommandHandler
        (IDocumentSession session   )
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
             var productFromDb = await session.LoadAsync<Product>(request.Id, cancellationToken);

            if(productFromDb is null)
            {
                throw new ProductNotFoundException(request.Id);
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
