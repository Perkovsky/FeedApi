using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FeedApi.Models;

namespace FeedApi.Filters
{
    public class ValidateFeedsCollectionExistsAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var value = ((ObjectResult)context.Result).Value as IEnumerable<Feed>;
            if (!context.ModelState.IsValid || !value.Any())
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
