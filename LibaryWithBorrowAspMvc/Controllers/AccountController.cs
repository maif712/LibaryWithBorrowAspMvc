using Domain.Common;
using LibaryWithBorrowAspMvc.Core.Interfaces;
using LibaryWithBorrowAspMvc.Extension;
using LibaryWithBorrowAspMvc.Models.Dtos.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace LibaryWithBorrowAspMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                this.AddError("All Field are requied!");
                return View(dto);
            }

            try
            {
                var result = await _accountService.RegisterAsync(dto);
                if (!result.Success)
                {
                    this.AddError(result.ErrorMessage!);
                    return View(dto);
                }



                this.AddSuccess("Registerd Successfully!");
                return RedirectToAction("Login");
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var (result, principal) = await _accountService.LoginAsync(dto);

            if (!result.Success)
            {
                this.AddError(result.ErrorMessage!);
                return View(dto);
            }

            try
            {
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = dto.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal!, authProperties);
                this.AddSuccess("Loged in successfully!");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }

            
        }

        [HttpPost, ActionName("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                var result = await _accountService.LogoutAsync();

                if (result.Success)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    this.AddSuccess("Loggedout successfully.");
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                this.AddError("Something went wrong while logging out.");

                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
