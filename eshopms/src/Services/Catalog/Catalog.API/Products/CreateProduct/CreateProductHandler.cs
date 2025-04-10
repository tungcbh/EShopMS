namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(
        string Name, 
        List<string> Category, 
        string Description, 
        string ImageFile, 
        decimal Price)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandHandler(IDocumentSession sesssion)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //create product entity form command 
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };
            //save to DB?
            sesssion.Store(product);
            await sesssion.SaveChangesAsync(cancellationToken);
            //return result
            return new CreateProductResult(product.Id);
        }
    }
}
