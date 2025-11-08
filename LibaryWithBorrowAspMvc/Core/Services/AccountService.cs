using Domain.Common;
using LibaryWithBorrowAspMvc.Core.Interfaces;
using LibaryWithBorrowAspMvc.Data;
using LibaryWithBorrowAspMvc.Models.Dtos.User;
using LibaryWithBorrowAspMvc.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibaryWithBorrowAspMvc.Core.Services
{
    public class AccountService : IAccountService
    {
		private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountService(ApplicationDbContext context, PasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResult> RegisterAsync(RegisterUserDto dto)
        {
            var isUserExists = await _context.Users.AnyAsync(user => user.UserName == dto.UserName || user.Email == dto.Email);
            if (isUserExists)
            {
                var response = new ServiceResult()
                {
                    Success = false,
                    ErrorMessage = "User already Exists!"
                };
                return response;
            }

            User newUser = new()
            {
                Id = Guid.CreateVersion7(),
                UserName = dto.UserName,
                Email = dto.Email,
                RegisteredAt = DateTime.UtcNow,
                Role = UserRole.user
            };

            newUser.PasswordHashed = _passwordHasher.HashPassword(newUser, dto.Password);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return new ServiceResult()
            {
                Success = true,
            };
        }


        // Service layer
        public async Task<(ServiceResult, ClaimsPrincipal?)> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return (new ServiceResult { Success = false, ErrorMessage = "Invalid Credentials!" }, null);

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHashed, dto.Password);
            if (result == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                return (new ServiceResult { Success = true }, principal);
            }

            return (new ServiceResult { Success = false, ErrorMessage = "Invalid Credentials!" }, null);
        }


        public Task<ServiceResult> LogoutAsync()
        {
            // Nothing to do here except return success.
            return Task.FromResult(new ServiceResult
            {
                Success = true
            });
        }

    }
}
