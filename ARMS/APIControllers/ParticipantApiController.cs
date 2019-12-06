using ARMS.Data.Helpers;
using ARMS.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using ARMS.Data;
using ARMS.Helpers;

namespace ARMS.APIControllers
{
    [RoutePrefix("participants")]
    public class ParticipantApiController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("get-for-course")]
        public IHttpActionResult GetById(int id)
        {
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var results = dc.Participants.Include(x => x.User).Where(x => x.CourseID == id);
                return Ok(results.ToList());
            }
        }
    }
}