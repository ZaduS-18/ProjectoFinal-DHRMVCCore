using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DHRMVCCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace DHRMVCCore.Controllers
{
    [Authorize]
    public class EspBimsController : Controller
    {
        private readonly BdDhrContext _context;

        public EspBimsController(BdDhrContext context)
        {
            _context = context;
        }

        // GET: EspBims
        public async Task<IActionResult> Index()
        {
            var bdDhrContext = _context.EspBims.Include(e => e.ActaIdActaNavigation);
            return View(await bdDhrContext.ToListAsync());
        }

        // GET: EspBims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EspBims == null)
            {
                return NotFound();
            }

            var espBim = await _context.EspBims
                .Include(e => e.ActaIdActaNavigation)
                                .ThenInclude(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(j => j.ActaIdActaNavigation.RazonSocialIdRazonSocialNavigation)
                .Include(j => j.ActaIdActaNavigation.ObraIdObraNavigation)
                .FirstOrDefaultAsync(m => m.IdEspBim == id);
            if (espBim == null)
            {
                return NotFound();
            }

            return View(espBim);
        }

        // GET: EspBims/Create
        public IActionResult Create()
        {
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa");
            return View();
        }

        // POST: EspBims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEspBim,Nombre,EjEeTt,CumAvances,IncuNorma,InfProblema,Observacion,Firma,ActaIdActa")] EspBim espBim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(espBim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", espBim.ActaIdActa);
            return View(espBim);
        }

        // GET: EspBims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EspBims == null)
            {
                return NotFound();
            }

            // Incluye las relaciones de navegación 
            var espBim = await _context.EspBims
                .Include(e => e.ActaIdActaNavigation)
                .ThenInclude(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(e => e.ActaIdActaNavigation.RazonSocialIdRazonSocialNavigation)
                .Include(e => e.ActaIdActaNavigation.ObraIdObraNavigation)
                .FirstOrDefaultAsync(m => m.IdEspBim == id); // Suponiendo que el ID se llama IdEspBim

            // Aquí comienza la lógica para actualizar la fecha de aprobación en Actum
            if (espBim.Firma && espBim.ActaIdActa.HasValue)
            {
                var acta = await _context.Acta.FindAsync(espBim.ActaIdActa.Value);
                if (acta != null)
                {
                    acta.FechaApro = DateTime.Now;
                    _context.Update(acta);
                    await _context.SaveChangesAsync();
                }
            }

            if (espBim == null)
            {
                return NotFound();
            }

            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", espBim.ActaIdActa);
            return View(espBim);
        }

        public IActionResult RedirectToListarActums()
        {
            return RedirectToAction("ListarActas", "Actums");
        }
        // POST: EspBims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("IdEspBim,Nombre,EjEeTt,CumAvances,IncuNorma,InfProblema,Observacion,Firma,ActaIdActa")] EspBim espBim)
        {
            if (id != espBim.IdEspBim)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(espBim);
                    await _context.SaveChangesAsync();
                    // Aquí comienza la lógica para las notificaciones

                    var obra = await _context.EspBims
                        .Include(j => j.ActaIdActaNavigation)
                        .Include(j => j.ActaIdActaNavigation.ObraIdObraNavigation)
                        .ThenInclude(o => o.UsuarioUsuarios)
                        .FirstOrDefaultAsync(a => a.IdEspBim == espBim.IdEspBim);




                    if (obra != null)
                    {
                        int nextNotificationId = 1;
                        if (await _context.UsuarioNotificaciones.AnyAsync())
                        {
                            nextNotificationId = await _context.UsuarioNotificaciones.MaxAsync(n => n.IdNotificaciones) + 1;
                        }

                        foreach (var usuario in obra.ActaIdActaNavigation.ObraIdObraNavigation.UsuarioUsuarios)
                        {
                            var usuarioExistente = await _context.Usuarios
                                                                 .AnyAsync(u => u.UsuarioId == usuario.UsuarioId);
                            if (usuarioExistente)
                            {
                                UsuarioNotificacione notificacion = new UsuarioNotificacione
                                {
                                    IdNotificaciones = nextNotificationId++,
                                    Descripcion = $"Especialidades BIM firmo en el acta:" + espBim.ActaIdActaNavigation.IdActa,
                                    Fecha = DateTime.Now,
                                    UsuarioUsuarioId = usuario.UsuarioId
                                };

                                _context.UsuarioNotificaciones.Add(notificacion);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                    // Fin de la lógica para las notificaciones
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspBimExists(espBim.IdEspBim))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(RedirectToListarActums));
            }
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", espBim.ActaIdActa);
            return View(espBim);
        }

        // GET: EspBims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EspBims == null)
            {
                return NotFound();
            }

            var espBim = await _context.EspBims
                .Include(e => e.ActaIdActaNavigation)
                .FirstOrDefaultAsync(m => m.IdEspBim == id);
            if (espBim == null)
            {
                return NotFound();
            }

            return View(espBim);
        }

        // POST: EspBims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EspBims == null)
            {
                return Problem("Entity set 'BdDhrContext.EspBims'  is null.");
            }
            var espBim = await _context.EspBims.FindAsync(id);
            if (espBim != null)
            {
                _context.EspBims.Remove(espBim);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspBimExists(int id)
        {
          return (_context.EspBims?.Any(e => e.IdEspBim == id)).GetValueOrDefault();
        }
    }
}
