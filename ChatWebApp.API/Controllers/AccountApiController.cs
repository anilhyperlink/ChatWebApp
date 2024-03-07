using ChatWebApp.Models;
using ChatWebApp.Models.Comman;
using ChatWebApp.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ChatWebApp.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AccountApiController : Controller
    {
        private readonly IAccountHelper _accountHelper;
        public AccountApiController(IAccountHelper accountHelper)
        {
            _accountHelper = accountHelper;
        }

        [Route("signin")]
        [HttpPost]
        public IActionResult SignIn(SignInModel signInModel)
        {
            Response response = new();
            try
            {
                var userData = _accountHelper.SignIn(signInModel);
                if (userData == null)
                {
                    response.code = StatusCodes.Status401Unauthorized;
                    response.status = false;
                    response.message = "Incorrect Login Details!";
                    return BadRequest(response);
                }
                userData.Token = _accountHelper.GenerateToken(userData);
                response.code = StatusCodes.Status200OK;
                response.status = true;
                response.message = "Welcome to ChatApp!";
                response.data = userData;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = "Something Went Wrong." + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [Route("signup")]
        [HttpPost]
        public IActionResult Signup(SignUpModel signUpModel)
        {
            Response response = new();
            try
            {
                _accountHelper.SignUp(signUpModel);
                response.code = StatusCodes.Status200OK;
                response.status = true;
                response.data = "Registration Successful!";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = "Something Went Wrong." + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
