using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARMS.Data;
using ARMS.Data.Helpers;
using ARMS.Data.Models;

namespace ARMS.Helpers
{
    public static class CurrentWebContext
    {
        private static User _CurrentUser { get; set; }
        public static User CurrentUser
        {
            get
            {
                var user = _CurrentUser;

                if (user == null)
                {
                    var email = HttpContext.Current.User.Identity.Name;

                    user = UserHelper.GetByEmail(email);
                    
                    _CurrentUser = user;
                }

                return user;
            }
            set
            {
                var user = value;
                _CurrentUser = user;
            }
        }
    }
}