using ChatWebApp.Models.Comman;
using ChatWebApp.Models;
using ChatWebApp.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Routing;
using ChatWebApp.Services.Repository;

namespace ChatWebApp.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class HomeApiController : Controller
    {
        private readonly IHomeHelper _homeHelper;
        public HomeApiController(IHomeHelper homeHelper)
        {
            _homeHelper = homeHelper;
        }

        [Route("getUserList{UserId}")]
        [HttpGet]
        public IActionResult getUserListWithMessage(string UserId)
        {

            Response response = new();
            try
            {
                Guid gid = Guid.Parse(UserId);
                var userData = _homeHelper.GetUserList(gid);
                if (userData == null)
                {
                    response.code = StatusCodes.Status401Unauthorized;
                    response.status = false;
                    response.message = "Incorrect Login Details!";
                    return BadRequest(response);
                }
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

        [Route("getUserMessages")]
        [HttpPost]
        public IActionResult GetUserMessages(getUserMessage getUserMessage)
        {

            Response response = new();
            try
            {
                var userData = _homeHelper.GetUserMessages(getUserMessage);
                if (userData == null)
                {
                    response.code = StatusCodes.Status401Unauthorized;
                    response.status = false;
                    response.message = "Incorrect Login Details!";
                    return BadRequest(response);
                }
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

        [Route("insertMessage")]
        [HttpPost]
        public IActionResult InsertMessage(MessageModel messageModel)
        {
            Response response = new();
            try
            {
                _homeHelper.InsertMessage(messageModel);
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
