using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WCA.Web.Areas.API
{
    public class ValidateModelFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            base.OnActionExecuting(context);

            if (context.ModelState.IsValid == false)
            {
                context.Result = new BadRequestObjectResult(new ErrorViewModel(
                    "Some of the data provided is invalid.",
                    context.ModelState));
            }
        }
    }
}