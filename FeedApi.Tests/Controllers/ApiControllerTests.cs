using Xunit;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FeedApi.Models;
using FeedApi.Controllers;

namespace FeedApi.Tests.Controllers
{
    /// <summary>
    /// For example I implemented a couple actions tests
    /// </summary>
    [Trait("Controllers", "Api")]
    public class ApiControllerTests
    {
        [Fact]
        public void FeedCollections_ReturnsResponseModel_Success()
        {
            // Arrange
            var feedCollections = new List<FeedCollection>
            {
                new FeedCollection { Id = 1, UserId = 1, Title = "Test1" },
                new FeedCollection { Id = 2, UserId = 1, Title = "Test2" },
                new FeedCollection { Id = 3, UserId = 1, Title = "Test3" }
            };
            var mock = new Mock<IRepository>();
            mock.Setup(m => m.GetUserFeedCollections(1)).Returns(feedCollections);

            var target = new ApiController(mock.Object);

            // Act
            var result = target.FeedCollections(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<FeedCollection>>(objectResult.Value);
            mock.Verify(m => m.GetUserFeedCollections(1));
            Assert.Equal(3, model.Count());
            Assert.NotNull(model.FirstOrDefault(i => i.Title == "Test2"));
        }

        [Fact]
        public void CreateFeedCollection_ReturnsInt_Success()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(m => m.CreateFeedCollection(It.IsAny<FeedCollection>())).Returns(7);

            var target = new ApiController(mock.Object);

            // Act
            var result = target.CreateFeedCollection(new FeedCollection
            {
                UserId = 1,
                Title = "Test1"
            });

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<int>(objectResult.Value);
            Assert.Equal((int)HttpStatusCode.Created, objectResult.StatusCode);
            mock.Verify(m => m.CreateFeedCollection(It.IsAny<FeedCollection>()));
            Assert.Equal(7, model);
        }

        [Fact]
        public void CreateFeedCollection_ReturnsStatusCodeInternalServerError_Failed()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(m => m.CreateFeedCollection(It.IsAny<FeedCollection>())).Throws<ArgumentException>();

            var target = new ApiController(mock.Object);

            // Act
            var result = target.CreateFeedCollection(new FeedCollection());

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }
    }
}
