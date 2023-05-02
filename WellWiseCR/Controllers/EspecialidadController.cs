using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.Data.SqlClient;
using Microsoft.Data;
using Microsoft.EntityFrameworkCore;
using WellWiseCR.Data;
using WellWiseCR.Datos;
using WellWiseCR.Models;

namespace WellWiseCR.Controllers
{
    public class EspecialidadController : Controller
    {
        private readonly WellWiseCRContext _context;

        public EspecialidadController(WellWiseCRContext context)
        {
            ViewData["ValidateMessage3"] = "";
            _context = context;
        }

        // GET: Especialidad
        public async Task<IActionResult> Index()
        {
              return _context.Especialidad != null ? 
                          View(await _context.Especialidad.ToListAsync()) :
                          Problem("Entity set 'WellWiseCRContext.Especialidad'  is null.");
        }

        // GET: Especialidad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Especialidad == null)
            {
                return NotFound();
            }

            var especialidad = await _context.Especialidad
                .FirstOrDefaultAsync(m => m.IdEspecialidad == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            return View(especialidad);
        }

        // GET: Especialidad/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especialidad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEspecialidad,NombreEspecialidad,Descripcion,Estado")] Especialidad especialidad)
        {
            especialidad.IdEspecialidad = GenerarId();

            Conexion con = new Conexion();
            string sql = "select * from [Especialidad] where nombreEspecialidad = '" + especialidad.NombreEspecialidad + "';";
            SqlCommand comando = new SqlCommand(sql, con.Conectar());

            SqlDataReader dr = comando.ExecuteReader();
            Especialidad dbe = new Especialidad();
            if (dr.Read())
                dbe.NombreEspecialidad = dr["nombreEspecialidad"].ToString();
            con.Desconectar();

            if (especialidad.NombreEspecialidad.Equals(dbe.NombreEspecialidad)) {
                ViewData["ValidateMessage3"] = "El nombre de especialidad ya se encuentra registrado. Por favor intente con uno nuevo.";
                return View();
            }

            if (ModelState.IsValid)
            {
                _context.Add(especialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(especialidad);
        }

        public int GenerarId()
        {
            int nuevoId = 0;
            Conexion con = new Conexion();
            string sql = "select count(*)+1 from especialidad;";
            SqlCommand comando = new SqlCommand(sql, con.Conectar());

            SqlDataReader dr = comando.ExecuteReader();
            
            if(dr.Read())
                nuevoId = dr.GetInt32(0);
            con.Desconectar();
            return nuevoId;
            //return nuevoId;
        }//fin del metodo generar id

        // GET: Especialidad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Especialidad == null)
            {
                return NotFound();
            }

            var especialidad = await _context.Especialidad.FindAsync(id);
            if (especialidad == null)
            {
                return NotFound();
            }
            return View(especialidad);
        }

        // POST: Especialidad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEspecialidad,NombreEspecialidad,Descripcion,Estado")] Especialidad especialidad)
        {
            if (id != especialidad.IdEspecialidad)
            {
                return NotFound();
            }

            Conexion con = new Conexion();
            string sql = "select * from [Especialidad] where nombreEspecialidad = '" + especialidad.NombreEspecialidad + "';";
            SqlCommand comando = new SqlCommand(sql, con.Conectar());

            SqlDataReader dr = comando.ExecuteReader();
            Especialidad dbe = new Especialidad();
            if (dr.Read())
                dbe.NombreEspecialidad = dr["nombreEspecialidad"].ToString();
            con.Desconectar();

            if (especialidad.NombreEspecialidad.Equals(dbe.NombreEspecialidad))
            {
                ViewData["ValidateMessage3"] = "El nombre de especialidad ya se encuentra registrado. Por favor intente con uno nuevo.";
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(especialidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecialidadExists(especialidad.IdEspecialidad))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(especialidad);
        }

        // GET: Especialidad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Especialidad == null)
            {
                return NotFound();
            }

            var especialidad = await _context.Especialidad
                .FirstOrDefaultAsync(m => m.IdEspecialidad == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            return View(especialidad);
        }

        // POST: Especialidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Especialidad == null)
            {
                return Problem("Entity set 'WellWiseCRContext.Especialidad'  is null.");
            }
            var especialidad = await _context.Especialidad.FindAsync(id);
            if (especialidad != null)
            {
                //_context.Especialidad.Remove(especialidad);
                Conexion con = new Conexion();
                string sql;
                if (especialidad.Estado.Equals("Activo"))
                    sql = "update especialidad set estado='Inactivo' where idEspecialidad ='" + especialidad.IdEspecialidad + "';";
                else
                    sql = "update especialidad set estado='Activo' where idEspecialidad ='" + especialidad.IdEspecialidad + "';";

                SqlCommand comando = new SqlCommand(sql, con.Conectar());
                int registrosAfectados = comando.ExecuteNonQuery();

                con.Desconectar();
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspecialidadExists(int id)
        {
          return (_context.Especialidad?.Any(e => e.IdEspecialidad == id)).GetValueOrDefault();
        }
    }
}
