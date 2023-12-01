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
    public class AdministradorObrasController : Controller
    {
        private readonly BdDhrContext _context;

        public AdministradorObrasController(BdDhrContext context)
        {
            _context = context;
        }

        // GET: AdministradorObras
        public async Task<IActionResult> Index()
        {
            var bdDhrContext = _context.AdministradorObras.Include(a => a.ActaIdActaNavigation);
            return View(await bdDhrContext.ToListAsync());
        }

        // GET: AdministradorObras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdministradorObras == null)
            {
                return NotFound();
            }

            var administradorObra = await _context.AdministradorObras
                .Include(a => a.ActaIdActaNavigation)
                                .ThenInclude(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(j => j.ActaIdActaNavigation.RazonSocialIdRazonSocialNavigation)
                .Include(j => j.ActaIdActaNavigation.ObraIdObraNavigation)
                .FirstOrDefaultAsync(m => m.IdEncalidad == id);
            if (administradorObra == null)
            {
                return NotFound();
            }

            return View(administradorObra);
        }

        // GET: AdministradorObras/Create
        public IActionResult Create()
        {
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa");
            return View();
        }

        // POST: AdministradorObras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEncalidad,Nombre,IvaCancel,PantImposicion,Finiquito,LiqSueldo,CerAntecedente,CerF301,LibAsist,Observacion,Firma,ActaIdActa")] AdministradorObra administradorObra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administradorObra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", administradorObra.ActaIdActa);
            return View(administradorObra);
        }

        // GET: AdministradorObras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdministradorObras == null)
            {
                return NotFound();
            }

            // Incluye las relaciones de navegación 
            var administradorObra = await _context.AdministradorObras
                .Include(a => a.ActaIdActaNavigation)
                .ThenInclude(a => a.EspecialidadIdEspecialidadNavigation)
                .Include(a => a.ActaIdActaNavigation.RazonSocialIdRazonSocialNavigation)
                .Include(a => a.ActaIdActaNavigation.ObraIdObraNavigation)
                .FirstOrDefaultAsync(m => m.IdEncalidad == id);

            if (administradorObra == null)
            {
                return NotFound();
            }

            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", administradorObra.ActaIdActa);
            return View(administradorObra);
        }
        public IActionResult RedirectToListarActums()
        {
            return RedirectToAction("ListarActas", "Actums");
        }
        // POST: AdministradorObras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("IdEncalidad,Nombre,IvaCancel,PantImposicion,Finiquito,LiqSueldo,CerAntecedente,CerF301,LibAsist,Observacion,Firma,ActaIdActa")] AdministradorObra administradorObra)
        {
            if (id != administradorObra.IdEncalidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administradorObra);
                    await _context.SaveChangesAsync();

                    // Aquí comienza la lógica para las notificaciones

                    var obra = await _context.AdministradorObras
                        .Include(j => j.ActaIdActaNavigation)
                        .Include(j => j.ActaIdActaNavigation.ObraIdObraNavigation)
                        .ThenInclude(o => o.UsuarioUsuarios)
                        .FirstOrDefaultAsync(a => a.IdEncalidad == administradorObra.IdEncalidad);


                    // Aquí comienza la lógica para actualizar la fecha de aprobación en Actum
                    if (administradorObra.Firma && administradorObra.ActaIdActa.HasValue)
                    {
                        var acta = await _context.Acta.FindAsync(administradorObra.ActaIdActa.Value);
                        if (acta != null)
                        {
                            acta.FechaApro = DateTime.Now;
                            _context.Update(acta);
                            await _context.SaveChangesAsync();
                        }
                    }

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
                                    Descripcion = $"Administrador de obra firmo en el acta:" + administradorObra.ActaIdActaNavigation.IdActa,
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
                    if (!AdministradorObraExists(administradorObra.IdEncalidad))
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
            ViewData["ActaIdActa"] = new SelectList(_context.Acta, "IdActa", "IdActa", administradorObra.ActaIdActa);
            return View(administradorObra);
        }

        // GET: AdministradorObras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdministradorObras == null)
            {
                return NotFound();
            }

            var administradorObra = await _context.AdministradorObras
                .Include(a => a.ActaIdActaNavigation)
                .FirstOrDefaultAsync(m => m.IdEncalidad == id);
            if (administradorObra == null)
            {
                return NotFound();
            }

            return View(administradorObra);
        }

        // POST: AdministradorObras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdministradorObras == null)
            {
                return Problem("Entity set 'BdDhrContext.AdministradorObras'  is null.");
            }
            var administradorObra = await _context.AdministradorObras.FindAsync(id);
            if (administradorObra != null)
            {
                _context.AdministradorObras.Remove(administradorObra);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministradorObraExists(int id)
        {
          return (_context.AdministradorObras?.Any(e => e.IdEncalidad == id)).GetValueOrDefault();
        }
    }
}
