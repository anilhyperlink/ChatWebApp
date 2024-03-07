using ChatWebApp.Models;
using ChatWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System;

namespace ChatWebApp.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly IManageService<SignUpModel, string> _userRegistration;
        private readonly IManageService<SignInModel, UserModel> _signInUser;
        public AccountController
            (
                IManageService<SignUpModel, string> userRegistration,
                IManageService<SignInModel, UserModel> signInUser
            )
        {
            _userRegistration = userRegistration;
            _signInUser = signInUser;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            try
            {
                if (HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0)
                {
                    var tokenClaim = User.Claims.FirstOrDefault(c => c.Type == "Token");
                    var tokenValue = tokenClaim?.Value;
                    var token = new JwtSecurityToken(jwtEncodedString: tokenValue);

                    if (token != null)
                    {
                        return RedirectToAction("Index", "Home");
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            try
            {
                TryValidateModel(signInModel);
                if (ModelState.IsValid)
                {
                    var response = _signInUser.PostAsync("api/v1/signin", signInModel);
                    if (response.Result != null)
                    {
                        if (!string.IsNullOrEmpty(response.Result.Token))
                        {
                            var claims = new List<Claim>()
                                            {
                                                new Claim("Token", response.Result.Token),
                                                new Claim("UserId", response.Result.UserId.ToString()),
                                            };
                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                            {
                                IsPersistent = true
                            });
                            HttpContext.Session.SetString("Token", response.Result.Token);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    return View(signInModel);
                }
                return View(signInModel);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp()
        {
            try
            {
                if (HttpContext.User.Claims != null && HttpContext.User.Claims.Count() > 0)
                {
                    var tokenClaim = User.Claims.FirstOrDefault(c => c.Type == "Token");
                    var tokenValue = tokenClaim?.Value;
                    var token = new JwtSecurityToken(jwtEncodedString: tokenValue);

                    if (token != null)
                    {
                        return RedirectToAction("Index", "Home");
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

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignUp(SignUpModel signUpModel)
        {
            try
            {
                TryValidateModel(signUpModel);
                if (ModelState.IsValid)
                {
                    _userRegistration.PostAsync("api/v1/signup", signUpModel);
                    return RedirectToAction("SignIn", "Account");
                }
                return View(signUpModel);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignOut()
        {
            try
            {
                // Clear session
                HttpContext.Session.Clear();

                // Delete cookies
                Response.Cookies.Delete("Token");
                Response.Cookies.Delete(".AspNetCore.Session");

                // Redirect the user to the Signin page or any other desired page
                return RedirectToAction("SignIn");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
