using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WellWiseCR.Data;
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
            return View();
        }

        // POST: Diagnostico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDiagnostico,NombreUsuario,FechaHora,Peso,Estatura,ActividadFisica,CondicionCardiaca,Estado")] Diagnostico diagnostico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnostico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NombreUsuario"] = new SelectList(_context.Usuario, "NombreUsuario", "NombreUsuario", diagnostico.NombreUsuario);
            return View(diagnostico);
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
