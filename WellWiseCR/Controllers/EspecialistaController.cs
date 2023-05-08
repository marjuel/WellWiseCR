using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WellWiseCR.Data;
using WellWiseCR.Datos;
using WellWiseCR.Models;

namespace WellWiseCR.Controllers
{
    public class EspecialistaController : Controller
    {
        private readonly WellWiseCRContext _context;

        public EspecialistaController(WellWiseCRContext context)
        {
            _context = context;
        }

        // GET: Especialista
        public async Task<IActionResult> Index()
        {
            var wellWiseCRContext = _context.Especialista.Include(e => e.Especialidad);
            return View(await wellWiseCRContext.ToListAsync());
        }

        // GET: Especialista/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Especialista == null)
            {
                return NotFound();
            }

            var especialista = await _context.Especialista
                .Include(e => e.Especialidad)
                .FirstOrDefaultAsync(m => m.IdEspecialista == id);
            if (especialista == null)
            {
                return NotFound();
            }

            return View(especialista);
        }

        // GET: Especialista/Create
        public IActionResult Create()
        {
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidad.Where(e => e.Estado == "Activo").ToList(), "IdEspecialidad", "NombreEspecialidad");
            return View();
        }

        public int GenerarId()
        {
            int nuevoId = 0;
            Conexion con = new Conexion();
            string sql = "select count(*)+1 from especialista;";
            SqlCommand comando = new SqlCommand(sql, con.Conectar());

            SqlDataReader dr = comando.ExecuteReader();

            if (dr.Read())
                nuevoId = dr.GetInt32(0);
            con.Desconectar();
            return nuevoId;
        }//fin del metodo generar id

        // POST: Especialista/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEspecialista,IdEspecialidad,Email,NombreCompleto,Provincia,Canton,Estado")] Especialista esp)
        {
            try
            {
                Conexion con = new Conexion();
                esp.IdEspecialista = GenerarId();
                string sql = "insert into [Especialista] values('" + esp.IdEspecialista + "', '" + esp.IdEspecialidad + "', '"
                    + esp.Email + "', '" + esp.NombreCompleto + "', '" + esp.Provincia + "', '" + esp.Canton + "', '" + esp.Estado + "')";
                SqlCommand comando = new SqlCommand(sql, con.Conectar());
                int registrosAfectados = comando.ExecuteNonQuery();
                con.Desconectar();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Especialista/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Especialista == null)
            {
                return NotFound();
            }

            var especialista = await _context.Especialista.FindAsync(id);
            if (especialista == null)
            {
                return NotFound();
            }
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidad.Where(e => e.Estado == "Activo").ToList(), "IdEspecialidad", "NombreEspecialidad");
            return View(especialista);
        }

        // POST: Especialista/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEspecialista,IdEspecialidad,Email,NombreCompleto,Provincia,Canton,Estado")] Especialista esp)
        {
            if (id != esp.IdEspecialista)
            {
                return NotFound();
            }

            try
            {
                Conexion con = new Conexion();
                string sql = "update especialista set IdEspecialidad='" + esp.IdEspecialidad + "',Email='" + esp.Email+ "',nombreCompleto='" 
                    + esp.NombreCompleto+ "',Provincia='" + esp.Provincia + "',Canton='" + esp.Canton + "' where IdEspecialista ='" + esp.IdEspecialista+ "';";
                SqlCommand comando = new SqlCommand(sql, con.Conectar());
                int registrosAfectados = comando.ExecuteNonQuery();
                con.Desconectar();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidad.Where(e => e.Estado == "Activo").ToList(), "IdEspecialidad", "NombreEspecialidad");
            return RedirectToAction(nameof(Index));
        }

        // GET: Especialista/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Especialista == null)
            {
                return NotFound();
            }

            var especialista = await _context.Especialista
                .Include(e => e.Especialidad)
                .FirstOrDefaultAsync(m => m.IdEspecialista == id);
            if (especialista == null)
            {
                return NotFound();
            }

            return View(especialista);
        }

        // POST: Especialista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Especialista == null)
            {
                return Problem("Entity set 'WellWiseCRContext.Especialista'  is null.");
            }
            var especialista = await _context.Especialista.FindAsync(id);
            if (especialista != null)
            {
                //_context.Especialidad.Remove(especialidad);
                Conexion con = new Conexion();
                string sql;
                if (especialista.Estado.Equals("Activo"))
                    sql = "update especialista set estado='Inactivo' where idEspecialista ='" + especialista.IdEspecialista + "';";
                else
                    sql = "update especialista set estado='Activo' where idEspecialista ='" + especialista.IdEspecialista + "';";

                SqlCommand comando = new SqlCommand(sql, con.Conectar());
                int registrosAfectados = comando.ExecuteNonQuery();

                con.Desconectar();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspecialistaExists(int id)
        {
          return (_context.Especialista?.Any(e => e.IdEspecialista == id)).GetValueOrDefault();
        }
    }
}
