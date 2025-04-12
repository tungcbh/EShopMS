using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketValidator()
        {
            RuleFor(x => x.Cart)
                .NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName)
                .NotEmpty().WithMessage("Username is required");
        }
        public class StoreBasketCommandHandler
            (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
            : ICommandHandler<StoreBasketCommand, StoreBasketResult>
        {
            public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
            {
                ShoppingCart cart = command.Cart;
                await DeductDiscount(cart, cancellationToken);

                //store cart in db - use Marten upsert
                await repository.StoreBasket(cart, cancellationToken); 
                //update cache

                return new StoreBasketResult(cart.UserName);
            }

            private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
            {
                //communicate with Discount.Grpc and calculate lastest prices
                foreach (var item in cart.Items)
                {
                    var coupon = await discountProto
                        .GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
                    item.Price -= coupon.Amount;
                }
            }
        }
    }
}
