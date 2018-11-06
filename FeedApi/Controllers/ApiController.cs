using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FeedApi.Models;
using FeedApi.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace FeedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IRepository repository;

        public ApiController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Feeds(int userId)
        {
            try
            {
                var result = repository.GetUserFeedCollections(userId);
                return new ObjectResult(result);
            }
            catch (Exception e)
            {
                Log.Error(e, nameof(Feeds));
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult Feeds(int userId, int id)
        {
            try
            {
                var result = repository.GetUserFeedCollection(userId, id);
                return new ObjectResult(result);
            }
            catch (Exception e)
            {
                Log.Error(e, nameof(Feeds));
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateFeedCollection([FromBody]FeedCollection item)
        {
            try
            {
                int id = repository.CreateFeedCollection(item);
                return StatusCode((int)HttpStatusCode.Created, id);
            }
            catch (Exception e)
            {
                Log.Error(e, nameof(CreateFeedCollection));
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
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
                return BadRequest(e.Message);
            }
        }
    }
}
