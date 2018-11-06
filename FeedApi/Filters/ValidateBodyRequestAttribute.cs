using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FeedApi.Filters
{
    public class ValidateBodyRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorDescription = new StringBuilder();
                errorDescription.AppendLine("Error(s):");
                foreach (var value in context.ModelState)
                {
                    foreach (var error in value.Value.Errors)
                    {
                        var msg = string.IsNullOrEmpty(error.ErrorMessage) ? error.Exception.Message : error.ErrorMessage;
                        errorDescription.AppendLine(msg);
                    }
                }
                
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Content = errorDescription.ToString()
                };
            }
        }
    }
}
