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
    public class RazonSocialsController : Controller
    {
        private readonly BdDhrContext _context;

        public RazonSocialsController(BdDhrContext context)
        {
            _context = context;
        }

        // GET: RazonSocials
        public async Task<IActionResult> ListarRazonSocial()
        {
              return _context.RazonSocials != null ? 
                          View(await _context.RazonSocials.ToListAsync()) :
                          Problem("Entity set 'BdDhrContext.RazonSocials'  is null.");
        }

        // GET: RazonSocials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RazonSocials == null)
            {
                return NotFound();
            }

            var razonSocial = await _context.RazonSocials
                .FirstOrDefaultAsync(m => m.IdRazonSocial == id);
            if (razonSocial == null)
            {
                return NotFound();
            }

            return View(razonSocial);
        }

        // GET: RazonSocials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RazonSocials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("IdRazonSocial,RutRs,NombreRs")] RazonSocial razonSocial)
        {
            if (ModelState.IsValid)
            {
                // Verifica si hay elementos en la tabla Actas
                if (await _context.RazonSocials.AnyAsync())
                {

                    // Si hay, obten el máximo ID y agrégale 1
                    var nextId = await _context.RazonSocials.MaxAsync(a => a.IdRazonSocial) + 1;
                    razonSocial.IdRazonSocial = nextId;
                }
                else
                {
                    // Si no hay registros, asigna 1 como el primer ID
                    razonSocial.IdRazonSocial = 1;
                }
                _context.Add(razonSocial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListarRazonSocial));
            }
            return View(razonSocial);
        }

        // GET: RazonSocials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RazonSocials == null)
            {
                return NotFound();
            }

            var razonSocial = await _context.RazonSocials.FindAsync(id);
            if (razonSocial == null)
            {
                return NotFound();
            }
            return View(razonSocial);
        }

        // POST: RazonSocials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("IdRazonSocial,RutRs,NombreRs")] RazonSocial razonSocial)
        {
            if (id != razonSocial.IdRazonSocial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(razonSocial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RazonSocialExists(razonSocial.IdRazonSocial))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListarRazonSocial));
            }
            return View(razonSocial);
        }

        // GET: RazonSocials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RazonSocials == null)
            {
                return NotFound();
            }

            var razonSocial = await _context.RazonSocials
                    .Include(o => o.Acta) // Incluir actas asociadas
            .FirstOrDefaultAsync(m => m.IdRazonSocial == id);
            if (razonSocial == null)
            {
                return NotFound();
            }
            // Pasar la información de las actas asociadas a la vista
            ViewBag.ActasAsociadas = razonSocial.Acta.Any()
                ? razonSocial.Acta.Select(a => a.IdActa).ToList()
                : new List<int>();

            return View(razonSocial);
        }

        // POST: RazonSocials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RazonSocials == null)
            {
                return Problem("Entity set 'BdDhrContext.RazonSocials'  is null.");
            }
            var razonSocial = await _context.RazonSocials.FindAsync(id);
            if (razonSocial != null)
            {
                _context.RazonSocials.Remove(razonSocial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListarRazonSocial));
        }

        private bool RazonSocialExists(int id)
        {
          return (_context.RazonSocials?.Any(e => e.IdRazonSocial == id)).GetValueOrDefault();
        }
    }
}
