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
    public class UsuarioNotificacionesController : Controller
    {
        private readonly BdDhrContext _context;

        public UsuarioNotificacionesController(BdDhrContext context)
        {
            _context = context;
        }

        // GET: UsuarioNotificaciones
        public async Task<IActionResult> Index()
        {
            var bdDhrContext = _context.UsuarioNotificaciones.Include(u => u.UsuarioUsuario);
            return View(await bdDhrContext.ToListAsync());
        }

        // GET: UsuarioNotificaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsuarioNotificaciones == null)
            {
                return NotFound();
            }

            var usuarioNotificacione = await _context.UsuarioNotificaciones
                .Include(u => u.UsuarioUsuario)
                .FirstOrDefaultAsync(m => m.IdNotificaciones == id);
            if (usuarioNotificacione == null)
            {
                return NotFound();
            }

            return View(usuarioNotificacione);
        }

        // GET: UsuarioNotificaciones/Create
        public IActionResult Create()
        {
            ViewData["UsuarioUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: UsuarioNotificaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNotificaciones,Descripcion,Fecha,UsuarioUsuarioId")] UsuarioNotificacione usuarioNotificacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioNotificacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuarioNotificacione.UsuarioUsuarioId);
            return View(usuarioNotificacione);
        }

        // GET: UsuarioNotificaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsuarioNotificaciones == null)
            {
                return NotFound();
            }

            var usuarioNotificacione = await _context.UsuarioNotificaciones.FindAsync(id);
            if (usuarioNotificacione == null)
            {
                return NotFound();
            }
            ViewData["UsuarioUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuarioNotificacione.UsuarioUsuarioId);
            return View(usuarioNotificacione);
        }

        // POST: UsuarioNotificaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNotificaciones,Descripcion,Fecha,UsuarioUsuarioId")] UsuarioNotificacione usuarioNotificacione)
        {
            if (id != usuarioNotificacione.IdNotificaciones)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioNotificacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioNotificacioneExists(usuarioNotificacione.IdNotificaciones))
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
            ViewData["UsuarioUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", usuarioNotificacione.UsuarioUsuarioId);
            return View(usuarioNotificacione);
        }

        // GET: UsuarioNotificaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsuarioNotificaciones == null)
            {
                return NotFound();
            }

            var usuarioNotificacione = await _context.UsuarioNotificaciones
                .Include(u => u.UsuarioUsuario)
                .FirstOrDefaultAsync(m => m.IdNotificaciones == id);
            if (usuarioNotificacione == null)
            {
                return NotFound();
            }

            return View(usuarioNotificacione);
        }

        // POST: UsuarioNotificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsuarioNotificaciones == null)
            {
                return Problem("Entity set 'BdDhrContext.UsuarioNotificaciones'  is null.");
            }
            var usuarioNotificacione = await _context.UsuarioNotificaciones.FindAsync(id);
            if (usuarioNotificacione != null)
            {
                _context.UsuarioNotificaciones.Remove(usuarioNotificacione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioNotificacioneExists(int id)
        {
          return (_context.UsuarioNotificaciones?.Any(e => e.IdNotificaciones == id)).GetValueOrDefault();
        }
    }
}
