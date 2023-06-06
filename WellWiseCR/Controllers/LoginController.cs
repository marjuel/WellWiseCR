using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WellWiseCR.Models;
using WellWiseCR.Datos;
using System.Data.SqlClient;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Microsoft.EntityFrameworkCore;
using WellWiseCR.Data;

namespace WellWiseCR.Controllers
{
    public class LoginController : Controller
    {
        private readonly WellWiseCRContext dbcontext;

        public LoginController(WellWiseCRContext context)
        {
            ViewData["ValidateMessage"] = "";
            ViewData["ValidateMessage2"] = "";
            ViewData["ValidateMessage3"] = "";
            dbcontext = context;
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
            if (usuario.NombreUsuario == null || usuario.Password == null)
            {
                ViewData["ValidateMessage"] = "No se permiten campos vacíos.";
                return View();
            }

            //Se crea una conexion con la BD y el parametro usuario (que almacena los datos digitados en frontend) se relaciona con su registro
            Conexion con = new Conexion();
            SqlConnection sqlConnection = con.Conectar();
            string sql = "select * from [Usuario] where nombreUsuario = '" + usuario.NombreUsuario + "';";
            SqlCommand comando = new SqlCommand(sql, sqlConnection);

            SqlDataReader dr = comando.ExecuteReader();
            //Se crea un objeto Usuario llamado dbu (para almacenar los datos extraidos de la BD)
            Usuario dbu = new Usuario();
            if (dr.Read())
            {
                dbu.NombreUsuario = dr["nombreUsuario"].ToString().ToUpper();
                dbu.Password = dr["password"].ToString();
                dbu.ConfirmacionPassword = dr["confirmacionPassword"].ToString();
                dbu.Email = dr["email"].ToString();
                dbu.NombreCompleto = dr["nombreCompleto"].ToString();
                dbu.FechaNacimiento = DateTime.Today;
                dbu.Provincia = dr["provincia"].ToString();
                dbu.Canton = dr["canton"].ToString();
                dbu.Rol = dr["rol"].ToString();
                dbu.Estado = dr["estado"].ToString();
            }
            con.Desconectar();

            //Compara si los datos del usuario digitado son iguales a los que hay en BD
            if (usuario.NombreUsuario.ToUpper().Equals(dbu.NombreUsuario) && usuario.Password.Equals(dbu.Password) && dbu.Estado.Equals("Activo"))
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

                GlobalData.nombreGlobal = dbu.NombreUsuario;
                GlobalData.rolGlobal = dbu.Rol;

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                //Si la autenticación es correcta permite dirigirse a Home
                return RedirectToAction("Index", "Home");
            }
            else if (usuario.NombreUsuario.ToUpper().Equals(dbu.NombreUsuario) && usuario.Password.Equals(dbu.Password) && dbu.Estado.Equals("Inactivo"))
            {
                ViewData["ValidateMessage"] = "El usuario no puede ingresar ya que se encuentra desactivado. Contacte a un administrador para la reactivación.";
                return View();
            }
            //En caso de que la contrasena no sea correcta
            else if (usuario.NombreUsuario.ToUpper().Equals(dbu.NombreUsuario) && !usuario.Password.Equals(dbu.Password))
            {
                ViewData["ValidateMessage"] = "El usuario o la contraseña no son correctos.";
                return View();
            }

            //En caso de que el usuario no exista
            ViewData["ValidateMessage"] = "El usuario digitado no existe en el sitema.";
            return View();
        }//fin del metodo IniciarSesion post

        public string GenerarPassword()
        {
            const string caracteres = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            StringBuilder nuevoPassword = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                int indice = random.Next(caracteres.Length);
                nuevoPassword.Append(caracteres[indice]);
            }

