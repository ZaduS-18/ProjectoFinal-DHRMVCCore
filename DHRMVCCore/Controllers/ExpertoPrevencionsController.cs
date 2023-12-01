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
    public class ExpertoPrevencionsController : Controller
    {
        private readonly BdDhrContext _context;

        public ExpertoPrevencionsController(BdDhrContext context)
        {
            _context = context;
        }

        // GET: ExpertoPrevencions
        public async Task<IActionResult> Index()
        {
            var bdDhrContext = _context.ExpertoPrevencions.Include(e => e.ActaIdActaNavigation);
            return View(await bdDhrContext.ToListAsync());
        }

        // GET: ExpertoPrevencions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExpertoPrevencions == null)
            {
                return NotFound();
            }

            var expertoPrevencion = await _context.ExpertoPrevencions
                .Include(e => e.ActaIdActaNavigation)
                                .ThenInclude(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(j => j.ActaIdActaNavigation.RazonSocialIdRazonSocialNavigation)
                .Include(j => j.ActaIdActaNavigation.ObraIdObraNavigation)
                .FirstOrDefaultAsync(m => m.IdExpertoprev == id);
            if (expertoPrevencion == null)
            {
                return NotFound();
            }

            return View(expertoPrevencion);
        }

        // GET: ExpertoPrevencions/Create
        public IActionResult Create()
        {
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa");
            return View();
        }

        // POST: ExpertoPrevencions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExpertoprev,Nombre,CuenContTrab,CuenRegInterno,CompRegInsec,CuentaEpp,IntroEspecifica,Cumple100,CumpleIntrucciones,Observacion,Firma,ActaIdActa")] ExpertoPrevencion expertoPrevencion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expertoPrevencion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", expertoPrevencion.ActaIdActa);
            return View(expertoPrevencion);
        }

        // GET: ExpertoPrevencions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExpertoPrevencions == null)
            {
                return NotFound();
            }

            // Incluye las relaciones de navegación 
            var expertoPrevencion = await _context.ExpertoPrevencions
                .Include(e => e.ActaIdActaNavigation)
                .ThenInclude(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(e => e.ActaIdActaNavigation.RazonSocialIdRazonSocialNavigation)
                .Include(e => e.ActaIdActaNavigation.ObraIdObraNavigation)
                .FirstOrDefaultAsync(m => m.IdExpertoprev == id); // Suponiendo que el ID se llama IdExpertoPrevencion
                                                                  // Aquí comienza la lógica para actualizar la fecha de aprobación en Actum
            if (expertoPrevencion.Firma && expertoPrevencion.ActaIdActa.HasValue)
            {
                var acta = await _context.Acta.FindAsync(expertoPrevencion.ActaIdActa.Value);
                if (acta != null)
                {
                    acta.FechaApro = DateTime.Now;
                    _context.Update(acta);
                    await _context.SaveChangesAsync();
                }
            }
            if (expertoPrevencion == null)
            {
                return NotFound();
            }

            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", expertoPrevencion.ActaIdActa);
            return View(expertoPrevencion);
        }

        public IActionResult RedirectToListarActums()
        {
            return RedirectToAction("ListarActas", "Actums");
        }
        // POST: ExpertoPrevencions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("IdExpertoprev,Nombre,CuenContTrab,CuenRegInterno,CompRegInsec,CuentaEpp,IntroEspecifica,Cumple100,CumpleIntrucciones,Observacion,Firma,ActaIdActa")] ExpertoPrevencion expertoPrevencion)
        {
            if (id != expertoPrevencion.IdExpertoprev)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expertoPrevencion);
                    await _context.SaveChangesAsync();
                    // Aquí comienza la lógica para las notificaciones

                    var obra = await _context.ExpertoPrevencions
                        .Include(j => j.ActaIdActaNavigation)
                        .Include(j => j.ActaIdActaNavigation.ObraIdObraNavigation)
                        .ThenInclude(o => o.UsuarioUsuarios)
                        .FirstOrDefaultAsync(a => a.IdExpertoprev == expertoPrevencion.IdExpertoprev);




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
                                    Descripcion = $"Experto en Prevencion firmo en el acta:" + expertoPrevencion.ActaIdActaNavigation.IdActa,
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
                    if (!ExpertoPrevencionExists(expertoPrevencion.IdExpertoprev))
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
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", expertoPrevencion.ActaIdActa);
            return View(expertoPrevencion);
        }

        // GET: ExpertoPrevencions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExpertoPrevencions == null)
            {
                return NotFound();
            }

            var expertoPrevencion = await _context.ExpertoPrevencions
                .Include(e => e.ActaIdActaNavigation)
                .FirstOrDefaultAsync(m => m.IdExpertoprev == id);
            if (expertoPrevencion == null)
            {
                return NotFound();
            }

            return View(expertoPrevencion);
        }

        // POST: ExpertoPrevencions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExpertoPrevencions == null)
            {
                return Problem("Entity set 'BdDhrContext.ExpertoPrevencions'  is null.");
            }
            var expertoPrevencion = await _context.ExpertoPrevencions.FindAsync(id);
            if (expertoPrevencion != null)
            {
                _context.ExpertoPrevencions.Remove(expertoPrevencion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpertoPrevencionExists(int id)
        {
          return (_context.ExpertoPrevencions?.Any(e => e.IdExpertoprev == id)).GetValueOrDefault();
        }
    }
}
