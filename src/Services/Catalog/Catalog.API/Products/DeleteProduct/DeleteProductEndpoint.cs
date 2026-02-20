
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.DeleteProduct
{

   // public record DeleteProductRequest()
   public record DeleteProductResponse(bool IsDeleted);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}",
               async (Guid id, ISender sender) =>
               {
                   var result = await sender.Send(new DeleteProductCommand(id));
                   var response = result.Adapt<DeleteProductResponse>();
                   return Results.Ok(response);

               })
               .WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Deletes Product")
                .WithDescription("Deletes product"); ;
        }
    }
}
