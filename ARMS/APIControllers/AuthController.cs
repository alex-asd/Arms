﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ARMS.Data.Helpers;
using ARMS.Data.Models;
using ARMS.Models;
using ARMS.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Tokens;

namespace ARMS.APIControllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("ok")]
        public IHttpActionResult Authenticated() => Ok("Authenticated");

        [HttpPost]
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteUser()
        {
            var claims = ClaimsPrincipal.Current.Claims;
            var user_email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userExists = userManager.FindByEmailAsync(user_email.Value).Result;
            if (userExists != null)
            {
                userManager.Delete(userExists);
                var dbUser = UserHelper.GetByEmail(user_email.Value);
                dbUser.Delete();
                return Ok(new ApiCallbackMessage("Success", true));
            }

            return BadRequest();
        }


        [HttpPost]
        [Authorize]
        [Route("edit")]
        public IHttpActionResult EditUser([FromBody] User user)
        {
            var claims = ClaimsPrincipal.Current.Claims;
            var user_email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (user_email.Value != user.Email)
            {
                return Unauthorized();
            }

            var success = user.Update();
            if (success)
            {
                return Ok(new ApiCallbackMessage("Success", true));
            }

            return BadRequest();
        }


        [HttpGet]
        [Authorize]
        [Route("user")]
        public IHttpActionResult GetUser()
        {
            var claims = ClaimsPrincipal.Current.Claims;
            var user_email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var user_name = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (user_email == null || user_name == null)
            {
                return Unauthorized();
            }

            var user = UserHelper.GetByEmail(user_email.Value);
            return Ok<User>(user);
        }


        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register([FromBody] RegistrationVM registrationVm)
        {
            if (registrationVm == null)
            {
                return BadRequest("Oops, something went wrong");
            }

            var email = registrationVm.Email;
            var password = registrationVm.Password;
            var type = registrationVm.TypeOfUser;
            if (email == null || password == null || type == null)
            {
                return BadRequest("Request parameters not met");
            }

            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser
                {UserName = registrationVm.Email, Email = registrationVm.Email, TypeOfUser = registrationVm.TypeOfUser};
            var data_user = new User(registrationVm.FirstName, registrationVm.LastName, registrationVm.Email,
                registrationVm.TypeOfUser);
            data_user.Upsert();
            var result = await userManager.CreateAsync(user, registrationVm.Password);
            if (result.Succeeded)
            {
                //Supplying token for automatic auth.
                var token = CreateToken(user.Email, user.UserName);
                return Ok(token);
            }
            else
            {
                return BadRequest(String.Join(";", result.Errors));
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IHttpActionResult Authenticate([FromBody] LoginVM loginVM)
        {
            if (loginVM == null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            
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
                if (specificUser != null)
                {
                    loginrequest.UserName = specificUser.UserName;
                    isUsernamePasswordValid = true;
                }
            }

            // if credentials are valid

            if (isUsernamePasswordValid)
            {
                var token = CreateToken(loginrequest.Email, loginrequest.UserName);
                //return the token
                return Ok(token);
            }

            // if credentials are not valid send unauthorized status code in response
            return Unauthorized();
        }


        private string CreateToken(string email, string userName)
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
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userName)
            });

            const string secrectKey = "your secret key goes here";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secrectKey));
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