            return nuevoPassword + "";
        }//Fin del metodo GenerarContrasena

        public void EnviarMail(Usuario user)
        {
            try
            {
                MailMessage email = new MailMessage();

                //Asunto
                email.Subject = "Su contraseña de WellWiseCR se ha reestablecido éxitosamente";
                //Direcccion del correo del administrador
                email.To.Add(new MailAddress("kevin.ra.business@gmail.com"));
                //direccion del correo del usuario
                email.To.Add(new MailAddress(user.Email));
                //emisor del correo 
                email.From = new MailAddress("kevin.ra.business@gmail.com");

                string html = "WellWiseCR es un software para el control de diagnósticos médicos desarrollado por SoftStyles®.";
                html += "<br><br> A continuación detallamos los nuevos datos del usuario asociado: ";
                html += "<br><b>Usuario: </b>" + user.NombreUsuario;
                html += "<br><b>Email: </b>" + user.Email;
                html += "<br><b>Su nueva contraseña: </b>" + user.Password;
                html += "<br>Este es un correo electrónico generado automáticamente. Por favor no intente responderlo.";
                html += "<br><br>¡El equipo de desarrollo de SoftStyles® le desea un feliz día!";

                //se indica el contenido es en html
                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;

                AlternateView view = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);
                email.AlternateViews.Add(view);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;


                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("kevin.ra.business@gmail.com", "ccmhhwlxodllrbsb");

                smtp.Send(email);
                email.Dispose();
                smtp.Dispose();

            }
            catch (Exception ex)
            {
                ViewData["ValidateMessage2"] = "System.Net.Mail.SmtpException: Cuota de correos electrónicos por día excedida.";
                Console.WriteLine(ex.ToString());
                //throw ex;
            }
        }//Fin del metodo EnviarEmail

        //Método post para reestablecer la contraseña
        [HttpPost]
        public async Task<IActionResult> ReestablecerPassword(Usuario usuario)
        {
            //En caso de que los campos de texto se encuentren vacios
            if (usuario.Email == null)
            {
                ViewData["ValidateMessage2"] = "Ingrese su correo electrónico para reestablecer la contraseña.";
                return View();
            }

            Conexion con = new Conexion();
            string nuevoPassword = this.GenerarPassword();
            string sql = "update usuario set password='" + nuevoPassword + "',confirmacionPassword='" + nuevoPassword
                + "' where email ='" + usuario.Email + "';";

            SqlCommand comando = new SqlCommand(sql, con.Conectar());
            int registrosAfectados = comando.ExecuteNonQuery();

            con.Desconectar();


            con = new Conexion();
            string consulta = "select * from [Usuario] where email = '" + usuario.Email + "';";
            SqlCommand comando2 = new SqlCommand(consulta, con.Conectar());

            SqlDataReader dr = comando2.ExecuteReader();
            if (dr.Read())
            {
                usuario.NombreUsuario = dr["nombreUsuario"].ToString().ToUpper();
                usuario.Password = dr["password"].ToString();
                usuario.ConfirmacionPassword = dr["confirmacionPassword"].ToString();
                usuario.Email = dr["email"].ToString();
                usuario.NombreCompleto = dr["nombreCompleto"].ToString();
                usuario.FechaNacimiento = DateTime.Today;
                usuario.Provincia = dr["provincia"].ToString();
                usuario.Canton = dr["canton"].ToString();
                usuario.Rol = dr["rol"].ToString();
                usuario.Estado = dr["estado"].ToString();
            }
            con.Desconectar();

            if (registrosAfectados >= 1)
            {
                ViewData["ValidateMessage2"] = "Contraseña reestablecida éxitosamente. Por favor revise su correo electrónico.";
                this.EnviarMail(usuario);
                return View();
            }

            ViewData["ValidateMessage2"] = "No se pudo reestablecer la contraseña.";
            return View();

        }//Fin del metodo ReestablecerContrasena post

        // GET: Usuarios/RegistrarUsuario
        public IActionResult RegistrarUsuario()
        {
            return View();
        }


        // POST: Login/RegistrarUsuario
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarUsuario([Bind("NombreUsuario,Password,ConfirmacionPassword,Email,NombreCompleto," +
            "FechaNacimiento,Provincia,Canton,Rol,Estado")] Usuario usuario)
        {
            Conexion con = new Conexion();
            string sql = "select * from [Usuario] where nombreUsuario = '" + usuario.NombreUsuario + "';";
            SqlCommand comando = new SqlCommand(sql, con.Conectar());

            SqlDataReader dr = comando.ExecuteReader();
            Usuario dbu = new Usuario();
            if (dr.Read())
                dbu.NombreUsuario = dr["nombreUsuario"].ToString().ToUpper();
            con.Desconectar();

            usuario.NombreUsuario = usuario.NombreUsuario.ToUpper();
            usuario.Rol = "Paciente";
            usuario.Estado = "Activo";

            try {

                dbcontext.Add(usuario);
                await dbcontext.SaveChangesAsync();
                return RedirectToAction("IniciarSesion", "Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine("8\n8\n8\n8\n");
                Console.WriteLine(ex.ToString());
                ViewData["ValidateMessage3"] = "El nombre de usuario ya se encuentra registrado. Por favor intente con uno nuevo.";
            }

            return View(usuario);
        }

    }//fin de la clase LoginController
}//fin del namespace
