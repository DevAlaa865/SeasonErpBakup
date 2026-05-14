using BranchERP.Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BranchERP.Api.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value !.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value !.Errors.Select(e => new ErrorItem
                    {
                        Code = "ValidationError",
                        Message = e.ErrorMessage
                    }))
                    .ToList();

                var response = ApiResponse<object>.Fail("Validation failed", errors);

                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
