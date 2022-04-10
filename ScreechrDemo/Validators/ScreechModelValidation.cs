using FluentValidation;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Contracts.Model;

namespace ScreechrDemo.Api.Validators
{
    public class ScreechModelValidation : AbstractValidator<ScreechModel>
    {
        public ScreechModelValidation()
        {
            RuleFor(x => x.Content).NotEmpty().NotNull().MaximumLength(FieldLimit.MAX_CONTENT_SIZE);
        }
    }
}
