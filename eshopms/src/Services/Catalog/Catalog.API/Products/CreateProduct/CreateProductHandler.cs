﻿namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(
        string Name, 
        List<string> Category, 
        string Description, 
        string ImageFile, 
        decimal Price)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateProductCommandHandler.Handle callled");

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
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            //return result
            return new CreateProductResult(product.Id);
        }
    }
}
