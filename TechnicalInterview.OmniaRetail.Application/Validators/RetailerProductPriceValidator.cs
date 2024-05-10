using FluentValidation;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Validators
{
    public class RetailerProductPriceValidator : AbstractValidator<RetailerProductPrice>
    {
        public RetailerProductPriceValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.RetailerId).NotEmpty();
        }
    }
}
