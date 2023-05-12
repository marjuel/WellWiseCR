using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
    public class DiagnosticoController : Controller
    {
        private readonly WellWiseCRContext _context;

        public DiagnosticoController(WellWiseCRContext context)
        {
            _context = context;
        }

        // GET: Diagnostico
        public async Task<IActionResult> Index()
        {
            var wellWiseCRContext = _context.Diagnostico.Include(d => d.Usuario);
            return View(await wellWiseCRContext.ToListAsync());
        }

        // GET: Diagnostico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Diagnostico == null)
            {
                return NotFound();
            }

            var diagnostico = await _context.Diagnostico
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.IdDiagnostico == id);
            if (diagnostico == null)
            {
                return NotFound();
            }

            return View(diagnostico);
        }

        // GET: Diagnostico/Create
        public IActionResult Create()
        {
            ViewData["NombreUsuario"] = new SelectList(_context.Usuario, "NombreUsuario", "NombreUsuario");
            ViewBag.Enfermedades = _context.Enfermedad.Where(e => e.Estado == "Activo").ToList();
            return View();
        }

        public int GenerarId()
        {
            try {
                int nuevoId = 0;
                Conexion con = new Conexion();
                //string sql = "select count(*)+1 from diagnostico";
                string sql = "select max(IdDiagnostico)+1 from diagnostico";
                SqlCommand comando = new SqlCommand(sql, con.Conectar());

                SqlDataReader dr = comando.ExecuteReader();

                Console.WriteLine("JUSTO ANTES DE Entrando al if para null");
                if (dr.Read() != null) {
                    Console.WriteLine("Entrando al if para null");
                    nuevoId = dr.GetInt32(0);
                }

                Console.WriteLine("IMPRIMIENDO " + nuevoId);

                con.Desconectar();
                return nuevoId;
            }
            catch (SqlNullValueException ex) {
                Console.WriteLine("Entrando al excepton");
                return 1;
            }
            
        }//fin del metodo generar id

        // POST: Diagnostico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDiagnostico,NombreUsuario,FechaHora,Peso,Estatura,ActividadFisica,CondicionCardiaca,Estado")] Diagnostico diagnostico, int[] enfermedadesSeleccionadas)
        {
            try
            {
                Console.WriteLine("Entra al try de create");
                Conexion con = new Conexion();
                diagnostico.IdDiagnostico = GenerarId();
                diagnostico.Estado = "Activo";
                string sql = "insert into [Diagnostico] values('" + diagnostico.IdDiagnostico + "', '" + diagnostico.NombreUsuario + "', '" + diagnostico.FechaHora + "', '"
                    + diagnostico.Peso + "', '" + diagnostico.Estatura + "', '" + diagnostico.ActividadFisica + "', '"
                    + diagnostico.CondicionCardiaca + "', '" + diagnostico.Estado + "')";
                SqlCommand comando = new SqlCommand(sql, con.Conectar());
                int registrosAfectados = comando.ExecuteNonQuery();
                Console.WriteLine("Ejecuta el query de create diagnostico");
                con.Desconectar();

                foreach (int idEnfermedad in enfermedadesSeleccionadas)
                {
                    Console.WriteLine("Entra al foreach de create");
                    string sql2 = "insert into [Detalle] values('" + diagnostico.IdDiagnostico + "', '" + idEnfermedad + "')";

                    SqlCommand comando2 = new SqlCommand(sql2, con.Conectar());
                    int registrosAfectados2 = comando2.ExecuteNonQuery();
                    con.Desconectar();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
                return NotFound();
            }
        }

        // GET: Diagnostico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Diagnostico == null)
            {
                return NotFound();
            }

            var diagnostico = await _context.Diagnostico.FindAsync(id);
            if (diagnostico == null)
            {
                return NotFound();
            }
            ViewData["NombreUsuario"] = new SelectList(_context.Usuario, "NombreUsuario", "NombreUsuario", diagnostico.NombreUsuario);
            return View(diagnostico);
        }

        // POST: Diagnostico/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDiagnostico,NombreUsuario,FechaHora,Peso,Estatura,ActividadFisica,CondicionCardiaca,Estado")] Diagnostico diagnostico)
        {
            if (id != diagnostico.IdDiagnostico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnostico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosticoExists(diagnostico.IdDiagnostico))
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
            ViewData["NombreUsuario"] = new SelectList(_context.Usuario, "NombreUsuario", "NombreUsuario", diagnostico.NombreUsuario);
            return View(diagnostico);
        }

        // GET: Diagnostico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Diagnostico == null)
            {
                return NotFound();
            }

            var diagnostico = await _context.Diagnostico
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.IdDiagnostico == id);
            if (diagnostico == null)
            {
                return NotFound();
            }

            return View(diagnostico);
        }

        // POST: Diagnostico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Diagnostico == null)
            {
                return Problem("Entity set 'WellWiseCRContext.Diagnostico'  is null.");
            }
            var diagnostico = await _context.Diagnostico.FindAsync(id);
            if (diagnostico != null)
            {
                _context.Diagnostico.Remove(diagnostico);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosticoExists(int id)
        {
          return (_context.Diagnostico?.Any(e => e.IdDiagnostico == id)).GetValueOrDefault();
        }
    }
}
