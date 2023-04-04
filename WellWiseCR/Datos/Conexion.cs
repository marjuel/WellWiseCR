using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellWiseCR.Datos
{
    internal class Conexion
    {
        //Es necesario administrar los paquetes NuGet para descargar System.Data.SqlClient
        SqlConnection con;

        //Contructor con la cadena de conexión
        public Conexion()
        {
            con = new SqlConnection("Data Source=DESKTOP-FRJUIOA\\SQLEXPRESS;Initial Catalog=DBWellWiseCR;Integrated Security=True");
        }

        //Método utilizado para abrir la conexión con la base de datos
        public SqlConnection Conectar()
        {
            try
            {
                con.Open();
                return con;
            }
            catch (Exception)
            {

                return null;
            }
        }

        //Método utilizado para cerrar la conexión con la base de datos
        public bool Desconectar()
        {
            try
            {
                con.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
