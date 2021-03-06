using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Only for testing purposes, handling error requests
    /// </summary>
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        ///<summary>
        /// Authorization is required
        ///</summary>
        ///<response code="200">If user is authorized</response>
        ///<response code="401">If user is not authorized</response>
        [Produces("application/json","text/plain")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("testauth")]
        public ActionResult<string> GetSecretText()
        {
            return "secret text";
        }

        ///<summary>
        /// To return Not Found error
        ///</summary>
        ///<response code="404"></response>
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(999);

            if (thing == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok();
        }

        ///<summary>
        /// To return Server Error error
        ///</summary>
        ///<response code="500"></response>
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status500InternalServerError)]
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(999);

            var thingToReturn = thing.ToString();   //generates exception

            return Ok();
        }

        ///<summary>
        /// To return Bad Request error
        ///</summary>
        ///<response code="400"></response>
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
    }
}