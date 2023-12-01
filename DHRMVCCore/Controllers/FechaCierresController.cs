using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DHRMVCCore.Models;

namespace DHRMVCCore.Controllers
{
    public class FechaCierresController : Controller
    {
        private readonly BdDhrContext _context;

        public FechaCierresController(BdDhrContext context)
        {
            _context = context;
        }

        // GET: FechaCierres
        public async Task<IActionResult> ListarFechaCierre()
        {
              return _context.FechaCierres != null ? 
                          View(await _context.FechaCierres.ToListAsync()) :
                          Problem("Entity set 'BdDhrContext.FechaCierres'  is null.");
        }

        // GET: FechaCierres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FechaCierres == null)
            {
                return NotFound();
            }

            var fechaCierre = await _context.FechaCierres
                .FirstOrDefaultAsync(m => m.IdFCierre == id);
            if (fechaCierre == null)
            {
                return NotFound();
            }

            return View(fechaCierre);
        }

        // GET: FechaCierres/Create
        public IActionResult Create()
        {
            var tipPagoOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "QUINCENA" },
        new SelectListItem { Value = "2", Text = "FIN DE MES" }
    };

            ViewBag.TipPagoOptions = tipPagoOptions;
            return View();
        }

        // POST: FechaCierres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFCierre,TipPago,FRecepcion,FCierre")] FechaCierre fechaCierre)
        {
            if (ModelState.IsValid)
            {
                // Verifica si hay elementos en la tabla FechaCierre
                if (await _context.FechaCierres.AnyAsync())
                {
                    // Si hay, obten el máximo ID y agrégale 1
                    var nextId = await _context.FechaCierres.MaxAsync(fc => fc.IdFCierre) + 1;
                    fechaCierre.IdFCierre = nextId;
                }
                else
                {
                    // Si no hay registros, asigna 1 como el primer ID
                    fechaCierre.IdFCierre = 1;
                }

                _context.Add(fechaCierre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListarFechaCierre));
            }
            return View(fechaCierre);
        }

        // GET: FechaCierres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FechaCierres == null)
            {
                return NotFound();
            }

            var fechaCierre = await _context.FechaCierres.FindAsync(id);
            if (fechaCierre == null)
            {
                return NotFound();
            }
            return View(fechaCierre);
        }

        // POST: FechaCierres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFCierre,TipPago,FRecepcion,FCierre")] FechaCierre fechaCierre)
        {
            if (id != fechaCierre.IdFCierre)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fechaCierre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FechaCierreExists(fechaCierre.IdFCierre))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListarFechaCierre));
            }
            return View(fechaCierre);
        }

        // GET: FechaCierres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FechaCierres == null)
            {
                return NotFound();
            }

            var fechaCierre = await _context.FechaCierres
                .FirstOrDefaultAsync(m => m.IdFCierre == id);
            if (fechaCierre == null)
            {
                return NotFound();
            }

            return View(fechaCierre);
        }

        // POST: FechaCierres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FechaCierres == null)
            {
                return Problem("Entity set 'BdDhrContext.FechaCierres'  is null.");
            }
            var fechaCierre = await _context.FechaCierres.FindAsync(id);
            if (fechaCierre != null)
            {
                _context.FechaCierres.Remove(fechaCierre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListarFechaCierre));
        }

        private bool FechaCierreExists(int id)
        {
          return (_context.FechaCierres?.Any(e => e.IdFCierre == id)).GetValueOrDefault();
        }
    }
}
