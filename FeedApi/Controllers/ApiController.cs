using System;
using System.Net;
using FeedApi.Filters;
using FeedApi.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FeedApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        private readonly IRepository repository;

        public ApiController(IRepository repository)
        {
            this.repository = repository;

            #region Only for testing
            if (repository.IsEmpty)
            {
                repository.CreateFeedCollection(new FeedCollection { Title = "Test", UserId = 1 });
                repository.AddFeed(1, 1, "https://norman.walsh.name/atom/whatsnew.xml");
                repository.AddFeed(1, 1, "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml");
            }
            #endregion
        }

        /// <summary>
        /// List All Feed Collections by User ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/FeedCollections/1
        ///
        /// </remarks>
        /// <param name="userId">User ID</param>
        /// <response code="200">Returns User's Feed Collections</response>
        /// <response code="400">Api key is misssing</response>
        /// <response code="401">Access Denied. Invalid Api key</response>
        /// <response code="404">Feed Collections are empty</response>
        [HttpGet]
        [Route("api/{version:apiVersion}/[action]/{userId}")]
        [ValidateFeedCollectionsExists]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult FeedCollections(int userId)
        {
            try
            {
                var result = repository.GetUserFeedCollections(userId);
                return new ObjectResult(result);
            }
            catch (Exception e)
            {
                Log.Error(e, nameof(FeedCollections));
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// List All Feeds Collection by User ID and Feed Collections ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Feeds/1/1
        ///
        /// </remarks>
        /// <param name="userId">User ID</param>
        /// <param name="feedCollectionId">Feed Collections ID</param>
        /// <response code="200">Returns User's Feed Collections</response>
        /// <response code="400">Api key is misssing</response>
        /// <response code="401">Access Denied. Invalid Api key</response>
        /// <response code="404">Feeds Collection are empty</response>
        [HttpGet]
        [Route("api/{version:apiVersion}/[action]/{userId}/{feedCollectionId}")]
        [ValidateFeedsCollectionExists]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult Feeds(int userId, int feedCollectionId)
        {
            try
            {
                var result = repository.GetUserFeeds(userId, feedCollectionId);
                return new ObjectResult(result);
            }
            catch (Exception e)
            {
                Log.Error(e, nameof(FeedCollections));
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Create Feed Collection
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/CreateFeedCollection
        ///     {
        ///        "userId": 1,
        ///        "Title": "Some title"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Returns collection ID</response>
        /// <response code="400">Api key is misssing</response>
        /// <response code="401">Access Denied. Invalid Api key</response>
        /// <response code="409">Incorrect or empty body of request</response>
        /// <response code="500">Create Feed Collection unexpected error</response>
        [HttpPost]
        [Route("api/{version:apiVersion}/[action]")]
        [ValidateBodyRequest]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult CreateFeedCollection([FromBody]FeedCollection request)
        {
            try
            {
                int id = repository.CreateFeedCollection(request);
                return StatusCode((int)HttpStatusCode.Created, id);
            }
            catch (Exception e)
            {
                Log.Error(e, nameof(CreateFeedCollection));
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Create Feed Collection unexpected error. {e.Message}");
            }
        }

        /// <summary>
        /// Add New Feed
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/AddFeed
        ///     {
        ///        "userId": 1,
        ///        "feedCollectionId": 1,
        ///        "url": "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Returns collection ID</response>
        /// <response code="400">Api key is misssing</response>
        /// <response code="401">Access Denied. Invalid Api key</response>
        /// <response code="409">Incorrect or empty body of request</response>
        /// <response code="500">Add Feed unexpected error</response>
        [HttpPost]
        [Route("api/{version:apiVersion}/[action]")]
        [ValidateBodyRequest]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult AddFeed([FromBody]AddFeedRequest request)
        {
            try
            {
                int id = repository.AddFeed(request.UserId, request.FeedCollectionId, request.Url);
                return StatusCode((int)HttpStatusCode.Created, id);
            }
            catch (Exception e)
            {
                Log.Error(e, nameof(AddFeed));
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Add Feed unexpected error. {e.Message}");
            }
        }
    }
}
