using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ARMS.ViewModel
{
   
    
        public class LoginResponseVM
        {
            public LoginResponseVM()
            {

                this.Token = "";
                this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
            }

            public string Token { get; set; }
            public HttpResponseMessage responseMsg { get; set; }
        }
    
}