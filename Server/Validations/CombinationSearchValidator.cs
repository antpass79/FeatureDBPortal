using FeatureDBPortal.Shared;
using FluentValidation;

namespace FeatureDBPortal.Server.Validations
{
    public class CombinationSearchValidator : AbstractValidator<CombinationSearchDTO>
    {
        public CombinationSearchValidator()
        {
            //RuleFor(model => model.Model.Id)
            //    .NotNull();
            //RuleFor(model => model.Country.Id)
            //    .NotNull();
        }
    }
}
