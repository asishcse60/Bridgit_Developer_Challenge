using FluentValidation;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Contracts.Model;

namespace ScreechrDemo.Api.Validators
{
    public class PageModelValidation:AbstractValidator<PageModel>
    {
        public PageModelValidation()
        {
            RuleFor(x => x.PageSize).Must(ValidateMaxPageSize);
            RuleFor(x => x.PageNo).Must(ValidateValidPageNo);
            RuleFor(x => x.SortDir).Must(ValidateSortDirection);
        }

        #region Private method for validation

        private bool ValidateMaxPageSize(int? pageSize)
        {
            if (!pageSize.HasValue)
                return true;
            return pageSize <= FieldLimit.MAXIMUM_PAGE_SIZE;
        }

        private bool ValidateValidPageNo(int? page)
        {
            if (!page.HasValue)
                return true;
            return page >= 1;
        }

        private bool ValidateSortDirection(string sortDir)
        {
            if (string.IsNullOrEmpty(sortDir))
                return true;
            return sortDir.Equals(FieldLimit.SortAsc) || sortDir.Equals(FieldLimit.SortDsc);
        }

        #endregion

    }
}
