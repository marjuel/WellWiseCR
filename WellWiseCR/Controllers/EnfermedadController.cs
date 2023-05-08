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
    public class EnfermedadController : Controller
    {
        private readonly WellWiseCRContext _context;

        public EnfermedadController(WellWiseCRContext context)
        {
            _context = context;
        }

        // GET: Enfermedad
        public async Task<IActionResult> Index()
        {
            var wellWiseCRContext = _context.Enfermedad.Include(e => e.Especialidad);
            return View(await wellWiseCRContext.ToListAsync());
        }

        // GET: Enfermedad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enfermedad == null)
            {
                return NotFound();
            }

            var enfermedad = await _context.Enfermedad
                .Include(e => e.Especialidad)
                .FirstOrDefaultAsync(m => m.IdEnfermedad == id);
            if (enfermedad == null)
            {
                return NotFound();
            }

            return View(enfermedad);
        }

        // GET: Enfermedad/Create
        public IActionResult Create()
        {
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidad.Where(e => e.Estado == "Activo").ToList(), "IdEspecialidad", "NombreEspecialidad");
            return View();
        }

        public int GenerarId()
        {
            int nuevoId = 0;
            Conexion con = new Conexion();
            string sql = "select count(*)+1 from enfermedad;";
            SqlCommand comando = new SqlCommand(sql, con.Conectar());

            SqlDataReader dr = comando.ExecuteReader();

            if (dr.Read())
                nuevoId = dr.GetInt32(0);
            con.Desconectar();
            return nuevoId;
        }//fin del metodo generar id

        // POST: Enfermedad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEnfermedad,IdEspecialidad,NombreEnfermedad,Sintomas,NivelAlerta,Recomendaciones,Estado")] Enfermedad enf)
        {
            try
            {
                Conexion con = new Conexion();
                enf.IdEnfermedad = GenerarId();
                enf.Estado = "Activo";
                string sql = "insert into [Enfermedad] values('" + enf.IdEnfermedad + "', '" + enf.IdEspecialidad + "', '"
                    + enf.NombreEnfermedad + "', '" + enf.Sintomas + "', '" + enf.NivelAlerta + "', '" + enf.Recomendaciones + "', '" + enf.Estado + "')";
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

        // GET: Enfermedad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Enfermedad == null)
            {
                return NotFound();
            }

            var enfermedad = await _context.Enfermedad.FindAsync(id);
            if (enfermedad == null)
            {
                return NotFound();
            }
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidad.Where(e => e.Estado == "Activo").ToList(), "IdEspecialidad", "NombreEspecialidad");
            return View(enfermedad);
        }

        // POST: Enfermedad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEnfermedad,IdEspecialidad,NombreEnfermedad,Sintomas,NivelAlerta,Recomendaciones,Estado")] Enfermedad enf)
        {
            if (id != enf.IdEnfermedad)
            {
                return NotFound();
            }

            try
            {
                Conexion con = new Conexion();
                string sql = "update enfermedad set IdEspecialidad='" + enf.IdEspecialidad + "',NombreEnfermedad='" + enf.NombreEnfermedad + "',Sintomas='"
                    + enf.Sintomas + "',NivelAlerta='" + enf.NivelAlerta + "',Recomendaciones='" + enf.Recomendaciones + "' where IdEnfermedad ='" + enf.IdEnfermedad + "';";
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

        // GET: Enfermedad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enfermedad == null)
            {
                return NotFound();
            }

            var enfermedad = await _context.Enfermedad
                .Include(e => e.Especialidad)
                .FirstOrDefaultAsync(m => m.IdEnfermedad == id);
            if (enfermedad == null)
            {
                return NotFound();
            }

            return View(enfermedad);
        }

        // POST: Enfermedad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Enfermedad == null)
            {
                return Problem("Entity set 'WellWiseCRContext.Enfermedad'  is null.");
            }
            var enfermedad = await _context.Enfermedad.FindAsync(id);
            if (enfermedad != null)
            {
                //_context.Enfermedad.Remove(enfermedad);
                Conexion con = new Conexion();
                string sql;
                if (enfermedad.Estado.Equals("Activo"))
                    sql = "update enfermedad set estado='Inactivo' where idEnfermedad ='" + enfermedad.IdEnfermedad + "';";
                else
                    sql = "update enfermedad set estado='Activo' where idEnfermedad ='" + enfermedad.IdEnfermedad + "';";

                SqlCommand comando = new SqlCommand(sql, con.Conectar());
                int registrosAfectados = comando.ExecuteNonQuery();

                con.Desconectar();
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnfermedadExists(int id)
        {
          return (_context.Enfermedad?.Any(e => e.IdEnfermedad == id)).GetValueOrDefault();
        }
    }
}
