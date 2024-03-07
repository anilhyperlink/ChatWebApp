using ChatWebApp.Models;
using ChatWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace ChatWebApp.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private readonly IManageService<string, UserListWithLastMessageModel> _getUserListWithMessage;
        private readonly IManageService<getUserMessage, List<UserListWithLastMessageModel>> _getUserMessages;
        private readonly IManageService<MessageModel, string> _insertUserMessage;

        public HomeController(IManageService<string, UserListWithLastMessageModel> getUserListWithMessage,
            IManageService<getUserMessage, List<UserListWithLastMessageModel>> getUserMessages,
            IManageService<MessageModel, string> insertUserMessage)
        {
            _getUserListWithMessage = getUserListWithMessage;
            _getUserMessages = getUserMessages;
            _insertUserMessage = insertUserMessage;
        }
        public IActionResult Index()
        {
            try
            {
                if (HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0)
                {
                    var tokenClaim = User.Claims.FirstOrDefault(c => c.Type == "Token");
                    var tokenValue = tokenClaim?.Value;
                    var token = new JwtSecurityToken(jwtEncodedString: tokenValue);

                    if (token == null)
                    {
                        return RedirectToAction("SignIn", "Account");
                    }
                    else
                    {
                        ViewData["UserName"] = token.Claims.FirstOrDefault(x => x.Type == "unique_name").Value;
                        ViewBag.UserId = token.Claims.FirstOrDefault(x => x.Type == "primarysid").Value;
                    }
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult LoadContacts()
        {
            try
            {
                if (HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0)
                {
                    return RedirectToAction("SignIn", "Account");
                }
                var tokenClaim = User.Claims.FirstOrDefault(c => c.Type == "Token");
                var tokenValue = tokenClaim?.Value;
                var token = new JwtSecurityToken(jwtEncodedString: tokenValue);
                string UserId = token.Claims.FirstOrDefault(x => x.Type == "primarysid").Value;

                var response = _getUserListWithMessage.GetAllByIdAsync("/api/v1/getUserList", UserId);
                if (response.Result != null)
                {
                    return PartialView("_UserList", response.Result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetUserMessages(string rId)
        {
            try
            {
                if (HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0)
                {
                    return RedirectToAction("SignIn", "Account");
                }
                var tokenClaim = User.Claims.FirstOrDefault(c => c.Type == "Token");
                var tokenValue = tokenClaim?.Value;
                var token = new JwtSecurityToken(jwtEncodedString: tokenValue);
                string sId = token.Claims.FirstOrDefault(x => x.Type == "primarysid").Value;
                getUserMessage getUserMessage = new();
                getUserMessage.SId = Guid.Parse(sId);
                getUserMessage.RId = Guid.Parse(rId);

                var response = _getUserMessages.PostAsync("/api/v1/getUserMessages", getUserMessage);
                if (response.Result != null)
                {
                    return PartialView("_MessageList", response.Result);
                    //return Ok(response.Result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult InsertMessage(MessageModel messageModel)
        {
            try
            {
                if (HttpContext.User.Claims == null || HttpContext.User.Claims.Count() == 0)
                {
                    return RedirectToAction("SignIn", "Account");
                }
                _insertUserMessage.PostAsync("/api/v1/insertMessage", messageModel);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
