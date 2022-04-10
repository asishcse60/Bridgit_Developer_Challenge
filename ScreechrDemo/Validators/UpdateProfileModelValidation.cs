using System.Text.RegularExpressions;
using FluentValidation;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Contracts.Model;

namespace ScreechrDemo.Api.Validators
{
    public class UpdateProfileModelValidation : AbstractValidator<UpdateProfileModel>
    {
        public UpdateProfileModelValidation()
        {
            RuleFor(model => model.FirstName).NotNull().NotEmpty().MaximumLength(FieldLimit.MAX_FIRST_NAME_LENGTH).Must(fv => !string.IsNullOrWhiteSpace(fv));
            RuleFor(model => model.LastName).NotNull().NotEmpty().MaximumLength(FieldLimit.MAX_LAST_NAME_LENGTH).Must(fv => !string.IsNullOrWhiteSpace(fv));
            RuleFor(model => model.ProfileImageUri).Must(UriValidate);

        }
        private bool UriValidate(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return true;
            }
            return Regex.IsMatch(uri, FieldLimit.ImageUrlRegexPattern);
        }
    }
}
