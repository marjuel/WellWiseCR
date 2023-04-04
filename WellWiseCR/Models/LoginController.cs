using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WellWiseCR.Models;


namespace WellWiseCR.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult IniciarSesion()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            //Verifica que el usuario está autenticado para poder ingresar al Home
            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        //Método post para el inicio de seseión
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Usuario usuario)
        {
            //aqui debo poner SQLCommands para interactuar con la BD
            if (usuario.NombreUsuario.Equals("marjueladmin") && usuario.Password.Equals("123"))
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.NombreUsuario),
                    new Claim("OtherProperties","Example Role")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                //Si la autenticación es correcta permite dirigirse a Home
                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "Usuario no encontrado.";
            return View();
        }
    }//Fin de la clase LoginController
}//Fin del name space

