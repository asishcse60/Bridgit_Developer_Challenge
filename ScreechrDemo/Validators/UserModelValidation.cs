using FluentValidation;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Contracts.Model;

namespace ScreechrDemo.Api.Validators
{
    public class UserModelValidation : AbstractValidator<UserModel>
    {
        public UserModelValidation()
        {
            RuleFor(model => model.UserName).NotNull().NotEmpty().MaximumLength(FieldLimit.MAX_USER_NAME_LENGTH).Must(fv => !string.IsNullOrWhiteSpace(fv));
            RuleFor(model => model.SecretToken).NotNull().NotEmpty().Length(FieldLimit.SECRET_TOKEN_MIN_LENGTH, FieldLimit.EXACT_TOKEN_LENGTH)
                .Must(fv => !string.IsNullOrWhiteSpace(fv));
        }
    }
}
