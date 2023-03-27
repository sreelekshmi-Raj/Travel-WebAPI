using FluentValidation;

namespace WalksAPI.Validators
{
    public class UpdateWalkDifficultyValidator:AbstractValidator<Models.DTO.UpdateWalkDifficulty>
    {
        public UpdateWalkDifficultyValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
