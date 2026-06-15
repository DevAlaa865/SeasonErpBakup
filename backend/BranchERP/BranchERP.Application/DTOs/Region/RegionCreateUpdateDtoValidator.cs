using FluentValidation;

namespace BranchERP.Application.DTOs.Region
{
    public class RegionCreateUpdateDtoValidator : AbstractValidator<RegionCreateUpdateDto>
    {
        public RegionCreateUpdateDtoValidator()
        {
            RuleFor(x => x.RegionName)
                .NotEmpty().WithMessage("RegionName is required")
                .MinimumLength(3).WithMessage("RegionName must be at least 3 characters")
                .MaximumLength(100).WithMessage("RegionName must not exceed 100 characters");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("CountryId must be greater than 0");
        }
    }
}
