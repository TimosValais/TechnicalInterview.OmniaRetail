using FluentValidation;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Validators
{
    public class ProductGroupValidator : AbstractValidator<ProductGroup>
    {
        public ProductGroupValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
