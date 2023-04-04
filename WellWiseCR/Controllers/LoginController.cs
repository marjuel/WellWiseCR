using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WellWiseCR.Models;
using WellWiseCR.Datos;
using System.Data.SqlClient;

namespace WellWiseCR.Controllers
{
    public class LoginController : Controller
    {
        public LoginController() {
            ViewData["ValidateMessage"] = "";
        }

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
            //En caso de que los campos de texto se encuentren vacios
            if (usuario.NombreUsuario == null || usuario.Password == null) {
                ViewData["ValidateMessage"] = "No se permiten campos vacíos.";
                return View();
            }
 
            //Se crea una conexion con la BD y el parametro usuario (que almacena los datos digitados en frontend) se relaciona con su registro
            Conexion con = new Conexion();
            string sql = "select * from [Usuario] where nombreUsuario = '" + usuario.NombreUsuario + "';";
            SqlCommand comando = new SqlCommand(sql, con.Conectar());

            SqlDataReader dr = comando.ExecuteReader();
            //Se crea un objeto Usuario llamado dbu (para almacenar los datos extraidos de la BD)
            Usuario dbu = new Usuario();
            if (dr.Read())
            {
                dbu.NombreUsuario = dr["nombreUsuario"].ToString().ToUpper();
                dbu.Password = dr["password"].ToString();
            }
            con.Desconectar();


            //Compara si los datos del usuario digitado son iguales a los que hay en BD
            if (usuario.NombreUsuario.ToUpper().Equals(dbu.NombreUsuario) && usuario.Password.Equals(dbu.Password))
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
                    IsPersistent = false
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                //Si la autenticación es correcta permite dirigirse a Home
                return RedirectToAction("Index", "Home");
            }
            //En caso de que la contrasena no sea correcta
            else if (usuario.NombreUsuario.ToUpper().Equals(dbu.NombreUsuario) && !usuario.Password.Equals(dbu.Password)) {
                ViewData["ValidateMessage"] = "El usuario o la contraseña no son correctos.";
                return View();
            }

                //En caso de que el usuario no exista
            ViewData["ValidateMessage"] = "El usuario digitado no existe en el sitema.";
            return View();
        }//Fin del metodo IniciarSesion post

    }//Fin de la clase LoginController
}//Fin del name space

