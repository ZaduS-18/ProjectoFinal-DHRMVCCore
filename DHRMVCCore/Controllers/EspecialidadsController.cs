using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DHRMVCCore.Models;
using System.Diagnostics;

namespace DHRMVCCore.Controllers
{
    public class EspecialidadsController : Controller
    {
        private readonly BdDhrContext _context;

        public EspecialidadsController(BdDhrContext context)
        {
            _context = context;
        }

        // GET: Especialidads
        public async Task<IActionResult> ListarEspecialidad()
        {
              return _context.Especialidads != null ? 
                          View(await _context.Especialidads.ToListAsync()) :
                          Problem("Entity set 'BdDhrContext.Especialidads'  is null.");
        }

        // GET: Especialidads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Especialidads == null)
            {
                return NotFound();
            }

            var especialidad = await _context.Especialidads
                .FirstOrDefaultAsync(m => m.IdEspecialidad == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            return View(especialidad);
        }

        // GET: Especialidads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especialidads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("IdEspecialidad,NombreEsp")] Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Especialidads.AnyAsync())
                {
                    // Si hay, obten el máximo ID y agrégale 1
                    var nextId = await _context.Especialidads.MaxAsync(a => a.IdEspecialidad) + 1;
                    especialidad.IdEspecialidad = nextId;
                }
                else
                {
                    // Si no hay registros, asigna 1 como el primer ID
                    especialidad.IdEspecialidad = 1;
                }

                _context.Add(especialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListarEspecialidad));
            }
            return View(especialidad);
        }

        // GET: Especialidads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Especialidads == null)
            {
                return NotFound();
            }

            var especialidad = await _context.Especialidads
    .Include(o => o.Acta) // Incluir actas asociadas
    .FirstOrDefaultAsync(m => m.IdEspecialidad == id);
            if (especialidad == null)
            {
                return NotFound();
            }
            // Pasar la información de las actas asociadas a la vista
            ViewBag.ActasAsociadas = especialidad.Acta.Any()
                ? especialidad.Acta.Select(a => a.IdActa).ToList()
                : new List<int>();
            return View(especialidad);
        }

        // POST: Especialidads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("IdEspecialidad,NombreEsp")] Especialidad especialidad)
        {
            if (id != especialidad.IdEspecialidad)
            {
                return NotFound();
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
                return RedirectToAction(nameof(ListarEspecialidad));
            }
            return View(especialidad);
        }

        // GET: Especialidads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Especialidads == null)
            {
                return NotFound();
            }

            var especialidad = await _context.Especialidads
    .Include(o => o.Acta) // Incluir actas asociadas
    .FirstOrDefaultAsync(m => m.IdEspecialidad == id);
            if (especialidad == null)
            {
                return NotFound();
            }
            // Pasar la información de las actas asociadas a la vista
            ViewBag.ActasAsociadas = especialidad.Acta.Any()
                ? especialidad.Acta.Select(a => a.IdActa).ToList()
                : new List<int>();
            return View(especialidad);
        }

        // POST: Especialidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Especialidads == null)
            {
                return Problem("Entity set 'BdDhrContext.Especialidads'  is null.");
            }

            try
            {
                var especialidad = await _context.Especialidads.FindAsync(id);
                if (especialidad != null)
                {
                    _context.Especialidads.Remove(especialidad);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(ListarEspecialidad));
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                // Aquí puedes manejar el error específico, por ejemplo, registrar el error o mostrar un mensaje al usuario.
                // Por ejemplo, puedes registrar el error y luego redirigir a una página de error.
                // Log the exception (consider using a logging framework like NLog, Serilog, etc.)
                // LogError(ex);

                // O retornar una vista con un mensaje de error.
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        private bool EspecialidadExists(int id)
        {
          return (_context.Especialidads?.Any(e => e.IdEspecialidad == id)).GetValueOrDefault();
        }
    }
}
