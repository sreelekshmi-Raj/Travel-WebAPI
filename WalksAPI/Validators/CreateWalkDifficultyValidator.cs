using FluentValidation;

namespace WalksAPI.Validators
{
    public class CreateWalkDifficultyValidator:AbstractValidator<Models.DTO.AddWalkDifficulty>
    {
        public CreateWalkDifficultyValidator()
        {
            RuleFor(x=>x.Code).NotEmpty();
        }
    }
}
