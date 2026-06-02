using CRM.Interface.Interfaces;
using CRM.Interface.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    public class AccountController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var User = await _serviceManager.AuthenticationService.LoginAsync(loginDTO);

            Response.Cookies.Append("jwt", User.Token!, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax, // خليها Lax عشان Swagger
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            return Ok(User);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var User = await _serviceManager.AuthenticationService.RegisterAsync(registerDTO);
            return Ok(User);
        }

        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var Result = await _serviceManager.AuthenticationService.CheckEmailAsync(email);
            return Ok(Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var AppUser = await _serviceManager.AuthenticationService.GetCurrentUserAsync(GetEmailForToken());
            return Ok(AppUser);
        }
    }
}
