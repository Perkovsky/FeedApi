using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Xunit;
using Moq;
using FeedApi.Filters;

namespace FeedApi.Tests.Filters
{
    [Trait("Filters", "ValidateBodyRequestAttribute")]
    public class ValidateBodyRequestAttributeTests
    {
        [Fact]
        public void OnActionExecuting_NoReturns_ModelStateIsValid()
        {
            // Arrange
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ActionDescriptor());
            var context = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), null);
            var target = new ValidateBodyRequestAttribute();

            // Act
            target.OnActionExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }

        [Fact]
        public void OnActionExecuting_NoReturns_ModelStateIsNotValid()
        {
            // Arrange
            string err1 = "Error N1";
            string err2 = "Error N2";
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ActionDescriptor());
            var context = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), null);
            context.ModelState.AddModelError("Error1", err1);
            context.ModelState.AddModelError("Error1", err2);
            var target = new ValidateBodyRequestAttribute();

            // Act
            target.OnActionExecuting(context);
            string result = $"Error(s):\r\n{err1}\r\n{err2}\r\n";

            // Assert
            Assert.IsType<ContentResult>(context.Result);
            Assert.Equal((int)HttpStatusCode.Conflict, ((ContentResult)context.Result).StatusCode);
            Assert.Equal(result, ((ContentResult)context.Result).Content);
        }
    }
}
