using FluentValidation;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Validators
{
    public class RetailerValidator : AbstractValidator<Retailer>
    {
        public RetailerValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
