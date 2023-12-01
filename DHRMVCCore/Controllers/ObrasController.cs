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
    public class ObrasController : Controller
    {
        private readonly BdDhrContext _context;

        public ObrasController(BdDhrContext context)
        {
            _context = context;
        }

        // GET: Obras
        public async Task<IActionResult> ListarObras()
        {
              return _context.Obras != null ? 
                          View(await _context.Obras.ToListAsync()) :
                          Problem("Entity set 'BdDhrContext.Obras'  is null.");
        }

        // GET: Obras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Obras == null)
            {
                return NotFound();
            }

            var obra = await _context.Obras
                .FirstOrDefaultAsync(m => m.IdObra == id);
            if (obra == null)
            {
                return NotFound();
            }

            return View(obra);
        }

        // GET: Obras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Obras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("IdObra,NombreObra")] Obra obra)
        {
            if (ModelState.IsValid)
            {
                // Verifica si hay elementos en la tabla Obra
                if (await _context.Obras.AnyAsync())
                {
                    // Si hay, obten el máximo ID y agrégale 1
                    var nextId = await _context.Obras.MaxAsync(o => o.IdObra) + 1;
                    obra.IdObra = nextId;
                }
                else
                {
                    // Si no hay registros, asigna 1 como el primer ID
                    obra.IdObra = 1;
                }

                _context.Add(obra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListarObras));
            }
            return View(obra);
        }

        // GET: Obras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Obras == null)
            {
                return NotFound();
            }

            var obra = await _context.Obras.FindAsync(id);
            if (obra == null)
            {
                return NotFound();
            }
            return View(obra);
        }

        // POST: Obras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("IdObra,NombreObra")] Obra obra)
        {
            if (id != obra.IdObra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObraExists(obra.IdObra))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListarObras));
            }
            return View(obra);
        }

        // GET: Obras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Obras == null)
            {
                return NotFound();
            }

            var obra = await _context.Obras
                .Include(o => o.Acta) // Incluir actas asociadas
                .FirstOrDefaultAsync(m => m.IdObra == id);

            if (obra == null)
            {
                return NotFound();
            }

            // Pasar la información de las actas asociadas a la vista
            ViewBag.ActasAsociadas = obra.Acta.Any()
                ? obra.Acta.Select(a => a.IdActa).ToList()
                : new List<int>();

            return View(obra);
        }

        // POST: Obras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Obras == null)
            {
                return Problem("Entity set 'BdDhrContext.Obras' is null.");
            }

            var obra = await _context.Obras
                .Include(o => o.UsuarioUsuarios) // Incluir la relación con UsuarioUsuarios
                .FirstOrDefaultAsync(o => o.IdObra == id);

            if (obra != null)
            {
                // Eliminar todas las relaciones de esta obra con los usuarios
                foreach (var usuario in obra.UsuarioUsuarios.ToList())
                {
                    obra.UsuarioUsuarios.Remove(usuario);
                }

                // Ahora eliminar la obra
                _context.Obras.Remove(obra);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ListarObras));
        }


        private bool ObraExists(int id)
        {
          return (_context.Obras?.Any(e => e.IdObra == id)).GetValueOrDefault();
        }
    }
}
