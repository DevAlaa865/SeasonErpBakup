using FluentValidation;

namespace BranchERP.Application.DTOs.City
{
    public class CityCreateUpdateDtoValidator : AbstractValidator<CityCreateUpdateDto>
    {
        public CityCreateUpdateDtoValidator()
        {
            RuleFor(x => x.CityName)
                .NotEmpty().WithMessage("CityName is required")
                .MinimumLength(3).WithMessage("CityName must be at least 3 characters")
                .MaximumLength(100).WithMessage("CityName must not exceed 100 characters");

            RuleFor(x => x.RegionId)
                .GreaterThan(0).WithMessage("RegionId must be greater than 0");
        }
    }
}
