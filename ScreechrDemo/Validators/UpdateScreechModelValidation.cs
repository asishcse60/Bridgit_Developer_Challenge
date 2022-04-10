using FluentValidation;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Contracts.Model;

namespace ScreechrDemo.Api.Validators
{
    public class UpdateScreechModelValidation:AbstractValidator<UpdateScreechModel>
    {
        public UpdateScreechModelValidation()
        {
            RuleFor(x => x.Text).NotEmpty().NotNull().MaximumLength(FieldLimit.MAX_CONTENT_SIZE);

        }
    }
}
