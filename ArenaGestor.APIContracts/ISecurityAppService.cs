using ArenaGestor.APIContracts.Security;
using Microsoft.AspNetCore.Mvc;

namespace ArenaGestor.APIContracts
{
    public interface ISecurityAppService
    {
        IActionResult Login(SecurityLoginDto loginRequest);
        IActionResult Logout(string token);
    }
}
