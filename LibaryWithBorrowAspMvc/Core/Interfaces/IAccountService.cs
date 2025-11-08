using Domain.Common;
using LibaryWithBorrowAspMvc.Models.Dtos.User;
using System.Security.Claims;

namespace LibaryWithBorrowAspMvc.Core.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult> RegisterAsync(RegisterUserDto dto);
        Task<(ServiceResult, ClaimsPrincipal?)> LoginAsync(LoginUserDto dto);
        Task<ServiceResult> LogoutAsync();
    }
}
