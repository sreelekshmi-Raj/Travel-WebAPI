using FluentValidation;

namespace WalksAPI.Validators
{
    public class AddRegionValidator:AbstractValidator<Models.DTO.AddRegion>
    {
        //fluent validator for AddRegion in Region controller
        //code is not null ,not empty,not white space 
        public AddRegionValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThan(0);
            RuleFor(x=>x.Population).GreaterThanOrEqualTo(0);
        }

        //fluent validation is very readable and minimalistic 
    }
}
