﻿using ARMS.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ARMS.Controllers
{
    [RoutePrefix("auth")]
    public class JwtController : ApiController
    {

        [HttpGet]
        [Authorize]
        [Route("ok")]
        public IHttpActionResult Authenticated() => Ok("Authenticated");
        
        
        [HttpGet]
        [Authorize]
        [Route("user")]
        public IHttpActionResult GetUser()
        {
            var claims = ClaimsPrincipal.Current.Claims;
            var possible_user_emails = from Claim claim in claims where claim.Type == ClaimTypes.Email
                             select claim.Value;
            var user_emails = possible_user_emails.ToList();
            if(user_emails.Count > 1)
            {
                //More than 1 users found with this claim, error.
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Conflict));
            }

           
            return Ok<string>(user_emails[0]);
        }


        [HttpPost]
        [Route("login")]
        public IHttpActionResult Authenticate([FromBody] LoginVM loginVM)
        {
            if(loginVM == null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

         


            var loginResponse = new LoginResponseVM();
            var loginrequest = new LoginVM
            {
                Email = loginVM.Email.ToLower(),
                Password = loginVM.Password
            };

            var isUsernamePasswordValid = false;
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userExists = userManager.FindByEmailAsync(loginrequest.Email).Result;
            if (userExists != null)
            {
                var specificUser = userManager.FindAsync(userExists.UserName, loginrequest.Password).Result;
                if(specificUser != null)
                {
                    isUsernamePasswordValid = true;
                }
            }

            // if credentials are valid

            if (isUsernamePasswordValid)
            {
                var token = CreateToken(loginrequest.Email);
                //return the token
                return Ok(token);
            }
            // if credentials are not valid send unauthorized status code in response
            loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
            IHttpActionResult response = ResponseMessage(loginResponse.responseMsg);
            return response;
        }


        private string CreateToken(string email)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(30);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email)
            });

            const string secrectKey = "your secret key goes here";
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secrectKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            //Create the jwt (JSON Web Token)
            //Replace the issuer and audience with your URL (ex. http:localhost:12345)
            var token =
                (JwtSecurityToken)
                tokenHandler.CreateJwtSecurityToken(
                    issuer: "http://localhost:12345/",
                    audience: "http://localhost:12345/",
                    subject: claimsIdentity,
                    notBefore: issuedAt,
                    expires: expires,
                    signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
