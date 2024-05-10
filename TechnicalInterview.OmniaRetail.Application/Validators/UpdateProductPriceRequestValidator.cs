using FluentValidation;
using TechincalInterview.OmniaRetail.Contracts.Requests;

namespace TechnicalInterview.OmniaRetail.Application.Validators
{
    public class UpdateProductPriceRequestValidator : AbstractValidator<UpdateProductPriceRequest>
    {
        public UpdateProductPriceRequestValidator()
        {
            RuleFor(r => r.Price).LessThan(1000000m);
            RuleFor(r => r.Price).PrecisionScale(2, 10, true);
        }
    }
}
