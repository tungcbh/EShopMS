using FluentValidation;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(bool IsSuccess);
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
            : ICommandHandler<StoreBasketCommand, StoreBasketResult>
        {
            public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
            {
                ShoppingCart cart = command.Cart;

                //store cart in db - use Marten upsert

                //update cache

                return new StoreBasketResult(true);
            }
        }
    }
}
