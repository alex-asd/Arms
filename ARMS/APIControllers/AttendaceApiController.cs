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
    [RoutePrefix("attendance")]
    public class AttendanceApiController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("has-attended-lecture")]
        public IHttpActionResult UserAttendedLecture(int lecture_id, int user_id, string bluetooth_address)
        {
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var attendance = dc.Attendees.Where(x => x.LectureID == lecture_id && x.UserID == user_id).Count();
                if (attendance == 1)
                {
                    return Ok();
                }

                return NotFound();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("attend")]
        public IHttpActionResult UserAttend(int lecture_id, int user_id, string bluetooth_address)
        {
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var existing_attendance = dc.Attendees
                    .Where(x => x.LectureID == lecture_id && x.BluetoothAddress == bluetooth_address).Count();
                if (existing_attendance >= 1)
                {
                    return BadRequest("This device was already used to check-in.");
                }

                var attendace = new Attendee(user_id, lecture_id, bluetooth_address);
                attendace.Insert(BonusEnum.UpsertType.Insert);

                return Ok();
            }
        }
    }
}