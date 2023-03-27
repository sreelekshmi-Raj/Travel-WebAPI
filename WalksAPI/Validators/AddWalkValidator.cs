using FluentValidation;

namespace WalksAPI.Validators
{
    public class AddWalkValidator: AbstractValidator<Models.DTO.AddWalk>
    {
        public AddWalkValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x=>x.Length).GreaterThanOrEqualTo(0);
            //implement validations for forign keys is bit difficult
        }
    }
}
