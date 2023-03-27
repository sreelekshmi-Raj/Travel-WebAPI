using FluentValidation;

namespace WalksAPI.Validators
{
    public class UpdateRegionValidator: AbstractValidator<Models.DTO.UpdateRegion>
    {
        public UpdateRegionValidator()
        {
            //these are custom or prebuilt validation available in fluentvalidation site
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThan(0);
            RuleFor(x=>x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
