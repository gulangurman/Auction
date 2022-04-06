using Auction.Ordering.Models;
using FluentValidation;

namespace Auction.Ordering.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDTO>
    {
        public CreateOrderValidator()
        {
            RuleFor(v => v.SellerUserName).EmailAddress().NotEmpty();
            RuleFor(v => v.ProductId).NotEmpty();
        }
    }
}
