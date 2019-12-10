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
using ARMS.ViewModel;

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

        [HttpGet]
        [Authorize]
        [Route("is-participant")]
        public IHttpActionResult GetById(int course_id, int user_id)
        {
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var results = dc.Participants.Count(x => x.CourseID == course_id && x.UserID == user_id);
                if (results == 1)
                {
                    return Ok(new ApiCallbackMessage("Success", true));
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        [Authorize]
        [Route("apply")]
        public IHttpActionResult ApplyParticipant(int course_id, int user_id)
        {
            using (var dc = new ArmsContext())
            {
                //TODO: Add check if exists
                Participant part = new Participant(user_id, course_id, "pending");
                part.Insert();
                return Ok(new ApiCallbackMessage("Success", true));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("accept")]
        public IHttpActionResult AcceptParticipant([FromBody] Participant participant)
        {
            using (var dc = new ArmsContext())
            {
                //todo: add check if exists
                var db_part = dc.Participants.FirstOrDefault(x => x.ParticipantID == participant.ParticipantID);
                db_part.ParticipantStatus = "active";
                db_part.Update();
                return Ok(new ApiCallbackMessage("Success", true));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("decline")]
        public IHttpActionResult DeclineParticipant([FromBody] Participant participant)
        {
            using (var dc = new ArmsContext())
            {
                var db_part = dc.Participants.FirstOrDefault(x => x.ParticipantID == participant.ParticipantID);
                db_part.ParticipantStatus = "active";
                db_part.Delete();
                return Ok(new ApiCallbackMessage("Success", true));
            }
        }
    }
}